using Capa_Entidades;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class HistorialLaboralModelo
{
    public List<PeriodoLaboral> Periodos { get; set; } = new();
    public List<ExpLaboral> Experiencias { get; set; } = new();
    public List<Rotacion> Rotaciones { get; set; } = new();
    public List<Resolucion> Resoluciones { get; set; } = new();
}