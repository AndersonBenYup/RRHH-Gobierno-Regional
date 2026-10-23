using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class EstadoCivilData
{
    private readonly EvoRRLDbContext _contexto;

    public EstadoCivilData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<EstadoCivil>> Listar() => await _contexto.Set<EstadoCivil>().ToListAsync();

    public async Task<EstadoCivil?> Obtener(int IdEstadoCivilParam)
    {
        return await _contexto.Set<EstadoCivil>().FindAsync(IdEstadoCivilParam);
    }

    public async Task Crear(EstadoCivil registro)
    {
        _contexto.Set<EstadoCivil>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(EstadoCivil registro)
    {
        _contexto.Set<EstadoCivil>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(EstadoCivil registro)
    {
        _contexto.Set<EstadoCivil>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
