using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Bitacora")]
    public class Bitacora
    {
        [Key]
        [Required(ErrorMessage = Mensajes.Obligatorio)]
        public int IdBitacora { get; set; }
        [Required(ErrorMessage = Mensajes.Obligatorio)]
        public DateTime Fecha { get; set; }
        [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
        public string? Usuario { get; set; }
        [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
        public string? Accion { get; set; }
        [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
        public string? Entidad { get; set; }
        public string? Detalle { get; set; }
    }
}