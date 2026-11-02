using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Rol_Acceso")]
    public class Rol_Acceso
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdRol { get; set; }
    [Key]
    [StringLength(7, ErrorMessage = "Máximo {1} caracteres.")]
    public string? IdSistemaOpcion { get; set; }
    }
}
