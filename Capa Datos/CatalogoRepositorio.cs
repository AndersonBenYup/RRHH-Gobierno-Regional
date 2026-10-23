using System.Globalization;
using System.Reflection;
using Capa_Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Capa_Datos;

// EsAutonumerico: clave IDENTITY, la genera SQL Server al crear.
public record CatalogoColumna(string Nombre, string Tipo, bool EsClave, bool EsArchivo, bool EsAutonumerico = false);

public class CatalogoRepositorio
{
    private static readonly string[] Editable =
    {
        "Cargo", "MotivoBaja", "EstadoCivil", "TipoDocID", "TipoTrabajador",
        "TipoSangre", "Nacionalidad", "RegimenPension", "ViveCon", "TipoPermiso",
        "Motivo_Perm", "TipoEstudio", "TipoInstitucion", "Discapacidad",
        "TipoComportamiento", "TipoResolucion",
        "TipoFamiliar", "RRHH_TipoFamilia", "RRHH_RelacPareja", "RRHH_RelacPH",
        "RRHH_RelacHermano", "AFP", "AguaViv", "AlumbradoViv", "ConservacionViv",
        "ExcretasViv", "MaterialViv", "TenenciaViv", "TipoViv", "UbicacionViv",
        "RRHH_AcudeEnferm", "Horario", "VinculoAsegurado", "ServicioViv"
    };

    private readonly EvoRRLDbContext _contexto;

    public CatalogoRepositorio(EvoRRLDbContext contexto) => _contexto = contexto;

    public static bool EsEditable(string tabla) => Editable.Contains(tabla);

    public static IEnumerable<string> CatalogoEditable() => Editable;

    public Type ObtenerTipo(string tabla)
    {
        var tipo = typeof(Empleado).Assembly.GetType($"Capa_Entidades.{tabla}")
            ?? throw new InvalidOperationException($"No existe la entidad {tabla}");
        if (_contexto.Model.FindEntityType(tipo) is null)
            throw new InvalidOperationException($"La entidad {tabla} no esta mapeada");
        return tipo;
    }

    public async Task<List<object>> Listar(string tabla)
    {
        var tipo = ObtenerTipo(tabla);
        return await ListarEntidadesAsync(tipo);
    }

    public async Task<object?> Obtener(string tabla, string[] ids)
    {
        var tipo = ObtenerTipo(tabla);
        var valores = ConvertirClaves(tipo, ids);
        return await ObtenerEntidadAsync(tipo, valores);
    }

    public IReadOnlyList<CatalogoColumna> Columnas(string tabla)
    {
        var tipo = ObtenerTipo(tabla);
        var entityType = _contexto.Model.FindEntityType(tipo)!;
        var claves = entityType.FindPrimaryKey()!.Properties.Select(p => p.Name).ToHashSet();
        return entityType.GetProperties()
            .Where(p => p.GetColumnName() is not null)
            .Select(p =>
            {
                var tipoClase = Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType;
                return new CatalogoColumna(
                    p.Name,
                    tipoClase.Name,
                    claves.Contains(p.Name),
                    tipoClase == typeof(byte[]),
                    EsAutonumerico(p));
            })
            .ToList();
    }

    public async Task Crear(string tabla, IReadOnlyDictionary<string, string> datos)
    {
        if (!EsEditable(tabla))
            throw new InvalidOperationException($"El catálogo {tabla} no es editable.");

        var tipo = ObtenerTipo(tabla);
        var entidad = Activator.CreateInstance(tipo)!;
        AplicarValores(tipo, entidad, datos, omitirAutonumerico: true);
        _contexto.Add(entidad);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(string tabla, string[] ids, IReadOnlyDictionary<string, string> datos)
    {
        if (!EsEditable(tabla))
            throw new InvalidOperationException($"El catálogo {tabla} no es editable.");

        var tipo = ObtenerTipo(tabla);
        var actual = await ObtenerEntidadAsync(tipo, ConvertirClaves(tipo, ids))
            ?? throw new InvalidOperationException("Registro no encontrado");
        AplicarValores(tipo, actual, datos);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(string tabla, string[] ids)
    {
        if (!EsEditable(tabla))
            throw new InvalidOperationException($"El catálogo {tabla} no es editable.");

        var tipo = ObtenerTipo(tabla);
        var actual = await ObtenerEntidadAsync(tipo, ConvertirClaves(tipo, ids))
            ?? throw new InvalidOperationException("Registro no encontrado");
        _contexto.Remove(actual);
        await _contexto.SaveChangesAsync();
    }

    public string[] ClavesPrimarias(string tabla)
    {
        var tipo = ObtenerTipo(tabla);
        return _contexto.Model.FindEntityType(tipo)!
            .FindPrimaryKey()!.Properties.Select(p => p.Name).ToArray();
    }

    private async Task<List<object>> ListarEntidadesAsync(Type tipo)
    {
        var metodo = GetType().GetMethod(nameof(ListarTodos), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(tipo);
        return await (Task<List<object>>)metodo.Invoke(this, null)!;
    }

    private async Task<object?> ObtenerEntidadAsync(Type tipo, object?[] valores)
    {
        var metodo = GetType().GetMethod(nameof(ObtenerPorClave), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(tipo);
        var task = (Task<object?>)metodo.Invoke(this, new object?[] { valores })!;
        return await task.ConfigureAwait(false);
    }

    private async Task<List<object>> ListarTodos<T>() where T : class
        => (await _contexto.Set<T>().AsNoTracking().ToListAsync()).Cast<object>().ToList();

    private async Task<object?> ObtenerPorClave<T>(object?[] valores) where T : class
        => await _contexto.Set<T>().FindAsync(valores);

    private object?[] ConvertirClaves(Type tipo, string[] ids)
    {
        var entityType = _contexto.Model.FindEntityType(tipo)!;
        var propiedades = entityType.FindPrimaryKey()!.Properties
            .Select(p => p.PropertyInfo!).ToList();
        var valores = new object?[propiedades.Count];
        for (var i = 0; i < propiedades.Count; i++)
            valores[i] = Convertir(propiedades[i].PropertyType, ids[i]);
        return valores;
    }

    private void AplicarValores(Type tipo, object entidad, IReadOnlyDictionary<string, string> datos, bool omitirAutonumerico = false)
    {
        var entityType = _contexto.Model.FindEntityType(tipo)!;
        foreach (var propiedad in entityType.GetProperties())
        {
            var columna = propiedad.GetColumnName();
            if (columna is null) continue;
            if (omitirAutonumerico && EsAutonumerico(propiedad)) continue;
            if (!datos.TryGetValue(propiedad.Name, out var texto)) continue;
            if (propiedad.ClrType == typeof(byte[])) continue;

            var info = propiedad.PropertyInfo;
            if (info is null) continue;

            if (propiedad.ClrType == typeof(string))
            {
                info.SetValue(entidad, texto);
            }
            else if (string.IsNullOrWhiteSpace(texto))
            {
                info.SetValue(entidad, null);
            }
            else
            {
                info.SetValue(entidad, Convertir(propiedad.ClrType, texto));
            }
        }
    }

    private static bool EsAutonumerico(IReadOnlyProperty propiedad)
        => propiedad.IsPrimaryKey() && propiedad.ValueGenerated == ValueGenerated.OnAdd;

    private static object? Convertir(Type tipo, string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return null;
        var destino = Nullable.GetUnderlyingType(tipo) ?? tipo;
        if (destino == typeof(TimeOnly)) return TimeOnly.Parse(texto, CultureInfo.InvariantCulture);
        if (destino == typeof(DateOnly)) return DateOnly.Parse(texto, CultureInfo.InvariantCulture);
        if (destino == typeof(bool)) return texto.Split(',')[0].Equals("true", StringComparison.OrdinalIgnoreCase);
        return Convert.ChangeType(texto, destino, CultureInfo.InvariantCulture);
    }
}