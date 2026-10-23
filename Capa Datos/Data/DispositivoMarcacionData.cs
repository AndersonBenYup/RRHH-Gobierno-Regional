using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class DispositivoMarcacionData
{
    private readonly EvoRRLDbContext _contexto;

    public DispositivoMarcacionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<DispositivoMarcacion>> Listar() => await _contexto.Set<DispositivoMarcacion>().ToListAsync();

    public async Task<DispositivoMarcacion?> Obtener(string CodigoParam)
    {
        return await _contexto.Set<DispositivoMarcacion>().FindAsync(CodigoParam);
    }

    public async Task Crear(DispositivoMarcacion registro)
    {
        _contexto.Set<DispositivoMarcacion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(DispositivoMarcacion registro)
    {
        _contexto.Set<DispositivoMarcacion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(DispositivoMarcacion registro)
    {
        _contexto.Set<DispositivoMarcacion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
