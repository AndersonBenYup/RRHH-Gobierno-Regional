using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Persona")]
    public class Persona
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdPersona { get; set; }
    [StringLength(1, ErrorMessage = "Máximo {1} caracteres.")]
    public string? TipoPersona { get; set; }
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Nombres { get; set; }
    [StringLength(30, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Apellido_Paterno { get; set; }
    [StringLength(30, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Apellido_Materno { get; set; }
    public int? TipoDocID { get; set; }
    [StringLength(12, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NumDocID { get; set; }
    [StringLength(1, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Sexo { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public int? IdEstadoCivil { get; set; }
    public int? IdNacionalidad { get; set; }
    public int? IdTipoSangre { get; set; }
    public int? IdDiscapacidad { get; set; }
    [StringLength(15, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NumAutogenSalud { get; set; }
    [StringLength(12, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NumRUC { get; set; }
    [StringLength(12, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Brevete { get; set; }
    [StringLength(10, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NumLibretaMilitar { get; set; }
    [StringLength(80, ErrorMessage = "Máximo {1} caracteres.")]
    public string? GradoInstruccion { get; set; }
    [StringLength(80, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Profesion { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Ocupacion { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? LugarTrabajo { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Procedencia { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Enfermedad { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    public string? TiempoEnfermedad { get; set; }
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Direccion { get; set; }
    [StringLength(6, ErrorMessage = "Máximo {1} caracteres.")]
    public string? IdUbigeo { get; set; }
    [StringLength(6, ErrorMessage = "Máximo {1} caracteres.")]
    public string? UbigeoDireccion { get; set; }
    [StringLength(15, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Telefono { get; set; }
    [StringLength(15, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NumCelular { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Email { get; set; }
    public byte[]? Foto { get; set; }
    public DateTime? FechaRegistro { get; set; }
    }
}
