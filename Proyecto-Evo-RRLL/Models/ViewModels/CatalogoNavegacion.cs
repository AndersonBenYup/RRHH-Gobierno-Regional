namespace Proyecto_Evo_RRLL.Models.ViewModels;

public static class CatalogoNavegacion
{
    public static readonly IReadOnlyDictionary<string, string> Items = new Dictionary<string, string>
    {
        ["Cargo"] = "Cargos",
        ["MotivoBaja"] = "Motivos de Baja",
        ["EstadoCivil"] = "Estado Civil",
        ["TipoDocID"] = "Tipos de Documento",
        ["TipoTrabajador"] = "Tipos de Trabajador",
        ["TipoSangre"] = "Grupos Sanguíneos",
        ["Nacionalidad"] = "Nacionalidades",
        ["RegimenPension"] = "Regímenes de Pensión",
        ["ViveCon"] = "¿Vive con?",
        ["TipoPermiso"] = "Tipos de Permiso",
        ["Motivo_Perm"] = "Motivos de Permiso",
        ["TipoEstudio"] = "Tipos de Estudio",
        ["TipoInstitucion"] = "Tipos de Institución",
        ["Discapacidad"] = "Discapacidades",
        ["TipoComportamiento"] = "Tipos de Comportamiento",
        ["TipoResolucion"] = "Tipos de Resolución",
        ["TipoFamiliar"] = "Tipos de Familiar",
        ["RRHH_TipoFamilia"] = "Tipos de Familia",
        ["RRHH_RelacPareja"] = "Relación con Pareja",
        ["RRHH_RelacPH"] = "Relación con Jefe de Hogar",
        ["RRHH_RelacHermano"] = "Relación con Hermanos",
        ["AFP"] = "AFPs",
        ["AguaViv"] = "Servicio de Agua",
        ["AlumbradoViv"] = "Servicio de Alumbrado",
        ["ConservacionViv"] = "Conservación de Vivienda",
        ["ExcretasViv"] = "Servicio de Excretas",
        ["MaterialViv"] = "Material de Vivienda",
        ["TenenciaViv"] = "Tenencia de Vivienda",
        ["TipoViv"] = "Tipos de Vivienda",
        ["UbicacionViv"] = "Ubicación de Vivienda",
        ["RRHH_AcudeEnferm"] = "A quién acudir en enfermedad",
        ["Horario"] = "Horarios",
        ["VinculoAsegurado"] = "Vínculos de Asegurado",
        ["ServicioViv"] = "Servicios de Vivienda"
    };
}