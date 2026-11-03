using Capa_Entidades;
using Capa_Logica;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Services;

/// <summary>Catálogos del formulario de Personal (alta y edición).</summary>
public sealed record EmpleadoCatalogos(
    List<Cargo> Cargos, List<Horario> Horarios, List<Local> Locales, List<EstructOrganiz> Areas,
    List<TipoTrabajador> TiposTrabajador, List<RegimenPension> Regimenes, List<AFP> Afps,
    List<EstadoCivil> EstadosCiviles, List<TipoSangre> TiposSangre, List<Discapacidad> Discapacidades,
    List<ViveCon> ViveCon, List<Nacionalidad> Nacionalidades, List<TipoDocID> TiposDocumento);

/// <summary>
/// Catálogos de la ficha de Personal: los lista para el formulario y traduce los Id a texto para el detalle.
/// Antes eran 14 dependencias de EmpleadoController (SRP).
/// </summary>
public class EmpleadoCatalogosServicio
{
    private readonly CargoLogica _cargos;
    private readonly HorarioLogica _horarios;
    private readonly LocalLogica _locales;
    private readonly EstructOrganizLogica _areas;
    private readonly TipoTrabajadorLogica _tiposTrabajador;
    private readonly RegimenPensionLogica _regimenes;
    private readonly AFPLogica _afps;
    private readonly EstadoCivilLogica _estadosCiviles;
    private readonly TipoSangreLogica _tiposSangre;
    private readonly DiscapacidadLogica _discapacidades;
    private readonly ViveConLogica _viveCon;
    private readonly NacionalidadLogica _nacionalidades;
    private readonly TipoDocIDLogica _tiposDocumento;
    private readonly UbigeoLogica _ubigeos;
    private readonly EmpleadoAreaActualLogica _areasActuales;

    public EmpleadoCatalogosServicio(CargoLogica cargos, HorarioLogica horarios, LocalLogica locales,
        EstructOrganizLogica areas, TipoTrabajadorLogica tiposTrabajador, RegimenPensionLogica regimenes,
        AFPLogica afps, EstadoCivilLogica estadosCiviles, TipoSangreLogica tiposSangre,
        DiscapacidadLogica discapacidades, ViveConLogica viveCon, NacionalidadLogica nacionalidades,
        TipoDocIDLogica tiposDocumento, UbigeoLogica ubigeos, EmpleadoAreaActualLogica areasActuales)
    {
        _cargos = cargos;
        _horarios = horarios;
        _locales = locales;
        _areas = areas;
        _tiposTrabajador = tiposTrabajador;
        _regimenes = regimenes;
        _afps = afps;
        _estadosCiviles = estadosCiviles;
        _tiposSangre = tiposSangre;
        _discapacidades = discapacidades;
        _viveCon = viveCon;
        _nacionalidades = nacionalidades;
        _tiposDocumento = tiposDocumento;
        _ubigeos = ubigeos;
        _areasActuales = areasActuales;
    }

    public async Task<EmpleadoCatalogos> ListarParaFormularioAsync() => new(
        await _cargos.Listar(), await _horarios.Listar(), await _locales.Listar(), await _areas.Listar(),
        await _tiposTrabajador.Listar(), await _regimenes.Listar(), await _afps.Listar(),
        await _estadosCiviles.Listar(), await _tiposSangre.Listar(), await _discapacidades.Listar(),
        await _viveCon.Listar(), await _nacionalidades.Listar(), await _tiposDocumento.Listar());

    /// <summary>Arma la vista de detalle: cada Id de la ficha se muestra con su descripción.</summary>
    public async Task<EmpleadoDetalleViewModel> DetalleAsync(EmpleadoFicha empleado, Persona? persona)
    {
        var ubigeo = persona?.IdUbigeo is null ? null : await _ubigeos.Obtener(persona.IdUbigeo);
        return new EmpleadoDetalleViewModel
        {
            IdEmpleado = empleado.IdEmpleado,
            Empleado = empleado,
            Persona = persona,
            NombresCompletos = persona is null ? null : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim(),
            TipoDoc = persona?.TipoDocID is null ? null : (await _tiposDocumento.Listar()).FirstOrDefault(t => t.Nombre == persona.TipoDocID)?.Descripcion,
            Cargo = empleado.IdCargo is null ? null : (await _cargos.Listar()).FirstOrDefault(c => c.IdCargo == empleado.IdCargo)?.Descripcion,
            Area = (await _areasActuales.Obtener(empleado.IdEmpleado))?.AreaOrganizacional,
            Ubigeo = ubigeo is null ? null : $"{ubigeo.Descripcion} - {ubigeo.NombreProvincia} - {ubigeo.NombreDepartamento}",
            Local = empleado.idLocal is null ? null : (await _locales.Listar()).FirstOrDefault(l => l.idLocal == empleado.idLocal)?.NombreLocal,
            Horario = empleado.idHorario is null ? null : (await _horarios.Listar()).FirstOrDefault(h => h.idHorario == empleado.idHorario)?.Descrip_Horario,
            Estado = empleado.Estado,
            AFP = empleado.IdAFP is null ? null : (await _afps.Listar()).FirstOrDefault(a => a.IdAFP == empleado.IdAFP)?.DescripAFP,
            EstadoCivil = empleado.IdEstadoCivil is null ? null : (await _estadosCiviles.Listar()).FirstOrDefault(ec => ec.IdEstadoCivil == empleado.IdEstadoCivil)?.DescripEstCivil,
            TipoSangre = empleado.IdTipoSangre is null ? null : (await _tiposSangre.Listar()).FirstOrDefault(s => s.IdTipoSangre == empleado.IdTipoSangre)?.DescripTS,
            Discapacidad = empleado.IdDiscapacidad is null ? null : (await _discapacidades.Listar()).FirstOrDefault(d => d.IdDiscapacidad == empleado.IdDiscapacidad)?.Descrip_Discapacidad,
            ViveCon = empleado.IdViveCon is null ? null : (await _viveCon.Listar()).FirstOrDefault(v => v.IdViveCon == empleado.IdViveCon)?.DescripViveCon,
            Nacionalidad = empleado.IdNacionalidad is null ? null : (await _nacionalidades.Listar()).FirstOrDefault(n => n.IdNacionalidad == empleado.IdNacionalidad)?.Descripcion,
            TipoTrabajador = empleado.IdTipoTrabajador is null ? null : (await _tiposTrabajador.Listar()).FirstOrDefault(t => t.IdTipoTrabajador == empleado.IdTipoTrabajador)?.Descripcion,
            RegimenPension = empleado.IdRegimenPen is null ? null : (await _regimenes.Listar()).FirstOrDefault(r => r.IdRegimenPen == empleado.IdRegimenPen)?.LeyRegimen,
            TieneFoto = persona?.Foto is not null
        };
    }
}
