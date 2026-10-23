namespace Proyecto_Evo_RRLL.Models.ViewModels;

// Vista plana de un empleado para los formularios. En la BD normalizada estos datos
// viven en Empleado, Persona, Empleado_Area, RRHH_AspSocio y Resolucion;
// EmpleadoFichaServicio se encarga de leerlos y guardarlos en su tabla.
public class EmpleadoFicha
{
    public const string EstadoActivo = "A";
    public const string EstadoInactivo = "I";

    // Empleado (vinculo laboral)
    public int IdEmpleado { get; set; }
    public int? IdPersona { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public string? Estado { get; set; }
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

    // Area vigente (Empleado_Area / vw_EmpleadoAreaActual)
    public int Year { get; set; }
    public int idAreaOrganiz { get; set; }

    // Persona
    public string? Sexo { get; set; }
    public int? IdEstadoCivil { get; set; }
    public int? IdNacionalidad { get; set; }
    public int? IdTipoSangre { get; set; }
    public int? IdDiscapacidad { get; set; }
    public string? NumAutogenSalud { get; set; }
    public string? NumRUC { get; set; }
    public string? Brevete { get; set; }
    public string? NumLibretaMilitar { get; set; }
    public string? GradoInstruccion { get; set; }
    public string? Profesion { get; set; }

    // Ficha social (RRHH_AspSocio)
    public string? ObsFam { get; set; }
    public string? AspecSalud { get; set; }
    public string? AspecEducacion { get; set; }
    public string? AspecRecreacion { get; set; }
    public string? AspecPsico { get; set; }
    public string? AspecSocial { get; set; }
    public string? DiagnosSocial { get; set; }

    // Resoluciones de ingreso (Resolucion, tipos "Ingreso al Estado" / "Ingreso a la Institución")
    public string? NumResoIngreEsta { get; set; }
    public DateTime? FechaResolIngreEsta { get; set; }
    public DateTime? FechaIngreEstado { get; set; }
    public string? NumResoIngreInstitu { get; set; }
    public DateTime? FechaResoIngreInstitu { get; set; }

    // Calculado (vw_EmpleadoResumenFamiliar): solo lectura
    public int? NumHijos { get; set; }
}
