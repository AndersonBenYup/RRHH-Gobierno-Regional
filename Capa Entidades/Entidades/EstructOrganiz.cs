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
    public string? AreaOrganizacional { get; set; }
    public string? Abrev { get; set; }
    public string? Sigla { get; set; }
    public string? Mision { get; set; }
    public string? Vision { get; set; }
    public int? YearDe { get; set; }
    public int? idDependeDe { get; set; }
    public int? IdEmpleado { get; set; }
    }
}
