using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ResolucionLogica : LogicaBase<Resolucion>
{
    public ResolucionLogica(IRepositorio<Resolucion> repositorio) : base(repositorio) { }

    public Task<Resolucion?> Obtener(int idEmpleado, int idResolucion) => Repositorio.Obtener(idEmpleado, idResolucion);
}
