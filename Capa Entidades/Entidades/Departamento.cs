using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Departamento")]
    public class Departamento
    {
    [Key]
    [StringLength(2, ErrorMessage = "Máximo {1} caracteres.")]
    public string CodDepartamento { get; set; } = "";
    [StringLength(120, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Nombre { get; set; }
    }
}
