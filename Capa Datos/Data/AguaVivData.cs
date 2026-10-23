using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class AguaVivData
{
    private readonly EvoRRLDbContext _contexto;

    public AguaVivData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<AguaViv>> Listar() => await _contexto.Set<AguaViv>().ToListAsync();

    public async Task<AguaViv?> Obtener(int IdAguaParam)
    {
        return await _contexto.Set<AguaViv>().FindAsync(IdAguaParam);
    }

    public async Task Crear(AguaViv registro)
    {
        _contexto.Set<AguaViv>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(AguaViv registro)
    {
        _contexto.Set<AguaViv>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(AguaViv registro)
    {
        _contexto.Set<AguaViv>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
