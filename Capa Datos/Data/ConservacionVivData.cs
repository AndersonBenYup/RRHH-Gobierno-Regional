using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ConservacionVivData
{
    private readonly EvoRRLDbContext _contexto;

    public ConservacionVivData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<ConservacionViv>> Listar() => await _contexto.Set<ConservacionViv>().ToListAsync();

    public async Task<ConservacionViv?> Obtener(int IdConservacionParam)
    {
        return await _contexto.Set<ConservacionViv>().FindAsync(IdConservacionParam);
    }

    public async Task Crear(ConservacionViv registro)
    {
        _contexto.Set<ConservacionViv>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(ConservacionViv registro)
    {
        _contexto.Set<ConservacionViv>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(ConservacionViv registro)
    {
        _contexto.Set<ConservacionViv>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
