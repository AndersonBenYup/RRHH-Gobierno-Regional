using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class PersonaData
{
    private readonly EvoRRLDbContext _contexto;

    public PersonaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Persona>> Listar() => await _contexto.Set<Persona>().ToListAsync();

    /// <summary>SC-008: true si otra persona (distinta de <paramref name="excluirIdPersona"/>) usa ese documento.</summary>
    public async Task<bool> ExisteDocumento(string numDoc, int? excluirIdPersona = null)
    {
        var numero = numDoc.Trim();
        return await _contexto.Set<Persona>().AnyAsync(p =>
            p.NumDocID != null && p.NumDocID.Trim() == numero
            && (excluirIdPersona == null || p.IdPersona != excluirIdPersona));
    }

    public async Task<Persona?> Obtener(int IdPersonaParam)
    {
        return await _contexto.Set<Persona>().FindAsync(IdPersonaParam);
    }

    public async Task Crear(Persona registro)
    {
        _contexto.Set<Persona>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Persona registro)
    {
        _contexto.Set<Persona>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Persona registro)
    {
        _contexto.Set<Persona>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
