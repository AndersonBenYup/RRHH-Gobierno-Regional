using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ViviendaServicio")]
    public class ViviendaServicio
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdVivienda { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdServicio { get; set; }
    }
}
