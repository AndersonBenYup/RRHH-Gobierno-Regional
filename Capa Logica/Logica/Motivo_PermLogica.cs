using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class Motivo_PermLogica
{
    private readonly Motivo_PermData _data;

    public Motivo_PermLogica(Motivo_PermData data) => _data = data;

    public async Task<List<Motivo_Perm>> Listar() => await _data.Listar();
    public async Task<Motivo_Perm?> Obtener(int idMotivoParam) => await _data.Obtener(idMotivoParam);
    public async Task Crear(Motivo_Perm registro) => await _data.Crear(registro);
    public async Task Actualizar(Motivo_Perm registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Motivo_Perm registro) => await _data.Eliminar(registro);
}

