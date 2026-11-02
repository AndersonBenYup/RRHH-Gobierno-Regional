using System.Reflection;
using Capa_Datos;
using Capa_Entidades;
using Capa_Entidades.Contratos;
using Microsoft.Extensions.DependencyInjection;

namespace Capa_Logica.Tests.Arquitectura;

/// <summary>
/// SC-006: impide que una capa vuelva a depender de la base de datos.
/// Si alguien agrega una referencia prohibida, estas pruebas fallan en el pipeline.
/// </summary>
public class ArquitecturaTests
{
    private static readonly Assembly Entidades = typeof(Persona).Assembly;
    private static readonly Assembly Logica = typeof(LogicaBase<>).Assembly;
    private static readonly Assembly Datos = typeof(EvoRRLDbContext).Assembly;

    private static string[] Referencias(Assembly a) => a.GetReferencedAssemblies().Select(r => r.Name!).ToArray();

    [Fact]
    public void Entidades_no_depende_de_ninguna_capa_ni_de_EF()
    {
        var refs = Referencias(Entidades);
        Assert.DoesNotContain(refs, r => r.StartsWith("Capa", StringComparison.Ordinal));
        Assert.DoesNotContain(refs, r => r.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal));
    }

    [Fact]
    public void Logica_no_depende_de_Datos_ni_de_EF()
    {
        var refs = Referencias(Logica);
        Assert.DoesNotContain(Datos.GetName().Name, refs);
        Assert.DoesNotContain(refs, r => r.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal));
    }

    [Fact]
    public void Los_contratos_viven_en_Capa_Entidades()
    {
        Assert.Equal(Entidades, typeof(IRepositorio<>).Assembly);
        Assert.Equal(Entidades, typeof(IPermisoRepositorio).Assembly);
    }

    [Fact]
    public void Cada_entidad_con_tabla_se_puede_resolver_como_IRepositorio()
    {
        using var proveedor = Proveedor();
        using var alcance = proveedor.CreateScope();
        var entidades = Entidades.GetTypes().Where(t => t.Namespace == "Capa_Entidades" && t.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>() is not null);
        foreach (var entidad in entidades)
            Assert.NotNull(alcance.ServiceProvider.GetService(typeof(IRepositorio<>).MakeGenericType(entidad)));
    }

    [Fact]
    public void Todas_las_clases_Logica_se_pueden_crear_desde_el_contenedor()
    {
        using var proveedor = Proveedor();
        using var alcance = proveedor.CreateScope();
        var logicas = Logica.GetTypes().Where(t => t is { IsClass: true, IsAbstract: false, IsPublic: true } && t.Name.EndsWith("Logica")).ToList();
        Assert.True(logicas.Count > 70, $"Se esperaban más de 70 clases Logica y hay {logicas.Count}");
        foreach (var tipo in logicas)
            Assert.NotNull(alcance.ServiceProvider.GetRequiredService(tipo));
    }

    [Fact]
    public void Los_controllers_no_usan_tipos_de_Capa_Datos()
    {
        var raiz = BuscarRaiz();
        var archivos = Directory.GetFiles(Path.Combine(raiz, "Proyecto-Evo-RRLL", "Controllers"), "*.cs")
            .Concat(Directory.GetFiles(Path.Combine(raiz, "Proyecto-Evo-RRLL", "Services"), "*.cs"))
            .Concat(Directory.GetFiles(Path.Combine(raiz, "Proyecto-Evo-RRLL", "Views"), "*.cshtml", SearchOption.AllDirectories));
        var infractores = archivos.Where(f => File.ReadAllText(f).Contains("Capa_Datos")).Select(Path.GetFileName).ToList();
        Assert.True(infractores.Count == 0, "Usan Capa_Datos: " + string.Join(", ", infractores));
    }

    private static ServiceProvider Proveedor()
    {
        var servicios = new ServiceCollection();
        servicios.AddSingleton(TimeProvider.System);
        servicios.AgregarCapaDatos("Server=.;Database=NoSeUsa;Trusted_Connection=True");
        servicios.AgregarCapaLogica();
        return servicios.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
    }

    private static string BuscarRaiz()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "Proyecto-Evo-RRLL.slnx")))
            dir = dir.Parent;
        return dir?.FullName ?? throw new InvalidOperationException("No se encontró la raíz de la solución.");
    }
}
