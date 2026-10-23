using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_Asegurado")]
    public class RRHH_Asegurado
    {
    [Key]
    public int idEmpleado { get; set; }
    [Key]
    public int NroBeneficiario { get; set; }
    public int? idPersona { get; set; }
    public int? IdVinculoAsegurado { get; set; }
    public string? NroPartida { get; set; }
    public string? NroResolMayorIndiscap { get; set; }
    public DateTime? FechaInicioVinculo { get; set; }
    public DateTime? FechaFinVinculo { get; set; }
    public int? idMotivoBaja { get; set; }
    public string? NroPartidaDef { get; set; }
    public string? OtrosMotivos { get; set; }
    public string? Reevaluar { get; set; }
    }
}
