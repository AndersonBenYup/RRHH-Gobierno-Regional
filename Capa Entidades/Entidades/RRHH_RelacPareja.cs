using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_RelacPareja")]
    public class RRHH_RelacPareja
    {
    [Key]
    public int idRelacPareja { get; set; }
    public string? RelacParejaDescrip { get; set; }
    }
}
