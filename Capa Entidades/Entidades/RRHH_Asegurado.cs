using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_Asegurado")]
    public class RRHH_Asegurado
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int idEmpleado { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int NroBeneficiario { get; set; }
    public int? idPersona { get; set; }
    public int? IdVinculoAsegurado { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NroPartida { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NroResolMayorIndiscap { get; set; }
    public DateTime? FechaInicioVinculo { get; set; }
    public DateTime? FechaFinVinculo { get; set; }
    public int? idMotivoBaja { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NroPartidaDef { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? OtrosMotivos { get; set; }
    [StringLength(1, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Reevaluar { get; set; }
    }
}
