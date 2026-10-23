using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class HorarioLogica
{
    private readonly HorarioData _data;

    public HorarioLogica(HorarioData data) => _data = data;

    public async Task<List<Horario>> Listar() => await _data.Listar();
    public async Task<Horario?> Obtener(int idHorarioParam) => await _data.Obtener(idHorarioParam);
    public async Task Crear(Horario registro) => await _data.Crear(registro);
    public async Task Actualizar(Horario registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Horario registro) => await _data.Eliminar(registro);
}

