using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class UbicacionVivLogica : LogicaBase<UbicacionViv>
{
    public UbicacionVivLogica(IRepositorio<UbicacionViv> repositorio) : base(repositorio) { }

    public Task<UbicacionViv?> Obtener(int idUbicacion) => Repositorio.Obtener(idUbicacion);
}
