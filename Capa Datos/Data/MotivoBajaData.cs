using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class MotivoBajaData
{
    private readonly EvoRRLDbContext _contexto;

    public MotivoBajaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<MotivoBaja>> Listar() => await _contexto.Set<MotivoBaja>().ToListAsync();

    public async Task<MotivoBaja?> Obtener(int idMotivoBajaParam)
    {
        return await _contexto.Set<MotivoBaja>().FindAsync(idMotivoBajaParam);
    }

    public async Task Crear(MotivoBaja registro)
    {
        _contexto.Set<MotivoBaja>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(MotivoBaja registro)
    {
        _contexto.Set<MotivoBaja>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(MotivoBaja registro)
    {
        _contexto.Set<MotivoBaja>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
