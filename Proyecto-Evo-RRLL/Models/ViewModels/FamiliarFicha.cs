using Capa_Entidades;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

// Familiar del empleado para los formularios. En la BD normalizada la relacion
// (parentesco, vive en casa) esta en Familiar y los datos personales en Persona.
public class FamiliarFicha
{
    // Familiar
    public int IdEmpleado { get; set; }
    public int IdPersona { get; set; }
    public int? IdTipoFam { get; set; }
    public bool? ViveCasa { get; set; }

    // Persona
    public string? FamSexo { get; set; }
    public string? FamLugTrab { get; set; }
    public int? IdEstadoCivil { get; set; }
    public string? NumAutogenSalud { get; set; }
    public string? FamOcupacion { get; set; }
    public string? FamGradoInstruc { get; set; }
    public int? IdDiscapacidad { get; set; }
    public string? Procedencia { get; set; }
    public string? Enfermedad { get; set; }
    public string? TiempoEnfermedad { get; set; }
    public int? IdTipoSangre { get; set; }

    // Calculado (vw_FamiliarDetalle): solo lectura
    public bool? LaboraInstitucion { get; set; }

    public static FamiliarFicha Desde(Familiar f, Persona? p, bool? laboraInstitucion) => new()
    {
        IdEmpleado = f.IdEmpleado,
        IdPersona = f.IdPersona,
        IdTipoFam = f.IdTipoFam,
        ViveCasa = f.ViveCasa,
        FamSexo = p?.Sexo,
        FamLugTrab = p?.LugarTrabajo,
        IdEstadoCivil = p?.IdEstadoCivil,
        NumAutogenSalud = p?.NumAutogenSalud,
        FamOcupacion = p?.Ocupacion,
        FamGradoInstruc = p?.GradoInstruccion,
        IdDiscapacidad = p?.IdDiscapacidad,
        Procedencia = p?.Procedencia,
        Enfermedad = p?.Enfermedad,
        TiempoEnfermedad = p?.TiempoEnfermedad,
        IdTipoSangre = p?.IdTipoSangre,
        LaboraInstitucion = laboraInstitucion
    };

    public void AplicarA(Familiar f)
    {
        f.IdTipoFam = IdTipoFam;
        f.ViveCasa = ViveCasa;
    }

    public void AplicarA(Persona p)
    {
        p.Sexo = FamSexo;
        p.LugarTrabajo = FamLugTrab;
        p.IdEstadoCivil = IdEstadoCivil;
        p.NumAutogenSalud = NumAutogenSalud;
        p.Ocupacion = FamOcupacion;
        p.GradoInstruccion = FamGradoInstruc;
        p.IdDiscapacidad = IdDiscapacidad;
        p.Procedencia = Procedencia;
        p.Enfermedad = Enfermedad;
        p.TiempoEnfermedad = TiempoEnfermedad;
        p.IdTipoSangre = IdTipoSangre;
    }
}
