using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class BitacoraLogica
{
    private readonly BitacoraData _data;

    public BitacoraLogica(BitacoraData data) => _data = data;

    public async Task Registrar(string usuario, string accion, string entidad, string? detalle = null) => await _data.Registrar(usuario, accion, entidad, detalle);
    public async Task<List<Bitacora>> Ultimas(int cantidad) => await _data.Ultimas(cantidad);
}

