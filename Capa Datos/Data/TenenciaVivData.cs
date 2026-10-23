using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TenenciaVivData
{
    private readonly EvoRRLDbContext _contexto;

    public TenenciaVivData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TenenciaViv>> Listar() => await _contexto.Set<TenenciaViv>().ToListAsync();

    public async Task<TenenciaViv?> Obtener(int IdTenenciaParam)
    {
        return await _contexto.Set<TenenciaViv>().FindAsync(IdTenenciaParam);
    }

    public async Task Crear(TenenciaViv registro)
    {
        _contexto.Set<TenenciaViv>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TenenciaViv registro)
    {
        _contexto.Set<TenenciaViv>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TenenciaViv registro)
    {
        _contexto.Set<TenenciaViv>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
