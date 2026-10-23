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
    public string? NroAFP { get; set; }
    public DateTime? FechaAFP { get; set; }
    public string? CtaCorriente { get; set; }
    public string? DecLey { get; set; }
    public string? ExpSocial { get; set; }
    public byte? CargaFam { get; set; }
    public bool? JefeHogar { get; set; }
    public int? IdViveCon { get; set; }
    }
}
