using System.ComponentModel.DataAnnotations;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ingrese su usuario")]
    [Display(Name = "Usuario")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingrese su clave")]
    [DataType(DataType.Password)]
    [Display(Name = "Clave")]
    public string Clave { get; set; } = string.Empty;

    public string? Error { get; set; }
}