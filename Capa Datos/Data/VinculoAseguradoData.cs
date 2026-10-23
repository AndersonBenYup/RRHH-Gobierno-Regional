using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class VinculoAseguradoData
{
    private readonly EvoRRLDbContext _contexto;

    public VinculoAseguradoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<VinculoAsegurado>> Listar() => await _contexto.Set<VinculoAsegurado>().ToListAsync();

    public async Task<VinculoAsegurado?> Obtener(int IdVinculoAseguradoParam)
    {
        return await _contexto.Set<VinculoAsegurado>().FindAsync(IdVinculoAseguradoParam);
    }

    public async Task Crear(VinculoAsegurado registro)
    {
        _contexto.Set<VinculoAsegurado>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(VinculoAsegurado registro)
    {
        _contexto.Set<VinculoAsegurado>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(VinculoAsegurado registro)
    {
        _contexto.Set<VinculoAsegurado>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
