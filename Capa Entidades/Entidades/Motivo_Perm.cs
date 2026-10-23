using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Motivo_Perm")]
    public class Motivo_Perm
    {
    [Key]
    public int idMotivo { get; set; }
    public string? Descrip_Motivo { get; set; }
    public bool? Salario { get; set; }
    public int? idTipoPermiso { get; set; }
    public string? Abrev { get; set; }
    public string? TipoModalidad { get; set; }
    public bool? Activo { get; set; }
    }
}
