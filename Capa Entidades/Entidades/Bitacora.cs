using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Bitacora")]
    public class Bitacora
    {
        [Key]
        public int IdBitacora { get; set; }
        public DateTime Fecha { get; set; }
        public string? Usuario { get; set; }
        public string? Accion { get; set; }
        public string? Entidad { get; set; }
        public string? Detalle { get; set; }
    }
}