using System.Linq.Expressions;
using Capa_Entidades;
using Capa_Entidades.Contratos;
using Capa_Logica.Reglas;

namespace Capa_Logica.Tests.Validacion;

/// <summary>SC-031: búsqueda de empleados por número de documento para los desplegables con buscador.</summary>
public class BusquedaEmpleadoTests
{
    [Theory]
    [InlineData("12345678", "12345678")]
    [InlineData(" 12.345-678 ", "12345678")]
    [InlineData("123", "123")]
    [InlineData("12", null)]        // muy corto: coincidiría con casi todos
    [InlineData("perez 123", null)] // tiene letras: es una búsqueda por nombre
    [InlineData("", null)]
    [InlineData(null, null)]
    public void Normaliza_el_documento(string? texto, string? esperado)
        => Assert.Equal(esperado, ReglasBusquedaEmpleado.NormalizarDocumento(texto));

    // CP-SC031-01: el documento lleva a todos los vínculos de empleado de esa persona (reingresos incluidos).
    [Fact]
    public async Task Devuelve_los_empleados_de_las_personas_cuyo_documento_contiene_los_digitos()
    {
        var logica = Crear(
            [Persona(1, "40123456"), Persona(2, "40999999"), Persona(3, null)],
            [Empleado(10, 1), Empleado(11, 1), Empleado(20, 2), Empleado(30, 3), Empleado(40, null)]);

        Assert.Equal([10, 11], (await logica.PorDocumento("0123")).Order());
        Assert.Equal([10, 11, 20], (await logica.PorDocumento("409")).Concat(await logica.PorDocumento("401")).Order());
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("12")]
    [InlineData("77777777")]
    public async Task Sin_documento_valido_o_sin_coincidencias_devuelve_vacio(string texto)
    {
        var logica = Crear([Persona(1, "40123456")], [Empleado(10, 1)]);

        Assert.Empty(await logica.PorDocumento(texto));
    }

    private static BusquedaEmpleadoLogica Crear(Persona[] personas, Empleado[] empleados)
        => new(new EnMemoria<Persona>(personas), new EnMemoria<Empleado>(empleados));

    private static Persona Persona(int id, string? documento) => new() { IdPersona = id, NumDocID = documento };

    private static Empleado Empleado(int id, int? idPersona) => new() { IdEmpleado = id, IdPersona = idPersona };

    private sealed class EnMemoria<T> : IRepositorio<T> where T : class
    {
        private readonly List<T> _datos;

        public EnMemoria(IEnumerable<T> datos) => _datos = datos.ToList();

        public Task<List<T>> Listar() => Task.FromResult(_datos.ToList());

        public Task<Pagina<T>> ListarPagina(Expression<Func<T, bool>>? filtro, Func<IQueryable<T>, IOrderedQueryable<T>> ordenar,
            int pagina, int tamanoPagina)
        {
            var consulta = _datos.AsQueryable();
            if (filtro is not null)
                consulta = consulta.Where(filtro);
            var lista = ordenar(consulta).ToList();
            return Task.FromResult(new Pagina<T>(lista.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina).ToList(), lista.Count, pagina, tamanoPagina));
        }

        public Task<T?> Obtener(params object[] claves) => throw new NotSupportedException();
        public Task Crear(T registro) => throw new NotSupportedException();
        public Task Actualizar(T registro) => throw new NotSupportedException();
        public Task Eliminar(T registro) => throw new NotSupportedException();
    }
}
