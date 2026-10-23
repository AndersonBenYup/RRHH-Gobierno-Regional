using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class BitacoraData
{
    private readonly EvoRRLDbContext _contexto;

    public BitacoraData(EvoRRLDbContext contexto) => _contexto = contexto;

    // Registro tolerante: si la tabla aun no existe en la BD, no se rompe la operacion principal.
    public async Task Registrar(string usuario, string accion, string entidad, string? detalle = null)
    {
        try
        {
            _contexto.Set<Bitacora>().Add(new Bitacora
            {
                Fecha = DateTime.Now,
                Usuario = usuario,
                Accion = accion,
                Entidad = entidad,
                Detalle = detalle
            });
            await _contexto.SaveChangesAsync();
        }
        catch
        {
            // La bitacora nunca debe impedir el flujo principal del negocio.
        }
    }

    public async Task<List<Bitacora>> Ultimas(int cantidad)
    {
        try
        {
            return await _contexto.Set<Bitacora>()
                .OrderByDescending(b => b.Fecha)
                .Take(cantidad)
                .ToListAsync();
        }
        catch
        {
            return new List<Bitacora>();
        }
    }
}