using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class MaterialVivData
{
    private readonly EvoRRLDbContext _contexto;

    public MaterialVivData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<MaterialViv>> Listar() => await _contexto.Set<MaterialViv>().ToListAsync();

    public async Task<MaterialViv?> Obtener(int IdMaterialParam)
    {
        return await _contexto.Set<MaterialViv>().FindAsync(IdMaterialParam);
    }

    public async Task Crear(MaterialViv registro)
    {
        _contexto.Set<MaterialViv>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(MaterialViv registro)
    {
        _contexto.Set<MaterialViv>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(MaterialViv registro)
    {
        _contexto.Set<MaterialViv>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
