using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Cargo")]
    public class Cargo
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdCargo { get; set; }
    [Column("Cargo")]
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    [StringLength(8, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Abreviatura { get; set; }
    }
}
