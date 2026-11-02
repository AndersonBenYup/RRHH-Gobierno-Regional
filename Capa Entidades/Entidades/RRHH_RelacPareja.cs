using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_RelacPareja")]
    public class RRHH_RelacPareja
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int idRelacPareja { get; set; }
    [StringLength(12, ErrorMessage = "Máximo {1} caracteres.")]
    public string? RelacParejaDescrip { get; set; }
    }
}
