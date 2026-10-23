using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Persona")]
    public class Persona
    {
    [Key]
    public int IdPersona { get; set; }
    public string? TipoPersona { get; set; }
    public string? Nombres { get; set; }
    public string? Apellido_Paterno { get; set; }
    public string? Apellido_Materno { get; set; }
    public int? TipoDocID { get; set; }
    public string? NumDocID { get; set; }
    public string? Sexo { get; set; }
    public DateTime? FechaNacimiento { get; set; }
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
    public string? Ocupacion { get; set; }
    public string? LugarTrabajo { get; set; }
    public string? Procedencia { get; set; }
    public string? Enfermedad { get; set; }
    public string? TiempoEnfermedad { get; set; }
    public string? Direccion { get; set; }
    public string? IdUbigeo { get; set; }
    public string? UbigeoDireccion { get; set; }
    public string? Telefono { get; set; }
    public string? NumCelular { get; set; }
    public string? Email { get; set; }
    public byte[]? Foto { get; set; }
    public DateTime? FechaRegistro { get; set; }
    }
}
