using Capa_Datos;
using Capa_Entidades;
using Microsoft.Extensions.Caching.Memory;

namespace Capa_Logica.Tests.Datos;

/// <summary>SC-005: los catálogos se leen de memoria y se recargan cuando cambian.</summary>
public class CacheCatalogosTests
{
    private static CacheCatalogos Nuevo() => new(new MemoryCache(new MemoryCacheOptions()));

    [Fact]
    public async Task La_segunda_lectura_no_vuelve_a_la_base_de_datos()
    {
        var cache = Nuevo();
        var cargas = 0;
        Task<List<Cargo>> Cargar() { cargas++; return Task.FromResult(new List<Cargo> { new() { IdCargo = 1 } }); }

        await cache.ObtenerAsync(Cargar);
        await cache.ObtenerAsync(Cargar);

        Assert.Equal(1, cargas);
    }

    [Fact]
    public async Task Al_invalidar_se_vuelve_a_cargar()
    {
        var cache = Nuevo();
        var cargas = 0;
        Task<List<Cargo>> Cargar() { cargas++; return Task.FromResult(new List<Cargo>()); }

        await cache.ObtenerAsync(Cargar);
        cache.Invalidar(typeof(Cargo));
        await cache.ObtenerAsync(Cargar);

        Assert.Equal(2, cargas);
    }

    [Fact]
    public async Task Cada_lectura_devuelve_una_lista_nueva()
    {
        var cache = Nuevo();
        var primera = await cache.ObtenerAsync(() => Task.FromResult(new List<Cargo> { new() { IdCargo = 1 } }));
        primera.Clear();

        var segunda = await cache.ObtenerAsync(() => Task.FromResult(new List<Cargo>()));

        Assert.Single(segunda);
    }

    [Theory]
    [InlineData(typeof(Cargo), true)]
    [InlineData(typeof(AFP), true)]
    [InlineData(typeof(EstructOrganiz), true)]
    [InlineData(typeof(Empleado), false)]
    [InlineData(typeof(Marcacion), false)]
    [InlineData(typeof(Permiso), false)]
    public void Solo_los_catalogos_pequenos_usan_cache(Type tipo, bool esperado)
        => Assert.Equal(esperado, CacheCatalogos.EsCatalogo(tipo));
}
