using Capa_Entidades;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class EmpleadoDetalleViewModel
{
    public int IdEmpleado { get; set; }

    public EmpleadoFicha? Empleado { get; set; }

    public Persona? Persona { get; set; }

    public string? NombresCompletos { get; set; }

    public string? TipoDoc { get; set; }

    public string? Cargo { get; set; }

    public string? Area { get; set; }

    public string? Ubigeo { get; set; }

    public string? Local { get; set; }

    public string? Horario { get; set; }

    public string? Estado { get; set; }

    public string? AFP { get; set; }

    public string? EstadoCivil { get; set; }

    public string? TipoSangre { get; set; }

    public string? Discapacidad { get; set; }

    public string? ViveCon { get; set; }

    public string? Nacionalidad { get; set; }

    public string? TipoTrabajador { get; set; }

    public string? RegimenPension { get; set; }

    public bool TieneFoto { get; set; }
}