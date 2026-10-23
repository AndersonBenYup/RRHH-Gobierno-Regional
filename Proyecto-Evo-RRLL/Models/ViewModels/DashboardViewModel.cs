namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class DashboardViewModel
{
    public int Empleados { get; set; }

    public int EmpleadosActivos { get; set; }

    public int FeriadosDelMes { get; set; }

    public List<string> FeriadosMes { get; set; } = new();

    public int Personas { get; set; }

    public int asistenciasHoy { get; set; }

    public int Areas { get; set; }

    public int Cargos { get; set; }

    // Empleados por área (primer gráfico)
    public List<string> AreasNombres { get; set; } = new();
    public List<int> EmpleadosPorArea { get; set; } = new();

    // Asistencias en los últimos 7 días (segundo gráfico)
    public List<string> DiasAsistencia { get; set; } = new();
    public List<int> AsistenciasPorDia { get; set; } = new();

    // Distribución por estado del empleado (tercer gráfico)
    public List<string> EstadosNombres { get; set; } = new();
    public List<int> EmpleadosPorEstado { get; set; } = new();
}