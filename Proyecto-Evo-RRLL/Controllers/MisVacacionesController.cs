using System.Security.Claims;
using Capa_Logica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

/// <summary>
/// RF-12: el usuario final consulta sus vacaciones pendientes y usadas. No pertenece a ningún módulo de Rol_Acceso,
/// así que lo abre cualquier usuario con sesión, pero solo muestra el empleado vinculado a su propia cuenta
/// (Usuario.IdEmpleado): no recibe el empleado por parámetro.
/// </summary>
[Authorize]
public class MisVacacionesController : Controller
{
    private readonly VacacionesLogica _vacaciones;
    private readonly UsuarioLogica _usuarios;
    private readonly Services.EmpleadoServicio _empleados;

    public MisVacacionesController(VacacionesLogica vacaciones, UsuarioLogica usuarios, Services.EmpleadoServicio empleados)
    {
        _vacaciones = vacaciones;
        _usuarios = usuarios;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var nombreUsuario = User.FindFirstValue(ClaimTypes.GivenName) ?? User.Identity?.Name ?? "";
        var usuario = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var idUsuario)
            ? await _usuarios.Obtener(idUsuario)
            : null;

        if (usuario?.IdEmpleado is not int idEmpleado)
            return View(new MisVacacionesModelo(nombreUsuario, null,
                "Su usuario no está vinculado a un empleado. Pida al administrador que lo vincule en «Usuarios y roles»."));

        var nombre = await _empleados.NombreEmpleadoAsync(idEmpleado) ?? nombreUsuario;
        var vacaciones = await _vacaciones.DeEmpleado(idEmpleado, DateOnly.FromDateTime(DateTime.Today));
        return View(new MisVacacionesModelo(nombre, vacaciones,
            vacaciones is null ? "Su periodo laboral no tiene fecha de ingreso registrada. Consulte con Recursos Humanos." : null));
    }
}
