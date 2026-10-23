using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Nacionalidad")]
    public class Nacionalidad
    {
    [Key]
    public int IdNacionalidad { get; set; }
    public string? Descripcion { get; set; }
    public string? Abreviatura { get; set; }
    }
}
