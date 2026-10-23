using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ComportamientoData
{
    private readonly EvoRRLDbContext _contexto;

    public ComportamientoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Comportamiento>> Listar() => await _contexto.Set<Comportamiento>().ToListAsync();

    public async Task<Comportamiento?> Obtener(int idComportamientoParam)
    {
        return await _contexto.Set<Comportamiento>().FindAsync(idComportamientoParam);
    }

    public async Task Crear(Comportamiento registro)
    {
        _contexto.Set<Comportamiento>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Comportamiento registro)
    {
        _contexto.Set<Comportamiento>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Comportamiento registro)
    {
        _contexto.Set<Comportamiento>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
