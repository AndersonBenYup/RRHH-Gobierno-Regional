using Microsoft.EntityFrameworkCore;
using Capa_Entidades;
namespace Capa_Datos;
public class EvoRRLDbContext : DbContext
{
    public EvoRRLDbContext(DbContextOptions<EvoRRLDbContext> options) : base(options) { }
        public DbSet<AFP> AFP { get; set; } = null!;
        public DbSet<Bitacora> Bitacora { get; set; } = null!;
        public DbSet<AguaViv> AguaViv { get; set; } = null!;
        public DbSet<AlumbradoViv> AlumbradoViv { get; set; } = null!;
        public DbSet<Asistencia> Asistencia { get; set; } = null!;
        public DbSet<Capacitacion> Capacitacion { get; set; } = null!;
        public DbSet<Cargo> Cargo { get; set; } = null!;
        public DbSet<Comportamiento> Comportamiento { get; set; } = null!;
        public DbSet<ConservacionViv> ConservacionViv { get; set; } = null!;
        public DbSet<Discapacidad> Discapacidad { get; set; } = null!;
        public DbSet<Empleado> Empleado { get; set; } = null!;
        public DbSet<Empleado_Area> Empleado_Area { get; set; } = null!;
        public DbSet<EstadoCivil> EstadoCivil { get; set; } = null!;
        public DbSet<EstadoEstudio> EstadoEstudio { get; set; } = null!;
        public DbSet<EstructOrganiz> EstructOrganiz { get; set; } = null!;
        public DbSet<EstudiosRealizado> EstudiosRealizado { get; set; } = null!;
        public DbSet<ExcretasViv> ExcretasViv { get; set; } = null!;
        public DbSet<ExpLaboral> ExpLaboral { get; set; } = null!;
        public DbSet<Familiar> Familiar { get; set; } = null!;
        public DbSet<Horario> Horario { get; set; } = null!;
        public DbSet<HorarioTemporal> HorarioTemporal { get; set; } = null!;
        public DbSet<Local> Local { get; set; } = null!;
        public DbSet<Marcacion> Marcacion { get; set; } = null!;
        public DbSet<MaterialViv> MaterialViv { get; set; } = null!;
        public DbSet<Motivo_Perm> Motivo_Perm { get; set; } = null!;
        public DbSet<MotivoBaja> MotivoBaja { get; set; } = null!;
        public DbSet<Nacionalidad> Nacionalidad { get; set; } = null!;
        public DbSet<PeriodoLaboral> PeriodoLaboral { get; set; } = null!;
        public DbSet<Permiso> Permiso { get; set; } = null!;
        public DbSet<Persona> Persona { get; set; } = null!;
        public DbSet<RegAsisDiario> RegAsisDiario { get; set; } = null!;
        public DbSet<RegimenPension> RegimenPension { get; set; } = null!;
        public DbSet<Resolucion> Resolucion { get; set; } = null!;
        public DbSet<ResponsableXUO> ResponsableXUO { get; set; } = null!;
        public DbSet<Rol> Rol { get; set; } = null!;
        public DbSet<Rol_Acceso> Rol_Acceso { get; set; } = null!;
        public DbSet<Rotacion> Rotacion { get; set; } = null!;
        public DbSet<RRHH_AcudeEnferm> RRHH_AcudeEnferm { get; set; } = null!;
        public DbSet<RRHH_Asegurado> RRHH_Asegurado { get; set; } = null!;
        public DbSet<RRHH_AspSocio> RRHH_AspSocio { get; set; } = null!;
        public DbSet<RRHH_DinamicaFamiliar> RRHH_DinamicaFamiliar { get; set; } = null!;
        public DbSet<RRHH_Feriado> RRHH_Feriado { get; set; } = null!;
        public DbSet<RRHH_FuncFam> RRHH_FuncFam { get; set; } = null!;
        public DbSet<RRHH_RelacHermano> RRHH_RelacHermano { get; set; } = null!;
        public DbSet<RRHH_RelacPareja> RRHH_RelacPareja { get; set; } = null!;
        public DbSet<RRHH_RelacPH> RRHH_RelacPH { get; set; } = null!;
        public DbSet<RRHH_Salud> RRHH_Salud { get; set; } = null!;
        public DbSet<RRHH_TipoFamilia> RRHH_TipoFamilia { get; set; } = null!;
        public DbSet<SistemaOpcion> SistemaOpcion { get; set; } = null!;
        public DbSet<TenenciaViv> TenenciaViv { get; set; } = null!;
        public DbSet<TipoComportamiento> TipoComportamiento { get; set; } = null!;
        public DbSet<TipoDocID> TipoDocID { get; set; } = null!;
        public DbSet<TipoEstudio> TipoEstudio { get; set; } = null!;
        public DbSet<TipoFamiliar> TipoFamiliar { get; set; } = null!;
        public DbSet<TipoInstitucion> TipoInstitucion { get; set; } = null!;
        public DbSet<TipoPermiso> TipoPermiso { get; set; } = null!;
        public DbSet<TipoResolucion> TipoResolucion { get; set; } = null!;
        public DbSet<TipoSangre> TipoSangre { get; set; } = null!;
        public DbSet<TipoTrabajador> TipoTrabajador { get; set; } = null!;
        public DbSet<TipoViv> TipoViv { get; set; } = null!;
        public DbSet<TitulosEmpleado> TitulosEmpleado { get; set; } = null!;
        public DbSet<UbicacionViv> UbicacionViv { get; set; } = null!;
        public DbSet<Ubigeo> Ubigeo { get; set; } = null!;
        public DbSet<Usuario> Usuario { get; set; } = null!;
        public DbSet<Usuario_Rol> Usuario_Rol { get; set; } = null!;
        public DbSet<ViveCon> ViveCon { get; set; } = null!;
        public DbSet<Vivienda> Vivienda { get; set; } = null!;
        public DbSet<Departamento> Departamento { get; set; } = null!;
        public DbSet<Provincia> Provincia { get; set; } = null!;
        public DbSet<Distrito> Distrito { get; set; } = null!;
        public DbSet<ServicioViv> ServicioViv { get; set; } = null!;
        public DbSet<ViviendaServicio> ViviendaServicio { get; set; } = null!;
        public DbSet<VinculoAsegurado> VinculoAsegurado { get; set; } = null!;
        public DbSet<RRHH_FuncFamPregunta> RRHH_FuncFamPregunta { get; set; } = null!;
        public DbSet<RRHH_FuncFamRespuesta> RRHH_FuncFamRespuesta { get; set; } = null!;
        public DbSet<EstadoAsistencia> EstadoAsistencia { get; set; } = null!;
        public DbSet<TipoMarcacion> TipoMarcacion { get; set; } = null!;
        public DbSet<DispositivoMarcacion> DispositivoMarcacion { get; set; } = null!;
        public DbSet<EmpleadoAreaActual> EmpleadoAreaActual { get; set; } = null!;
        public DbSet<EmpleadoResumenFamiliar> EmpleadoResumenFamiliar { get; set; } = null!;
        public DbSet<FamiliarDetalle> FamiliarDetalle { get; set; } = null!;
        public DbSet<EstructOrganizDotacion> EstructOrganizDotacion { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AFP>(entity =>
            {
            });
            modelBuilder.Entity<Bitacora>(entity =>
            {
            });
            modelBuilder.Entity<AguaViv>(entity =>
            {
            });
            modelBuilder.Entity<AlumbradoViv>(entity =>
            {
            });
            modelBuilder.Entity<Asistencia>(entity =>
            {
                entity.HasKey(e => new { e.Fecha, e.IdEmpleado });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.idHorario);
            });
            modelBuilder.Entity<Capacitacion>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<Cargo>(entity =>
            {
            });
            modelBuilder.Entity<Comportamiento>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<TipoComportamiento>().WithMany().HasForeignKey(e => e.idTipoComportamiento);
            });
            modelBuilder.Entity<ConservacionViv>(entity =>
            {
            });
            modelBuilder.Entity<Discapacidad>(entity =>
            {
            });
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasOne<Persona>().WithMany().HasForeignKey(e => e.IdPersona);
                entity.HasOne<TipoTrabajador>().WithMany().HasForeignKey(e => e.IdTipoTrabajador);
                entity.HasOne<ViveCon>().WithMany().HasForeignKey(e => e.IdViveCon);
                entity.HasOne<RegimenPension>().WithMany().HasForeignKey(e => e.IdRegimenPen);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.idHorario);
                entity.HasOne<Local>().WithMany().HasForeignKey(e => e.idLocal);
                entity.HasOne<AFP>().WithMany().HasForeignKey(e => e.IdAFP);
                entity.HasOne<Cargo>().WithMany().HasForeignKey(e => e.IdCargo);
            });
            modelBuilder.Entity<Empleado_Area>(entity =>
            {
                entity.HasKey(e => new { e.Year, e.idAreaOrganiz, e.IdEmpleado });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstructOrganiz>().WithMany().HasForeignKey(e => new { e.Year, e.idAreaOrganiz });
            });
            modelBuilder.Entity<EstadoCivil>(entity =>
            {
            });
            modelBuilder.Entity<EstadoEstudio>(entity =>
            {
            });
            modelBuilder.Entity<EstructOrganiz>(entity =>
            {
                entity.HasKey(e => new { e.Year, e.idAreaOrganiz });
                entity.HasOne<EstructOrganiz>().WithMany().HasPrincipalKey(e => new { e.Year, e.idAreaOrganiz }).HasForeignKey(e => new { e.YearDe, e.idDependeDe });
            });
            modelBuilder.Entity<EstudiosRealizado>(entity =>
            {
                entity.HasKey(e => new { e.IdTipoEstudios, e.IdEmpleado, e.Correlativo });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstadoEstudio>().WithMany().HasForeignKey(e => e.IdEstadoEstudio);
            });
            modelBuilder.Entity<ExcretasViv>(entity =>
            {
            });
            modelBuilder.Entity<ExpLaboral>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleado, e.IdExpLab });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<TipoInstitucion>().WithMany().HasForeignKey(e => e.IdTipoInstitucion);
            });
            modelBuilder.Entity<Familiar>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleado, e.IdPersona });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Persona>().WithMany().HasForeignKey(e => e.IdPersona);
                entity.HasOne<TipoFamiliar>().WithMany().HasForeignKey(e => e.IdTipoFam);
            });
            modelBuilder.Entity<Horario>(entity =>
            {
            });
            modelBuilder.Entity<HorarioTemporal>(entity =>
            {
                entity.HasKey(e => new { e.idEmpleado, e.N });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.HorarioAsignado);
            });
            modelBuilder.Entity<Local>(entity =>
            {
            });
            modelBuilder.Entity<Marcacion>(entity =>
            {
                entity.HasKey(e => new { e.Fecha, e.IdEmpleado });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.idHorario);
                entity.HasOne<TipoMarcacion>().WithMany().HasForeignKey(e => e.TipoMarcacion);
                entity.HasOne<DispositivoMarcacion>().WithMany().HasForeignKey(e => e.CodDispositivo);
            });
            modelBuilder.Entity<MaterialViv>(entity =>
            {
            });
            modelBuilder.Entity<Motivo_Perm>(entity =>
            {
                entity.HasOne<TipoPermiso>().WithMany().HasForeignKey(e => e.idTipoPermiso);
            });
            modelBuilder.Entity<MotivoBaja>(entity =>
            {
            });
            modelBuilder.Entity<Nacionalidad>(entity =>
            {
            });
            modelBuilder.Entity<PeriodoLaboral>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleado, e.NroPeriodo });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleado, e.NPermiso });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Motivo_Perm>().WithMany().HasForeignKey(e => e.idMotivo);
            });
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasOne<Distrito>().WithMany().HasForeignKey(e => e.IdUbigeo);
                entity.HasOne<TipoDocID>().WithMany().HasForeignKey(e => e.TipoDocID);
                entity.HasOne<EstadoCivil>().WithMany().HasForeignKey(e => e.IdEstadoCivil);
                entity.HasOne<Nacionalidad>().WithMany().HasForeignKey(e => e.IdNacionalidad);
                entity.HasOne<TipoSangre>().WithMany().HasForeignKey(e => e.IdTipoSangre);
                entity.HasOne<Discapacidad>().WithMany().HasForeignKey(e => e.IdDiscapacidad);
            });
            modelBuilder.Entity<RegAsisDiario>(entity =>
            {
                entity.HasKey(e => new { e.Fecha, e.IdEmpleado });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstadoAsistencia>().WithMany().HasForeignKey(e => e.Estado);
            });
            modelBuilder.Entity<RegimenPension>(entity =>
            {
            });
            modelBuilder.Entity<Resolucion>(entity =>
            {
                entity.HasKey(e => new { e.idEmpleado, e.idResolucion });
                entity.HasOne<TipoResolucion>().WithMany().HasForeignKey(e => e.idTipoResolucion);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
            });
            modelBuilder.Entity<ResponsableXUO>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstructOrganiz>().WithMany().HasForeignKey(e => new { e.Year, e.idAreaOrganiz });
            });
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasOne<SistemaOpcion>().WithMany().HasForeignKey(e => e.IdSistema);
            });
            modelBuilder.Entity<Rol_Acceso>(entity =>
            {
                entity.HasKey(e => new { e.IdRol, e.IdSistemaOpcion });
                entity.HasOne<Rol>().WithMany().HasForeignKey(e => e.IdRol);
                entity.HasOne<SistemaOpcion>().WithMany().HasForeignKey(e => e.IdSistemaOpcion);
            });
            modelBuilder.Entity<Rotacion>(entity =>
            {
                entity.HasKey(e => new { e.idEmpleado, e.idRotacion });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
                entity.HasOne<EstructOrganiz>().WithMany().HasForeignKey(e => new { e.Year, e.idAreaOrganiz });
            });
            modelBuilder.Entity<RRHH_AcudeEnferm>(entity =>
            {
            });
            modelBuilder.Entity<RRHH_Asegurado>(entity =>
            {
                entity.HasKey(e => new { e.idEmpleado, e.NroBeneficiario });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
                entity.HasOne<MotivoBaja>().WithMany().HasForeignKey(e => e.idMotivoBaja);
                entity.HasOne<Persona>().WithMany().HasForeignKey(e => e.idPersona);
                entity.HasOne<VinculoAsegurado>().WithMany().HasForeignKey(e => e.IdVinculoAsegurado);
            });
            modelBuilder.Entity<RRHH_AspSocio>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RRHH_DinamicaFamiliar>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RRHH_Feriado>(entity =>
            {
            });
            modelBuilder.Entity<RRHH_FuncFam>(entity =>
            {
                entity.ToView("vw_RRHH_FuncFam");
            });
            modelBuilder.Entity<RRHH_RelacHermano>(entity =>
            {
            });
            modelBuilder.Entity<RRHH_RelacPareja>(entity =>
            {
            });
            modelBuilder.Entity<RRHH_RelacPH>(entity =>
            {
            });
            modelBuilder.Entity<RRHH_Salud>(entity =>
            {
                entity.HasOne<RRHH_AcudeEnferm>().WithMany().HasForeignKey(e => e.idAcudeEnferm);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RRHH_TipoFamilia>(entity =>
            {
            });
            modelBuilder.Entity<SistemaOpcion>(entity =>
            {
            });
            modelBuilder.Entity<TenenciaViv>(entity =>
            {
            });
            modelBuilder.Entity<TipoComportamiento>(entity =>
            {
            });
            modelBuilder.Entity<TipoDocID>(entity =>
            {
            });
            modelBuilder.Entity<TipoEstudio>(entity =>
            {
            });
            modelBuilder.Entity<TipoFamiliar>(entity =>
            {
            });
            modelBuilder.Entity<TipoInstitucion>(entity =>
            {
            });
            modelBuilder.Entity<TipoPermiso>(entity =>
            {
            });
            modelBuilder.Entity<TipoResolucion>(entity =>
            {
            });
            modelBuilder.Entity<TipoSangre>(entity =>
            {
            });
            modelBuilder.Entity<TipoTrabajador>(entity =>
            {
            });
            modelBuilder.Entity<TipoViv>(entity =>
            {
            });
            modelBuilder.Entity<TitulosEmpleado>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<UbicacionViv>(entity =>
            {
            });
            modelBuilder.Entity<Ubigeo>(entity =>
            {
                entity.ToView("vw_Ubigeo");
            });
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<Usuario_Rol>(entity =>
            {
                entity.HasKey(e => new { e.IdUsuario, e.IdRol });
                entity.HasOne<Rol>().WithMany().HasForeignKey(e => e.IdRol);
                entity.HasOne<Usuario>().WithMany().HasForeignKey(e => e.IdUsuario);
            });
            modelBuilder.Entity<ViveCon>(entity =>
            {
            });
            modelBuilder.Entity<Vivienda>(entity =>
            {
                entity.HasOne<AguaViv>().WithMany().HasForeignKey(e => e.IdAgua);
                entity.HasOne<AlumbradoViv>().WithMany().HasForeignKey(e => e.IdAlumbrado);
                entity.HasOne<ConservacionViv>().WithMany().HasForeignKey(e => e.IdConservacion);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<ExcretasViv>().WithMany().HasForeignKey(e => e.IdExcretas);
                entity.HasOne<MaterialViv>().WithMany().HasForeignKey(e => e.IdMaterial);
                entity.HasOne<TenenciaViv>().WithMany().HasForeignKey(e => e.IdTenencia);
                entity.HasOne<TipoViv>().WithMany().HasForeignKey(e => e.IdTipo);
                entity.HasOne<UbicacionViv>().WithMany().HasForeignKey(e => e.IdUbicacion);
            });
            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.HasKey(e => new { e.CodDepartamento, e.CodProvincia });
                entity.HasOne<Departamento>().WithMany().HasForeignKey(e => e.CodDepartamento);
            });
            modelBuilder.Entity<Distrito>(entity =>
            {
                entity.HasOne<Provincia>().WithMany().HasForeignKey(e => new { e.CodDepartamento, e.CodProvincia });
            });
            modelBuilder.Entity<ViviendaServicio>(entity =>
            {
                entity.HasKey(e => new { e.IdVivienda, e.IdServicio });
                entity.HasOne<Vivienda>().WithMany().HasForeignKey(e => e.IdVivienda);
                entity.HasOne<ServicioViv>().WithMany().HasForeignKey(e => e.IdServicio);
            });
            modelBuilder.Entity<RRHH_FuncFamPregunta>(entity =>
            {
                entity.Property(e => e.IdPregunta).ValueGeneratedNever();
            });
            modelBuilder.Entity<RRHH_FuncFamRespuesta>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleado, e.IdPregunta });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<RRHH_FuncFamPregunta>().WithMany().HasForeignKey(e => e.IdPregunta);
            });
            // Vistas de solo lectura con los datos derivados que antes se guardaban en las tablas.
            modelBuilder.Entity<EmpleadoAreaActual>(entity => entity.ToView("vw_EmpleadoAreaActual"));
            modelBuilder.Entity<EmpleadoResumenFamiliar>(entity => entity.ToView("vw_EmpleadoResumenFamiliar"));
            modelBuilder.Entity<FamiliarDetalle>(entity =>
            {
                entity.ToView("vw_FamiliarDetalle");
                entity.HasKey(e => new { e.IdEmpleado, e.IdPersona });
            });
            modelBuilder.Entity<EstructOrganizDotacion>(entity =>
            {
                entity.ToView("vw_EstructOrganizDotacion");
                entity.HasKey(e => new { e.Year, e.idAreaOrganiz });
            });
    }
}
