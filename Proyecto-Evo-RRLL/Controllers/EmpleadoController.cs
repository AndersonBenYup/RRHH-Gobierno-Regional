using Capa_Logica;
using Proyecto_Evo_RRLL.Utilidades;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class EmpleadoController : Controller
{
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly CargoLogica _cargoData;
    private readonly EstructOrganizLogica _estructOrganizData;
    private readonly LocalLogica _localData;
    private readonly HorarioLogica _horarioData;
    private readonly TipoTrabajadorLogica _tipoTrabajadorData;
    private readonly RegimenPensionLogica _regimenPensionData;
    private readonly TipoDocIDLogica _tipoDocData;
    private readonly AFPLogica _afpData;
    private readonly EstadoCivilLogica _estadoCivilData;
    private readonly TipoSangreLogica _tipoSangreData;
    private readonly DiscapacidadLogica _discapacidadData;
    private readonly ViveConLogica _viveConData;
    private readonly NacionalidadLogica _nacionalidadData;
    private readonly UbigeoLogica _ubigeoData;
    private readonly EmpleadoAreaActualLogica _areaActualData;
    private readonly Services.EmpleadoFichaServicio _fichas;

    public EmpleadoController(
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        CargoLogica cargoData,
        EstructOrganizLogica estructuraOrganizData,
        LocalLogica localData,
        HorarioLogica horarioData,
        TipoTrabajadorLogica tipoTrabajadorData,
        RegimenPensionLogica regimenPensionData,
        TipoDocIDLogica tipoDocData,
        AFPLogica afpData,
        EstadoCivilLogica estadoCivilData,
        TipoSangreLogica tipoSangreData,
        DiscapacidadLogica discapacidadData,
        ViveConLogica viveConData,
        NacionalidadLogica nacionalidadData,
        UbigeoLogica ubigeoData,
        EmpleadoAreaActualLogica areaActualData,
        Services.EmpleadoFichaServicio fichas)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
        _cargoData = cargoData;
        _estructOrganizData = estructuraOrganizData;
        _localData = localData;
        _horarioData = horarioData;
        _tipoTrabajadorData = tipoTrabajadorData;
        _regimenPensionData = regimenPensionData;
        _tipoDocData = tipoDocData;
        _afpData = afpData;
        _estadoCivilData = estadoCivilData;
        _tipoSangreData = tipoSangreData;
        _discapacidadData = discapacidadData;
        _viveConData = viveConData;
        _nacionalidadData = nacionalidadData;
        _ubigeoData = ubigeoData;
        _areaActualData = areaActualData;
        _fichas = fichas;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? q, int pagina = 1)
    {
        const int tamanoPagina = 25;
        var (filtrados, personasPorId, cargosPorId, areasPorEmpleado) = await FiltrarEmpleadosAsync(q);

        var total = filtrados.Count;
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamanoPagina));
        if (pagina < 1) pagina = 1;
        if (pagina > totalPaginas) pagina = totalPaginas;

        var vm = new EmpleadoViewModel { Busqueda = q, Pagina = pagina, TotalPaginas = totalPaginas, Total = total };

        foreach (var e in filtrados.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina))
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null
                ? null
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            var cargo = e.IdCargo is null ? null : cargosPorId.GetValueOrDefault(e.IdCargo.Value)?.Descripcion;
            var area = areasPorEmpleado.GetValueOrDefault(e.IdEmpleado)?.AreaOrganizacional;

            vm.Empleados.Add(new EmpleadoFila
            {
                IdEmpleado = e.IdEmpleado,
                Nombres = nombre,
                NumDoc = persona?.NumDocID,
                Cargo = cargo,
                Area = area,
                Estado = Services.EmpleadoFichaServicio.EstadoTexto(e.Estado),
                FechaIngreso = e.FechaIngreso,
                TieneFoto = persona?.Foto is not null
            });
        }

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Exportar(string? q)
    {
        var (filtrados, personasPorId, cargosPorId, areasPorEmpleado) = await FiltrarEmpleadosAsync(q);

        var filas = new List<string[]>
        {
            new[] { "IdEmpleado", "Nombres", "NumDoc", "Cargo", "Area", "Estado", "FechaIngreso" }
        };

        foreach (var e in filtrados)
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null ? null : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            var cargo = e.IdCargo is null ? null : cargosPorId.GetValueOrDefault(e.IdCargo.Value)?.Descripcion;
            var area = areasPorEmpleado.GetValueOrDefault(e.IdEmpleado)?.AreaOrganizacional;

            filas.Add(new[]
            {
                e.IdEmpleado.ToString(),
                nombre ?? string.Empty,
                persona?.NumDocID ?? string.Empty,
                cargo ?? string.Empty,
                area ?? string.Empty,
                Services.EmpleadoFichaServicio.EstadoTexto(e.Estado) ?? string.Empty,
                e.FechaIngreso?.ToString("dd/MM/yyyy") ?? string.Empty
            });
        }

        var contenido = string.Join("\r\n", filas.Select(f => string.Join(";", f.Select(CsvUtilidad.EscaparCsv))));
        var nombreArchivo = $"empleados_{(string.IsNullOrWhiteSpace(q) ? "todos" : "filtro")}_{DateTime.Now:yyyyMMdd_HHmm}.csv";
        return File(new System.Text.UTF8Encoding(true).GetBytes(contenido), "text/csv; charset=utf-8", nombreArchivo);
    }

    private async Task<(List<Empleado> Filtrados, Dictionary<int, Persona> Personas, Dictionary<int, Cargo> Cargos, Dictionary<int, EmpleadoAreaActual> Areas)> FiltrarEmpleadosAsync(string? q)
    {
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();
        var cargos = await _cargoData.Listar();
        var areas = await _areaActualData.Listar();

        var personasPorId = personas.ToDictionary(p => p.IdPersona);
        var cargosPorId = cargos.ToDictionary(c => c.IdCargo);
        var areasPorEmpleado = areas.ToDictionary(a => a.IdEmpleado);

        if (string.IsNullOrWhiteSpace(q))
            return (empleados.OrderBy(e => e.IdEmpleado).ToList(), personasPorId, cargosPorId, areasPorEmpleado);

        var filtrados = new List<Empleado>();
        foreach (var e in empleados)
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null ? null : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            if (nombre?.Contains(q, StringComparison.OrdinalIgnoreCase) == true
                || persona?.NumDocID?.Contains(q, StringComparison.OrdinalIgnoreCase) == true)
                filtrados.Add(e);
        }

        return (filtrados.OrderBy(e => e.IdEmpleado).ToList(), personasPorId, cargosPorId, areasPorEmpleado);
    }


    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        var empleado = await _fichas.CargarAsync(id);
        if (empleado is null)
            return NotFound();

        var persona = empleado.IdPersona is null ? null : await _personaData.Obtener(empleado.IdPersona.Value);
        var ubigeo = persona?.IdUbigeo is null ? null : await _ubigeoData.Obtener(persona.IdUbigeo);

        var vm = new EmpleadoDetalleViewModel
        {
            IdEmpleado = empleado.IdEmpleado,
            Empleado = empleado,
            Persona = persona,
            NombresCompletos = persona is null
                ? null
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim(),
            TipoDoc = persona?.TipoDocID is null ? null : (await _tipoDocData.Listar()).FirstOrDefault(t => t.Nombre == persona.TipoDocID)?.Descripcion,
            Cargo = empleado.IdCargo is null ? null : (await _cargoData.Listar()).FirstOrDefault(c => c.IdCargo == empleado.IdCargo)?.Descripcion,
            Area = (await _areaActualData.Obtener(id))?.AreaOrganizacional,
            Ubigeo = ubigeo is null ? null : $"{ubigeo.Descripcion} - {ubigeo.NombreProvincia} - {ubigeo.NombreDepartamento}",
            Local = empleado.idLocal is null ? null : (await _localData.Listar()).FirstOrDefault(l => l.idLocal == empleado.idLocal)?.NombreLocal,
            Horario = empleado.idHorario is null ? null : (await _horarioData.Listar()).FirstOrDefault(h => h.idHorario == empleado.idHorario)?.Descrip_Horario,
            Estado = empleado.Estado,
            AFP = empleado.IdAFP is null ? null : (await _afpData.Listar()).FirstOrDefault(a => a.IdAFP == empleado.IdAFP)?.DescripAFP,
            EstadoCivil = empleado.IdEstadoCivil is null ? null : (await _estadoCivilData.Listar()).FirstOrDefault(ec => ec.IdEstadoCivil == empleado.IdEstadoCivil)?.DescripEstCivil,
            TipoSangre = empleado.IdTipoSangre is null ? null : (await _tipoSangreData.Listar()).FirstOrDefault(s => s.IdTipoSangre == empleado.IdTipoSangre)?.DescripTS,
            Discapacidad = empleado.IdDiscapacidad is null ? null : (await _discapacidadData.Listar()).FirstOrDefault(d => d.IdDiscapacidad == empleado.IdDiscapacidad)?.Descrip_Discapacidad,
            ViveCon = empleado.IdViveCon is null ? null : (await _viveConData.Listar()).FirstOrDefault(v => v.IdViveCon == empleado.IdViveCon)?.DescripViveCon,
            Nacionalidad = empleado.IdNacionalidad is null ? null : (await _nacionalidadData.Listar()).FirstOrDefault(n => n.IdNacionalidad == empleado.IdNacionalidad)?.Descripcion,
            TipoTrabajador = empleado.IdTipoTrabajador is null ? null : (await _tipoTrabajadorData.Listar()).FirstOrDefault(t => t.IdTipoTrabajador == empleado.IdTipoTrabajador)?.Descripcion,
            RegimenPension = empleado.IdRegimenPen is null ? null : (await _regimenPensionData.Listar()).FirstOrDefault(r => r.IdRegimenPen == empleado.IdRegimenPen)?.LeyRegimen,
            TieneFoto = persona?.Foto is not null
        };
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var empleado = await _fichas.CargarAsync(id);
        if (empleado is null)
            return NotFound();

        await CargarCatalogosEdicion();
        return View(empleado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, EmpleadoFicha empleado, IFormFile? foto)
    {
        if (id != empleado.IdEmpleado)
            return NotFound();

        var archivo = foto is not null && foto.Length > 0 ? await LeerArchivo(foto) : null;
        if (await _fichas.ActualizarAsync(empleado, archivo))
            return RedirectToAction(nameof(Detalles), new { id });

        ModelState.AddModelError("", "No se pudo guardar el empleado: intente nuevamente.");
        await CargarCatalogosEdicion();
        return View(empleado);
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo()
    {
        await CargarCatalogosEdicion();
        return View(new EmpleadoNuevoViewModel
        {
            Empleado = new EmpleadoFicha
            {
                Year = DateTime.Now.Year,
                Estado = EmpleadoFicha.EstadoActivo,
                FechaIngreso = DateTime.Today
            }
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(EmpleadoNuevoViewModel modelo, IFormFile? foto)
    {
        var persona = modelo.Persona;
        if (string.IsNullOrWhiteSpace(persona.Nombres))
            ModelState.AddModelError(nameof(modelo.Persona.Nombres), "El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(persona.Apellido_Paterno))
            ModelState.AddModelError(nameof(modelo.Persona.Apellido_Paterno), "El apellido paterno es obligatorio.");
        if (persona.TipoDocID is not null && string.IsNullOrWhiteSpace(persona.NumDocID))
            ModelState.AddModelError(nameof(modelo.Persona.NumDocID), "Ingrese el número del documento.");

        if (!string.IsNullOrWhiteSpace(persona.NumDocID))
        {
            var personas = await _personaData.Listar();
            if (personas.Any(p => p.NumDocID == persona.NumDocID))
                ModelState.AddModelError(nameof(modelo.Persona.NumDocID), "Ya existe una persona con ese número de documento.");
        }

        if (ModelState.IsValid)
        {
            var archivo = foto is not null && foto.Length > 0 ? await LeerArchivo(foto) : null;
            var idEmpleado = await _fichas.CrearAsync(persona, modelo.Empleado, archivo);
            if (idEmpleado is not null)
            {
                TempData["MensajeExito"] = "Empleado registrado.";
                return RedirectToAction(nameof(Detalles), new { id = idEmpleado });
            }
            ModelState.AddModelError("", "No se pudo registrar el empleado: intente nuevamente.");
        }

        await CargarCatalogosEdicion();
        return View(modelo);
    }

    private async Task CargarCatalogosEdicion()
    {
        ViewBag.Cargos = await _cargoData.Listar();
        ViewBag.Horarios = await _horarioData.Listar();
        ViewBag.Locales = await _localData.Listar();
        ViewBag.Areas = await _estructOrganizData.Listar();
        ViewBag.TiposTrabajador = await _tipoTrabajadorData.Listar();
        ViewBag.Regimenes = await _regimenPensionData.Listar();
        ViewBag.Afps = await _afpData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.ViveCon = await _viveConData.Listar();
        ViewBag.Nacionalidades = await _nacionalidadData.Listar();
        ViewBag.TiposDocumento = await _tipoDocData.Listar();
    }

    [HttpGet]
    public async Task<IActionResult> MostrarFoto(int id)
    {
        var empleado = await _empleadoData.Obtener(id);
        var persona = empleado?.IdPersona is null ? null : await _personaData.Obtener(empleado.IdPersona.Value);
        if (persona?.Foto is null)
            return NotFound();
        return File(persona.Foto, "image/jpeg");
    }

    private static async Task<byte[]> LeerArchivo(IFormFile archivo)
    {
        using var ms = new MemoryStream();
        await archivo.CopyToAsync(ms);
        return ms.ToArray();
    }
}