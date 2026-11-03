using System.Security.Claims;
using Capa_Logica;
using Capa_Logica.Seguridad;
using Capa_Entidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

public class SeguridadController : Controller
{
    private readonly UsuarioLogica _usuarioData;
    private readonly BitacoraLogica _bitacora;
    private readonly BloqueoCuentaLogica _bloqueo;
    private readonly ILogger<SeguridadController> _logger;

    public SeguridadController(UsuarioLogica usuarioData, BitacoraLogica bitacora, BloqueoCuentaLogica bloqueo,
        ILogger<SeguridadController> logger)
    {
        _usuarioData = usuarioData;
        _bitacora = bitacora;
        _bloqueo = bloqueo;
        _logger = logger;
    }

    internal static string NombreRol(string? descripcion)
    {
        var nombre = descripcion?.Trim();
        if (string.IsNullOrEmpty(nombre))
            return "SinRol";
        return nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ? "Administrador" : nombre;
    }

    private string MensajeBloqueo(EstadoBloqueo estado)
        => $"La cuenta está bloqueada por {_bloqueo.MaxIntentos} intentos fallidos. " +
           $"Intente de nuevo en {estado.MinutosRestantes(_bloqueo.Ahora)} minuto(s) o pida al administrador que la desbloquee.";

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login(LoginViewModel modelo, string? returnUrl)
    {
        if (!ModelState.IsValid)
            return View(modelo);

        var login = modelo.Login.Trim();
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

        // SC-009: una cuenta bloqueada no se verifica (ni siquiera con la clave correcta) hasta que pase el bloqueo.
        var estado = await _bloqueo.Estado(login);
        if (estado.Bloqueada)
        {
            _logger.LogWarning("Intento de acceso a la cuenta bloqueada {Login} desde {Ip}", login, ip);
            await _bloqueo.RegistrarIntentoBloqueado(login, ip);
            modelo.Error = MensajeBloqueo(estado);
            return View(modelo);
        }

        var usuario = await _usuarioData.ObtenerPorLogin(login);
        var claveCorrecta = usuario is not null && usuario.Estado == true && !string.IsNullOrEmpty(usuario.PasswordHash)
            && new PasswordHasher<Usuario>().VerifyHashedPassword(new Usuario(), usuario.PasswordHash, modelo.Clave)
                != PasswordVerificationResult.Failed;

        if (!claveCorrecta)
        {
            estado = await _bloqueo.RegistrarFallo(login, ip);
            if (estado.Bloqueada)
                _logger.LogWarning("Cuenta {Login} bloqueada tras {Fallos} intentos fallidos (IP {Ip})", login, estado.FallosRecientes, ip);
            else
                _logger.LogInformation("Inicio de sesión fallido para {Login} desde {Ip}", login, ip);
            var restantes = _bloqueo.MaxIntentos - estado.FallosRecientes;
            modelo.Error = estado.Bloqueada
                ? MensajeBloqueo(estado)
                : restantes <= 2
                    ? $"Usuario o clave incorrectos. Le quedan {restantes} intento(s) antes de que la cuenta se bloquee."
                    : "Usuario o clave incorrectos.";
            return View(modelo);
        }

        _logger.LogInformation("Inicio de sesión correcto de {Login}", login);
        TempData["MensajeExito"] = $"Bienvenido, {usuario!.Login!.Trim()}.";
        await _bitacora.Registrar(usuario.Login.Trim(), ConstantesSeguridad.AccionLoginCorrecto, ConstantesSeguridad.EntidadSeguridad, "Acceso correcto al sistema");

        var roles = await _usuarioData.ObtenerRoles(usuario.IdUsuario);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new(ClaimTypes.Name, usuario.Login!.Trim()),
            new(ClaimTypes.GivenName, usuario.Descripcion ?? usuario.Login!.Trim())
        };
        // SC-009: [Authorize(Roles = "Administrador")] distingue mayúsculas; en la BD el rol puede estar como
        // "ADMINISTRADOR", así que se normaliza para que el administrador no quede sin acceso.
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, NombreRol(r.Descripcion))));

        var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identidad);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CerrarSesion()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [Authorize]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
}