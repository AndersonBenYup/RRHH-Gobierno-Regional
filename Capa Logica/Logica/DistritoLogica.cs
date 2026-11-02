using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class DistritoLogica : LogicaBase<Distrito>
{
    public DistritoLogica(IRepositorio<Distrito> repositorio) : base(repositorio) { }

    public Task<Distrito?> Obtener(string IdUbigeoParam) => Repositorio.Obtener(IdUbigeoParam);
}
