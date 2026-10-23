using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Cargo")]
    public class Cargo
    {
    [Key]
    public int IdCargo { get; set; }
    [Column("Cargo")]
    public string? Descripcion { get; set; }
    public string? Abreviatura { get; set; }
    }
}
