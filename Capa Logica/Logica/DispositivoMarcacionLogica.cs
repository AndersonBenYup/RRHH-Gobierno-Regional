using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class DispositivoMarcacionLogica : LogicaBase<DispositivoMarcacion>
{
    public DispositivoMarcacionLogica(IRepositorio<DispositivoMarcacion> repositorio) : base(repositorio) { }

    public Task<DispositivoMarcacion?> Obtener(string CodigoParam) => Repositorio.Obtener(CodigoParam);
}
