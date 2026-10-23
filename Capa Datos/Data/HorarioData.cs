using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class HorarioData
{
    private readonly EvoRRLDbContext _contexto;

    public HorarioData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Horario>> Listar() => await _contexto.Set<Horario>().ToListAsync();

    public async Task<Horario?> Obtener(int idHorarioParam)
    {
        return await _contexto.Set<Horario>().FindAsync(idHorarioParam);
    }

    public async Task Crear(Horario registro)
    {
        _contexto.Set<Horario>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Horario registro)
    {
        _contexto.Set<Horario>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Horario registro)
    {
        _contexto.Set<Horario>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
