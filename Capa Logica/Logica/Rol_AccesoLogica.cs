using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class Rol_AccesoLogica : LogicaBase<Rol_Acceso>
{
    public Rol_AccesoLogica(IRepositorio<Rol_Acceso> repositorio) : base(repositorio) { }

    public Task<Rol_Acceso?> Obtener(int idRol, string idSistemaOpcion) => Repositorio.Obtener(idRol, idSistemaOpcion);
}
