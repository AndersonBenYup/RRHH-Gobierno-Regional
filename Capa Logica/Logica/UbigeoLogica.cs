using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class UbigeoLogica : LogicaBase<Ubigeo>
{
    public UbigeoLogica(IRepositorio<Ubigeo> repositorio) : base(repositorio) { }

    public Task<Ubigeo?> Obtener(string IdUbigeoParam) => Repositorio.Obtener(IdUbigeoParam);
}
