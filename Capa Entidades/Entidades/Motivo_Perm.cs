using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Motivo_Perm")]
    public class Motivo_Perm
    {
    [Key]
    public int idMotivo { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descrip_Motivo { get; set; }
    public bool? Salario { get; set; }
    public int? idTipoPermiso { get; set; }
    [StringLength(3, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Abrev { get; set; }
    [StringLength(1, ErrorMessage = "Máximo {1} caracteres.")]
    public string? TipoModalidad { get; set; }
    public bool? Activo { get; set; }
    }
}
