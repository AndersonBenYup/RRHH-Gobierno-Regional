using System.Globalization;
using System.Text;
using Capa_Datos;
using Capa_Entidades;
using Capa_Logica;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Services;

// Arma y guarda la ficha plana del empleado repartiendo cada dato en su tabla normalizada.
public class EmpleadoFichaServicio
{
    private const string TipoResolIngresoEstado = "Ingreso al Estado";
    private const string TipoResolIngresoInstitucion = "Ingreso a la Institucion";

    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly Empleado_AreaLogica _empleadoAreaData;
    private readonly EmpleadoAreaActualLogica _areaActualData;
    private readonly RRHH_AspSocioLogica _aspSocioData;
    private readonly ResolucionLogica _resolucionData;
    private readonly TipoResolucionLogica _tipoResolucionData;
    private readonly EmpleadoResumenFamiliarLogica _resumenFamiliarData;
    private readonly SecuenciaService _secuencia;

    public EmpleadoFichaServicio(
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        Empleado_AreaLogica empleadoAreaData,
        EmpleadoAreaActualLogica areaActualData,
        RRHH_AspSocioLogica aspSocioData,
        ResolucionLogica resolucionData,
        TipoResolucionLogica tipoResolucionData,
        EmpleadoResumenFamiliarLogica resumenFamiliarData,
        SecuenciaService secuencia)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
        _empleadoAreaData = empleadoAreaData;
        _areaActualData = areaActualData;
        _aspSocioData = aspSocioData;
        _resolucionData = resolucionData;
        _tipoResolucionData = tipoResolucionData;
        _resumenFamiliarData = resumenFamiliarData;
        _secuencia = secuencia;
    }

    public static string? EstadoTexto(bool? estado)
        => estado switch { true => EmpleadoFicha.EstadoActivo, false => EmpleadoFicha.EstadoInactivo, null => null };

    public async Task<EmpleadoFicha?> CargarAsync(int idEmpleado)
    {
        var e = await _empleadoData.Obtener(idEmpleado);
        if (e is null)
            return null;

        var p = e.IdPersona is null ? null : await _personaData.Obtener(e.IdPersona.Value);
        var area = await _areaActualData.Obtener(idEmpleado);
        var social = await AspSocioAsync(idEmpleado);
        var (tipoEstado, tipoInstitucion) = await TiposResolucionIngresoAsync();
        var resoluciones = (await _resolucionData.Listar()).Where(r => r.idEmpleado == idEmpleado).ToList();
        var resolEstado = resoluciones.LastOrDefault(r => r.idTipoResolucion == tipoEstado);
        var resolInstitucion = resoluciones.LastOrDefault(r => r.idTipoResolucion == tipoInstitucion);

        return new EmpleadoFicha
        {
            IdEmpleado = e.IdEmpleado,
            IdPersona = e.IdPersona,
            FechaIngreso = e.FechaIngreso,
            Estado = EstadoTexto(e.Estado),
            IdTipoTrabajador = e.IdTipoTrabajador,
            IdCargo = e.IdCargo,
            idHorario = e.idHorario,
            idLocal = e.idLocal,
            IdRegimenPen = e.IdRegimenPen,
            IdAFP = e.IdAFP,
            NroAFP = e.NroAFP,
            FechaAFP = e.FechaAFP,
            CtaCorriente = e.CtaCorriente,
            DecLey = e.DecLey,
            ExpSocial = e.ExpSocial,
            CargaFam = e.CargaFam,
            JefeHogar = e.JefeHogar,
            IdViveCon = e.IdViveCon,

            Year = area?.Year ?? DateTime.Now.Year,
            idAreaOrganiz = area?.idAreaOrganiz ?? 0,

            Sexo = p?.Sexo,
            IdEstadoCivil = p?.IdEstadoCivil,
            IdNacionalidad = p?.IdNacionalidad,
            IdTipoSangre = p?.IdTipoSangre,
            IdDiscapacidad = p?.IdDiscapacidad,
            NumAutogenSalud = p?.NumAutogenSalud,
            NumRUC = p?.NumRUC,
            Brevete = p?.Brevete,
            NumLibretaMilitar = p?.NumLibretaMilitar,
            GradoInstruccion = p?.GradoInstruccion,
            Profesion = p?.Profesion,

            ObsFam = social?.ObsFam,
            AspecSalud = social?.AspecSalud,
            AspecEducacion = social?.AspecEducacion,
            AspecRecreacion = social?.AspecRecreacion,
            AspecPsico = social?.AspecPsico,
            AspecSocial = social?.AspecSocial,
            DiagnosSocial = social?.DiagnosSocial,

            NumResoIngreEsta = resolEstado?.NumeroResolucion,
            FechaResolIngreEsta = ADateTime(resolEstado?.FechaResolucion),
            FechaIngreEstado = ADateTime(resolEstado?.FechaVigencia),
            NumResoIngreInstitu = resolInstitucion?.NumeroResolucion,
            FechaResoIngreInstitu = ADateTime(resolInstitucion?.FechaResolucion),

            NumHijos = (await _resumenFamiliarData.Obtener(idEmpleado))?.NumHijos
        };
    }

    // Registra persona + empleado. Devuelve el IdEmpleado generado, o null si la transaccion no se pudo completar.
    public async Task<int?> CrearAsync(Persona persona, EmpleadoFicha ficha, byte[]? foto)
    {
        var idEmpleado = 0;
        var guardo = await _secuencia.EjecutarTransaccionalAsync(async () =>
        {
            persona.IdPersona = 0;
            persona.TipoPersona ??= "N";
            persona.FechaRegistro = DateTime.Now;
            persona.Foto = foto;
            AplicarPersona(persona, ficha);
            await _personaData.Crear(persona);

            var empleado = new Empleado { IdPersona = persona.IdPersona };
            AplicarEmpleado(empleado, ficha);
            empleado.Estado ??= true;
            await _empleadoData.Crear(empleado);
            idEmpleado = empleado.IdEmpleado;

            ficha.IdEmpleado = idEmpleado;
            await GuardarAreaAsync(ficha);
            await GuardarAspSocioAsync(ficha);
            await GuardarResolucionesAsync(ficha);
        });
        return guardo ? idEmpleado : null;
    }

    public async Task<bool> ActualizarAsync(EmpleadoFicha ficha, byte[]? foto)
    {
        return await _secuencia.EjecutarTransaccionalAsync(async () =>
        {
            var empleado = await _empleadoData.Obtener(ficha.IdEmpleado)
                ?? throw new InvalidOperationException("Empleado no encontrado");
            AplicarEmpleado(empleado, ficha);
            await _empleadoData.Actualizar(empleado);

            if (empleado.IdPersona is not null && await _personaData.Obtener(empleado.IdPersona.Value) is { } persona)
            {
                AplicarPersona(persona, ficha);
                if (foto is not null)
                    persona.Foto = foto;
                await _personaData.Actualizar(persona);
            }

            await GuardarAreaAsync(ficha);
            await GuardarAspSocioAsync(ficha);
            await GuardarResolucionesAsync(ficha);
        });
    }

    private static void AplicarEmpleado(Empleado e, EmpleadoFicha f)
    {
        e.FechaIngreso = f.FechaIngreso;
        e.Estado = f.Estado switch { EmpleadoFicha.EstadoActivo => true, EmpleadoFicha.EstadoInactivo => false, _ => e.Estado };
        e.IdTipoTrabajador = f.IdTipoTrabajador;
        e.IdCargo = f.IdCargo;
        e.idHorario = f.idHorario;
        e.idLocal = f.idLocal;
        e.IdRegimenPen = f.IdRegimenPen;
        e.IdAFP = f.IdAFP;
        e.NroAFP = f.NroAFP;
        e.FechaAFP = f.FechaAFP;
        e.CtaCorriente = f.CtaCorriente;
        e.DecLey = f.DecLey;
        e.ExpSocial = f.ExpSocial;
        e.CargaFam = f.CargaFam;
        e.JefeHogar = f.JefeHogar;
        e.IdViveCon = f.IdViveCon;
    }

    private static void AplicarPersona(Persona p, EmpleadoFicha f)
    {
        p.Sexo = f.Sexo;
        p.IdEstadoCivil = f.IdEstadoCivil;
        p.IdNacionalidad = f.IdNacionalidad;
        p.IdTipoSangre = f.IdTipoSangre;
        p.IdDiscapacidad = f.IdDiscapacidad;
        p.NumAutogenSalud = f.NumAutogenSalud;
        p.NumRUC = f.NumRUC;
        p.Brevete = f.Brevete;
        p.NumLibretaMilitar = f.NumLibretaMilitar;
        p.GradoInstruccion = f.GradoInstruccion;
        p.Profesion = f.Profesion;
    }

    // El area vigente es la del año mas reciente del historial: si cambia, se agrega una fila al historial.
    private async Task GuardarAreaAsync(EmpleadoFicha f)
    {
        if (f.idAreaOrganiz <= 0 || f.Year <= 0)
            return;
        if (await _empleadoAreaData.Obtener(f.Year, f.idAreaOrganiz, f.IdEmpleado) is not null)
            return;
        await _empleadoAreaData.Crear(new Empleado_Area { Year = f.Year, idAreaOrganiz = f.idAreaOrganiz, IdEmpleado = f.IdEmpleado });
    }

    private async Task GuardarAspSocioAsync(EmpleadoFicha f)
    {
        var social = await AspSocioAsync(f.IdEmpleado);
        var hayDatos = new[] { f.ObsFam, f.AspecSalud, f.AspecEducacion, f.AspecRecreacion, f.AspecPsico, f.AspecSocial, f.DiagnosSocial }
            .Any(v => !string.IsNullOrWhiteSpace(v));
        if (social is null && !hayDatos)
            return;

        social ??= new RRHH_AspSocio { IdEmpleado = f.IdEmpleado };
        social.ObsFam = f.ObsFam;
        social.AspecSalud = f.AspecSalud;
        social.AspecEducacion = f.AspecEducacion;
        social.AspecRecreacion = f.AspecRecreacion;
        social.AspecPsico = f.AspecPsico;
        social.AspecSocial = f.AspecSocial;
        social.DiagnosSocial = f.DiagnosSocial;

        if (social.idAspSocio == 0)
            await _aspSocioData.Crear(social);
        else
            await _aspSocioData.Actualizar(social);
    }

    private async Task GuardarResolucionesAsync(EmpleadoFicha f)
    {
        var (tipoEstado, tipoInstitucion) = await TiposResolucionIngresoAsync();
        var resoluciones = (await _resolucionData.Listar()).Where(r => r.idEmpleado == f.IdEmpleado).ToList();

        await GuardarResolucionAsync(resoluciones, f.IdEmpleado, tipoEstado,
            f.NumResoIngreEsta, f.FechaResolIngreEsta, f.FechaIngreEstado);
        await GuardarResolucionAsync(resoluciones, f.IdEmpleado, tipoInstitucion,
            f.NumResoIngreInstitu, f.FechaResoIngreInstitu, null);
    }

    private async Task GuardarResolucionAsync(List<Resolucion> resoluciones, int idEmpleado, int? idTipo,
        string? numero, DateTime? fechaResolucion, DateTime? fechaVigencia)
    {
        if (idTipo is null)
            return;

        var actual = resoluciones.LastOrDefault(r => r.idTipoResolucion == idTipo);
        var vacio = string.IsNullOrWhiteSpace(numero) && fechaResolucion is null && fechaVigencia is null;
        if (actual is null && vacio)
            return;

        if (actual is null)
        {
            actual = new Resolucion
            {
                idEmpleado = idEmpleado,
                idResolucion = resoluciones.Select(r => r.idResolucion).DefaultIfEmpty(0).Max() + 1,
                idTipoResolucion = idTipo
            };
            resoluciones.Add(actual);
            AplicarResolucion(actual, numero, fechaResolucion, fechaVigencia);
            await _resolucionData.Crear(actual);
            return;
        }

        AplicarResolucion(actual, numero, fechaResolucion, fechaVigencia);
        await _resolucionData.Actualizar(actual);
    }

    private static void AplicarResolucion(Resolucion r, string? numero, DateTime? fechaResolucion, DateTime? fechaVigencia)
    {
        r.NumeroResolucion = numero;
        r.FechaResolucion = fechaResolucion is null ? null : DateOnly.FromDateTime(fechaResolucion.Value);
        r.FechaVigencia = fechaVigencia is null ? null : DateOnly.FromDateTime(fechaVigencia.Value);
    }

    private async Task<RRHH_AspSocio?> AspSocioAsync(int idEmpleado)
        => (await _aspSocioData.Listar()).Where(a => a.IdEmpleado == idEmpleado).OrderBy(a => a.idAspSocio).FirstOrDefault();

    private async Task<(int? Estado, int? Institucion)> TiposResolucionIngresoAsync()
    {
        var tipos = await _tipoResolucionData.Listar();
        int? Buscar(string descripcion) => tipos
            .FirstOrDefault(t => SinTildes(t.DescripTipoResolucion).Equals(descripcion, StringComparison.OrdinalIgnoreCase))?.idTipoResolucion;
        return (Buscar(TipoResolIngresoEstado), Buscar(TipoResolIngresoInstitucion));
    }

    private static string SinTildes(string? texto)
    {
        var normalizado = (texto ?? string.Empty).Trim().Normalize(NormalizationForm.FormD);
        return new string(normalizado.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray());
    }

    private static DateTime? ADateTime(DateOnly? fecha) => fecha?.ToDateTime(TimeOnly.MinValue);
}
