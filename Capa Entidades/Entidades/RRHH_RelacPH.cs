using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_RelacPH")]
    public class RRHH_RelacPH
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int idRelacPH { get; set; }
    [StringLength(14, ErrorMessage = "Máximo {1} caracteres.")]
    public string? RelacPHDescrip { get; set; }
    }
}
