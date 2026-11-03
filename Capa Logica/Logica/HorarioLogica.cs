using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class HorarioLogica : LogicaBase<Horario>
{
    public HorarioLogica(IRepositorio<Horario> repositorio) : base(repositorio) { }

    public Task<Horario?> Obtener(int idHorario) => Repositorio.Obtener(idHorario);
}
