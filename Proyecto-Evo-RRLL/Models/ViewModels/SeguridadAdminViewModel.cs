using Capa_Entidades;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class UsuarioFila
{
    public Usuario Usuario { get; set; } = new();

    public string Empleado { get; set; } = "";

    public string Roles { get; set; } = "";
}

public class RolFila
{
    public Rol Rol { get; set; } = new();

    public int Usuarios { get; set; }

    public int TotalUsuarios { get; set; }
}

public class OpcionCheck
{
    public int Valor { get; set; }

    public string Texto { get; set; } = "";

    public bool Seleccionado { get; set; }
}

public class OpcionCheckTexto
{
    public string Valor { get; set; } = "";

    public string Texto { get; set; } = "";

    public bool Seleccionado { get; set; }
}