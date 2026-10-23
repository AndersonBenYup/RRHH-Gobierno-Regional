using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Departamento")]
    public class Departamento
    {
    [Key]
    public string CodDepartamento { get; set; } = "";
    public string? Nombre { get; set; }
    }
}
