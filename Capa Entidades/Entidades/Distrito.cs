using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Distrito")]
    public class Distrito
    {
    [Key]
    public string IdUbigeo { get; set; } = "";
    public string CodDepartamento { get; set; } = "";
    public string CodProvincia { get; set; } = "";
    public string CodDistrito { get; set; } = "";
    public string? Nombre { get; set; }
    }
}
