using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_AcudeEnferm")]
    public class RRHH_AcudeEnferm
    {
    [Key]
    public int idAcudeEnferm { get; set; }
    public string? AcudeEnfermDescrip { get; set; }
    }
}
