using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_AcudeEnferm")]
    public class RRHH_AcudeEnferm
    {
    [Key]
    public int idAcudeEnferm { get; set; }
    [StringLength(16, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? AcudeEnfermDescrip { get; set; }
    }
}
