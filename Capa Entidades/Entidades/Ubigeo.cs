using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("vw_Ubigeo")]
    public class Ubigeo
    {
    [Key]
    public string? IdUbigeo { get; set; }
    public string? Departamento { get; set; }
    public string? Provincia { get; set; }
    public string? Distrito { get; set; }
    public string? Descripcion { get; set; }
    public string? NombreProvincia { get; set; }
    public string? NombreDepartamento { get; set; }
    }
}
