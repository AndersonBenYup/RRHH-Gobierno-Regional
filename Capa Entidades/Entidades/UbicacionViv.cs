using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("UbicacionViv")]
    public class UbicacionViv
    {
    [Key]
    public int IdUbicacion { get; set; }
    [StringLength(22, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripUbicacion { get; set; }
    }
}
