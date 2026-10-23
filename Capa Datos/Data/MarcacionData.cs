using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class MarcacionData
{
    private readonly EvoRRLDbContext _contexto;

    public MarcacionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Marcacion>> Listar() => await _contexto.Set<Marcacion>().ToListAsync();

    public async Task<Marcacion?> Obtener(DateTime FechaParam, int IdEmpleadoParam)
    {
        return await _contexto.Set<Marcacion>().FindAsync(FechaParam, IdEmpleadoParam);
    }

    public async Task Crear(Marcacion registro)
    {
        _contexto.Set<Marcacion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Marcacion registro)
    {
        _contexto.Set<Marcacion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Marcacion registro)
    {
        _contexto.Set<Marcacion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
