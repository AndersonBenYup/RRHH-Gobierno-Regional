using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class DepartamentoLogica : LogicaBase<Departamento>
{
    public DepartamentoLogica(IRepositorio<Departamento> repositorio) : base(repositorio) { }

    public Task<Departamento?> Obtener(string codDepartamento) => Repositorio.Obtener(codDepartamento);
}
