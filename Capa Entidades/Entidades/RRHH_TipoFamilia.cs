using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_TipoFamilia")]
    public class RRHH_TipoFamilia
    {
    [Key]
    public int idTipoFam { get; set; }
    public string? TipoFamDescrip { get; set; }
    }
}
