using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class EstructOrganizDotacionData
{
    private readonly EvoRRLDbContext _contexto;

    public EstructOrganizDotacionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<EstructOrganizDotacion>> Listar() => await _contexto.Set<EstructOrganizDotacion>().AsNoTracking().ToListAsync();

    public async Task<EstructOrganizDotacion?> Obtener(int YearParam, int idAreaOrganizParam)
    {
        return await _contexto.Set<EstructOrganizDotacion>().FindAsync(YearParam, idAreaOrganizParam);
    }
}
