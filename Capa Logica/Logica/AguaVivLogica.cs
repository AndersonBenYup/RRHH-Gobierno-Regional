using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class AguaVivLogica : LogicaBase<AguaViv>
{
    public AguaVivLogica(IRepositorio<AguaViv> repositorio) : base(repositorio) { }

    public Task<AguaViv?> Obtener(int idAgua) => Repositorio.Obtener(idAgua);
}
