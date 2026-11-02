using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TenenciaViv")]
    public class TenenciaViv
    {
    [Key]
    public int IdTenencia { get; set; }
    [StringLength(30, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripTenencia { get; set; }
    }
}
