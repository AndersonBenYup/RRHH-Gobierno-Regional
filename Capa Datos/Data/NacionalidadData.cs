using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class NacionalidadData
{
    private readonly EvoRRLDbContext _contexto;

    public NacionalidadData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Nacionalidad>> Listar() => await _contexto.Set<Nacionalidad>().ToListAsync();

    public async Task<Nacionalidad?> Obtener(int IdNacionalidadParam)
    {
        return await _contexto.Set<Nacionalidad>().FindAsync(IdNacionalidadParam);
    }

    public async Task Crear(Nacionalidad registro)
    {
        _contexto.Set<Nacionalidad>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Nacionalidad registro)
    {
        _contexto.Set<Nacionalidad>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Nacionalidad registro)
    {
        _contexto.Set<Nacionalidad>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
