using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Provincia")]
    public class Provincia
    {
    [Key]
    [StringLength(2, ErrorMessage = "Máximo {1} caracteres.")]
    public string CodDepartamento { get; set; } = "";
    [Key]
    [StringLength(2, ErrorMessage = "Máximo {1} caracteres.")]
    public string CodProvincia { get; set; } = "";
    [StringLength(120, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Nombre { get; set; }
    }
}
