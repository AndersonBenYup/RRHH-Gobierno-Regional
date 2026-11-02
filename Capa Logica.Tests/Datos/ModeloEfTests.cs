using Capa_Datos;
using Capa_Entidades;
using Microsoft.EntityFrameworkCore;

namespace Capa_Logica.Tests.Datos;

/// <summary>
/// SC-008: los [Required] de validación no deben volver NOT NULL las columnas en el modelo de EF,
/// porque las filas antiguas con NULL harían fallar las consultas (SqlNullValueException).
/// </summary>
public class ModeloEfTests
{
    private static EvoRRLDbContext Contexto()
        => new(new DbContextOptionsBuilder<EvoRRLDbContext>()
            .UseSqlServer("Server=.;Database=NoSeUsa;Trusted_Connection=True")
            .Options);

    [Theory]
    [InlineData(typeof(Resolucion), nameof(Resolucion.NumeroResolucion))]
    [InlineData(typeof(Permiso), nameof(Permiso.idMotivo))]
    [InlineData(typeof(Permiso), nameof(Permiso.FechaInicio))]
    [InlineData(typeof(Capacitacion), nameof(Capacitacion.NombreCurso))]
    [InlineData(typeof(RRHH_Feriado), nameof(RRHH_Feriado.Motivo))]
    public void Propiedades_anulables_con_Required_siguen_aceptando_null(Type entidad, string propiedad)
    {
        using var db = Contexto();
        var p = db.Model.FindEntityType(entidad)!.FindProperty(propiedad)!;
        Assert.True(p.IsNullable, $"{entidad.Name}.{propiedad} no debe ser NOT NULL en EF");
    }

    [Fact]
    public void Las_claves_siguen_siendo_obligatorias()
    {
        using var db = Contexto();
        Assert.False(db.Model.FindEntityType(typeof(Permiso))!.FindProperty(nameof(Permiso.IdEmpleado))!.IsNullable);
    }
}
