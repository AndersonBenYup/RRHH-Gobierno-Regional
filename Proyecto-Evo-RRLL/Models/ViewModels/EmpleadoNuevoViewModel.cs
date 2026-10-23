using Capa_Entidades;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class EmpleadoNuevoViewModel
{
    public Persona Persona { get; set; } = new();

    public EmpleadoFicha Empleado { get; set; } = new();
}