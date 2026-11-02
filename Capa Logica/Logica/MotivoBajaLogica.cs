using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class MotivoBajaLogica : LogicaBase<MotivoBaja>
{
    public MotivoBajaLogica(IRepositorio<MotivoBaja> repositorio) : base(repositorio) { }

    public Task<MotivoBaja?> Obtener(int idMotivoBajaParam) => Repositorio.Obtener(idMotivoBajaParam);
}
