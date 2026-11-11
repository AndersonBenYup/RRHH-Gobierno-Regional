using System.ComponentModel.DataAnnotations;
using Capa_Entidades.Validacion;

namespace Capa_Entidades;

public class PeriodoTeletrabajo
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int IdEmpleado { get; set; }

    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public DateOnly? FechaInicio { get; set; }

    [Required(ErrorMessage = Mensajes.Obligatorio)]
    [FechaNoAnteriorA(nameof(FechaInicio), ErrorMessage = Mensajes.FechaFinAnterior)]
    public DateOnly? FechaFin { get; set; }

    public bool Parcial { get; set; }
}
