using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ResponsableXUO")]
    public class ResponsableXUO
    {
    [Key]
    public int IdResponsable { get; set; }
    public int? Year { get; set; }
    public int? idAreaOrganiz { get; set; }
    public int? IdEmpleado { get; set; }
    public DateTime? F_Inicio { get; set; }
    public DateTime? F_Termino { get; set; }
    }
}
