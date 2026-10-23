/* ============================================================ 
   BD_CMI_LIMPIA_NuevoProyecto.sql 
   Proyecto nuevo RRHH - Bd limpia [RRHH_Nuevo] a partir de CMI 
   - 76 tablas conservadas de 100 
   - 24 tablas muertas eliminadas (aspnet_*, marcaciones, audit, etc.) 
   - 5 triggers eliminados que referenciaban tablas caidas 
   - FKs originales internas conservadas + FKs nuevas agregadas 
   - Encoding UTF-8 (con BOM) 
   ============================================================ */

IF DB_ID('RRHH_Nuevo') IS NOT NULL BEGIN
	ALTER DATABASE [RRHH_Nuevo] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE [RRHH_Nuevo]
END
GO
CREATE DATABASE [RRHH_Nuevo]
GO
USE [RRHH_Nuevo]
GO

GO
/****** Object:  Table [dbo].[AFP]    Script Date: 06/09/2026 13:08:14 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AFP](
	[IdAFP] [int] NOT NULL,
	[DescripAFP] [varchar](50) NULL,
	[DireccAFP] [varchar](200) NULL,
	[TelefAFP] [varchar](12) NULL,
	[NomContAFP] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AguaViv]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AguaViv](
	[IdAgua] [int] NOT NULL,
	[DescripAgua] [varchar](33) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlumbradoViv]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlumbradoViv](
	[IdAlumbrado] [int] NOT NULL,
	[DescripAlumbrado] [varchar](20) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Asistencia]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asistencia](
	[Fecha] [datetime] NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[Hor_Ent] [char](8) NULL,
	[Hor_Sal] [char](8) NULL,
	[Alm_Sal] [char](8) NULL,
	[Alm_Ent] [char](8) NULL,
	[Flag_Ent] [bit] NULL,
	[Flag_Sal] [bit] NULL,
	[Estado] [bit] NULL,
	[idHorario] [int] NULL,
	[Usuario] [nvarchar](50) NULL,
	[PC] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Capacitacion]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Capacitacion](
	[IdCapacitacion] [int] NOT NULL,
	[NombreCurso] [varchar](300) NULL,
	[FechaInicio] [datetime] NULL,
	[FechaFin] [datetime] NULL,
	[NumHoras] [varchar](5) NULL,
	[InstitucionOrganizadora] [varchar](300) NULL,
	[IdEmpleado] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cargo]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cargo](
	[IdCargo] [int] NOT NULL,
	[Cargo] [varchar](50) NOT NULL,
	[Abreviatura] [varchar](8) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comportamiento]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comportamiento](
	[idComportamiento] [int] NOT NULL,
	[Fecha] [datetime] NULL,
	[IdEmpleado] [int] NULL,
	[NumResol] [varchar](200) NULL,
	[Titulo] [varchar](1000) NULL,
	[Descripcion] [varchar](2000) NULL,
	[idTipoComportamiento] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConservacionViv]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConservacionViv](
	[IdConservacion] [int] NOT NULL,
	[DescripConserva] [varchar](17) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discapacidad]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discapacidad](
	[IdDiscapacidad] [int] NOT NULL,
	[Descrip_Discapacidad] [varchar](12) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empleado]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado](
	[IdEmpleado] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[idAreaOrganiz] [int] NOT NULL,
	[IdPersona] [int] NULL,
	[ExpSocial] [varchar](20) NULL,
	[FechaIngreso] [smalldatetime] NULL,
	[IdAFP] [int] NULL,
	[Brevete] [varchar](12) NULL,
	[IdEstadoCivil] [int] NULL,
	[Sexo] [char](1) NULL,
	[CtaCorriente] [varchar](16) NULL,
	[NroAFP] [varchar](20) NULL,
	[FechaAFP] [datetime] NULL,
	[IdSeguro] [int] NULL,
	[NroSeguro] [varchar](20) NULL,
	[IdTipoSangre] [int] NULL,
	[DecLey] [varchar](10) NULL,
	[Foto] [image] NULL,
	[IdTipoTrabajador] [int] NULL,
	[Estado] [char](1) NULL,
	[IdCargo] [int] NULL,
	[IdDiscapacidad] [int] NULL,
	[Profesion] [varchar](80) NULL,
	[JefeHogar] [bit] NULL,
	[CargaFam] [varchar](2) NULL,
	[NumHijos] [varchar](2) NULL,
	[IdViveCon] [int] NULL,
	[ObsFam] [nvarchar](500) NULL,
	[AspecSalud] [varchar](1000) NULL,
	[AspecEducacion] [varchar](1000) NULL,
	[AspecRecreacion] [varchar](1000) NULL,
	[AspecPsico] [varchar](1000) NULL,
	[AspecSocial] [varchar](1000) NULL,
	[DiagnosSocial] [varchar](1000) NULL,
	[IdNacionalidad] [int] NULL,
	[NumLibretaMilitar] [varchar](10) NULL,
	[GradoInstruccion] [varchar](80) NULL,
	[IdRegimenPen] [int] NULL,
	[NumRUC] [varchar](12) NULL,
	[NumAutogenSalud] [varchar](15) NULL,
	[NumResolCese] [varchar](20) NULL,
	[FechaResolCese] [datetime] NULL,
	[NumResol20530] [varchar](20) NULL,
	[FechaResol20530] [datetime] NULL,
	[NumResolPenMen] [varchar](20) NULL,
	[FechaResolPenMen] [datetime] NULL,
	[NumResoIngreEsta] [varchar](30) NULL,
	[FechaIngreEstado] [datetime] NULL,
	[FechaResolIngreEsta] [datetime] NULL,
	[NumResoIngreInstitu] [varchar](30) NULL,
	[FechaResoIngreInstitu] [datetime] NULL,
	[idHorario] [int] NULL,
	[Userid] [uniqueidentifier] NULL,
	[idLocal] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empleado_Area]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado_Area](
	[Year] [int] NOT NULL,
	[idAreaOrganiz] [int] NOT NULL,
	[IdEmpleado] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoCivil]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoCivil](
	[IdEstadoCivil] [int] NOT NULL,
	[DescripEstCivil] [varchar](11) NULL,
	[AbrevEstCiv] [varchar](3) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoEstudio]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoEstudio](
	[IdEstadoEstudio] [int] NOT NULL,
	[DescripEstadoEstudio] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstructOrganiz]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstructOrganiz](
	[Year] [int] NOT NULL,
	[idAreaOrganiz] [int] NOT NULL,
	[AreaOrganizacional] [varchar](200) NOT NULL,
	[Abrev] [varchar](10) NOT NULL,
	[Sigla] [varchar](6) NOT NULL,
	[PresupAnual] [decimal](11, 2) NULL,
	[PromedMensSueld] [decimal](11, 2) NULL,
	[NroTrabajaUO] [int] NULL,
	[Mision] [varchar](1000) NULL,
	[Vision] [varchar](1000) NULL,
	[IdPresupuesto] [int] NULL,
	[YearDe] [int] NULL,
	[idDependeDe] [int] NULL,
	[IdEmpleado] [int] NULL,
	[InversPublic] [decimal](11, 2) NULL,
	[ApruebaPedido] [char](1) NULL,
	[MontoAprobacion] [decimal](18, 2) NULL,
	[PresupCompra] [decimal](11, 2) NULL,
	[PresupUtilizado] [decimal](11, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstudiosRealizado]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstudiosRealizado](
	[IdTipoEstudios] [int] NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[Correlativo] [int] NOT NULL,
	[CentroEstudios] [varchar](150) NULL,
	[AñoInicio] [datetime] NULL,
	[Especialidad] [varchar](100) NULL,
	[AñoTermino] [datetime] NULL,
	[IdEstadoEstudio] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExcretasViv]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExcretasViv](
	[IdExcretas] [int] NOT NULL,
	[DescripExcretas] [varchar](33) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExpLaboral]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpLaboral](
	[IdEmpleado] [int] NOT NULL,
	[IdExpLab] [int] NOT NULL,
	[NomInstitucion] [varchar](120) NULL,
	[Cargo] [varchar](40) NULL,
	[FechaIngreso] [datetime] NULL,
	[FechaEgreso] [datetime] NULL,
	[UnidadOrganica] [varchar](100) NULL,
	[IdTipoInstitucion] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Familia]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Familia](
	[IdFamilia] [char](4) NOT NULL,
	[Descripcion] [varchar](140) NULL,
	[Observaciones] [varchar](120) NULL,
	[IdClase] [char](2) NOT NULL,
	[IdGrupo] [char](3) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Familiar]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Familiar](
	[IdEmpleado] [int] NOT NULL,
	[IdPersona] [int] NOT NULL,
	[FamSexo] [char](1) NULL,
	[FamLugTrab] [varchar](50) NULL,
	[IdEstadoCivil] [int] NULL,
	[NumAutogenSalud] [varchar](15) NULL,
	[FamOcupacion] [varchar](20) NULL,
	[FamGradoInstruc] [varchar](15) NULL,
	[IdTipoFam] [int] NULL,
	[ViveCasa] [bit] NULL,
	[LaboraInstitucion] [bit] NULL,
	[IdDiscapacidad] [int] NULL,
	[Procedencia] [varchar](50) NULL,
	[Enfermedad] [varchar](200) NULL,
	[TiempoEnfermedad] [varchar](200) NULL,
	[IdTipoSangre] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Horario]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Horario](
	[idHorario] [int] NOT NULL,
	[Ingreso1] [char](8) NULL,
	[Salida1] [char](8) NULL,
	[Ingreso2] [char](8) NULL,
	[Salida2] [char](8) NULL,
	[Descrip_Horario] [varchar](20) NULL,
	[Estado] [bit] NULL,
	[SgtDia] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HorarioTemporal]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HorarioTemporal](
	[idEmpleado] [int] NOT NULL,
	[N] [int] NOT NULL,
	[FechaInicio] [date] NULL,
	[FechaFin] [date] NULL,
	[HorarioAsignado] [int] NULL,
	[NroDocumento] [nvarchar](50) NULL,
	[Sisgedo] [nvarchar](50) NULL,
	[FechaRegistro] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inconveniente]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inconveniente](
	[IdObjetivo] [int] NOT NULL,
	[Id_Actividad] [int] NOT NULL,
	[IdInconveniente] [int] NOT NULL,
	[Inconveniente] [varchar](70) NOT NULL,
	[Importancia] [int] NOT NULL,
	[Desventaja] [varchar](70) NULL,
	[Observacion] [varchar](300) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Institucion]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Institucion](
	[IdInstitucion] [int] NOT NULL,
	[Descripcion] [varchar](120) NULL,
	[Direccion] [varchar](120) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Limitacion]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Limitacion](
	[IdLimitacion] [int] NOT NULL,
	[Descripcion] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Local]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Local](
	[idLocal] [int] NOT NULL,
	[NombreLocal] [nvarchar](50) NULL,
	[Estado] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Marcacion]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marcacion](
	[Fecha] [datetime] NOT NULL,
	[Estado] [char](2) NULL,
	[IdEmpleado] [int] NOT NULL,
	[Lugar] [varchar](10) NULL,
	[idHorario] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialViv]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialViv](
	[IdMaterial] [int] NOT NULL,
	[DescripMaterial] [varchar](12) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motivo_Perm]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Motivo_Perm](
	[idMotivo] [int] NOT NULL,
	[Descrip_Motivo] [varchar](50) NULL,
	[Salario] [bit] NULL,
	[idTipoPermiso] [int] NULL,
	[Abrev] [char](3) NULL,
	[TipoModalidad] [char](1) NULL,
	[Activo] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotivoBaja]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotivoBaja](
	[idMotivoBaja] [int] NOT NULL,
	[Descripcion] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nacionalidad]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nacionalidad](
	[IdNacionalidad] [int] NOT NULL,
	[Descripcion] [varchar](50) NULL,
	[Abreviatura] [varchar](3) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PeriodoLaboral]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeriodoLaboral](
	[IdEmpleado] [int] NOT NULL,
	[NroPeriodo] [int] NOT NULL,
	[NumResoIngreInstitu] [varchar](30) NULL,
	[FechaSalida] [smalldatetime] NULL,
	[FechaResoIngreInstitu] [datetime] NULL,
	[FechaIngreso] [smalldatetime] NULL,
	[Judicial] [bit] NULL,
	[Obs] [varchar](2000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permiso]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permiso](
	[IdEmpleado] [int] NOT NULL,
	[NPermiso] [int] NOT NULL,
	[FechaInicio] [datetime] NULL,
	[FechaFin] [datetime] NULL,
	[Hora_Sal] [char](8) NULL,
	[Hora_Ret] [char](8) NULL,
	[Dia] [bit] NULL,
	[Retorno] [bit] NULL,
	[idMotivo] [int] NULL,
	[Lugar] [varchar](20) NULL,
	[Referencia] [varchar](100) NULL,
	[Autorizacion] [varchar](50) NULL,
	[Obs] [varchar](200) NULL,
	[AutorizacionRRHH] [varchar](50) NULL,
	[Autorizado] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Persona]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persona](
	[IdPersona] [int] NOT NULL,
	[Nombres] [varchar](100) NULL,
	[Apellido_Paterno] [varchar](30) NULL,
	[Apellido_Materno] [varchar](30) NULL,
	[TipoDocID] [int] NULL,
	[NumDocID] [char](12) NULL,
	[Direccion] [varchar](100) NULL,
	[Telefono] [varchar](15) NULL,
	[Fax] [varchar](15) NULL,
	[Email] [varchar](50) NULL,
	[IdUbigeo] [char](6) NULL,
	[UbigeoDireccion] [char](6) NULL,
	[TipoPersona] [char](1) NULL,
	[FechaNacimiento] [datetime] NULL,
	[FechaRegistro] [datetime] NULL,
	[NumCelular] [varchar](15) NULL,
	[FonoCentroLab] [varchar](15) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegAsisDiario]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegAsisDiario](
	[Fecha] [datetime] NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[MinNormales] [varchar](4) NULL,
	[MinTarde] [varchar](18) NULL,
	[Estado] [varchar](3) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegimenPension]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegimenPension](
	[IdRegimenPen] [int] NOT NULL,
	[LeyRegimen] [varchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resolucion]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resolucion](
	[idEmpleado] [int] NOT NULL,
	[idResolucion] [int] NOT NULL,
	[idTipoResolucion] [int] NULL,
	[FechaResolucion] [date] NULL,
	[NumeroResolucion] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResponsableXUO]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResponsableXUO](
	[IdResponsable] [int] NOT NULL,
	[Year] [int] NULL,
	[idAreaOrganiz] [int] NULL,
	[AreaOrganizacional] [varchar](80) NULL,
	[idDependeDe] [int] NULL,
	[IdEmpleado] [int] NULL,
	[F_Inicio] [smalldatetime] NULL,
	[F_Termino] [smalldatetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[IdRol] [int] NOT NULL,
	[Descripcion] [varchar](30) NULL,
	[IdSistema] [char](7) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol_Acceso]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol_Acceso](
	[IdRol] [int] NOT NULL,
	[Permiso] [char](8) NULL,
	[IdSistemaOpcion] [char](7) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rotacion]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rotacion](
	[idEmpleado] [int] NOT NULL,
	[idRotacion] [int] NOT NULL,
	[FechaMemo] [date] NULL,
	[NroMemo] [nvarchar](50) NULL,
	[idAreaOrganiz] [int] NULL,
	[Year] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_AcudeEnferm]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_AcudeEnferm](
	[idAcudeEnferm] [int] NOT NULL,
	[AcudeEnfermDescrip] [varchar](16) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_Asegurado]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_Asegurado](
	[idEmpleado] [int] NOT NULL,
	[NroBeneficiario] [int] NOT NULL,
	[idPersona] [int] NULL,
	[idEstadoCivil] [int] NULL,
	[Sexo] [char](1) NULL,
	[VinculoFamiliar] [varchar](50) NULL,
	[NroPartida] [varchar](50) NULL,
	[NroResolMayorIndiscap] [varchar](50) NULL,
	[FechaInicioVinculo] [datetime] NULL,
	[idMotivoBaja] [int] NULL,
	[NroPartidaDef] [varchar](50) NULL,
	[OtrosMotivos] [varchar](50) NULL,
	[FechaFinVinculo] [datetime] NULL,
	[Reevaluar] [char](1) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_AspSocio]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_AspSocio](
	[idAspSocio] [int] NOT NULL,
	[IdEmpleado] [int] NULL,
	[idPregunta1] [int] NULL,
	[idPregunta2] [int] NULL,
	[idPregunta3] [int] NULL,
	[PrincipProblemas] [varchar](400) NULL,
	[PropMejoraInstitu] [varchar](400) NULL,
	[hobby] [varchar](400) NULL,
	[ActRecreaInstitu] [varchar](400) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_DinamicaFamiliar]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_DinamicaFamiliar](
	[idDinamica] [int] NOT NULL,
	[IdEmpleado] [int] NULL,
	[idTipoFam] [int] NULL,
	[idRelaPareja] [int] NULL,
	[idRelaPH] [int] NULL,
	[idRelacHermano] [int] NULL,
	[Observacion] [varchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_Feriado]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_Feriado](
	[idFeriado] [int] NOT NULL,
	[Fecha] [datetime] NULL,
	[Motivo] [varchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_FuncFam]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_FuncFam](
	[idFuncFamiliar] [int] NOT NULL,
	[IdEmpleado] [int] NULL,
	[Com1] [varchar](2) NULL,
	[Com2] [varchar](2) NULL,
	[Afec1] [varchar](2) NULL,
	[Afec2] [varchar](2) NULL,
	[Decis1] [varchar](2) NULL,
	[Decis2] [varchar](2) NULL,
	[Soc1] [varchar](2) NULL,
	[Soc2] [varchar](2) NULL,
	[Soc3] [varchar](2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_RelacHermano]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_RelacHermano](
	[idRelacHermano] [int] NOT NULL,
	[RelacHermanoDescrip] [varchar](12) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_RelacPareja]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_RelacPareja](
	[idRelacPareja] [int] NOT NULL,
	[RelacParejaDescrip] [varchar](12) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_RelacPH]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_RelacPH](
	[idRelacPH] [int] NOT NULL,
	[RelacPHDescrip] [varchar](14) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_Salud]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_Salud](
	[idSalud] [int] NOT NULL,
	[IdEmpleado] [int] NULL,
	[EnferCronica] [bit] NULL,
	[EnferCronicaDescrip] [varchar](200) NULL,
	[FamDiscapacidad] [bit] NULL,
	[FamDiscapDescrip] [varchar](200) NULL,
	[Alergico] [bit] NULL,
	[AlergicoDescrip] [varchar](200) NULL,
	[idAcudeEnferm] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RRHH_TipoFamilia]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RRHH_TipoFamilia](
	[idTipoFam] [int] NOT NULL,
	[TipoFamDescrip] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SistemaOpcion]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SistemaOpcion](
	[IdSistemaOpcion] [char](7) NOT NULL,
	[Descripcion] [varchar](150) NULL,
	[ConClave] [bit] NULL,
	[Clave] [varchar](30) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TenenciaViv]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TenenciaViv](
	[IdTenencia] [int] NOT NULL,
	[DescripTenencia] [varchar](30) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoComportamiento]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoComportamiento](
	[idTipoComportamiento] [int] NOT NULL,
	[TipoCompDescrip] [varchar](8) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoDoc]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoDoc](
	[IdTipodoc] [char](2) NOT NULL,
	[Descripcion] [varchar](40) NULL,
	[Numero] [int] NULL,
	[Serie] [tinyint] NULL,
	[Anio] [char](4) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoDocID]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoDocID](
	[TipoDocID] [int] NOT NULL,
	[Descripcion] [varchar](40) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoEstudio]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoEstudio](
	[IdTipoEstudios] [int] NOT NULL,
	[DescripTipoEst] [varchar](15) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoFamiliar]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoFamiliar](
	[IdTipoFam] [int] NOT NULL,
	[DescripTipoFam] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoInstitucion]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoInstitucion](
	[IdTipoInstitucion] [int] NOT NULL,
	[DescripTipoInstitucion] [varchar](15) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoMoneda]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoMoneda](
	[IdTipoMoneda] [int] NOT NULL,
	[Descripcion] [varchar](50) NULL,
	[Abreviatura] [varchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoMovimiento]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoMovimiento](
	[IdTipoMovimiento] [char](1) NOT NULL,
	[Descripcion] [varchar](40) NULL,
	[IdTipodoc] [char](2) NOT NULL,
	[TipoOperacion] [char](1) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoPermiso]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoPermiso](
	[idTipoPermiso] [int] NOT NULL,
	[DescripTipoPermiso] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoRecurso]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoRecurso](
	[IdTipoRecurso] [char](3) NOT NULL,
	[Descripcion] [varchar](120) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoResolucion]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoResolucion](
	[idTipoResolucion] [int] NOT NULL,
	[DescripTipoResolucion] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoSangre]    Script Date: 06/09/2026 13:08:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoSangre](
	[IdTipoSangre] [int] NOT NULL,
	[DescripTS] [varchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoTrabajador]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoTrabajador](
	[IdTipoTrabajador] [int] NOT NULL,
	[Descripcion] [varchar](30) NULL,
	[Observaciones] [varchar](150) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoTransaccion]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoTransaccion](
	[Anio] [char](5) NOT NULL,
	[IdTipoTransaccion] [char](2) NOT NULL,
	[Descripcion] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoViv]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoViv](
	[IdTipo] [int] NOT NULL,
	[DescripTipo] [varchar](20) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TitulosEmpleado]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TitulosEmpleado](
	[NumColegiatura] [varchar](20) NOT NULL,
	[DenominacionGrado] [varchar](100) NULL,
	[Institucion] [varchar](60) NULL,
	[Fecha] [datetime] NULL,
	[IdEmpleado] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UbicacionViv]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UbicacionViv](
	[IdUbicacion] [int] NOT NULL,
	[DescripUbicacion] [varchar](22) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ubigeo]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ubigeo](
	[IdUbigeo] [char](6) NOT NULL,
	[Departamento] [char](2) NOT NULL,
	[Provincia] [char](2) NOT NULL,
	[Distrito] [char](2) NOT NULL,
	[Descripcion] [varchar](120) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UnidadMedida]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UnidadMedida](
	[IdUnidadMedida] [char](4) NOT NULL,
	[Descripcion] [varchar](30) NULL,
	[Abreviado] [char](4) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [int] NOT NULL,
	[Login] [char](20) NULL,
	[Descripcion] [varchar](40) NULL,
	[Fecha] [datetime] NULL,
	[Estado] [bit] NULL,
	[IdEmpleado] [int] NULL,
	[PasswordHash] [nvarchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario_Rol]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario_Rol](
	[IdUsuario] [int] NOT NULL,
	[IdRol] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ViveCon]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ViveCon](
	[IdViveCon] [int] NOT NULL,
	[DescripViveCon] [varchar](27) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vivienda]    Script Date: 06/09/2026 13:08:16 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vivienda](
	[IdEmpleado] [int] NULL,
	[IdVivienda] [int] NOT NULL,
	[IdTenencia] [int] NULL,
	[IdTipo] [int] NULL,
	[IdMaterial] [int] NULL,
	[IdConservacion] [int] NULL,
	[NumDormitorio] [varchar](2) NULL,
	[NumServHig] [varchar](2) NULL,
	[IdUbicacion] [int] NULL,
	[IdAlumbrado] [int] NULL,
	[OtroAlumbrado] [varchar](50) NULL,
	[IdAgua] [int] NULL,
	[OtroAgua] [varchar](50) NULL,
	[IdExcretas] [int] NULL,
	[OtroExcreta] [varchar](50) NULL,
	[NumCompart] [varchar](2) NULL,
	[Observacion] [varchar](200) NULL,
	[NumPerDorm] [varchar](2) NULL,
	[Telefono] [bit] NULL,
	[Cable] [bit] NULL,
	[Internet] [bit] NULL,
	[Otros] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Index [PK__AFP__0E24B82260AB1FA3]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[AFP] ADD PRIMARY KEY CLUSTERED 
(
	[IdAFP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__AguaViv__29116B45044715E2]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[AguaViv] ADD PRIMARY KEY CLUSTERED 
(
	[IdAgua] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Alumbrad__BC18D33E6F0061EE]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[AlumbradoViv] ADD PRIMARY KEY CLUSTERED 
(
	[IdAlumbrado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Asistenc__4FEA52E6C6E47373]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Asistencia] ADD PRIMARY KEY CLUSTERED 
(
	[Fecha] ASC,
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Capacitaciones__23AA061E]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Capacitacion] ADD  CONSTRAINT [PK__Capacitaciones__23AA061E] PRIMARY KEY CLUSTERED 
(
	[IdCapacitacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Cargo__6C9856258A33B0A6]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Cargo] ADD PRIMARY KEY CLUSTERED 
(
	[IdCargo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Comporta__D4ADCD3BD3C0BF4B]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Comportamiento] ADD PRIMARY KEY CLUSTERED 
(
	[idComportamiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Conserva__CC8FE4448655E5BC]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[ConservacionViv] ADD PRIMARY KEY CLUSTERED 
(
	[IdConservacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Discapac__1FB8CE1C6C32B208]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Discapacidad] ADD PRIMARY KEY CLUSTERED 
(
	[IdDiscapacidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Empleado__4B03CA61]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Empleado] ADD  CONSTRAINT [PK__Empleado__4B03CA61] PRIMARY KEY CLUSTERED 
(
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Empleado__65C52DD073BAC7C9]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Empleado_Area] ADD PRIMARY KEY CLUSTERED 
(
	[Year] ASC,
	[idAreaOrganiz] ASC,
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__EstadoCivil__564A9484]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[EstadoCivil] ADD  CONSTRAINT [PK__EstadoCivil__564A9484] PRIMARY KEY CLUSTERED 
(
	[IdEstadoCivil] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__EstadoEs__65D8C38EBD78B634]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[EstadoEstudio] ADD PRIMARY KEY CLUSTERED 
(
	[IdEstadoEstudio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__EstructO__FA0B405B2D16569E]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[EstructOrganiz] ADD PRIMARY KEY CLUSTERED 
(
	[Year] ASC,
	[idAreaOrganiz] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__EstudiosRealizad__2B4B27E6]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[EstudiosRealizado] ADD  CONSTRAINT [PK__EstudiosRealizad__2B4B27E6] PRIMARY KEY CLUSTERED 
(
	[IdTipoEstudios] ASC,
	[IdEmpleado] ASC,
	[Correlativo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Excretas__36ACFEF84A5D0AB6]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[ExcretasViv] ADD PRIMARY KEY CLUSTERED 
(
	[IdExcretas] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__ExpLabor__2D6E15639B053890]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[ExpLaboral] ADD PRIMARY KEY CLUSTERED 
(
	[IdEmpleado] ASC,
	[IdExpLab] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK__Familia__6E0C4425]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Familia] ADD  CONSTRAINT [PK__Familia__6E0C4425] PRIMARY KEY CLUSTERED 
(
	[IdFamilia] ASC,
	[IdClase] ASC,
	[IdGrupo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Familiar__1C8106B4C5F5B6C7]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Familiar] ADD PRIMARY KEY CLUSTERED 
(
	[IdEmpleado] ASC,
	[IdPersona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Horario__DE60F33AA15A7F4F]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Horario] ADD PRIMARY KEY CLUSTERED 
(
	[idHorario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK_HorarioTemporal]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[HorarioTemporal] ADD  CONSTRAINT [PK_HorarioTemporal] PRIMARY KEY CLUSTERED 
(
	[idEmpleado] ASC,
	[N] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Inconven__71BF8942DB501A80]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Inconveniente] ADD PRIMARY KEY CLUSTERED 
(
	[IdObjetivo] ASC,
	[Id_Actividad] ASC,
	[IdInconveniente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Institucion__666B225D]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Institucion] ADD  CONSTRAINT [PK__Institucion__666B225D] PRIMARY KEY CLUSTERED 
(
	[IdInstitucion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Limitaci__A09F11FCD0747C0C]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Limitacion] ADD PRIMARY KEY CLUSTERED 
(
	[IdLimitacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK_Local]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Local] ADD  CONSTRAINT [PK_Local] PRIMARY KEY CLUSTERED 
(
	[idLocal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Marcacio__4FEA52E6F6EE858D]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Marcacion] ADD PRIMARY KEY CLUSTERED 
(
	[Fecha] ASC,
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Material__94356E58CC0A7C5B]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[MaterialViv] ADD PRIMARY KEY CLUSTERED 
(
	[IdMaterial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Motivo_Perm__37F0F5ED]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Motivo_Perm] ADD  CONSTRAINT [PK__Motivo_Perm__37F0F5ED] PRIMARY KEY CLUSTERED 
(
	[idMotivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK_MotivoBaja]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[MotivoBaja] ADD  CONSTRAINT [PK_MotivoBaja] PRIMARY KEY CLUSTERED 
(
	[idMotivoBaja] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Nacional__021E36BE6F3CFE5E]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Nacionalidad] ADD PRIMARY KEY CLUSTERED 
(
	[IdNacionalidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__PeriodoLaboral__37BBEBC3]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[PeriodoLaboral] ADD  CONSTRAINT [PK__PeriodoLaboral__37BBEBC3] PRIMARY KEY CLUSTERED 
(
	[IdEmpleado] ASC,
	[NroPeriodo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Permiso__58B2CB3A]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Permiso] ADD  CONSTRAINT [PK__Permiso__58B2CB3A] PRIMARY KEY CLUSTERED 
(
	[IdEmpleado] ASC,
	[NPermiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Persona__2EC6B30F]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Persona] ADD  CONSTRAINT [PK__Persona__2EC6B30F] PRIMARY KEY CLUSTERED 
(
	[IdPersona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RegAsisDiario__4362A899]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RegAsisDiario] ADD  CONSTRAINT [PK__RegAsisDiario__4362A899] PRIMARY KEY CLUSTERED 
(
	[Fecha] ASC,
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RegimenP__6E538133A1393E1C]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RegimenPension] ADD PRIMARY KEY CLUSTERED 
(
	[IdRegimenPen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK_Resolucion]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Resolucion] ADD  CONSTRAINT [PK_Resolucion] PRIMARY KEY CLUSTERED 
(
	[idEmpleado] ASC,
	[idResolucion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Responsa__CCF9B550D1CB7975]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[ResponsableXUO] ADD PRIMARY KEY CLUSTERED 
(
	[IdResponsable] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Rol__2A49584C10225A26]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Rol] ADD PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK__Rol_Acce__7957971890E90BD1]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Rol_Acceso] ADD PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC,
	[IdSistemaOpcion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK_Rotacion]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Rotacion] ADD  CONSTRAINT [PK_Rotacion] PRIMARY KEY CLUSTERED 
(
	[idEmpleado] ASC,
	[idRotacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RRHH_Acu__2DA4D4C805544269]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_AcudeEnferm] ADD PRIMARY KEY CLUSTERED 
(
	[idAcudeEnferm] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK_RRHH_Asegurados]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_Asegurado] ADD  CONSTRAINT [PK_RRHH_Asegurados] PRIMARY KEY CLUSTERED 
(
	[idEmpleado] ASC,
	[NroBeneficiario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RRHH_Asp__B6882CEC80C00156]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_AspSocio] ADD PRIMARY KEY CLUSTERED 
(
	[idAspSocio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RRHH_Din__B78CF7B880942362]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_DinamicaFamiliar] ADD PRIMARY KEY CLUSTERED 
(
	[idDinamica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RRHH_Fer__77769448EC7250C9]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_Feriado] ADD PRIMARY KEY CLUSTERED 
(
	[idFeriado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RRHH_Fun__8A166C38C25104CD]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_FuncFam] ADD PRIMARY KEY CLUSTERED 
(
	[idFuncFamiliar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RRHH_Rel__B5CFF59E75768D9A]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_RelacHermano] ADD PRIMARY KEY CLUSTERED 
(
	[idRelacHermano] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RRHH_Rel__F8ACBDC393631352]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_RelacPareja] ADD PRIMARY KEY CLUSTERED 
(
	[idRelacPareja] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RRHH_Rel__6362F614D3108DE3]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_RelacPH] ADD PRIMARY KEY CLUSTERED 
(
	[idRelacPH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RRHH_Sal__F820946E8F33CF09]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_Salud] ADD PRIMARY KEY CLUSTERED 
(
	[idSalud] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__RRHH_Tip__CD392099C5D07267]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[RRHH_TipoFamilia] ADD PRIMARY KEY CLUSTERED 
(
	[idTipoFam] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK__SistemaO__31ECF5446C06786E]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[SistemaOpcion] ADD PRIMARY KEY CLUSTERED 
(
	[IdSistemaOpcion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Tenencia__877CF7465C6D3992]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TenenciaViv] ADD PRIMARY KEY CLUSTERED 
(
	[IdTenencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__TipoComp__7A7DCD309E5AFC50]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoComportamiento] ADD PRIMARY KEY CLUSTERED 
(
	[idTipoComportamiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_TipoDoc]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoDoc] ADD  CONSTRAINT [PK_TipoDoc] PRIMARY KEY CLUSTERED 
(
	[IdTipodoc] ASC,
	[Anio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK_TipoDocID]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoDocID] ADD  CONSTRAINT [PK_TipoDocID] PRIMARY KEY CLUSTERED 
(
	[TipoDocID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__TipoEstu__099DEA35E8630773]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoEstudio] ADD PRIMARY KEY CLUSTERED 
(
	[IdTipoEstudios] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__TipoFami__0891500EF2D2351F]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoFamiliar] ADD PRIMARY KEY CLUSTERED 
(
	[IdTipoFam] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__TipoInst__AC6141D8C75D02BD]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoInstitucion] ADD PRIMARY KEY CLUSTERED 
(
	[IdTipoInstitucion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__TipoMoneda__4381D8D3]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoMoneda] ADD  CONSTRAINT [PK__TipoMoneda__4381D8D3] PRIMARY KEY CLUSTERED 
(
	[IdTipoMoneda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK__TipoMovimiento__797DF6D1]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoMovimiento] ADD  CONSTRAINT [PK__TipoMovimiento__797DF6D1] PRIMARY KEY CLUSTERED 
(
	[IdTipoMovimiento] ASC,
	[IdTipodoc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__TipoPerm__9BCF8F62EE56D7BA]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoPermiso] ADD PRIMARY KEY CLUSTERED 
(
	[idTipoPermiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_TipoRecurso_1]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoRecurso] ADD  CONSTRAINT [PK_TipoRecurso_1] PRIMARY KEY CLUSTERED 
(
	[IdTipoRecurso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK_TipoResolucion]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoResolucion] ADD  CONSTRAINT [PK_TipoResolucion] PRIMARY KEY CLUSTERED 
(
	[idTipoResolucion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK_TipoTrabajador]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoTrabajador] ADD  CONSTRAINT [PK_TipoTrabajador] PRIMARY KEY CLUSTERED 
(
	[IdTipoTrabajador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_TipoTransaccion]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoTransaccion] ADD  CONSTRAINT [PK_TipoTransaccion] PRIMARY KEY CLUSTERED 
(
	[IdTipoTransaccion] ASC,
	[Anio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__TipoViv__9E3A29A513C680BB]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoViv] ADD PRIMARY KEY CLUSTERED 
(
	[IdTipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK__TitulosE__B1AB3360DE8499F3]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TitulosEmpleado] ADD PRIMARY KEY CLUSTERED 
(
	[NumColegiatura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Ubicacio__778CAB1D101384E9]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[UbicacionViv] ADD PRIMARY KEY CLUSTERED 
(
	[IdUbicacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_Ubigeo]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Ubigeo] ADD  CONSTRAINT [PK_Ubigeo] PRIMARY KEY CLUSTERED 
(
	[IdUbigeo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK__UnidadMedida__6FF48C97]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[UnidadMedida] ADD  CONSTRAINT [PK__UnidadMedida__6FF48C97] PRIMARY KEY CLUSTERED 
(
	[IdUnidadMedida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Usuario__5B65BF97C0508762]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Usuario] ADD PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Usuario___89C12A13A5F08FDB]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Usuario_Rol] ADD PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC,
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__ViveCon__4C9E0B35F62E97B4]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[ViveCon] ADD PRIMARY KEY CLUSTERED 
(
	[IdViveCon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Vivienda__2BD4746B7169EB09]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[Vivienda] ADD PRIMARY KEY CLUSTERED 
(
	[IdVivienda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_idPersona]    Script Date: 06/09/2026 13:08:16 ******/
CREATE NONCLUSTERED INDEX [IDX_idPersona] ON [dbo].[Empleado]
(
	[IdPersona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Nombre]    Script Date: 06/09/2026 13:08:16 ******/
CREATE NONCLUSTERED INDEX [IX_Nombre] ON [dbo].[Persona]
(
	[Apellido_Paterno] ASC,
	[Apellido_Materno] ASC,
	[Nombres] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_NumDocId]    Script Date: 06/09/2026 13:08:16 ******/
CREATE NONCLUSTERED INDEX [IX_NumDocId] ON [dbo].[Persona]
(
	[NumDocID] ASC,
	[TipoDocID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Indice]    Script Date: 06/09/2026 13:08:16 ******/
CREATE NONCLUSTERED INDEX [Indice] ON [dbo].[RegAsisDiario]
(
	[Fecha] ASC,
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = OFF) ON [PRIMARY]
GO
/****** Object:  Index [XPKTipoSangre]    Script Date: 06/09/2026 13:08:16 ******/
ALTER TABLE [dbo].[TipoSangre] ADD  CONSTRAINT [XPKTipoSangre] PRIMARY KEY NONCLUSTERED 
(
	[IdTipoSangre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HorarioTemporal] ADD  CONSTRAINT [DF_HorarioTemporal_FechaRegistro]  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK__Empleado__IdAFP__5769A146] FOREIGN KEY([IdAFP])
REFERENCES [dbo].[AFP] ([IdAFP])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK__Empleado__IdAFP__5769A146]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK__Empleado__IdCarg__52A4EC29] FOREIGN KEY([IdCargo])
REFERENCES [dbo].[Cargo] ([IdCargo])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK__Empleado__IdCarg__52A4EC29]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK__Empleado__IdDisc__51B0C7F0] FOREIGN KEY([IdDiscapacidad])
REFERENCES [dbo].[Discapacidad] ([IdDiscapacidad])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK__Empleado__IdDisc__51B0C7F0]
GO
GO
GO
ALTER TABLE [dbo].[Empleado_Area]  WITH CHECK ADD FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[EstructOrganiz]  WITH CHECK ADD FOREIGN KEY([YearDe], [idDependeDe])
REFERENCES [dbo].[EstructOrganiz] ([Year], [idAreaOrganiz])
GO
ALTER TABLE [dbo].[EstudiosRealizado]  WITH CHECK ADD  CONSTRAINT [FK_EstudiosRealizado_Empleado] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[EstudiosRealizado] CHECK CONSTRAINT [FK_EstudiosRealizado_Empleado]
GO
ALTER TABLE [dbo].[EstudiosRealizado]  WITH CHECK ADD  CONSTRAINT [FK_EstudiosRealizado_EstadoEstudio] FOREIGN KEY([IdEstadoEstudio])
REFERENCES [dbo].[EstadoEstudio] ([IdEstadoEstudio])
GO
ALTER TABLE [dbo].[EstudiosRealizado] CHECK CONSTRAINT [FK_EstudiosRealizado_EstadoEstudio]
GO
ALTER TABLE [dbo].[ExpLaboral]  WITH CHECK ADD  CONSTRAINT [FK__ExpLabora__IdEmp__65B7C09D] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[ExpLaboral] CHECK CONSTRAINT [FK__ExpLabora__IdEmp__65B7C09D]
GO
ALTER TABLE [dbo].[Familiar]  WITH CHECK ADD FOREIGN KEY([IdDiscapacidad])
REFERENCES [dbo].[Discapacidad] ([IdDiscapacidad])
GO
ALTER TABLE [dbo].[Familiar]  WITH CHECK ADD  CONSTRAINT [FK__Familiar__IdEmpl__64C39C64] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Familiar] CHECK CONSTRAINT [FK__Familiar__IdEmpl__64C39C64]
GO
ALTER TABLE [dbo].[Familiar]  WITH CHECK ADD FOREIGN KEY([IdEstadoCivil])
REFERENCES [dbo].[EstadoCivil] ([IdEstadoCivil])
GO
ALTER TABLE [dbo].[HorarioTemporal]  WITH CHECK ADD  CONSTRAINT [FK_HorarioTemporal_Empleado] FOREIGN KEY([idEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[HorarioTemporal] CHECK CONSTRAINT [FK_HorarioTemporal_Empleado]
GO
ALTER TABLE [dbo].[HorarioTemporal]  WITH CHECK ADD  CONSTRAINT [FK_HorarioTemporal_Horario] FOREIGN KEY([HorarioAsignado])
REFERENCES [dbo].[Horario] ([idHorario])
GO
ALTER TABLE [dbo].[HorarioTemporal] CHECK CONSTRAINT [FK_HorarioTemporal_Horario]
GO
ALTER TABLE [dbo].[Marcacion]  WITH CHECK ADD  CONSTRAINT [FK__Marcacion__IdEmp__70354F10] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Marcacion] CHECK CONSTRAINT [FK__Marcacion__IdEmp__70354F10]
GO
ALTER TABLE [dbo].[PeriodoLaboral]  WITH CHECK ADD  CONSTRAINT [FK__PeriodoLa__IdEmp__38B00FFC] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[PeriodoLaboral] CHECK CONSTRAINT [FK__PeriodoLa__IdEmp__38B00FFC]
GO
ALTER TABLE [dbo].[Permiso]  WITH CHECK ADD  CONSTRAINT [FK__Permiso__IdEmple__5A9B13AC] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Permiso] CHECK CONSTRAINT [FK__Permiso__IdEmple__5A9B13AC]
GO
ALTER TABLE [dbo].[Permiso]  WITH CHECK ADD  CONSTRAINT [FK__Permiso__idMotiv__59A6EF73] FOREIGN KEY([idMotivo])
REFERENCES [dbo].[Motivo_Perm] ([idMotivo])
GO
ALTER TABLE [dbo].[Permiso] CHECK CONSTRAINT [FK__Permiso__idMotiv__59A6EF73]
GO
ALTER TABLE [dbo].[Resolucion]  WITH CHECK ADD  CONSTRAINT [FK_Resolucion_Empleado] FOREIGN KEY([idEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Resolucion] CHECK CONSTRAINT [FK_Resolucion_Empleado]
GO
ALTER TABLE [dbo].[Rol_Acceso]  WITH CHECK ADD FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[Rotacion]  WITH CHECK ADD  CONSTRAINT [FK_Rotacion_Empleado] FOREIGN KEY([idEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Rotacion] CHECK CONSTRAINT [FK_Rotacion_Empleado]
GO
ALTER TABLE [dbo].[Rotacion]  WITH CHECK ADD  CONSTRAINT [FK_Rotacion_EstructOrganiz] FOREIGN KEY([Year], [idAreaOrganiz])
REFERENCES [dbo].[EstructOrganiz] ([Year], [idAreaOrganiz])
GO
ALTER TABLE [dbo].[Rotacion] CHECK CONSTRAINT [FK_Rotacion_EstructOrganiz]
GO
ALTER TABLE [dbo].[RRHH_Asegurado]  WITH CHECK ADD  CONSTRAINT [FK_RRHH_Asegurados_Empleado] FOREIGN KEY([idEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[RRHH_Asegurado] CHECK CONSTRAINT [FK_RRHH_Asegurados_Empleado]
GO
ALTER TABLE [dbo].[RRHH_Asegurado]  WITH CHECK ADD  CONSTRAINT [FK_RRHH_Asegurados_EstadoCivil] FOREIGN KEY([idEstadoCivil])
REFERENCES [dbo].[EstadoCivil] ([IdEstadoCivil])
GO
ALTER TABLE [dbo].[RRHH_Asegurado] CHECK CONSTRAINT [FK_RRHH_Asegurados_EstadoCivil]
GO
ALTER TABLE [dbo].[RRHH_Asegurado]  WITH CHECK ADD  CONSTRAINT [FK_RRHH_Asegurados_MotivoBaja] FOREIGN KEY([idMotivoBaja])
REFERENCES [dbo].[MotivoBaja] ([idMotivoBaja])
GO
ALTER TABLE [dbo].[RRHH_Asegurado] CHECK CONSTRAINT [FK_RRHH_Asegurados_MotivoBaja]
GO
ALTER TABLE [dbo].[RRHH_Asegurado]  WITH CHECK ADD  CONSTRAINT [FK_RRHH_Asegurados_Persona] FOREIGN KEY([idPersona])
REFERENCES [dbo].[Persona] ([IdPersona])
GO
ALTER TABLE [dbo].[RRHH_Asegurado] CHECK CONSTRAINT [FK_RRHH_Asegurados_Persona]
GO
ALTER TABLE [dbo].[RRHH_AspSocio]  WITH CHECK ADD  CONSTRAINT [FK__RRHH_AspS__IdEmp__61E72FB9] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[RRHH_AspSocio] CHECK CONSTRAINT [FK__RRHH_AspS__IdEmp__61E72FB9]
GO
ALTER TABLE [dbo].[RRHH_DinamicaFamiliar]  WITH CHECK ADD  CONSTRAINT [FK__RRHH_Dina__IdEmp__60F30B80] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[RRHH_DinamicaFamiliar] CHECK CONSTRAINT [FK__RRHH_Dina__IdEmp__60F30B80]
GO
ALTER TABLE [dbo].[RRHH_FuncFam]  WITH CHECK ADD  CONSTRAINT [FK__RRHH_Func__IdEmp__5FFEE747] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[RRHH_FuncFam] CHECK CONSTRAINT [FK__RRHH_Func__IdEmp__5FFEE747]
GO
ALTER TABLE [dbo].[RRHH_Salud]  WITH CHECK ADD FOREIGN KEY([idAcudeEnferm])
REFERENCES [dbo].[RRHH_AcudeEnferm] ([idAcudeEnferm])
GO
ALTER TABLE [dbo].[RRHH_Salud]  WITH CHECK ADD  CONSTRAINT [FK__RRHH_Salu__IdEmp__5F0AC30E] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[RRHH_Salud] CHECK CONSTRAINT [FK__RRHH_Salu__IdEmp__5F0AC30E]
GO
ALTER TABLE [dbo].[TitulosEmpleado]  WITH CHECK ADD  CONSTRAINT [FK__TitulosEm__IdEmp__5E169ED5] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[TitulosEmpleado] CHECK CONSTRAINT [FK__TitulosEm__IdEmp__5E169ED5]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK__Usuario__IdEmple__5C2E5663] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK__Usuario__IdEmple__5C2E5663]
GO
ALTER TABLE [dbo].[Usuario_Rol]  WITH CHECK ADD FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[Usuario_Rol]  WITH CHECK ADD FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Vivienda]  WITH CHECK ADD FOREIGN KEY([IdAgua])
REFERENCES [dbo].[AguaViv] ([IdAgua])
GO
ALTER TABLE [dbo].[Vivienda]  WITH CHECK ADD FOREIGN KEY([IdAlumbrado])
REFERENCES [dbo].[AlumbradoViv] ([IdAlumbrado])
GO
ALTER TABLE [dbo].[Vivienda]  WITH CHECK ADD FOREIGN KEY([IdConservacion])
REFERENCES [dbo].[ConservacionViv] ([IdConservacion])
GO
ALTER TABLE [dbo].[Vivienda]  WITH CHECK ADD  CONSTRAINT [FK__Vivienda__IdEmpl__5B3A322A] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Vivienda] CHECK CONSTRAINT [FK__Vivienda__IdEmpl__5B3A322A]
GO
ALTER TABLE [dbo].[Vivienda]  WITH CHECK ADD FOREIGN KEY([IdExcretas])
REFERENCES [dbo].[ExcretasViv] ([IdExcretas])
GO
ALTER TABLE [dbo].[Vivienda]  WITH CHECK ADD FOREIGN KEY([IdMaterial])
REFERENCES [dbo].[MaterialViv] ([IdMaterial])
GO
ALTER TABLE [dbo].[Vivienda]  WITH CHECK ADD FOREIGN KEY([IdTenencia])
REFERENCES [dbo].[TenenciaViv] ([IdTenencia])
GO
ALTER TABLE [dbo].[Vivienda]  WITH CHECK ADD FOREIGN KEY([IdTipo])
REFERENCES [dbo].[TipoViv] ([IdTipo])
GO
ALTER TABLE [dbo].[Vivienda]  WITH CHECK ADD FOREIGN KEY([IdUbicacion])
REFERENCES [dbo].[UbicacionViv] ([IdUbicacion])
GO

/* ============================================================ 
   FKs NUEVAS agregadas para conectar tablas flotantes 
   (antes sin relaciÃ³n formal) - verificadas sin huerfanos en CMI 
   ============================================================ */

-- Empleado -> tablas maestras
ALTER TABLE [dbo].[Empleado] ADD CONSTRAINT [FK_Empleado_TipoSangre] FOREIGN KEY([IdTipoSangre]) REFERENCES [dbo].[TipoSangre]([IdTipoSangre])
ALTER TABLE [dbo].[Empleado] ADD CONSTRAINT [FK_Empleado_TipoTrabajador] FOREIGN KEY([IdTipoTrabajador]) REFERENCES [dbo].[TipoTrabajador]([IdTipoTrabajador])
ALTER TABLE [dbo].[Empleado] ADD CONSTRAINT [FK_Empleado_Nacionalidad] FOREIGN KEY([IdNacionalidad]) REFERENCES [dbo].[Nacionalidad]([IdNacionalidad])
ALTER TABLE [dbo].[Empleado] ADD CONSTRAINT [FK_Empleado_ViveCon] FOREIGN KEY([IdViveCon]) REFERENCES [dbo].[ViveCon]([IdViveCon])
ALTER TABLE [dbo].[Empleado] ADD CONSTRAINT [FK_Empleado_RegimenPension] FOREIGN KEY([IdRegimenPen]) REFERENCES [dbo].[RegimenPension]([IdRegimenPen])
ALTER TABLE [dbo].[Empleado] ADD CONSTRAINT [FK_Empleado_EstadoCivil] FOREIGN KEY([IdEstadoCivil]) REFERENCES [dbo].[EstadoCivil]([IdEstadoCivil])
ALTER TABLE [dbo].[Empleado] ADD CONSTRAINT [FK_Empleado_Horario] FOREIGN KEY([idHorario]) REFERENCES [dbo].[Horario]([idHorario])
ALTER TABLE [dbo].[Empleado] ADD CONSTRAINT [FK_Empleado_Local] FOREIGN KEY([idLocal]) REFERENCES [dbo].[Local]([idLocal])
ALTER TABLE [dbo].[Empleado] ADD CONSTRAINT [FK_Empleado_EstructOrganiz] FOREIGN KEY([Year], [idAreaOrganiz]) REFERENCES [dbo].[EstructOrganiz]([Year], [idAreaOrganiz])

-- Asistencia
ALTER TABLE [dbo].[Asistencia] ADD CONSTRAINT [FK_Asistencia_Empleado] FOREIGN KEY([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado])
ALTER TABLE [dbo].[Asistencia] ADD CONSTRAINT [FK_Asistencia_Horario] FOREIGN KEY([idHorario]) REFERENCES [dbo].[Horario]([idHorario])

-- Marcacion
ALTER TABLE [dbo].[Marcacion] ADD CONSTRAINT [FK_Marcacion_Horario] FOREIGN KEY([idHorario]) REFERENCES [dbo].[Horario]([idHorario])

-- RegAsisDiario
ALTER TABLE [dbo].[RegAsisDiario] ADD CONSTRAINT [FK_RegAsisDiario_Empleado] FOREIGN KEY([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado])

-- Motivo_Perm
ALTER TABLE [dbo].[Motivo_Perm] ADD CONSTRAINT [FK_Motivo_Perm_TipoPermiso] FOREIGN KEY([idTipoPermiso]) REFERENCES [dbo].[TipoPermiso]([idTipoPermiso])

-- Empleado_Area
ALTER TABLE [dbo].[Empleado_Area] ADD CONSTRAINT [FK_Empleado_Area_EstructOrganiz] FOREIGN KEY([Year], [idAreaOrganiz]) REFERENCES [dbo].[EstructOrganiz]([Year], [idAreaOrganiz])

-- Capacitacion
ALTER TABLE [dbo].[Capacitacion] ADD CONSTRAINT [FK_Capacitacion_Empleado] FOREIGN KEY([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado])

-- Comportamiento
ALTER TABLE [dbo].[Comportamiento] ADD CONSTRAINT [FK_Comportamiento_Empleado] FOREIGN KEY([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado])

-- ResponsableXUO
ALTER TABLE [dbo].[ResponsableXUO] ADD CONSTRAINT [FK_ResponsableXUO_Empleado] FOREIGN KEY([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado])
ALTER TABLE [dbo].[ResponsableXUO] ADD CONSTRAINT [FK_ResponsableXUO_EstructOrganiz] FOREIGN KEY([Year], [idAreaOrganiz]) REFERENCES [dbo].[EstructOrganiz]([Year], [idAreaOrganiz])

/** Grupo B: FKs a catalogo flotantes (verificadas sin huerfanos) */
ALTER TABLE [dbo].[Comportamiento] ADD CONSTRAINT [FK_Comportamiento_TipoComportamiento] FOREIGN KEY([idTipoComportamiento]) REFERENCES [dbo].[TipoComportamiento]([idTipoComportamiento])
ALTER TABLE [dbo].[Resolucion] ADD CONSTRAINT [FK_Resolucion_TipoResolucion] FOREIGN KEY([idTipoResolucion]) REFERENCES [dbo].[TipoResolucion]([idTipoResolucion])
ALTER TABLE [dbo].[ExpLaboral] ADD CONSTRAINT [FK_ExpLaboral_TipoInstitucion] FOREIGN KEY([IdTipoInstitucion]) REFERENCES [dbo].[TipoInstitucion]([IdTipoInstitucion])
ALTER TABLE [dbo].[Rol] ADD CONSTRAINT [FK_Rol_SistemaOpcion] FOREIGN KEY([IdSistema]) REFERENCES [dbo].[SistemaOpcion]([IdSistemaOpcion])
ALTER TABLE [dbo].[RRHH_Asegurado] ADD CONSTRAINT [FK_RRHH_Asegurado_EstadoCivil] FOREIGN KEY([idEstadoCivil]) REFERENCES [dbo].[EstadoCivil]([IdEstadoCivil])
ALTER TABLE [dbo].[Persona] ADD CONSTRAINT [FK_Persona_Ubigeo] FOREIGN KEY([IdUbigeo]) REFERENCES [dbo].[Ubigeo]([IdUbigeo])
ALTER TABLE [dbo].[Persona] ADD CONSTRAINT [FK_Persona_TipoDocID] FOREIGN KEY([TipoDocID]) REFERENCES [dbo].[TipoDocID]([TipoDocID])

GO

/* NOTA: Triggers eliminados (referenciaban tablas caidas): tr_aud_asis_norm, tr_aud_asis_norm_Insert, tr_aud_permiso, tr_aud_permiso_Insert, trg_update_HorarioTemporal */
