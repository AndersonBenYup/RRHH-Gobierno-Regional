using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Vivienda")]
    public class Vivienda
    {
    [Key]
    public int IdVivienda { get; set; }
    public int? IdEmpleado { get; set; }
    public int? IdTenencia { get; set; }
    public int? IdTipo { get; set; }
    public int? IdMaterial { get; set; }
    public int? IdConservacion { get; set; }
    public int? IdUbicacion { get; set; }
    public int? IdAlumbrado { get; set; }
    public string? OtroAlumbrado { get; set; }
    public int? IdAgua { get; set; }
    public string? OtroAgua { get; set; }
    public int? IdExcretas { get; set; }
    public string? OtroExcreta { get; set; }
    public byte? NumDormitorio { get; set; }
    public byte? NumServHig { get; set; }
    public byte? NumCompart { get; set; }
    public byte? NumPerDorm { get; set; }
    public string? Observacion { get; set; }
    }
}
