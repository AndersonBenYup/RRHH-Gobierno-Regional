using Capa_Entidades;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class ViviendaViewModel
{
    public int IdEmpleado { get; set; }

    public string? NombreEmpleado { get; set; }

    public Vivienda? Registro { get; set; }

    public string? Tenencia { get; set; }

    public string? Tipo { get; set; }

    public string? Material { get; set; }

    public string? Conservacion { get; set; }

    public string? Ubicacion { get; set; }

    public string? Alumbrado { get; set; }

    public string? Agua { get; set; }

    public string? Excretas { get; set; }

    public List<(ServicioViv Servicio, bool Tiene)> Servicios { get; set; } = new();
}