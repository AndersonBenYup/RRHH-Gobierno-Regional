using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class CapacitacionData
{
    private readonly EvoRRLDbContext _contexto;

    public CapacitacionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Capacitacion>> Listar() => await _contexto.Set<Capacitacion>().ToListAsync();

    public async Task<Capacitacion?> Obtener(int IdCapacitacionParam)
    {
        return await _contexto.Set<Capacitacion>().FindAsync(IdCapacitacionParam);
    }

    public async Task Crear(Capacitacion registro)
    {
        _contexto.Set<Capacitacion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Capacitacion registro)
    {
        _contexto.Set<Capacitacion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Capacitacion registro)
    {
        _contexto.Set<Capacitacion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
