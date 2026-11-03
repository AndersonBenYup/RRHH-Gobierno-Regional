using Capa_Logica.Paginacion;
using Xunit;

namespace Capa_Logica.Tests.Paginacion;

public class VentanaPaginasTests
{
    [Theory]
    [InlineData(1, 1, new[] { 1 })]
    [InlineData(3, 7, new[] { 1, 2, 3, 4, 5, 6, 7 })]
    [InlineData(1, 120, new[] { 1, 2, 3, 4, 5, 0, 120 })]
    [InlineData(4, 120, new[] { 1, 2, 3, 4, 5, 0, 120 })]
    [InlineData(5, 120, new[] { 1, 0, 4, 5, 6, 0, 120 })]
    [InlineData(53, 120, new[] { 1, 0, 52, 53, 54, 0, 120 })]
    [InlineData(117, 120, new[] { 1, 0, 116, 117, 118, 119, 120 })]
    [InlineData(120, 120, new[] { 1, 0, 116, 117, 118, 119, 120 })]
    public void Muestra_como_maximo_siete_elementos(int actual, int total, int[] esperado)
    {
        var ventana = VentanaPaginas.Calcular(actual, total);

        Assert.Equal(esperado, ventana);
        Assert.True(ventana.Count <= VentanaPaginas.MaximoElementos);
    }

    [Fact]
    public void La_pagina_actual_aparece_una_sola_vez()
    {
        for (var actual = 1; actual <= 40; actual++)
            Assert.Single(VentanaPaginas.Calcular(actual, 40), p => p == actual);
    }

    [Theory]
    [InlineData(0, 5, new[] { 1, 2, 3, 4, 5 })]
    [InlineData(99, 5, new[] { 1, 2, 3, 4, 5 })]
    public void Una_pagina_fuera_de_rango_se_ajusta(int actual, int total, int[] esperado)
        => Assert.Equal(esperado, VentanaPaginas.Calcular(actual, total));

    [Fact]
    public void Sin_paginas_no_muestra_nada() => Assert.Empty(VentanaPaginas.Calcular(1, 0));

    [Theory]
    [InlineData(50, 50)]
    [InlineData(7, 25)]
    [InlineData(null, 25)]
    [InlineData(100000, 25)]
    public void Solo_acepta_los_tamanos_del_selector(int? pedido, int esperado)
        => Assert.Equal(esperado, VentanaPaginas.NormalizarTamano(pedido, 25));
}
