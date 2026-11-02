using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("EstructOrganiz")]
    public class EstructOrganiz
    {
    [Key]
    public int Year { get; set; }
    [Key]
    public int idAreaOrganiz { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? AreaOrganizacional { get; set; }
    [StringLength(10, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Abrev { get; set; }
    [StringLength(6, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Sigla { get; set; }
    [StringLength(1000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Mision { get; set; }
    [StringLength(1000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Vision { get; set; }
    public int? YearDe { get; set; }
    public int? idDependeDe { get; set; }
    public int? IdEmpleado { get; set; }
    }
}
