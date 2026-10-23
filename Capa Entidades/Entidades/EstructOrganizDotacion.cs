using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("vw_EstructOrganizDotacion")]
    public class EstructOrganizDotacion
    {
    [Key]
    public int Year { get; set; }
    [Key]
    public int idAreaOrganiz { get; set; }
    public string? AreaOrganizacional { get; set; }
    public int? NroTrabajaUO { get; set; }
    }
}
