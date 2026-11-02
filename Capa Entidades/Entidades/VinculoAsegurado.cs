using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("VinculoAsegurado")]
    public class VinculoAsegurado
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdVinculoAsegurado { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    }
}
