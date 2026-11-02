using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class NacionalidadLogica : LogicaBase<Nacionalidad>
{
    public NacionalidadLogica(IRepositorio<Nacionalidad> repositorio) : base(repositorio) { }

    public Task<Nacionalidad?> Obtener(int IdNacionalidadParam) => Repositorio.Obtener(IdNacionalidadParam);
}
