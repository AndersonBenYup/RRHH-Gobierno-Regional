using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_AspSocioData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_AspSocioData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_AspSocio>> Listar() => await _contexto.Set<RRHH_AspSocio>().ToListAsync();

    public async Task<RRHH_AspSocio?> Obtener(int idAspSocioParam)
    {
        return await _contexto.Set<RRHH_AspSocio>().FindAsync(idAspSocioParam);
    }

    public async Task Crear(RRHH_AspSocio registro)
    {
        _contexto.Set<RRHH_AspSocio>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_AspSocio registro)
    {
        _contexto.Set<RRHH_AspSocio>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_AspSocio registro)
    {
        _contexto.Set<RRHH_AspSocio>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
