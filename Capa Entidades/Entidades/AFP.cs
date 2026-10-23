using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("AFP")]
    public class AFP
    {
    [Key]
    public int IdAFP { get; set; }
    public string? DescripAFP { get; set; }
    public string? DireccAFP { get; set; }
    public string? TelefAFP { get; set; }
    public string? NomContAFP { get; set; }
    }
}
