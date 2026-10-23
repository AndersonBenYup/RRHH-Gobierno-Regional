using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_RelacPH")]
    public class RRHH_RelacPH
    {
    [Key]
    public int idRelacPH { get; set; }
    public string? RelacPHDescrip { get; set; }
    }
}
