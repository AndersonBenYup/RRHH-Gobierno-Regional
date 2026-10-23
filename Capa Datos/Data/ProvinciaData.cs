using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ProvinciaData
{
    private readonly EvoRRLDbContext _contexto;

    public ProvinciaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Provincia>> Listar() => await _contexto.Set<Provincia>().AsNoTracking().ToListAsync();

    public async Task<Provincia?> Obtener(string CodDepartamentoParam, string CodProvinciaParam)
    {
        return await _contexto.Set<Provincia>().FindAsync(CodDepartamentoParam, CodProvinciaParam);
    }
}
