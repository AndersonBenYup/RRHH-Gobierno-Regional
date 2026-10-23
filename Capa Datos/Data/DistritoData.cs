using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class DistritoData
{
    private readonly EvoRRLDbContext _contexto;

    public DistritoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Distrito>> Listar() => await _contexto.Set<Distrito>().AsNoTracking().ToListAsync();

    public async Task<Distrito?> Obtener(string IdUbigeoParam)
    {
        return await _contexto.Set<Distrito>().FindAsync(IdUbigeoParam);
    }
}
