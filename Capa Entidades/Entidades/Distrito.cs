using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Distrito")]
    public class Distrito
    {
    [Key]
    [StringLength(6, ErrorMessage = "Máximo {1} caracteres.")]
    public string IdUbigeo { get; set; } = "";
    [StringLength(2, ErrorMessage = "Máximo {1} caracteres.")]
    public string CodDepartamento { get; set; } = "";
    [StringLength(2, ErrorMessage = "Máximo {1} caracteres.")]
    public string CodProvincia { get; set; } = "";
    [StringLength(2, ErrorMessage = "Máximo {1} caracteres.")]
    public string CodDistrito { get; set; } = "";
    [StringLength(120, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Nombre { get; set; }
    }
}
