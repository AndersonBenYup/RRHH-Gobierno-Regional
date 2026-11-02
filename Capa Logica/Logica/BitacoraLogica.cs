using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class BitacoraLogica
{
    private readonly IBitacoraRepositorio _bitacora;

    public BitacoraLogica(IBitacoraRepositorio bitacora) => _bitacora = bitacora;

    public Task Registrar(string usuario, string accion, string entidad, string? detalle = null)
        => _bitacora.Registrar(usuario, accion, entidad, detalle);

    public Task<List<Bitacora>> Ultimas(int cantidad) => _bitacora.Ultimas(cantidad);
}
