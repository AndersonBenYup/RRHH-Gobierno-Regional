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

builder.Services.AddDbContext<EvoRRLDbContext>(options => options.UseSqlServer(cadenaConexion));

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

// Capa Datos (76)
builder.Services.AddScoped<AFPData>();
builder.Services.AddScoped<AguaVivData>();
builder.Services.AddScoped<BitacoraData>();
builder.Services.AddScoped<AlumbradoVivData>();
builder.Services.AddScoped<AsistenciaData>();
builder.Services.AddScoped<CapacitacionData>();
builder.Services.AddScoped<CargoData>();
builder.Services.AddScoped<ComportamientoData>();
builder.Services.AddScoped<ConservacionVivData>();
builder.Services.AddScoped<DiscapacidadData>();
builder.Services.AddScoped<EmpleadoData>();
builder.Services.AddScoped<Empleado_AreaData>();
builder.Services.AddScoped<EstadoCivilData>();
builder.Services.AddScoped<EstadoEstudioData>();
builder.Services.AddScoped<EstructOrganizData>();
builder.Services.AddScoped<EstudiosRealizadoData>();
builder.Services.AddScoped<ExcretasVivData>();
builder.Services.AddScoped<ExpLaboralData>();
builder.Services.AddScoped<FamiliarData>();
builder.Services.AddScoped<HorarioData>();
builder.Services.AddScoped<HorarioTemporalData>();
builder.Services.AddScoped<LocalData>();
builder.Services.AddScoped<MarcacionData>();
builder.Services.AddScoped<MaterialVivData>();
builder.Services.AddScoped<MotivoBajaData>();
builder.Services.AddScoped<Motivo_PermData>();
builder.Services.AddScoped<NacionalidadData>();
builder.Services.AddScoped<PeriodoLaboralData>();
builder.Services.AddScoped<PermisoData>();
builder.Services.AddScoped<PersonaData>();
builder.Services.AddScoped<RegAsisDiarioData>();
builder.Services.AddScoped<RegimenPensionData>();
builder.Services.AddScoped<ResolucionData>();
builder.Services.AddScoped<ResponsableXUOData>();
builder.Services.AddScoped<RolData>();
builder.Services.AddScoped<Rol_AccesoData>();
builder.Services.AddScoped<RotacionData>();
builder.Services.AddScoped<RRHH_AcudeEnfermData>();
builder.Services.AddScoped<RRHH_AseguradoData>();
builder.Services.AddScoped<RRHH_AspSocioData>();
builder.Services.AddScoped<RRHH_DinamicaFamiliarData>();
builder.Services.AddScoped<RRHH_FeriadoData>();
builder.Services.AddScoped<RRHH_FuncFamData>();
builder.Services.AddScoped<RRHH_RelacHermanoData>();
builder.Services.AddScoped<RRHH_RelacParejaData>();
builder.Services.AddScoped<RRHH_RelacPHData>();
builder.Services.AddScoped<RRHH_SaludData>();
builder.Services.AddScoped<RRHH_TipoFamiliaData>();
builder.Services.AddScoped<SistemaOpcionData>();
builder.Services.AddScoped<TenenciaVivData>();
builder.Services.AddScoped<TipoComportamientoData>();
builder.Services.AddScoped<TipoDocIDData>();
builder.Services.AddScoped<TipoEstudioData>();
builder.Services.AddScoped<TipoFamiliarData>();
builder.Services.AddScoped<TipoInstitucionData>();
builder.Services.AddScoped<TipoPermisoData>();
builder.Services.AddScoped<TipoResolucionData>();
builder.Services.AddScoped<TipoSangreData>();
builder.Services.AddScoped<TipoTrabajadorData>();
builder.Services.AddScoped<TipoVivData>();
builder.Services.AddScoped<TitulosEmpleadoData>();
builder.Services.AddScoped<UbicacionVivData>();
builder.Services.AddScoped<UbigeoData>();
builder.Services.AddScoped<UsuarioData>();
builder.Services.AddScoped<Usuario_RolData>();
builder.Services.AddScoped<ViveConData>();
builder.Services.AddScoped<ViviendaData>();
builder.Services.AddScoped<DepartamentoData>();
builder.Services.AddScoped<ProvinciaData>();
builder.Services.AddScoped<DistritoData>();
builder.Services.AddScoped<ServicioVivData>();
builder.Services.AddScoped<ViviendaServicioData>();
builder.Services.AddScoped<VinculoAseguradoData>();
builder.Services.AddScoped<RRHH_FuncFamPreguntaData>();
builder.Services.AddScoped<RRHH_FuncFamRespuestaData>();
builder.Services.AddScoped<EstadoAsistenciaData>();
builder.Services.AddScoped<TipoMarcacionData>();
builder.Services.AddScoped<DispositivoMarcacionData>();
builder.Services.AddScoped<EmpleadoAreaActualData>();
builder.Services.AddScoped<EmpleadoResumenFamiliarData>();
builder.Services.AddScoped<FamiliarDetalleData>();
builder.Services.AddScoped<EstructOrganizDotacionData>();

// Capa Logica (77)
builder.Services.AddScoped<AFPLogica>();
builder.Services.AddScoped<AguaVivLogica>();
builder.Services.AddScoped<AlumbradoVivLogica>();
builder.Services.AddScoped<AsistenciaLogica>();
builder.Services.AddScoped<BitacoraLogica>();
builder.Services.AddScoped<CapacitacionLogica>();
builder.Services.AddScoped<CargoLogica>();
builder.Services.AddScoped<ComportamientoLogica>();
builder.Services.AddScoped<ConservacionVivLogica>();
builder.Services.AddScoped<DiscapacidadLogica>();
builder.Services.AddScoped<Empleado_AreaLogica>();
builder.Services.AddScoped<EmpleadoLogica>();
builder.Services.AddScoped<EstadoCivilLogica>();
builder.Services.AddScoped<EstadoEstudioLogica>();
builder.Services.AddScoped<EstructOrganizLogica>();
builder.Services.AddScoped<EstudiosRealizadoLogica>();
builder.Services.AddScoped<ExcretasVivLogica>();
builder.Services.AddScoped<ExpLaboralLogica>();
builder.Services.AddScoped<FamiliarLogica>();
builder.Services.AddScoped<HorarioLogica>();
builder.Services.AddScoped<HorarioTemporalLogica>();
builder.Services.AddScoped<LocalLogica>();
builder.Services.AddScoped<MarcacionLogica>();
builder.Services.AddScoped<MaterialVivLogica>();
builder.Services.AddScoped<Motivo_PermLogica>();
builder.Services.AddScoped<MotivoBajaLogica>();
builder.Services.AddScoped<NacionalidadLogica>();
builder.Services.AddScoped<PeriodoLaboralLogica>();
builder.Services.AddScoped<PermisoLogica>();
builder.Services.AddScoped<PersonaLogica>();
builder.Services.AddScoped<RegAsisDiarioLogica>();
builder.Services.AddScoped<RegimenPensionLogica>();
builder.Services.AddScoped<ResolucionLogica>();
builder.Services.AddScoped<ResponsableXUOLogica>();
builder.Services.AddScoped<Rol_AccesoLogica>();
builder.Services.AddScoped<RolLogica>();
builder.Services.AddScoped<RotacionLogica>();
builder.Services.AddScoped<RRHH_AcudeEnfermLogica>();
builder.Services.AddScoped<RRHH_AseguradoLogica>();
builder.Services.AddScoped<RRHH_AspSocioLogica>();
builder.Services.AddScoped<RRHH_DinamicaFamiliarLogica>();
builder.Services.AddScoped<RRHH_FeriadoLogica>();
builder.Services.AddScoped<RRHH_FuncFamLogica>();
builder.Services.AddScoped<RRHH_RelacHermanoLogica>();
builder.Services.AddScoped<RRHH_RelacParejaLogica>();
builder.Services.AddScoped<RRHH_RelacPHLogica>();
builder.Services.AddScoped<RRHH_SaludLogica>();
builder.Services.AddScoped<RRHH_TipoFamiliaLogica>();
builder.Services.AddScoped<SistemaOpcionLogica>();
builder.Services.AddScoped<TenenciaVivLogica>();
builder.Services.AddScoped<TipoComportamientoLogica>();
builder.Services.AddScoped<TipoDocIDLogica>();
builder.Services.AddScoped<TipoEstudioLogica>();
builder.Services.AddScoped<TipoFamiliarLogica>();
builder.Services.AddScoped<TipoInstitucionLogica>();
builder.Services.AddScoped<TipoPermisoLogica>();
builder.Services.AddScoped<TipoResolucionLogica>();
builder.Services.AddScoped<TipoSangreLogica>();
builder.Services.AddScoped<TipoTrabajadorLogica>();
builder.Services.AddScoped<TipoVivLogica>();
builder.Services.AddScoped<TitulosEmpleadoLogica>();
builder.Services.AddScoped<UbicacionVivLogica>();
builder.Services.AddScoped<UbigeoLogica>();
builder.Services.AddScoped<Usuario_RolLogica>();
builder.Services.AddScoped<UsuarioLogica>();
builder.Services.AddScoped<ViveConLogica>();
builder.Services.AddScoped<ViviendaLogica>();
builder.Services.AddScoped<DepartamentoLogica>();
builder.Services.AddScoped<ProvinciaLogica>();
builder.Services.AddScoped<DistritoLogica>();
builder.Services.AddScoped<ServicioVivLogica>();
builder.Services.AddScoped<ViviendaServicioLogica>();
builder.Services.AddScoped<VinculoAseguradoLogica>();
builder.Services.AddScoped<RRHH_FuncFamPreguntaLogica>();
builder.Services.AddScoped<RRHH_FuncFamRespuestaLogica>();
builder.Services.AddScoped<EstadoAsistenciaLogica>();
builder.Services.AddScoped<TipoMarcacionLogica>();
builder.Services.AddScoped<DispositivoMarcacionLogica>();
builder.Services.AddScoped<EmpleadoAreaActualLogica>();
builder.Services.AddScoped<EmpleadoResumenFamiliarLogica>();
builder.Services.AddScoped<FamiliarDetalleLogica>();
builder.Services.AddScoped<EstructOrganizDotacionLogica>();

// Repositorio generico de catalogos
builder.Services.AddScoped<CatalogoRepositorio>();
builder.Services.AddScoped<SecuenciaService>();
builder.Services.AddScoped<Proyecto_Evo_RRLL.Services.EmpleadoServicio>();
builder.Services.AddScoped<Proyecto_Evo_RRLL.Services.EmpleadoFichaServicio>();

// SC-009: bloqueo de cuenta por intentos fallidos (los intentos se registran en la bitácora).
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<BloqueoCuentaLogica>();

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
