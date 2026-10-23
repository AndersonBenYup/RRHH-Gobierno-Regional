using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class CargoData
{
    private readonly EvoRRLDbContext _contexto;

    public CargoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Cargo>> Listar() => await _contexto.Set<Cargo>().ToListAsync();

    public async Task<Cargo?> Obtener(int IdCargoParam)
    {
        return await _contexto.Set<Cargo>().FindAsync(IdCargoParam);
    }

    public async Task Crear(Cargo registro)
    {
        _contexto.Set<Cargo>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Cargo registro)
    {
        _contexto.Set<Cargo>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Cargo registro)
    {
        _contexto.Set<Cargo>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
