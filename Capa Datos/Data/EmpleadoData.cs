using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class EmpleadoData
{
    private readonly EvoRRLDbContext _contexto;

    public EmpleadoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Empleado>> Listar() => await _contexto.Set<Empleado>().ToListAsync();

    public async Task<Empleado?> Obtener(int IdEmpleadoParam)
    {
        return await _contexto.Set<Empleado>().FindAsync(IdEmpleadoParam);
    }

    public async Task Crear(Empleado registro)
    {
        _contexto.Set<Empleado>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Empleado registro)
    {
        _contexto.Set<Empleado>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Empleado registro)
    {
        _contexto.Set<Empleado>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
