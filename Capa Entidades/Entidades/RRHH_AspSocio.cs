using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_AspSocio")]
    public class RRHH_AspSocio
    {
    [Key]
    public int idAspSocio { get; set; }
    public int? IdEmpleado { get; set; }
    public string? PrincipProblemas { get; set; }
    public string? PropMejoraInstitu { get; set; }
    public string? hobby { get; set; }
    public string? ActRecreaInstitu { get; set; }
    public string? ObsFam { get; set; }
    public string? AspecSalud { get; set; }
    public string? AspecEducacion { get; set; }
    public string? AspecRecreacion { get; set; }
    public string? AspecPsico { get; set; }
    public string? AspecSocial { get; set; }
    public string? DiagnosSocial { get; set; }
    }
}
