using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ServicioVivLogica : LogicaBase<ServicioViv>
{
    public ServicioVivLogica(IRepositorio<ServicioViv> repositorio) : base(repositorio) { }

    public Task<ServicioViv?> Obtener(int idServicio) => Repositorio.Obtener(idServicio);
}
