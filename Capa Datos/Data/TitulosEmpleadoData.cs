using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TitulosEmpleadoData
{
    private readonly EvoRRLDbContext _contexto;

    public TitulosEmpleadoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TitulosEmpleado>> Listar() => await _contexto.Set<TitulosEmpleado>().ToListAsync();

    public async Task<TitulosEmpleado?> Obtener(int IdTituloParam)
    {
        return await _contexto.Set<TitulosEmpleado>().FindAsync(IdTituloParam);
    }

    public async Task Crear(TitulosEmpleado registro)
    {
        _contexto.Set<TitulosEmpleado>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TitulosEmpleado registro)
    {
        _contexto.Set<TitulosEmpleado>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TitulosEmpleado registro)
    {
        _contexto.Set<TitulosEmpleado>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
