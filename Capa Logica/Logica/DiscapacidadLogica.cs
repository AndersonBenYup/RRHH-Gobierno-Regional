using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class DiscapacidadLogica : LogicaBase<Discapacidad>
{
    public DiscapacidadLogica(IRepositorio<Discapacidad> repositorio) : base(repositorio) { }

    public Task<Discapacidad?> Obtener(int idDiscapacidad) => Repositorio.Obtener(idDiscapacidad);
}
