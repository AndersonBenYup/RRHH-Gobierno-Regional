using System.Text;
using Capa_Logica.Exportacion;

namespace Capa_Logica.Tests.Exportacion;

/// <summary>SC-003: exportación a CSV (diseño OCP).</summary>
public class ExportacionTests
{
    private static string Texto(byte[] contenido) => Encoding.UTF8.GetString(contenido, 3, contenido.Length - 3);

    [Fact]
    public void Csv_empieza_con_BOM_para_que_Excel_lea_las_tildes()
    {
        var bytes = new ExportadorCsv().Exportar([["Año"], ["Peña"]]);
        Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, bytes.Take(3));
        Assert.Equal("Año\r\nPeña", Texto(bytes));
    }

    [Theory]
    [InlineData("simple", "simple")]
    [InlineData("a;b", "\"a;b\"")]
    [InlineData("dijo \"hola\"", "\"dijo \"\"hola\"\"\"")]
    [InlineData("linea1\nlinea2", "\"linea1\nlinea2\"")]
    [InlineData(null, "")]
    public void Escapa_separadores_comillas_y_saltos(string? valor, string esperado)
        => Assert.Equal(esperado, ExportadorCsv.Escapar(valor));

    [Theory]
    [InlineData("=SUMA(A1)", "'=SUMA(A1)")]
    [InlineData("@cmd", "'@cmd")]
    [InlineData("-5", "-5")]
    public void Evita_la_inyeccion_de_formulas(string valor, string esperado)
        => Assert.Equal(esperado, ExportadorCsv.Escapar(valor));

    [Fact]
    public void ExportadorTablas_elige_el_formato_y_arma_el_nombre()
    {
        var tablas = new ExportadorTablas([new ExportadorCsv()]);
        var archivo = tablas.Exportar([["Id"], ["1"]], "asistencia");
        Assert.Equal("text/csv; charset=utf-8", archivo.TipoContenido);
        Assert.StartsWith("asistencia_", archivo.NombreArchivo);
        Assert.EndsWith(".csv", archivo.NombreArchivo);
        Assert.Contains("csv", tablas.Formatos);
    }

    [Fact]
    public void Formato_no_registrado_es_error()
        => Assert.Throws<ArgumentException>(() => new ExportadorTablas([new ExportadorCsv()]).Exportar([["x"]], "a", "xlsx"));

    /// <summary>OCP: un formato nuevo se agrega sin tocar ExportadorTablas.</summary>
    private sealed class ExportadorTsv : IExportadorTabla
    {
        public string Formato => "tsv";
        public string TipoContenido => "text/tab-separated-values";
        public string Extension => "tsv";
        public byte[] Exportar(IReadOnlyList<string?[]> filas)
            => Encoding.UTF8.GetBytes(string.Join("\n", filas.Select(f => string.Join("\t", f))));
    }

    [Fact]
    public void Se_puede_agregar_un_formato_sin_modificar_el_codigo_existente()
    {
        var tablas = new ExportadorTablas([new ExportadorCsv(), new ExportadorTsv()]);
        var archivo = tablas.Exportar([["a", "b"]], "x", "tsv");
        Assert.Equal("a\tb", Encoding.UTF8.GetString(archivo.Contenido));
    }
}
