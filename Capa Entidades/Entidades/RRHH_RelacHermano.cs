using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_RelacHermano")]
    public class RRHH_RelacHermano
    {
    [Key]
    public int idRelacHermano { get; set; }
    public string? RelacHermanoDescrip { get; set; }
    }
}
