using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Rol")]
    public class Rol
    {
    [Key]
    public int IdRol { get; set; }
    [StringLength(30, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? Descripcion { get; set; }
    [StringLength(7, ErrorMessage = "Máximo {1} caracteres.")]
    public string? IdSistema { get; set; }
    }
}
