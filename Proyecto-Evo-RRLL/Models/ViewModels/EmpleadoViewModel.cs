namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class EmpleadoViewModel
{
    public string? Busqueda { get; set; }

    public int Pagina { get; set; } = 1;

    public int TotalPaginas { get; set; } = 1;

    public int Total { get; set; }

    public List<EmpleadoFila> Empleados { get; set; } = new();
}

public class EmpleadoFila
{
    public int IdEmpleado { get; set; }

    public string? Nombres { get; set; }

    public string? NumDoc { get; set; }

    public string? Cargo { get; set; }

    public string? Area { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public bool TieneFoto { get; set; }
}