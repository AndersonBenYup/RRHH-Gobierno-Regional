using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ExcretasVivData
{
    private readonly EvoRRLDbContext _contexto;

    public ExcretasVivData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<ExcretasViv>> Listar() => await _contexto.Set<ExcretasViv>().ToListAsync();

    public async Task<ExcretasViv?> Obtener(int IdExcretasParam)
    {
        return await _contexto.Set<ExcretasViv>().FindAsync(IdExcretasParam);
    }

    public async Task Crear(ExcretasViv registro)
    {
        _contexto.Set<ExcretasViv>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(ExcretasViv registro)
    {
        _contexto.Set<ExcretasViv>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(ExcretasViv registro)
    {
        _contexto.Set<ExcretasViv>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
