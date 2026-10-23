using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class DepartamentoData
{
    private readonly EvoRRLDbContext _contexto;

    public DepartamentoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Departamento>> Listar() => await _contexto.Set<Departamento>().AsNoTracking().ToListAsync();

    public async Task<Departamento?> Obtener(string CodDepartamentoParam)
    {
        return await _contexto.Set<Departamento>().FindAsync(CodDepartamentoParam);
    }
}
