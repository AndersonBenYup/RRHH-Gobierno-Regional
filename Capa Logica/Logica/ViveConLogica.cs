using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ViveConLogica : LogicaBase<ViveCon>
{
    public ViveConLogica(IRepositorio<ViveCon> repositorio) : base(repositorio) { }

    public Task<ViveCon?> Obtener(int IdViveConParam) => Repositorio.Obtener(IdViveConParam);
}
