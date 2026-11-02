using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("AFP")]
    public class AFP
    {
    [Key]
    public int IdAFP { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripAFP { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DireccAFP { get; set; }
    [StringLength(12, ErrorMessage = "Máximo {1} caracteres.")]
    public string? TelefAFP { get; set; }
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NomContAFP { get; set; }
    }
}
