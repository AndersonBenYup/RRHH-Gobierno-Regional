using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RotacionData
{
    private readonly EvoRRLDbContext _contexto;

    public RotacionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Rotacion>> Listar() => await _contexto.Set<Rotacion>().ToListAsync();

    public async Task<Rotacion?> Obtener(int idEmpleadoParam, int idRotacionParam)
    {
        return await _contexto.Set<Rotacion>().FindAsync(idEmpleadoParam, idRotacionParam);
    }

    public async Task Crear(Rotacion registro)
    {
        _contexto.Set<Rotacion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Rotacion registro)
    {
        _contexto.Set<Rotacion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Rotacion registro)
    {
        _contexto.Set<Rotacion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
