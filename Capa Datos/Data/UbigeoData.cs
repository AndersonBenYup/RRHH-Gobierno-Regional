using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class UbigeoData
{
    private readonly EvoRRLDbContext _contexto;

    public UbigeoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Ubigeo>> Listar() => await _contexto.Set<Ubigeo>().ToListAsync();

    public async Task<Ubigeo?> Obtener(string IdUbigeoParam)
    {
        return await _contexto.Set<Ubigeo>().FindAsync(IdUbigeoParam);
    }

    public async Task Crear(Ubigeo registro)
    {
        _contexto.Set<Ubigeo>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Ubigeo registro)
    {
        _contexto.Set<Ubigeo>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Ubigeo registro)
    {
        _contexto.Set<Ubigeo>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
