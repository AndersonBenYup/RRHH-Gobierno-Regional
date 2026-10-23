using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("vw_EmpleadoResumenFamiliar")]
    public class EmpleadoResumenFamiliar
    {
    [Key]
    public int IdEmpleado { get; set; }
    public int? NumHijos { get; set; }
    public int? NumFamiliares { get; set; }
    public byte? CargaFamDeclarada { get; set; }
    }
}
