using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class Empleado_AreaData
{
    private readonly EvoRRLDbContext _contexto;

    public Empleado_AreaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Empleado_Area>> Listar() => await _contexto.Set<Empleado_Area>().ToListAsync();

    public async Task<Empleado_Area?> Obtener(int YearParam, int idAreaOrganizParam, int IdEmpleadoParam)
    {
        return await _contexto.Set<Empleado_Area>().FindAsync(YearParam, idAreaOrganizParam, IdEmpleadoParam);
    }

    public async Task Crear(Empleado_Area registro)
    {
        _contexto.Set<Empleado_Area>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Empleado_Area registro)
    {
        _contexto.Set<Empleado_Area>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Empleado_Area registro)
    {
        _contexto.Set<Empleado_Area>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
