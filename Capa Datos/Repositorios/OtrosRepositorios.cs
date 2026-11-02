using Capa_Entidades;
using Capa_Entidades.Contratos;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos.Repositorios;

public class PersonaRepositorio : RepositorioBase<Persona>, IPersonaRepositorio
{
    public PersonaRepositorio(EvoRRLDbContext contexto) : base(contexto) { }

    /// <summary>SC-008: true si otra persona (distinta de <paramref name="excluirIdPersona"/>) usa ese documento.</summary>
    public async Task<bool> ExisteDocumento(string numDoc, int? excluirIdPersona = null)
    {
        var numero = numDoc.Trim();
        return await Tabla.AnyAsync(p =>
            p.NumDocID != null && p.NumDocID.Trim() == numero
            && (excluirIdPersona == null || p.IdPersona != excluirIdPersona));
    }
}

public class UsuarioRepositorio : RepositorioBase<Usuario>, IUsuarioRepositorio
{
    public UsuarioRepositorio(EvoRRLDbContext contexto) : base(contexto) { }

    public async Task<Usuario?> ObtenerPorLogin(string login)
        => await Tabla.FirstOrDefaultAsync(u => u.Login != null && u.Login.Trim() == login);

    public async Task<List<Rol>> ObtenerRoles(int idUsuario)
        => await Contexto.Set<Usuario_Rol>()
            .Where(ur => ur.IdUsuario == idUsuario)
            .Join(Contexto.Set<Rol>(), ur => ur.IdRol, r => r.IdRol, (ur, r) => r)
            .ToListAsync();
}

public class AsistenciaRepositorio : RepositorioBase<Asistencia>, IAsistenciaRepositorio
{
    public AsistenciaRepositorio(EvoRRLDbContext contexto) : base(contexto) { }

    public async Task<int> ContarEntreFechas(DateTime desde, DateTime hasta)
        => await Tabla.CountAsync(a => a.Fecha.Date >= desde.Date && a.Fecha.Date <= hasta.Date);

    public async Task<List<Asistencia>> ListarEntreFechas(DateTime desde, DateTime hastaExclusivo)
        => await Tabla.AsNoTracking().Where(a => a.Fecha >= desde && a.Fecha < hastaExclusivo).ToListAsync();
}

/// <summary>Bitácora tolerante: si la tabla aún no existe en la BD, no rompe la operación principal.</summary>
public class BitacoraRepositorio : IBitacoraRepositorio
{
    private readonly EvoRRLDbContext _contexto;

    public BitacoraRepositorio(EvoRRLDbContext contexto) => _contexto = contexto;

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
            // La bitácora nunca debe impedir el flujo principal del negocio.
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

    /// <summary>SC-009: fechas de un evento de un usuario desde una fecha (para contar intentos fallidos).</summary>
    public async Task<List<DateTime>> Fechas(string usuario, string accion, DateTime desde)
    {
        var u = usuario.Trim();
        return await _contexto.Set<Bitacora>().AsNoTracking()
            .Where(b => b.Usuario == u && b.Accion == accion && b.Fecha > desde)
            .Select(b => b.Fecha)
            .ToListAsync();
    }

    /// <summary>SC-009: fecha del último evento de un usuario entre las acciones indicadas.</summary>
    public async Task<DateTime?> UltimaFecha(string usuario, string[] acciones)
    {
        var u = usuario.Trim();
        return await _contexto.Set<Bitacora>().AsNoTracking()
            .Where(b => b.Usuario == u && b.Accion != null && acciones.Contains(b.Accion))
            .MaxAsync(b => (DateTime?)b.Fecha);
    }
}
