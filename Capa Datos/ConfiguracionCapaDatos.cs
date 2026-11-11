using Capa_Datos.Repositorios;
using Capa_Entidades;
using Capa_Entidades.Contratos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Capa_Datos;

/// <summary>
/// SC-006: único punto que conoce EF Core y SQL Server. Para cambiar de base de datos basta con
/// cambiar esta clase y las implementaciones de Capa Datos; Lógica y Web solo usan los contratos.
/// </summary>
public static class ConfiguracionCapaDatos
{
    public static IServiceCollection AgregarCapaDatos(this IServiceCollection servicios, string cadenaConexion)
    {
        servicios.AddDbContext<EvoRRLDbContext>(opciones => opciones.UseSqlServer(cadenaConexion));

        // SC-005: caché en memoria de los catálogos pequeños (ver CacheCatalogos).
        servicios.AddMemoryCache();
        servicios.AddSingleton<CacheCatalogos>();

        // CRUD genérico para todas las entidades.
        servicios.AddScoped(typeof(IRepositorio<>), typeof(RepositorioBase<>));

        // Repositorios con consultas propias (también responden como IRepositorio<T> de su entidad).
        AgregarEspecifico<IPermisoRepositorio, PermisoRepositorio, Permiso>(servicios);
        AgregarEspecifico<IPersonaRepositorio, PersonaRepositorio, Persona>(servicios);
        AgregarEspecifico<IUsuarioRepositorio, UsuarioRepositorio, Usuario>(servicios);
        AgregarEspecifico<IAsistenciaRepositorio, AsistenciaRepositorio, Asistencia>(servicios);
        AgregarEspecifico<IMarcacionRepositorio, MarcacionRepositorio, Marcacion>(servicios);
        AgregarEspecifico<IRegAsisDiarioRepositorio, RegAsisDiarioRepositorio, RegAsisDiario>(servicios);
        AgregarEspecifico<IPeriodoTeletrabajoRepositorio, PeriodoTeletrabajoRepositorio, PeriodoTeletrabajo>(servicios);

        servicios.AddScoped<IBitacoraRepositorio, BitacoraRepositorio>();
        servicios.AddScoped<ISecuenciaService, SecuenciaService>();
        servicios.AddScoped<ICatalogoRepositorio, CatalogoRepositorio>();
        return servicios;
    }

    private static void AgregarEspecifico<TContrato, TImplementacion, TEntidad>(IServiceCollection servicios)
        where TContrato : class, IRepositorio<TEntidad>
        where TImplementacion : class, TContrato
        where TEntidad : class
    {
        servicios.AddScoped<TImplementacion>();
        servicios.AddScoped<TContrato>(sp => sp.GetRequiredService<TImplementacion>());
        servicios.AddScoped<IRepositorio<TEntidad>>(sp => sp.GetRequiredService<TImplementacion>());
    }
}
