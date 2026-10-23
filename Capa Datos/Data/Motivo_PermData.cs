using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class Motivo_PermData
{
    private readonly EvoRRLDbContext _contexto;

    public Motivo_PermData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Motivo_Perm>> Listar() => await _contexto.Set<Motivo_Perm>().ToListAsync();

    public async Task<Motivo_Perm?> Obtener(int idMotivoParam)
    {
        return await _contexto.Set<Motivo_Perm>().FindAsync(idMotivoParam);
    }

    public async Task Crear(Motivo_Perm registro)
    {
        _contexto.Set<Motivo_Perm>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Motivo_Perm registro)
    {
        _contexto.Set<Motivo_Perm>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Motivo_Perm registro)
    {
        _contexto.Set<Motivo_Perm>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
