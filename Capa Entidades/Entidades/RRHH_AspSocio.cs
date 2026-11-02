using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_AspSocio")]
    public class RRHH_AspSocio
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int idAspSocio { get; set; }
    public int? IdEmpleado { get; set; }
    [StringLength(400, ErrorMessage = "Máximo {1} caracteres.")]
    public string? PrincipProblemas { get; set; }
    [StringLength(400, ErrorMessage = "Máximo {1} caracteres.")]
    public string? PropMejoraInstitu { get; set; }
    [StringLength(400, ErrorMessage = "Máximo {1} caracteres.")]
    public string? hobby { get; set; }
    [StringLength(400, ErrorMessage = "Máximo {1} caracteres.")]
    public string? ActRecreaInstitu { get; set; }
    [StringLength(500, ErrorMessage = "Máximo {1} caracteres.")]
    public string? ObsFam { get; set; }
    [StringLength(1000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? AspecSalud { get; set; }
    [StringLength(1000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? AspecEducacion { get; set; }
    [StringLength(1000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? AspecRecreacion { get; set; }
    [StringLength(1000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? AspecPsico { get; set; }
    [StringLength(1000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? AspecSocial { get; set; }
    [StringLength(1000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DiagnosSocial { get; set; }
    }
}
