using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Empleado")]
    public class Empleado
    {
    [Key]
    public int IdEmpleado { get; set; }
    public int? IdPersona { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public bool? Estado { get; set; }
    public int? IdTipoTrabajador { get; set; }
    public int? IdCargo { get; set; }
    public int? idHorario { get; set; }
    public int? idLocal { get; set; }
    public int? IdRegimenPen { get; set; }
    public int? IdAFP { get; set; }
    [StringLength(20, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NroAFP { get; set; }
    public DateTime? FechaAFP { get; set; }
    [StringLength(16, ErrorMessage = "Máximo {1} caracteres.")]
    public string? CtaCorriente { get; set; }
    [StringLength(10, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DecLey { get; set; }
    [StringLength(20, ErrorMessage = "Máximo {1} caracteres.")]
    public string? ExpSocial { get; set; }
    public byte? CargaFam { get; set; }
    public bool? JefeHogar { get; set; }
    public int? IdViveCon { get; set; }
    }
}
