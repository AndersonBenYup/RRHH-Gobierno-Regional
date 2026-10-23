using System.Security.Claims;
using Capa_Logica;
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

    public SeguridadController(UsuarioLogica usuarioData, BitacoraLogica bitacora)
    {
        _usuarioData = usuarioData;
        _bitacora = bitacora;
    }

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

        var usuario = await _usuarioData.ObtenerPorLogin(modelo.Login.Trim());
        if (usuario is null || usuario.Estado != true || string.IsNullOrEmpty(usuario.PasswordHash))
        {
            modelo.Error = "Usuario o clave incorrectos.";
            return View(modelo);
        }

        var verificador = new PasswordHasher<Usuario>();
        var resultado = verificador.VerifyHashedPassword(new Usuario(), usuario.PasswordHash, modelo.Clave);
        if (resultado == PasswordVerificationResult.Failed)
        {
            modelo.Error = "Usuario o clave incorrectos.";
            return View(modelo);
        }
        TempData["MensajeExito"] = $"Bienvenido, {usuario.Login!.Trim()}.";
        await _bitacora.Registrar(usuario.Login.Trim(), "IniciarSesion", "Seguridad", "Acceso correcto al sistema");

        var roles = await _usuarioData.ObtenerRoles(usuario.IdUsuario);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new(ClaimTypes.Name, usuario.Login!.Trim()),
            new(ClaimTypes.GivenName, usuario.Descripcion ?? usuario.Login!.Trim())
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.Descripcion ?? "SinRol")));

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