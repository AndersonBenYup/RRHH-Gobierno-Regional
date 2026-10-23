using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ViviendaData
{
    private readonly EvoRRLDbContext _contexto;

    public ViviendaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Vivienda>> Listar() => await _contexto.Set<Vivienda>().ToListAsync();

    public async Task<Vivienda?> Obtener(int IdViviendaParam)
    {
        return await _contexto.Set<Vivienda>().FindAsync(IdViviendaParam);
    }

    public async Task Crear(Vivienda registro)
    {
        _contexto.Set<Vivienda>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Vivienda registro)
    {
        _contexto.Set<Vivienda>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Vivienda registro)
    {
        _contexto.Set<Vivienda>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
