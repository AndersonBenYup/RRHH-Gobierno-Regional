using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class UbicacionVivData
{
    private readonly EvoRRLDbContext _contexto;

    public UbicacionVivData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<UbicacionViv>> Listar() => await _contexto.Set<UbicacionViv>().ToListAsync();

    public async Task<UbicacionViv?> Obtener(int IdUbicacionParam)
    {
        return await _contexto.Set<UbicacionViv>().FindAsync(IdUbicacionParam);
    }

    public async Task Crear(UbicacionViv registro)
    {
        _contexto.Set<UbicacionViv>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(UbicacionViv registro)
    {
        _contexto.Set<UbicacionViv>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(UbicacionViv registro)
    {
        _contexto.Set<UbicacionViv>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
