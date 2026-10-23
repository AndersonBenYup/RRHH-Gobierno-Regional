using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class AlumbradoVivData
{
    private readonly EvoRRLDbContext _contexto;

    public AlumbradoVivData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<AlumbradoViv>> Listar() => await _contexto.Set<AlumbradoViv>().ToListAsync();

    public async Task<AlumbradoViv?> Obtener(int IdAlumbradoParam)
    {
        return await _contexto.Set<AlumbradoViv>().FindAsync(IdAlumbradoParam);
    }

    public async Task Crear(AlumbradoViv registro)
    {
        _contexto.Set<AlumbradoViv>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(AlumbradoViv registro)
    {
        _contexto.Set<AlumbradoViv>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(AlumbradoViv registro)
    {
        _contexto.Set<AlumbradoViv>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
