using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Provincia")]
    public class Provincia
    {
    [Key]
    public string CodDepartamento { get; set; } = "";
    [Key]
    public string CodProvincia { get; set; } = "";
    public string? Nombre { get; set; }
    }
}
