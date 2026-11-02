using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Usuario")]
    public class Usuario
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdUsuario { get; set; }
    [StringLength(20, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Login { get; set; }
    [StringLength(40, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    public DateTime? Fecha { get; set; }
    public bool? Estado { get; set; }
    public int? IdEmpleado { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    public string? PasswordHash { get; set; }
    }
}
