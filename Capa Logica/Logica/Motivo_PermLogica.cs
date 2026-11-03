using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class Motivo_PermLogica : LogicaBase<Motivo_Perm>
{
    public Motivo_PermLogica(IRepositorio<Motivo_Perm> repositorio) : base(repositorio) { }

    public Task<Motivo_Perm?> Obtener(int idMotivo) => Repositorio.Obtener(idMotivo);
}
