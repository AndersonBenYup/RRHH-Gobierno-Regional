namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class AsistenciaViewModel
{
    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }

    public string? Busqueda { get; set; }

    public List<AsistenciaFila> Registros { get; set; } = new();
}

public class AsistenciaFila
{
    public DateTime Fecha { get; set; }

    public int IdEmpleado { get; set; }

    public string? Empleado { get; set; }

    public string? HorEntrada { get; set; }

    public string? HorSalida { get; set; }

    public string? AlmSalida { get; set; }

    public string? AlmEntrada { get; set; }

    public bool? Estado { get; set; }
}