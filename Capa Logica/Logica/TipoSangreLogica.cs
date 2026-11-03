using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoSangreLogica : LogicaBase<TipoSangre>
{
    public TipoSangreLogica(IRepositorio<TipoSangre> repositorio) : base(repositorio) { }

    public Task<TipoSangre?> Obtener(int idTipoSangre) => Repositorio.Obtener(idTipoSangre);
}
