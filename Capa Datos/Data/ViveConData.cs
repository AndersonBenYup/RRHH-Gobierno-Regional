using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ViveConData
{
    private readonly EvoRRLDbContext _contexto;

    public ViveConData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<ViveCon>> Listar() => await _contexto.Set<ViveCon>().ToListAsync();

    public async Task<ViveCon?> Obtener(int IdViveConParam)
    {
        return await _contexto.Set<ViveCon>().FindAsync(IdViveConParam);
    }

    public async Task Crear(ViveCon registro)
    {
        _contexto.Set<ViveCon>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(ViveCon registro)
    {
        _contexto.Set<ViveCon>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(ViveCon registro)
    {
        _contexto.Set<ViveCon>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
