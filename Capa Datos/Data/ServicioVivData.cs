using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ServicioVivData
{
    private readonly EvoRRLDbContext _contexto;

    public ServicioVivData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<ServicioViv>> Listar() => await _contexto.Set<ServicioViv>().ToListAsync();

    public async Task<ServicioViv?> Obtener(int IdServicioParam)
    {
        return await _contexto.Set<ServicioViv>().FindAsync(IdServicioParam);
    }

    public async Task Crear(ServicioViv registro)
    {
        _contexto.Set<ServicioViv>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(ServicioViv registro)
    {
        _contexto.Set<ServicioViv>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(ServicioViv registro)
    {
        _contexto.Set<ServicioViv>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
