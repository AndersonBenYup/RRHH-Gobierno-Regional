using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class DiscapacidadData
{
    private readonly EvoRRLDbContext _contexto;

    public DiscapacidadData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Discapacidad>> Listar() => await _contexto.Set<Discapacidad>().ToListAsync();

    public async Task<Discapacidad?> Obtener(int IdDiscapacidadParam)
    {
        return await _contexto.Set<Discapacidad>().FindAsync(IdDiscapacidadParam);
    }

    public async Task Crear(Discapacidad registro)
    {
        _contexto.Set<Discapacidad>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Discapacidad registro)
    {
        _contexto.Set<Discapacidad>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Discapacidad registro)
    {
        _contexto.Set<Discapacidad>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
