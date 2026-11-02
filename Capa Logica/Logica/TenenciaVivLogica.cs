using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TenenciaVivLogica : LogicaBase<TenenciaViv>
{
    public TenenciaVivLogica(IRepositorio<TenenciaViv> repositorio) : base(repositorio) { }

    public Task<TenenciaViv?> Obtener(int IdTenenciaParam) => Repositorio.Obtener(IdTenenciaParam);
}
