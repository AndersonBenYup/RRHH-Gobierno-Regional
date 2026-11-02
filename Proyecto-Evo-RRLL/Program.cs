using System.Threading.RateLimiting;
using Capa_Datos;
using Capa_Entidades;
using Capa_Logica;
using Capa_Logica.Seguridad;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Proyecto_Evo_RRLL.Filtros;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<RegistrarExcepcionFilter>();
    MensajesValidacionEspanol.Configurar(options); // SC-008
});

// SC-004: la cadena viene de la configuracion del entorno (en Azure: Connection string "RRHHNormalizado").
var cadenaConexion = builder.Configuration.GetConnectionString("RRHHNormalizado");
if (string.IsNullOrWhiteSpace(cadenaConexion))
    throw new InvalidOperationException(
        "Falta la cadena de conexion 'RRHHNormalizado'. Configurela en appsettings.Development.json, " +
        "en la variable de entorno ConnectionStrings__RRHHNormalizado o como Connection string de la Web App en Azure.");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Seguridad/Login";
        options.AccessDeniedPath = "/Seguridad/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        // SC-009: cookie de sesión no accesible desde JavaScript, solo por HTTPS en producción y sin envío cruzado.
        options.Cookie.Name = "EvoRRHH.Sesion";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
    });

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "EvoRRHH.Antiforgery";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdministradores", p => p.RequireRole("Administrador"));
});

// SC-006: cada capa registra sus propios servicios. Capa Datos es la unica que conoce EF Core.
builder.Services.AgregarCapaDatos(cadenaConexion);
builder.Services.AgregarCapaLogica();
builder.Services.AddScoped<Proyecto_Evo_RRLL.Services.EmpleadoServicio>();
builder.Services.AddScoped<Proyecto_Evo_RRLL.Services.EmpleadoFichaServicio>();

// SC-009: bloqueo de cuenta por intentos fallidos (los intentos se registran en la bitácora).
builder.Services.AddSingleton(TimeProvider.System);

// Limite de peticiones: evita que un usuario (o una IP) sature el servidor con demasiadas
// peticiones seguidas. Global: 100 paginas por minuto por usuario/IP; login: 10 por minuto por IP.
// SC-009: los archivos estaticos (css, js, imagenes) no cuentan, porque cada pagina carga varios y
// con 100 por minuto un usuario normal recibia 429 al navegar unas 10 paginas.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(contexto =>
    {
        if (Path.HasExtension(contexto.Request.Path.Value))
            return RateLimitPartition.GetNoLimiter("estaticos");

        var clave = contexto.User.Identity?.IsAuthenticated == true
            ? "u:" + contexto.User.Identity!.Name
            : "ip:" + (contexto.Connection.RemoteIpAddress?.ToString() ?? "anon");

        return RateLimitPartition.GetFixedWindowLimiter(clave, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 100,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        });
    });

    // SC-009: el limite de login es por IP (antes era uno solo para todos los usuarios a la vez).
    // El bloqueo de cuenta (BloqueoCuentaLogica) protege ademas cada usuario aunque el atacante cambie de IP.
    options.AddPolicy("login", contexto => RateLimitPartition.GetFixedWindowLimiter(
        "login:" + (contexto.Connection.RemoteIpAddress?.ToString() ?? "anon"),
        _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        }));
});

var app = builder.Build();

// Seed: usuario administrador inicial en BD existente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EvoRRLDbContext>();
    // SC-009: la clave inicial solo viene de la configuracion, nunca del codigo.
    // Desarrollo: dotnet user-secrets set "Seguridad:AdminClave" "<clave>"; Azure: App setting Seguridad__AdminClave.
    var adminClave = builder.Configuration["Seguridad:AdminClave"];
    if (!string.IsNullOrWhiteSpace(adminClave))
        await SeedAdminAsync(db, adminClave);
    else
        app.Logger.LogWarning("Seguridad:AdminClave no esta configurada: no se crea el usuario admin inicial.");
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// SC-009: cabeceras de seguridad en todas las respuestas.
// 'unsafe-inline' en scripts sigue siendo necesario por los manejadores onclick/onsubmit de las vistas.
const string politicaContenido =
    "default-src 'self'; " +
    "script-src 'self' 'unsafe-inline'; " +
    "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
    "font-src 'self' https://fonts.gstatic.com data:; " +
    "img-src 'self' data: blob:; " +
    "connect-src 'self'; " +
    "object-src 'none'; base-uri 'self'; form-action 'self'; frame-ancestors 'none'";
app.Use(async (contexto, siguiente) =>
{
    var h = contexto.Response.Headers;
    h["Content-Security-Policy"] = politicaContenido;
    h["X-Frame-Options"] = "DENY";
    h["X-Content-Type-Options"] = "nosniff";
    h["Referrer-Policy"] = "strict-origin-when-cross-origin";
    h["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
    h.Remove("X-Powered-By");
    await siguiente();
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseStatusCodePagesWithReExecute("/Home/PaginaNoEncontrada", "?statusCode={0}");

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

static async Task SeedAdminAsync(EvoRRLDbContext db, string adminClave)
{
    const string LoginAdmin = "admin";
    if (await db.Set<Usuario>().AnyAsync(u => u.Login != null && u.Login.Trim() == LoginAdmin))
        return;

    var hasher = new PasswordHasher<Usuario>();
    var admin = new Usuario
    {
        Login = LoginAdmin,
        Descripcion = "Administrador del Sistema",
        Fecha = DateTime.Now,
        Estado = true,
        PasswordHash = hasher.HashPassword(new Usuario(), adminClave)
    };

    var rol = await db.Set<Rol>().FirstOrDefaultAsync(r => r.Descripcion != null && r.Descripcion.Trim().Equals("Administrador"));
    if (rol is null)
    {
        rol = new Rol { Descripcion = "Administrador", IdSistema = "GRLL" };
        db.Set<Rol>().Add(rol);
    }

    db.Set<Usuario>().Add(admin);
    await db.SaveChangesAsync();
    db.Set<Usuario_Rol>().Add(new Usuario_Rol { IdUsuario = admin.IdUsuario, IdRol = rol.IdRol });
    await db.SaveChangesAsync();
}
