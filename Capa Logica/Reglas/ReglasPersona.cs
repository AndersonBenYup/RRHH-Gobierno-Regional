using System.Text.RegularExpressions;
using Capa_Entidades;
using Capa_Entidades.Validacion;

namespace Capa_Logica.Reglas;

/// <summary>SC-008: reglas de los datos personales (nombres, documento de identidad y duplicados).</summary>
public static class ReglasPersona
{
    public const string MensajeNombres = "El nombre es obligatorio.";
    public const string MensajeApellido = "El apellido paterno es obligatorio.";
    public const string MensajeSinDocumento = "Ingrese el número del documento.";
    public const string MensajeDuplicado = "Ya existe una persona con ese número de documento.";
    public const string MensajeRuc = "El RUC debe tener exactamente 11 dígitos.";

    /// <param name="documentoDuplicado">true si otra persona ya tiene ese número de documento.</param>
    /// <param name="exigirApellido">Los familiares pueden registrarse sin apellido paterno.</param>
    public static List<ErrorValidacion> Validar(Persona persona, bool documentoDuplicado, bool exigirApellido = true)
    {
        var errores = new List<ErrorValidacion>();

        if (string.IsNullOrWhiteSpace(persona.Nombres))
            errores.Add(new(nameof(Persona.Nombres), MensajeNombres));
        if (exigirApellido && string.IsNullOrWhiteSpace(persona.Apellido_Paterno))
            errores.Add(new(nameof(Persona.Apellido_Paterno), MensajeApellido));

        var numero = persona.NumDocID?.Trim();
        if (persona.TipoDocID is not null && string.IsNullOrEmpty(numero))
        {
            errores.Add(new(nameof(Persona.NumDocID), MensajeSinDocumento));
            return errores;
        }

        if (string.IsNullOrEmpty(numero))
            return errores;

        if (persona.TipoDocID == TiposDocumento.Dni && !DniAttribute.EsValido(numero))
            errores.Add(new(nameof(Persona.NumDocID), DniAttribute.Mensaje));
        else if (persona.TipoDocID == TiposDocumento.Ruc && !Regex.IsMatch(numero, @"^\d{11}$"))
            errores.Add(new(nameof(Persona.NumDocID), MensajeRuc));

        if (documentoDuplicado)
            errores.Add(new(nameof(Persona.NumDocID), MensajeDuplicado));

        return errores;
    }
}
