using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class MaterialVivLogica : LogicaBase<MaterialViv>
{
    public MaterialVivLogica(IRepositorio<MaterialViv> repositorio) : base(repositorio) { }

    public Task<MaterialViv?> Obtener(int idMaterial) => Repositorio.Obtener(idMaterial);
}
