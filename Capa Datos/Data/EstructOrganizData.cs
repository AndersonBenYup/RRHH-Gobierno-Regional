using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class EstructOrganizData
{
    private readonly EvoRRLDbContext _contexto;

    public EstructOrganizData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<EstructOrganiz>> Listar() => await _contexto.Set<EstructOrganiz>().ToListAsync();

    public async Task<EstructOrganiz?> Obtener(int YearParam, int idAreaOrganizParam)
    {
        return await _contexto.Set<EstructOrganiz>().FindAsync(YearParam, idAreaOrganizParam);
    }

    public async Task Crear(EstructOrganiz registro)
    {
        _contexto.Set<EstructOrganiz>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(EstructOrganiz registro)
    {
        _contexto.Set<EstructOrganiz>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(EstructOrganiz registro)
    {
        _contexto.Set<EstructOrganiz>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
