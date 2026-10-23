using Capa_Entidades;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class FamiliaSaludViewModel
{
    public int IdEmpleado { get; set; }

    public string? NombreEmpleado { get; set; }

    public bool TieneSalud { get; set; }

    public bool TieneDinamica { get; set; }

    public bool TieneAspSocio { get; set; }

    public bool TieneFuncFam { get; set; }

    public int Familiares { get; set; }

    public int Asegurados { get; set; }
}

public class FamiliarFila
{
    public int IdEmpleado { get; set; }

    public int IdPersona { get; set; }

    public string? Nombres { get; set; }

    public string? TipoFamiliar { get; set; }

    public string? Sexo { get; set; }

    public string? Ocupacion { get; set; }

    public string? EstadoCivil { get; set; }

    public string? ViveCasa { get; set; }

    public string? LaboraInstitucion { get; set; }

    public string? Discapacidad { get; set; }

    public string? TipoSangre { get; set; }
}

public class AseguradoFila
{
    public int NroBeneficiario { get; set; }

    public string? Nombre { get; set; }

    public string? EstadoCivil { get; set; }

    public string? VinculoFamiliar { get; set; }

    public string? Sexo { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }
}

public class FichaSocialViewModel
{
    public int IdEmpleado { get; set; }

    public string? NombreEmpleado { get; set; }

    public RRHH_DinamicaFamiliar Dinamica { get; set; } = new();

    public RRHH_AspSocio AspSocio { get; set; } = new();

    public RRHH_FuncFam FuncFam { get; set; } = new();
}