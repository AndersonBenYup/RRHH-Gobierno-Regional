using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class CargoLogica : LogicaBase<Cargo>
{
    public CargoLogica(IRepositorio<Cargo> repositorio) : base(repositorio) { }

    public Task<Cargo?> Obtener(int IdCargoParam) => Repositorio.Obtener(IdCargoParam);
}
