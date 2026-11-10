/* ============================================================
   01-esquema-RRHH_Normalizado.sql
   Crea la BD [RRHH_Normalizado]: version normalizada (3FN/FNBC,
   y 4FN donde aplica) de [RRHH_Nuevo].

   - NO modifica RRHH_Nuevo (sigue siendo la fuente de datos).
   - Los datos se copian despues con 02-migracion-desde-RRHH_Nuevo.sql
   - Detalle de cada decision: NORMALIZACION.md (misma carpeta)

   Orden de ejecucion:
     1. Scripts/bd-evo-RRLL.sql            (crea RRHH_Nuevo)
     2. Scripts/bd-Datos/bd-Datos.sql      (carga RRHH_Nuevo)
     3. este script                        (crea RRHH_Normalizado vacia)
     4. 02-migracion-desde-RRHH_Nuevo.sql  (copia y transforma)
   ============================================================ */

IF DB_ID('RRHH_Normalizado') IS NOT NULL BEGIN
	ALTER DATABASE [RRHH_Normalizado] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE [RRHH_Normalizado]
END
GO
CREATE DATABASE [RRHH_Normalizado]
GO
USE [RRHH_Normalizado]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/* ============================================================
   0. ESQUEMA DE MIGRACION (bitacora de incidencias de datos)
   ============================================================ */
CREATE SCHEMA [mig]
GO
CREATE TABLE [mig].[Incidencia](
	[IdIncidencia]   int IDENTITY(1,1) NOT NULL,
	[Tabla]          varchar(60)   NOT NULL,
	[Clave]          varchar(100)  NULL,
	[Campo]          varchar(60)   NULL,
	[ValorOriginal]  nvarchar(400) NULL,
	[Accion]         varchar(40)   NOT NULL,
	[Detalle]        nvarchar(400) NULL,
	[Fecha]          datetime2(0)  NOT NULL CONSTRAINT [DF_Incidencia_Fecha] DEFAULT (sysdatetime()),
	CONSTRAINT [PK_Incidencia] PRIMARY KEY CLUSTERED ([IdIncidencia])
)
GO

/* ============================================================
   1. UBIGEO  (antes: 1 tabla mezclando 3 niveles + 1069 codigos basura)
   ============================================================ */
CREATE TABLE [dbo].[Departamento](
	[CodDepartamento] char(2)      NOT NULL,
	[Nombre]          varchar(120) NOT NULL,
	CONSTRAINT [PK_Departamento] PRIMARY KEY CLUSTERED ([CodDepartamento])
)
CREATE TABLE [dbo].[Provincia](
	[CodDepartamento] char(2)      NOT NULL,
	[CodProvincia]    char(2)      NOT NULL,
	[Nombre]          varchar(120) NOT NULL,
	CONSTRAINT [PK_Provincia] PRIMARY KEY CLUSTERED ([CodDepartamento], [CodProvincia]),
	CONSTRAINT [FK_Provincia_Departamento] FOREIGN KEY ([CodDepartamento]) REFERENCES [dbo].[Departamento]([CodDepartamento])
)
CREATE TABLE [dbo].[Distrito](
	[CodDepartamento] char(2)      NOT NULL,
	[CodProvincia]    char(2)      NOT NULL,
	[CodDistrito]     char(2)      NOT NULL,
	[Nombre]          varchar(120) NOT NULL,
	-- codigo INEI de 6 digitos: se calcula, no se duplica a mano
	[IdUbigeo]        AS CAST([CodDepartamento] + [CodProvincia] + [CodDistrito] AS char(6)) PERSISTED NOT NULL,
	CONSTRAINT [PK_Distrito] PRIMARY KEY CLUSTERED ([IdUbigeo]),
	CONSTRAINT [UQ_Distrito_Codigos] UNIQUE ([CodDepartamento], [CodProvincia], [CodDistrito]),
	CONSTRAINT [FK_Distrito_Provincia] FOREIGN KEY ([CodDepartamento], [CodProvincia]) REFERENCES [dbo].[Provincia]([CodDepartamento], [CodProvincia])
)
GO

/* ============================================================
   2. CATALOGOS GENERALES
   ============================================================ */
CREATE TABLE [dbo].[TipoDocID](
	[TipoDocID]   int IDENTITY(1,1) NOT NULL,
	[Descripcion] varchar(40) NOT NULL,
	CONSTRAINT [PK_TipoDocID] PRIMARY KEY CLUSTERED ([TipoDocID])
)
CREATE TABLE [dbo].[EstadoCivil](
	[IdEstadoCivil]   int IDENTITY(1,1) NOT NULL,
	[DescripEstCivil] varchar(11) NOT NULL,
	[AbrevEstCiv]     varchar(3)  NULL,
	CONSTRAINT [PK_EstadoCivil] PRIMARY KEY CLUSTERED ([IdEstadoCivil])
)
CREATE TABLE [dbo].[Nacionalidad](
	[IdNacionalidad] int IDENTITY(1,1) NOT NULL,
	[Descripcion]    varchar(50) NOT NULL,
	[Abreviatura]    varchar(3)  NULL,
	CONSTRAINT [PK_Nacionalidad] PRIMARY KEY CLUSTERED ([IdNacionalidad])
)
CREATE TABLE [dbo].[TipoSangre](
	[IdTipoSangre] int IDENTITY(1,1) NOT NULL,
	[DescripTS]    varchar(5) NOT NULL,
	CONSTRAINT [PK_TipoSangre] PRIMARY KEY CLUSTERED ([IdTipoSangre])
)
CREATE TABLE [dbo].[Discapacidad](
	[IdDiscapacidad]       int IDENTITY(1,1) NOT NULL,
	[Descrip_Discapacidad] varchar(12) NOT NULL,
	CONSTRAINT [PK_Discapacidad] PRIMARY KEY CLUSTERED ([IdDiscapacidad])
)
CREATE TABLE [dbo].[AFP](
	[IdAFP]      int IDENTITY(1,1) NOT NULL,
	[DescripAFP] varchar(50)  NOT NULL,
	[DireccAFP]  varchar(200) NULL,
	[TelefAFP]   varchar(12)  NULL,
	[NomContAFP] varchar(100) NULL,
	CONSTRAINT [PK_AFP] PRIMARY KEY CLUSTERED ([IdAFP])
)
CREATE TABLE [dbo].[RegimenPension](
	[IdRegimenPen] int IDENTITY(1,1) NOT NULL,
	[LeyRegimen]   varchar(5) NOT NULL,
	CONSTRAINT [PK_RegimenPension] PRIMARY KEY CLUSTERED ([IdRegimenPen])
)
CREATE TABLE [dbo].[TipoTrabajador](
	[IdTipoTrabajador] int IDENTITY(1,1) NOT NULL,
	[Descripcion]      varchar(30)  NOT NULL,
	[Observaciones]    varchar(150) NULL,
	CONSTRAINT [PK_TipoTrabajador] PRIMARY KEY CLUSTERED ([IdTipoTrabajador])
)
CREATE TABLE [dbo].[Cargo](
	[IdCargo]     int IDENTITY(1,1) NOT NULL,
	[Cargo]       varchar(50) NOT NULL,
	[Abreviatura] varchar(8)  NULL,
	CONSTRAINT [PK_Cargo] PRIMARY KEY CLUSTERED ([IdCargo])
)
CREATE TABLE [dbo].[Local](
	[idLocal]     int IDENTITY(1,1) NOT NULL,
	[NombreLocal] nvarchar(50) NOT NULL,
	[Estado]      bit NULL,
	CONSTRAINT [PK_Local] PRIMARY KEY CLUSTERED ([idLocal])
)
CREATE TABLE [dbo].[ViveCon](
	[IdViveCon]      int IDENTITY(1,1) NOT NULL,
	[DescripViveCon] varchar(27) NOT NULL,
	CONSTRAINT [PK_ViveCon] PRIMARY KEY CLUSTERED ([IdViveCon])
)
CREATE TABLE [dbo].[TipoFamiliar](          -- parentesco (padre, hijo...)
	[IdTipoFam]      int IDENTITY(1,1) NOT NULL,
	[DescripTipoFam] varchar(10) NOT NULL,
	CONSTRAINT [PK_TipoFamiliar] PRIMARY KEY CLUSTERED ([IdTipoFam])
)
CREATE TABLE [dbo].[VinculoAsegurado](      -- NUEVO: antes texto libre en RRHH_Asegurado.VinculoFamiliar
	[IdVinculoAsegurado] int IDENTITY(1,1) NOT NULL,
	[Descripcion]        varchar(50) NOT NULL,
	CONSTRAINT [PK_VinculoAsegurado] PRIMARY KEY CLUSTERED ([IdVinculoAsegurado]),
	CONSTRAINT [UQ_VinculoAsegurado_Descripcion] UNIQUE ([Descripcion])
)
CREATE TABLE [dbo].[MotivoBaja](
	[idMotivoBaja] int IDENTITY(1,1) NOT NULL,
	[Descripcion]  varchar(50) NOT NULL,
	CONSTRAINT [PK_MotivoBaja] PRIMARY KEY CLUSTERED ([idMotivoBaja])
)
CREATE TABLE [dbo].[TipoEstudio](
	[IdTipoEstudios] int IDENTITY(1,1) NOT NULL,
	[DescripTipoEst] varchar(15) NOT NULL,
	CONSTRAINT [PK_TipoEstudio] PRIMARY KEY CLUSTERED ([IdTipoEstudios])
)
CREATE TABLE [dbo].[EstadoEstudio](
	[IdEstadoEstudio]      int IDENTITY(1,1) NOT NULL,
	[DescripEstadoEstudio] varchar(10) NOT NULL,
	CONSTRAINT [PK_EstadoEstudio] PRIMARY KEY CLUSTERED ([IdEstadoEstudio])
)
CREATE TABLE [dbo].[TipoInstitucion](
	[IdTipoInstitucion]      int IDENTITY(1,1) NOT NULL,
	[DescripTipoInstitucion] varchar(15) NOT NULL,
	CONSTRAINT [PK_TipoInstitucion] PRIMARY KEY CLUSTERED ([IdTipoInstitucion])
)
CREATE TABLE [dbo].[TipoComportamiento](
	[idTipoComportamiento] int IDENTITY(1,1) NOT NULL,
	[TipoCompDescrip]      varchar(8) NOT NULL,
	CONSTRAINT [PK_TipoComportamiento] PRIMARY KEY CLUSTERED ([idTipoComportamiento])
)
CREATE TABLE [dbo].[TipoResolucion](
	[idTipoResolucion]      int IDENTITY(1,1) NOT NULL,
	[DescripTipoResolucion] nvarchar(50) NOT NULL,
	CONSTRAINT [PK_TipoResolucion] PRIMARY KEY CLUSTERED ([idTipoResolucion])
)
CREATE TABLE [dbo].[TipoPermiso](
	[idTipoPermiso]      int IDENTITY(1,1) NOT NULL,
	[DescripTipoPermiso] varchar(50) NOT NULL,
	CONSTRAINT [PK_TipoPermiso] PRIMARY KEY CLUSTERED ([idTipoPermiso])
)
CREATE TABLE [dbo].[Motivo_Perm](
	[idMotivo]       int IDENTITY(1,1) NOT NULL,
	[Descrip_Motivo] varchar(50) NOT NULL,
	[Salario]        bit NULL,
	[idTipoPermiso]  int NOT NULL,
	[Abrev]          char(3) NULL,
	[TipoModalidad]  char(1) NULL,
	[Activo]         bit NULL,
	CONSTRAINT [PK_Motivo_Perm] PRIMARY KEY CLUSTERED ([idMotivo]),
	CONSTRAINT [FK_Motivo_Perm_TipoPermiso] FOREIGN KEY ([idTipoPermiso]) REFERENCES [dbo].[TipoPermiso]([idTipoPermiso]),
	CONSTRAINT [CK_Motivo_Perm_TipoModalidad] CHECK ([TipoModalidad] IN ('C','P'))
)
CREATE TABLE [dbo].[RRHH_Feriado](
	[idFeriado] int IDENTITY(1,1) NOT NULL,
	[Fecha]     date NOT NULL,
	[Motivo]    varchar(200) NULL,
	CONSTRAINT [PK_RRHH_Feriado] PRIMARY KEY CLUSTERED ([idFeriado]),
	CONSTRAINT [UQ_RRHH_Feriado_Fecha] UNIQUE ([Fecha])
)
GO

/* -------- catalogos de vivienda -------- */
CREATE TABLE [dbo].[TenenciaViv](     [IdTenencia]     int IDENTITY(1,1) NOT NULL, [DescripTenencia]  varchar(30) NOT NULL, CONSTRAINT [PK_TenenciaViv]     PRIMARY KEY CLUSTERED ([IdTenencia]))
CREATE TABLE [dbo].[TipoViv](         [IdTipo]         int IDENTITY(1,1) NOT NULL, [DescripTipo]      varchar(20) NOT NULL, CONSTRAINT [PK_TipoViv]         PRIMARY KEY CLUSTERED ([IdTipo]))
CREATE TABLE [dbo].[MaterialViv](     [IdMaterial]     int IDENTITY(1,1) NOT NULL, [DescripMaterial]  varchar(12) NOT NULL, CONSTRAINT [PK_MaterialViv]     PRIMARY KEY CLUSTERED ([IdMaterial]))
CREATE TABLE [dbo].[ConservacionViv]( [IdConservacion] int IDENTITY(1,1) NOT NULL, [DescripConserva]  varchar(17) NOT NULL, CONSTRAINT [PK_ConservacionViv] PRIMARY KEY CLUSTERED ([IdConservacion]))
CREATE TABLE [dbo].[UbicacionViv](    [IdUbicacion]    int IDENTITY(1,1) NOT NULL, [DescripUbicacion] varchar(22) NOT NULL, CONSTRAINT [PK_UbicacionViv]    PRIMARY KEY CLUSTERED ([IdUbicacion]))
CREATE TABLE [dbo].[AlumbradoViv](    [IdAlumbrado]    int IDENTITY(1,1) NOT NULL, [DescripAlumbrado] varchar(20) NOT NULL, CONSTRAINT [PK_AlumbradoViv]    PRIMARY KEY CLUSTERED ([IdAlumbrado]))
CREATE TABLE [dbo].[AguaViv](         [IdAgua]         int IDENTITY(1,1) NOT NULL, [DescripAgua]      varchar(33) NOT NULL, CONSTRAINT [PK_AguaViv]         PRIMARY KEY CLUSTERED ([IdAgua]))
CREATE TABLE [dbo].[ExcretasViv](     [IdExcretas]     int IDENTITY(1,1) NOT NULL, [DescripExcretas]  varchar(33) NOT NULL, CONSTRAINT [PK_ExcretasViv]     PRIMARY KEY CLUSTERED ([IdExcretas]))
CREATE TABLE [dbo].[ServicioViv](     -- NUEVO: reemplaza las columnas bit Telefono/Cable/Internet/Otros
	[IdServicio]  int IDENTITY(1,1) NOT NULL,
	[Descripcion] varchar(30) NOT NULL,
	CONSTRAINT [PK_ServicioViv] PRIMARY KEY CLUSTERED ([IdServicio]),
	CONSTRAINT [UQ_ServicioViv_Descripcion] UNIQUE ([Descripcion])
)
GO

/* -------- catalogos de ficha social -------- */
CREATE TABLE [dbo].[RRHH_AcudeEnferm](  [idAcudeEnferm]  int IDENTITY(1,1) NOT NULL, [AcudeEnfermDescrip]  varchar(16) NOT NULL, CONSTRAINT [PK_RRHH_AcudeEnferm]  PRIMARY KEY CLUSTERED ([idAcudeEnferm]))
CREATE TABLE [dbo].[RRHH_TipoFamilia](  [idTipoFam]      int IDENTITY(1,1) NOT NULL, [TipoFamDescrip]      varchar(10) NOT NULL, CONSTRAINT [PK_RRHH_TipoFamilia]  PRIMARY KEY CLUSTERED ([idTipoFam]))
CREATE TABLE [dbo].[RRHH_RelacPareja](  [idRelacPareja]  int IDENTITY(1,1) NOT NULL, [RelacParejaDescrip]  varchar(12) NOT NULL, CONSTRAINT [PK_RRHH_RelacPareja]  PRIMARY KEY CLUSTERED ([idRelacPareja]))
CREATE TABLE [dbo].[RRHH_RelacPH](      [idRelacPH]      int IDENTITY(1,1) NOT NULL, [RelacPHDescrip]      varchar(14) NOT NULL, CONSTRAINT [PK_RRHH_RelacPH]      PRIMARY KEY CLUSTERED ([idRelacPH]))
CREATE TABLE [dbo].[RRHH_RelacHermano]( [idRelacHermano] int IDENTITY(1,1) NOT NULL, [RelacHermanoDescrip] varchar(12) NOT NULL, CONSTRAINT [PK_RRHH_RelacHermano] PRIMARY KEY CLUSTERED ([idRelacHermano]))
CREATE TABLE [dbo].[RRHH_FuncFamPregunta](   -- NUEVO: las 9 columnas Com1..Soc3 pasan a ser filas
	[IdPregunta] tinyint     NOT NULL,
	[Codigo]     varchar(6)  NOT NULL,
	[Dimension]  varchar(30) NOT NULL,
	[Orden]      tinyint     NOT NULL,
	CONSTRAINT [PK_RRHH_FuncFamPregunta] PRIMARY KEY CLUSTERED ([IdPregunta]),
	CONSTRAINT [UQ_RRHH_FuncFamPregunta_Codigo] UNIQUE ([Codigo])
)
GO

/* -------- catalogos de asistencia (NUEVOS) -------- */
CREATE TABLE [dbo].[EstadoAsistencia](       -- codigos de RegAsisDiario.Estado (F, FR, TLT, PV...)
	[Codigo]      varchar(3)  NOT NULL,
	[Descripcion] varchar(80) NULL,
	CONSTRAINT [PK_EstadoAsistencia] PRIMARY KEY CLUSTERED ([Codigo])
)
CREATE TABLE [dbo].[TipoMarcacion](          -- 2da letra de Marcacion.Estado ('1N' -> N)
	[Codigo]      char(1)     NOT NULL,
	[Descripcion] varchar(50) NULL,
	CONSTRAINT [PK_TipoMarcacion] PRIMARY KEY CLUSTERED ([Codigo])
)
CREATE TABLE [dbo].[DispositivoMarcacion](   -- antes Marcacion.Lugar (texto libre: '*', '53423'...)
	[Codigo]      varchar(10) NOT NULL,
	[Descripcion] varchar(80) NULL,
	CONSTRAINT [PK_DispositivoMarcacion] PRIMARY KEY CLUSTERED ([Codigo])
)
GO

/* ============================================================
   3. PERSONA  (ahora concentra TODOS los atributos propios de la persona)
   ============================================================ */
CREATE TABLE [dbo].[Persona](
	[IdPersona]         int IDENTITY(1,1) NOT NULL,
	[TipoPersona]       char(1)      NOT NULL CONSTRAINT [DF_Persona_TipoPersona] DEFAULT ('N'),
	[Nombres]           varchar(100) NOT NULL,
	[Apellido_Paterno]  varchar(30)  NULL,
	[Apellido_Materno]  varchar(30)  NULL,
	[TipoDocID]         int          NULL,
	[NumDocID]          varchar(12)  NULL,
	[Sexo]              char(1)      NULL,          -- antes en Empleado / Familiar / RRHH_Asegurado
	[FechaNacimiento]   date         NULL,
	[IdEstadoCivil]     int          NULL,          -- antes en Empleado / Familiar / RRHH_Asegurado
	[IdNacionalidad]    int          NULL,          -- antes en Empleado
	[IdTipoSangre]      int          NULL,          -- antes en Empleado / Familiar
	[IdDiscapacidad]    int          NULL,          -- antes en Empleado / Familiar
	[NumAutogenSalud]   varchar(15)  NULL,          -- antes en Empleado / Familiar
	[NumRUC]            varchar(12)  NULL,          -- antes en Empleado
	[Brevete]           varchar(12)  NULL,          -- antes en Empleado
	[NumLibretaMilitar] varchar(10)  NULL,          -- antes en Empleado
	[GradoInstruccion]  varchar(80)  NULL,          -- antes Empleado.GradoInstruccion / Familiar.FamGradoInstruc
	[Profesion]         varchar(80)  NULL,          -- antes en Empleado
	[Ocupacion]         varchar(50)  NULL,          -- antes Familiar.FamOcupacion
	[LugarTrabajo]      varchar(50)  NULL,          -- antes Familiar.FamLugTrab
	[Procedencia]       varchar(50)  NULL,          -- antes en Familiar
	[Enfermedad]        varchar(200) NULL,          -- antes en Familiar
	[TiempoEnfermedad]  varchar(200) NULL,          -- antes en Familiar
	[Direccion]         varchar(100) NULL,
	[IdUbigeo]          char(6)      NULL,
	[UbigeoDireccion]   char(6)      NULL,
	[Telefono]          varchar(15)  NULL,
	[NumCelular]        varchar(15)  NULL,
	[Email]             varchar(50)  NULL,
	[Foto]              varbinary(max) NULL,        -- antes Empleado.Foto (tipo image, obsoleto)
	[FechaRegistro]     datetime     NULL CONSTRAINT [DF_Persona_FechaRegistro] DEFAULT (getdate()),
	CONSTRAINT [PK_Persona] PRIMARY KEY CLUSTERED ([IdPersona]),
	CONSTRAINT [CK_Persona_TipoPersona] CHECK ([TipoPersona] IN ('N','J')),
	CONSTRAINT [CK_Persona_Sexo] CHECK ([Sexo] IN ('M','F')),
	CONSTRAINT [FK_Persona_TipoDocID]      FOREIGN KEY ([TipoDocID])       REFERENCES [dbo].[TipoDocID]([TipoDocID]),
	CONSTRAINT [FK_Persona_EstadoCivil]    FOREIGN KEY ([IdEstadoCivil])   REFERENCES [dbo].[EstadoCivil]([IdEstadoCivil]),
	CONSTRAINT [FK_Persona_Nacionalidad]   FOREIGN KEY ([IdNacionalidad])  REFERENCES [dbo].[Nacionalidad]([IdNacionalidad]),
	CONSTRAINT [FK_Persona_TipoSangre]     FOREIGN KEY ([IdTipoSangre])    REFERENCES [dbo].[TipoSangre]([IdTipoSangre]),
	CONSTRAINT [FK_Persona_Discapacidad]   FOREIGN KEY ([IdDiscapacidad])  REFERENCES [dbo].[Discapacidad]([IdDiscapacidad]),
	CONSTRAINT [FK_Persona_Ubigeo]         FOREIGN KEY ([IdUbigeo])        REFERENCES [dbo].[Distrito]([IdUbigeo]),
	CONSTRAINT [FK_Persona_UbigeoDireccion] FOREIGN KEY ([UbigeoDireccion]) REFERENCES [dbo].[Distrito]([IdUbigeo])
)
GO
-- un documento no puede repetirse (clave candidata)
CREATE UNIQUE NONCLUSTERED INDEX [UX_Persona_Documento] ON [dbo].[Persona]([TipoDocID], [NumDocID]) WHERE [NumDocID] IS NOT NULL
CREATE NONCLUSTERED INDEX [IX_Persona_Nombre] ON [dbo].[Persona]([Apellido_Paterno], [Apellido_Materno], [Nombres])
GO

/* ============================================================
   4. ESTRUCTURA ORGANIZACIONAL
   ============================================================ */
CREATE TABLE [dbo].[EstructOrganiz](
	[Year]               int          NOT NULL,
	[idAreaOrganiz]      int          NOT NULL,
	[AreaOrganizacional] varchar(200) NOT NULL,
	[Abrev]              varchar(10)  NOT NULL,
	[Sigla]              varchar(6)   NOT NULL,
	[Mision]             varchar(1000) NULL,
	[Vision]             varchar(1000) NULL,
	[YearDe]             int          NULL,
	[idDependeDe]        int          NULL,
	[IdEmpleado]         int          NULL,      -- jefe del area (FK se agrega tras crear Empleado)
	CONSTRAINT [PK_EstructOrganiz] PRIMARY KEY CLUSTERED ([Year], [idAreaOrganiz]),
	CONSTRAINT [FK_EstructOrganiz_DependeDe] FOREIGN KEY ([YearDe], [idDependeDe]) REFERENCES [dbo].[EstructOrganiz]([Year], [idAreaOrganiz])
)
GO

/* ============================================================
   5. HORARIOS
   ============================================================ */
CREATE TABLE [dbo].[Horario](
	[idHorario]       int IDENTITY(1,1) NOT NULL,
	[Descrip_Horario] varchar(20) NULL,
	[Ingreso1]        time(0) NULL,
	[Salida1]         time(0) NULL,
	[Ingreso2]        time(0) NULL,
	[Salida2]         time(0) NULL,
	[SgtDia]          bit NULL,
	[Estado]          bit NULL,
	CONSTRAINT [PK_Horario] PRIMARY KEY CLUSTERED ([idHorario])
)
GO

/* ============================================================
   6. EMPLEADO  (= vinculo laboral; una persona puede tener varios)
   ============================================================ */
CREATE TABLE [dbo].[Empleado](
	[IdEmpleado]       int IDENTITY(1,1) NOT NULL,
	[IdPersona]        int          NOT NULL,
	[FechaIngreso]     date         NULL,
	[Estado]           bit          NULL,      -- antes char(1) '1'/'0'
	[IdTipoTrabajador] int          NULL,
	[IdCargo]          int          NULL,
	[idHorario]        int          NULL,
	[idLocal]          int          NULL,
	[IdRegimenPen]     int          NULL,
	[IdAFP]            int          NULL,
	[NroAFP]           varchar(20)  NULL,
	[FechaAFP]         date         NULL,
	[CtaCorriente]     varchar(16)  NULL,
	[DecLey]           varchar(10)  NULL,
	[ExpSocial]        varchar(20)  NULL,
	[CargaFam]         tinyint      NULL,      -- carga familiar DECLARADA (antes varchar)
	[JefeHogar]        bit          NULL,
	[IdViveCon]        int          NULL,
	CONSTRAINT [PK_Empleado] PRIMARY KEY CLUSTERED ([IdEmpleado]),
	CONSTRAINT [FK_Empleado_Persona]        FOREIGN KEY ([IdPersona])        REFERENCES [dbo].[Persona]([IdPersona]),
	CONSTRAINT [FK_Empleado_TipoTrabajador] FOREIGN KEY ([IdTipoTrabajador]) REFERENCES [dbo].[TipoTrabajador]([IdTipoTrabajador]),
	CONSTRAINT [FK_Empleado_Cargo]          FOREIGN KEY ([IdCargo])          REFERENCES [dbo].[Cargo]([IdCargo]),
	CONSTRAINT [FK_Empleado_Horario]        FOREIGN KEY ([idHorario])        REFERENCES [dbo].[Horario]([idHorario]),
	CONSTRAINT [FK_Empleado_Local]          FOREIGN KEY ([idLocal])          REFERENCES [dbo].[Local]([idLocal]),
	CONSTRAINT [FK_Empleado_RegimenPension] FOREIGN KEY ([IdRegimenPen])     REFERENCES [dbo].[RegimenPension]([IdRegimenPen]),
	CONSTRAINT [FK_Empleado_AFP]            FOREIGN KEY ([IdAFP])            REFERENCES [dbo].[AFP]([IdAFP]),
	CONSTRAINT [FK_Empleado_ViveCon]        FOREIGN KEY ([IdViveCon])        REFERENCES [dbo].[ViveCon]([IdViveCon])
)
GO
CREATE NONCLUSTERED INDEX [IX_Empleado_IdPersona] ON [dbo].[Empleado]([IdPersona])
GO
ALTER TABLE [dbo].[EstructOrganiz] ADD CONSTRAINT [FK_EstructOrganiz_Jefe] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado])
GO

/* -------- historial de areas -------- */
CREATE TABLE [dbo].[Empleado_Area](
	[Year]          int NOT NULL,
	[idAreaOrganiz] int NOT NULL,
	[IdEmpleado]    int NOT NULL,
	CONSTRAINT [PK_Empleado_Area] PRIMARY KEY CLUSTERED ([Year], [idAreaOrganiz], [IdEmpleado]),
	CONSTRAINT [FK_Empleado_Area_Empleado]       FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_Empleado_Area_EstructOrganiz] FOREIGN KEY ([Year], [idAreaOrganiz]) REFERENCES [dbo].[EstructOrganiz]([Year], [idAreaOrganiz])
)
CREATE NONCLUSTERED INDEX [IX_Empleado_Area_IdEmpleado] ON [dbo].[Empleado_Area]([IdEmpleado], [Year])
CREATE TABLE [dbo].[Rotacion](
	[idEmpleado]    int NOT NULL,
	[idRotacion]    int NOT NULL,
	[FechaMemo]     date NULL,
	[NroMemo]       nvarchar(50) NULL,
	[Year]          int NOT NULL,
	[idAreaOrganiz] int NOT NULL,
	CONSTRAINT [PK_Rotacion] PRIMARY KEY CLUSTERED ([idEmpleado], [idRotacion]),
	CONSTRAINT [FK_Rotacion_Empleado]       FOREIGN KEY ([idEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_Rotacion_EstructOrganiz] FOREIGN KEY ([Year], [idAreaOrganiz]) REFERENCES [dbo].[EstructOrganiz]([Year], [idAreaOrganiz])
)
CREATE TABLE [dbo].[ResponsableXUO](
	[IdResponsable] int IDENTITY(1,1) NOT NULL,
	[Year]          int NOT NULL,
	[idAreaOrganiz] int NOT NULL,
	[IdEmpleado]    int NOT NULL,
	[F_Inicio]      date NULL,
	[F_Termino]     date NULL,
	CONSTRAINT [PK_ResponsableXUO] PRIMARY KEY CLUSTERED ([IdResponsable]),
	CONSTRAINT [FK_ResponsableXUO_Empleado]       FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_ResponsableXUO_EstructOrganiz] FOREIGN KEY ([Year], [idAreaOrganiz]) REFERENCES [dbo].[EstructOrganiz]([Year], [idAreaOrganiz])
)
GO

/* -------- vida laboral -------- */
CREATE TABLE [dbo].[PeriodoLaboral](
	[IdEmpleado]            int NOT NULL,
	[NroPeriodo]            int NOT NULL,
	[FechaIngreso]          date NULL,
	[FechaSalida]           date NULL,
	[NumResoIngreInstitu]   varchar(30) NULL,
	[FechaResoIngreInstitu] date NULL,
	[Judicial]              bit NULL,
	[Obs]                   varchar(2000) NULL,
	CONSTRAINT [PK_PeriodoLaboral] PRIMARY KEY CLUSTERED ([IdEmpleado], [NroPeriodo]),
	CONSTRAINT [FK_PeriodoLaboral_Empleado] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado])
)
CREATE TABLE [dbo].[Resolucion](
	[idEmpleado]       int NOT NULL,
	[idResolucion]     int NOT NULL,
	[idTipoResolucion] int NOT NULL,
	[NumeroResolucion] nvarchar(50) NULL,
	[FechaResolucion]  date NULL,
	[FechaVigencia]    date NULL,     -- NUEVO: fecha desde la que surte efecto (p.ej. ingreso al Estado)
	CONSTRAINT [PK_Resolucion] PRIMARY KEY CLUSTERED ([idEmpleado], [idResolucion]),
	CONSTRAINT [FK_Resolucion_Empleado]       FOREIGN KEY ([idEmpleado])       REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_Resolucion_TipoResolucion] FOREIGN KEY ([idTipoResolucion]) REFERENCES [dbo].[TipoResolucion]([idTipoResolucion])
)
CREATE TABLE [dbo].[Comportamiento](
	[idComportamiento]     int IDENTITY(1,1) NOT NULL,
	[IdEmpleado]           int NOT NULL,
	[idTipoComportamiento] int NOT NULL,
	[Fecha]                date NULL,
	[NumResol]             varchar(200) NULL,
	[Titulo]               varchar(1000) NULL,
	[Descripcion]          varchar(2000) NULL,
	CONSTRAINT [PK_Comportamiento] PRIMARY KEY CLUSTERED ([idComportamiento]),
	CONSTRAINT [FK_Comportamiento_Empleado]           FOREIGN KEY ([IdEmpleado])           REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_Comportamiento_TipoComportamiento] FOREIGN KEY ([idTipoComportamiento]) REFERENCES [dbo].[TipoComportamiento]([idTipoComportamiento])
)
GO

/* -------- formacion -------- */
CREATE TABLE [dbo].[EstudiosRealizado](
	[IdEmpleado]      int NOT NULL,
	[IdTipoEstudios]  int NOT NULL,
	[Correlativo]     int NOT NULL,
	[CentroEstudios]  varchar(150) NULL,
	[Especialidad]    varchar(100) NULL,
	[FechaInicio]     date NULL,          -- antes [AñoInicio] datetime
	[FechaTermino]    date NULL,          -- antes [AñoTermino] datetime
	[IdEstadoEstudio] int NULL,
	CONSTRAINT [PK_EstudiosRealizado] PRIMARY KEY CLUSTERED ([IdEmpleado], [IdTipoEstudios], [Correlativo]),
	CONSTRAINT [FK_EstudiosRealizado_Empleado]      FOREIGN KEY ([IdEmpleado])      REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_EstudiosRealizado_TipoEstudio]   FOREIGN KEY ([IdTipoEstudios])  REFERENCES [dbo].[TipoEstudio]([IdTipoEstudios]),
	CONSTRAINT [FK_EstudiosRealizado_EstadoEstudio] FOREIGN KEY ([IdEstadoEstudio]) REFERENCES [dbo].[EstadoEstudio]([IdEstadoEstudio])
)
CREATE TABLE [dbo].[TitulosEmpleado](
	[IdTitulo]          int IDENTITY(1,1) NOT NULL,  -- NUEVO: antes la PK era solo NumColegiatura
	[IdEmpleado]        int NOT NULL,
	[NumColegiatura]    varchar(20) NOT NULL,
	[DenominacionGrado] varchar(100) NULL,
	[Institucion]       varchar(60) NULL,
	[Fecha]             date NULL,
	CONSTRAINT [PK_TitulosEmpleado] PRIMARY KEY CLUSTERED ([IdTitulo]),
	CONSTRAINT [UQ_TitulosEmpleado_Colegiatura] UNIQUE ([IdEmpleado], [NumColegiatura]),
	CONSTRAINT [FK_TitulosEmpleado_Empleado] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado])
)
CREATE TABLE [dbo].[Capacitacion](
	[IdCapacitacion]          int IDENTITY(1,1) NOT NULL,
	[IdEmpleado]              int NOT NULL,
	[NombreCurso]             varchar(300) NULL,
	[InstitucionOrganizadora] varchar(300) NULL,
	[FechaInicio]             date NULL,
	[FechaFin]                date NULL,
	[NumHoras]                smallint NULL,    -- antes varchar(5)
	CONSTRAINT [PK_Capacitacion] PRIMARY KEY CLUSTERED ([IdCapacitacion]),
	CONSTRAINT [FK_Capacitacion_Empleado] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [CK_Capacitacion_NumHoras] CHECK ([NumHoras] >= 0)
)
CREATE TABLE [dbo].[ExpLaboral](
	[IdEmpleado]        int NOT NULL,
	[IdExpLab]          int NOT NULL,
	[NomInstitucion]    varchar(120) NULL,
	[IdTipoInstitucion] int NULL,
	[UnidadOrganica]    varchar(100) NULL,
	[Cargo]             varchar(40) NULL,
	[FechaIngreso]      date NULL,
	[FechaEgreso]       date NULL,
	CONSTRAINT [PK_ExpLaboral] PRIMARY KEY CLUSTERED ([IdEmpleado], [IdExpLab]),
	CONSTRAINT [FK_ExpLaboral_Empleado]        FOREIGN KEY ([IdEmpleado])        REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_ExpLaboral_TipoInstitucion] FOREIGN KEY ([IdTipoInstitucion]) REFERENCES [dbo].[TipoInstitucion]([IdTipoInstitucion])
)
GO

/* ============================================================
   7. FAMILIA
   ============================================================ */
CREATE TABLE [dbo].[Familiar](
	[IdEmpleado] int NOT NULL,
	[IdPersona]  int NOT NULL,
	[IdTipoFam]  int NULL,       -- parentesco con el empleado
	[ViveCasa]   bit NULL,       -- depende del par (empleado, familiar): se queda aqui
	CONSTRAINT [PK_Familiar] PRIMARY KEY CLUSTERED ([IdEmpleado], [IdPersona]),
	CONSTRAINT [FK_Familiar_Empleado]     FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_Familiar_Persona]      FOREIGN KEY ([IdPersona])  REFERENCES [dbo].[Persona]([IdPersona]),
	CONSTRAINT [FK_Familiar_TipoFamiliar] FOREIGN KEY ([IdTipoFam])  REFERENCES [dbo].[TipoFamiliar]([IdTipoFam])
)
CREATE NONCLUSTERED INDEX [IX_Familiar_IdPersona] ON [dbo].[Familiar]([IdPersona])
CREATE TABLE [dbo].[RRHH_Asegurado](
	[idEmpleado]            int NOT NULL,
	[NroBeneficiario]       int NOT NULL,
	[idPersona]             int NOT NULL,
	[IdVinculoAsegurado]    int NULL,
	[NroPartida]            varchar(50) NULL,
	[NroResolMayorIndiscap] varchar(50) NULL,
	[FechaInicioVinculo]    date NULL,
	[FechaFinVinculo]       date NULL,
	[idMotivoBaja]          int NULL,
	[NroPartidaDef]         varchar(50) NULL,
	[OtrosMotivos]          varchar(50) NULL,
	[Reevaluar]             char(1) NULL,
	CONSTRAINT [PK_RRHH_Asegurado] PRIMARY KEY CLUSTERED ([idEmpleado], [NroBeneficiario]),
	CONSTRAINT [FK_RRHH_Asegurado_Empleado]   FOREIGN KEY ([idEmpleado])         REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_RRHH_Asegurado_Persona]    FOREIGN KEY ([idPersona])          REFERENCES [dbo].[Persona]([IdPersona]),
	CONSTRAINT [FK_RRHH_Asegurado_Vinculo]    FOREIGN KEY ([IdVinculoAsegurado]) REFERENCES [dbo].[VinculoAsegurado]([IdVinculoAsegurado]),
	CONSTRAINT [FK_RRHH_Asegurado_MotivoBaja] FOREIGN KEY ([idMotivoBaja])       REFERENCES [dbo].[MotivoBaja]([idMotivoBaja])
)
GO

/* ============================================================
   8. FICHA SOCIAL  (todas 1:1 con Empleado -> UNIQUE(IdEmpleado))
   ============================================================ */
CREATE TABLE [dbo].[Vivienda](
	[IdVivienda]     int IDENTITY(1,1) NOT NULL,
	[IdEmpleado]     int NOT NULL,
	[IdTenencia]     int NULL,
	[IdTipo]         int NULL,
	[IdMaterial]     int NULL,
	[IdConservacion] int NULL,
	[IdUbicacion]    int NULL,
	[IdAlumbrado]    int NULL,
	[OtroAlumbrado]  varchar(50) NULL,
	[IdAgua]         int NULL,
	[OtroAgua]       varchar(50) NULL,
	[IdExcretas]     int NULL,
	[OtroExcreta]    varchar(50) NULL,
	[NumDormitorio]  tinyint NULL,
	[NumServHig]     tinyint NULL,
	[NumCompart]     tinyint NULL,
	[NumPerDorm]     tinyint NULL,
	[Observacion]    varchar(200) NULL,
	CONSTRAINT [PK_Vivienda] PRIMARY KEY CLUSTERED ([IdVivienda]),
	CONSTRAINT [UQ_Vivienda_IdEmpleado] UNIQUE ([IdEmpleado]),
	CONSTRAINT [FK_Vivienda_Empleado]     FOREIGN KEY ([IdEmpleado])     REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_Vivienda_Tenencia]     FOREIGN KEY ([IdTenencia])     REFERENCES [dbo].[TenenciaViv]([IdTenencia]),
	CONSTRAINT [FK_Vivienda_Tipo]         FOREIGN KEY ([IdTipo])         REFERENCES [dbo].[TipoViv]([IdTipo]),
	CONSTRAINT [FK_Vivienda_Material]     FOREIGN KEY ([IdMaterial])     REFERENCES [dbo].[MaterialViv]([IdMaterial]),
	CONSTRAINT [FK_Vivienda_Conservacion] FOREIGN KEY ([IdConservacion]) REFERENCES [dbo].[ConservacionViv]([IdConservacion]),
	CONSTRAINT [FK_Vivienda_Ubicacion]    FOREIGN KEY ([IdUbicacion])    REFERENCES [dbo].[UbicacionViv]([IdUbicacion]),
	CONSTRAINT [FK_Vivienda_Alumbrado]    FOREIGN KEY ([IdAlumbrado])    REFERENCES [dbo].[AlumbradoViv]([IdAlumbrado]),
	CONSTRAINT [FK_Vivienda_Agua]         FOREIGN KEY ([IdAgua])         REFERENCES [dbo].[AguaViv]([IdAgua]),
	CONSTRAINT [FK_Vivienda_Excretas]     FOREIGN KEY ([IdExcretas])     REFERENCES [dbo].[ExcretasViv]([IdExcretas])
)
CREATE TABLE [dbo].[ViviendaServicio](   -- NUEVO (1FN): servicios de la vivienda como filas
	[IdVivienda] int NOT NULL,
	[IdServicio] int NOT NULL,
	CONSTRAINT [PK_ViviendaServicio] PRIMARY KEY CLUSTERED ([IdVivienda], [IdServicio]),
	CONSTRAINT [FK_ViviendaServicio_Vivienda] FOREIGN KEY ([IdVivienda]) REFERENCES [dbo].[Vivienda]([IdVivienda]),
	CONSTRAINT [FK_ViviendaServicio_Servicio] FOREIGN KEY ([IdServicio]) REFERENCES [dbo].[ServicioViv]([IdServicio])
)
CREATE TABLE [dbo].[RRHH_Salud](
	[idSalud]             int IDENTITY(1,1) NOT NULL,
	[IdEmpleado]          int NOT NULL,
	[EnferCronica]        bit NULL,
	[EnferCronicaDescrip] varchar(200) NULL,
	[FamDiscapacidad]     bit NULL,
	[FamDiscapDescrip]    varchar(200) NULL,
	[Alergico]            bit NULL,
	[AlergicoDescrip]     varchar(200) NULL,
	[idAcudeEnferm]       int NULL,
	CONSTRAINT [PK_RRHH_Salud] PRIMARY KEY CLUSTERED ([idSalud]),
	CONSTRAINT [UQ_RRHH_Salud_IdEmpleado] UNIQUE ([IdEmpleado]),
	CONSTRAINT [FK_RRHH_Salud_Empleado]    FOREIGN KEY ([IdEmpleado])    REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_RRHH_Salud_AcudeEnferm] FOREIGN KEY ([idAcudeEnferm]) REFERENCES [dbo].[RRHH_AcudeEnferm]([idAcudeEnferm])
)
CREATE TABLE [dbo].[RRHH_AspSocio](
	[idAspSocio]        int IDENTITY(1,1) NOT NULL,
	[IdEmpleado]        int NOT NULL,
	[PrincipProblemas]  varchar(400) NULL,
	[PropMejoraInstitu] varchar(400) NULL,
	[hobby]             varchar(400) NULL,
	[ActRecreaInstitu]  varchar(400) NULL,
	-- diagnostico social: antes eran columnas de Empleado
	[ObsFam]            nvarchar(500) NULL,
	[AspecSalud]        varchar(1000) NULL,
	[AspecEducacion]    varchar(1000) NULL,
	[AspecRecreacion]   varchar(1000) NULL,
	[AspecPsico]        varchar(1000) NULL,
	[AspecSocial]       varchar(1000) NULL,
	[DiagnosSocial]     varchar(1000) NULL,
	CONSTRAINT [PK_RRHH_AspSocio] PRIMARY KEY CLUSTERED ([idAspSocio]),
	CONSTRAINT [UQ_RRHH_AspSocio_IdEmpleado] UNIQUE ([IdEmpleado]),
	CONSTRAINT [FK_RRHH_AspSocio_Empleado] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado])
)
CREATE TABLE [dbo].[RRHH_DinamicaFamiliar](
	[idDinamica]     int IDENTITY(1,1) NOT NULL,
	[IdEmpleado]     int NOT NULL,
	[idTipoFam]      int NULL,
	[idRelaPareja]   int NULL,
	[idRelaPH]       int NULL,
	[idRelacHermano] int NULL,
	[Observacion]    varchar(200) NULL,
	CONSTRAINT [PK_RRHH_DinamicaFamiliar] PRIMARY KEY CLUSTERED ([idDinamica]),
	CONSTRAINT [UQ_RRHH_DinamicaFamiliar_IdEmpleado] UNIQUE ([IdEmpleado]),
	CONSTRAINT [FK_RRHH_DinamicaFamiliar_Empleado]     FOREIGN KEY ([IdEmpleado])     REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_RRHH_DinamicaFamiliar_TipoFamilia]  FOREIGN KEY ([idTipoFam])      REFERENCES [dbo].[RRHH_TipoFamilia]([idTipoFam]),
	CONSTRAINT [FK_RRHH_DinamicaFamiliar_RelacPareja]  FOREIGN KEY ([idRelaPareja])   REFERENCES [dbo].[RRHH_RelacPareja]([idRelacPareja]),
	CONSTRAINT [FK_RRHH_DinamicaFamiliar_RelacPH]      FOREIGN KEY ([idRelaPH])       REFERENCES [dbo].[RRHH_RelacPH]([idRelacPH]),
	CONSTRAINT [FK_RRHH_DinamicaFamiliar_RelacHermano] FOREIGN KEY ([idRelacHermano]) REFERENCES [dbo].[RRHH_RelacHermano]([idRelacHermano])
)
CREATE TABLE [dbo].[RRHH_FuncFamRespuesta](   -- NUEVO (1FN): reemplaza RRHH_FuncFam (Com1..Soc3)
	[IdEmpleado] int        NOT NULL,
	[IdPregunta] tinyint    NOT NULL,
	[Respuesta]  varchar(2) NOT NULL,
	CONSTRAINT [PK_RRHH_FuncFamRespuesta] PRIMARY KEY CLUSTERED ([IdEmpleado], [IdPregunta]),
	CONSTRAINT [FK_RRHH_FuncFamRespuesta_Empleado] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_RRHH_FuncFamRespuesta_Pregunta] FOREIGN KEY ([IdPregunta]) REFERENCES [dbo].[RRHH_FuncFamPregunta]([IdPregunta])
)
GO

/* ============================================================
   9. ASISTENCIA
   ============================================================ */
CREATE TABLE [dbo].[HorarioTemporal](
	[idEmpleado]      int NOT NULL,
	[N]               int NOT NULL,
	[FechaInicio]     date NULL,
	[FechaFin]        date NULL,
	[HorarioAsignado] int NULL,
	[NroDocumento]    nvarchar(50) NULL,
	[Sisgedo]         nvarchar(50) NULL,
	[FechaRegistro]   datetime NULL CONSTRAINT [DF_HorarioTemporal_FechaRegistro] DEFAULT (getdate()),
	CONSTRAINT [PK_HorarioTemporal] PRIMARY KEY CLUSTERED ([idEmpleado], [N]),
	CONSTRAINT [FK_HorarioTemporal_Empleado] FOREIGN KEY ([idEmpleado])      REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_HorarioTemporal_Horario]  FOREIGN KEY ([HorarioAsignado]) REFERENCES [dbo].[Horario]([idHorario])
)
CREATE TABLE [dbo].[Asistencia](
	[Fecha]      date    NOT NULL,
	[IdEmpleado] int     NOT NULL,
	[idHorario]  int     NULL,
	[Hor_Ent]    time(0) NULL,
	[Alm_Sal]    time(0) NULL,
	[Alm_Ent]    time(0) NULL,
	[Hor_Sal]    time(0) NULL,
	[Flag_Ent]   bit NULL,
	[Flag_Sal]   bit NULL,
	[Estado]     bit NULL,
	[Usuario]    nvarchar(50) NULL,
	[PC]         nvarchar(50) NULL,
	CONSTRAINT [PK_Asistencia] PRIMARY KEY CLUSTERED ([Fecha], [IdEmpleado]),
	CONSTRAINT [FK_Asistencia_Empleado] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_Asistencia_Horario]  FOREIGN KEY ([idHorario])  REFERENCES [dbo].[Horario]([idHorario])
)
CREATE NONCLUSTERED INDEX [IX_Asistencia_Empleado] ON [dbo].[Asistencia]([IdEmpleado], [Fecha])
CREATE TABLE [dbo].[Marcacion](
	[Fecha]          datetime   NOT NULL,
	[IdEmpleado]     int        NOT NULL,
	[NroMarcacion]   tinyint    NOT NULL,   -- antes 1er caracter de Estado ('1N' -> 1)
	[TipoMarcacion]  char(1)    NOT NULL,   -- antes 2do caracter de Estado ('1N' -> N)
	[CodDispositivo] varchar(10) NULL,      -- antes Lugar
	[idHorario]      int        NULL,
	CONSTRAINT [PK_Marcacion] PRIMARY KEY CLUSTERED ([Fecha], [IdEmpleado]),
	CONSTRAINT [CK_Marcacion_NroMarcacion] CHECK ([NroMarcacion] BETWEEN 1 AND 9),
	CONSTRAINT [FK_Marcacion_Empleado]      FOREIGN KEY ([IdEmpleado])     REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_Marcacion_TipoMarcacion] FOREIGN KEY ([TipoMarcacion])  REFERENCES [dbo].[TipoMarcacion]([Codigo]),
	CONSTRAINT [FK_Marcacion_Dispositivo]   FOREIGN KEY ([CodDispositivo]) REFERENCES [dbo].[DispositivoMarcacion]([Codigo]),
	CONSTRAINT [FK_Marcacion_Horario]       FOREIGN KEY ([idHorario])      REFERENCES [dbo].[Horario]([idHorario])
)
CREATE NONCLUSTERED INDEX [IX_Marcacion_Empleado] ON [dbo].[Marcacion]([IdEmpleado], [Fecha])
CREATE TABLE [dbo].[RegAsisDiario](
	[Fecha]         date       NOT NULL,
	[IdEmpleado]    int        NOT NULL,
	[MinNormales]   smallint   NULL,     -- antes varchar(4)
	[SegundosTarde] int        NULL,     -- antes MinTarde varchar(18) con texto 'hh:mm:ss'
	[Estado]        varchar(3) NULL,
	CONSTRAINT [PK_RegAsisDiario] PRIMARY KEY CLUSTERED ([Fecha], [IdEmpleado]),
	CONSTRAINT [CK_RegAsisDiario_SegundosTarde] CHECK ([SegundosTarde] >= 0),
	CONSTRAINT [FK_RegAsisDiario_Empleado] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_RegAsisDiario_Estado]   FOREIGN KEY ([Estado])     REFERENCES [dbo].[EstadoAsistencia]([Codigo])
)
CREATE NONCLUSTERED INDEX [IX_RegAsisDiario_Empleado] ON [dbo].[RegAsisDiario]([IdEmpleado], [Fecha])
CREATE TABLE [dbo].[Permiso](
	[IdEmpleado]       int NOT NULL,
	[NPermiso]         int NOT NULL,
	[idMotivo]         int NULL,
	[FechaInicio]      date NULL,
	[FechaFin]         date NULL,
	[Hora_Sal]         time(0) NULL,
	[Hora_Ret]         time(0) NULL,
	[Dia]              bit NULL,
	[Retorno]          bit NULL,
	[Lugar]            varchar(20) NULL,
	[Referencia]       varchar(100) NULL,
	[Autorizacion]     varchar(50) NULL,
	[AutorizacionRRHH] varchar(50) NULL,
	[Autorizado]       bit NULL,
	[Obs]              varchar(200) NULL,
	CONSTRAINT [PK_Permiso] PRIMARY KEY CLUSTERED ([IdEmpleado], [NPermiso]),
	CONSTRAINT [FK_Permiso_Empleado]    FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado]),
	CONSTRAINT [FK_Permiso_Motivo_Perm] FOREIGN KEY ([idMotivo])   REFERENCES [dbo].[Motivo_Perm]([idMotivo])
)
GO

/* ============================================================
   10. SEGURIDAD
   ============================================================ */
CREATE TABLE [dbo].[SistemaOpcion](
	[IdSistemaOpcion] char(7)      NOT NULL,
	[Descripcion]     varchar(150) NOT NULL,
	CONSTRAINT [PK_SistemaOpcion] PRIMARY KEY CLUSTERED ([IdSistemaOpcion])
)
CREATE TABLE [dbo].[Rol](
	[IdRol]       int IDENTITY(1,1) NOT NULL,
	[Descripcion] varchar(30) NOT NULL,
	[IdSistema]   char(7)     NULL,
	CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED ([IdRol]),
	CONSTRAINT [FK_Rol_SistemaOpcion] FOREIGN KEY ([IdSistema]) REFERENCES [dbo].[SistemaOpcion]([IdSistemaOpcion])
)
CREATE TABLE [dbo].[Rol_Acceso](
	[IdRol]           int     NOT NULL,
	[IdSistemaOpcion] char(7) NOT NULL,
	CONSTRAINT [PK_Rol_Acceso] PRIMARY KEY CLUSTERED ([IdRol], [IdSistemaOpcion]),
	CONSTRAINT [FK_Rol_Acceso_Rol]           FOREIGN KEY ([IdRol])           REFERENCES [dbo].[Rol]([IdRol]),
	CONSTRAINT [FK_Rol_Acceso_SistemaOpcion] FOREIGN KEY ([IdSistemaOpcion]) REFERENCES [dbo].[SistemaOpcion]([IdSistemaOpcion])
)
CREATE TABLE [dbo].[Usuario](
	[IdUsuario]    int IDENTITY(1,1) NOT NULL,
	[Login]        varchar(20)  NOT NULL,
	[Descripcion]  varchar(40)  NULL,
	[PasswordHash] nvarchar(200) NULL,
	[Estado]       bit          NULL,
	[Fecha]        datetime     NULL,
	[IdEmpleado]   int          NULL,
	CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED ([IdUsuario]),
	CONSTRAINT [UQ_Usuario_Login] UNIQUE ([Login]),
	CONSTRAINT [FK_Usuario_Empleado] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Empleado]([IdEmpleado])
)
CREATE TABLE [dbo].[Usuario_Rol](
	[IdUsuario] int NOT NULL,
	[IdRol]     int NOT NULL,
	CONSTRAINT [PK_Usuario_Rol] PRIMARY KEY CLUSTERED ([IdUsuario], [IdRol]),
	CONSTRAINT [FK_Usuario_Rol_Usuario] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario]([IdUsuario]),
	CONSTRAINT [FK_Usuario_Rol_Rol]     FOREIGN KEY ([IdRol])     REFERENCES [dbo].[Rol]([IdRol])
)
CREATE TABLE [dbo].[Bitacora](
	[IdBitacora] int IDENTITY(1,1) NOT NULL,
	[Fecha]      datetime      NOT NULL CONSTRAINT [DF_Bitacora_Fecha] DEFAULT (getdate()),
	[Usuario]    varchar(100)  NULL,
	[Accion]     varchar(50)   NULL,
	[Entidad]    varchar(100)  NULL,
	[Detalle]    nvarchar(max) NULL,   -- antes text (obsoleto)
	CONSTRAINT [PK_Bitacora] PRIMARY KEY CLUSTERED ([IdBitacora])
)
GO

/* ============================================================
   11. VISTAS: datos DERIVADOS que antes se guardaban en columnas
   ============================================================ */
CREATE VIEW [dbo].[vw_Ubigeo] AS
-- compatibilidad con la antigua tabla Ubigeo (solo distritos)
SELECT d.IdUbigeo, d.CodDepartamento AS Departamento, d.CodProvincia AS Provincia, d.CodDistrito AS Distrito,
       d.Nombre AS Descripcion, p.Nombre AS NombreProvincia, dp.Nombre AS NombreDepartamento
FROM dbo.Distrito d
JOIN dbo.Provincia p     ON p.CodDepartamento = d.CodDepartamento AND p.CodProvincia = d.CodProvincia
JOIN dbo.Departamento dp ON dp.CodDepartamento = d.CodDepartamento
GO
CREATE VIEW [dbo].[vw_EmpleadoAreaActual] AS
-- reemplaza Empleado.[Year]/[idAreaOrganiz]: el area vigente es la del anio mas reciente del historial
SELECT x.IdEmpleado, x.[Year], x.idAreaOrganiz, eo.AreaOrganizacional
FROM (
	SELECT ea.IdEmpleado, ea.[Year], ea.idAreaOrganiz,
	       ROW_NUMBER() OVER (PARTITION BY ea.IdEmpleado ORDER BY ea.[Year] DESC, ea.idAreaOrganiz DESC) AS rn
	FROM dbo.Empleado_Area ea
) x
JOIN dbo.EstructOrganiz eo ON eo.[Year] = x.[Year] AND eo.idAreaOrganiz = x.idAreaOrganiz
WHERE x.rn = 1
GO
CREATE VIEW [dbo].[vw_EmpleadoResumenFamiliar] AS
-- reemplaza Empleado.NumHijos (dato derivable de Familiar)
SELECT e.IdEmpleado,
       (SELECT COUNT(*) FROM dbo.Familiar f WHERE f.IdEmpleado = e.IdEmpleado AND f.IdTipoFam = 4) AS NumHijos,
       (SELECT COUNT(*) FROM dbo.Familiar f WHERE f.IdEmpleado = e.IdEmpleado) AS NumFamiliares,
       e.CargaFam AS CargaFamDeclarada
FROM dbo.Empleado e
GO
CREATE VIEW [dbo].[vw_FamiliarDetalle] AS
-- reemplaza Familiar.LaboraInstitucion (derivable: el familiar tiene un vinculo laboral activo)
SELECT f.IdEmpleado, f.IdPersona, f.IdTipoFam, f.ViveCasa,
       CAST(CASE WHEN EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdPersona = f.IdPersona AND ISNULL(e.Estado, 0) = 1)
                 THEN 1 ELSE 0 END AS bit) AS LaboraInstitucion
FROM dbo.Familiar f
GO
CREATE VIEW [dbo].[vw_EstructOrganizDotacion] AS
-- reemplaza EstructOrganiz.NroTrabajaUO (derivable del historial de areas)
SELECT eo.[Year], eo.idAreaOrganiz, eo.AreaOrganizacional,
       (SELECT COUNT(*) FROM dbo.Empleado_Area ea WHERE ea.[Year] = eo.[Year] AND ea.idAreaOrganiz = eo.idAreaOrganiz) AS NroTrabajaUO
FROM dbo.EstructOrganiz eo
GO
CREATE VIEW [dbo].[vw_RRHH_FuncFam] AS
-- compatibilidad: devuelve la forma antigua (1 fila por empleado, 9 columnas)
SELECT r.IdEmpleado,
       MAX(CASE WHEN p.Codigo = 'Com1'  THEN r.Respuesta END) AS Com1,
       MAX(CASE WHEN p.Codigo = 'Com2'  THEN r.Respuesta END) AS Com2,
       MAX(CASE WHEN p.Codigo = 'Afec1' THEN r.Respuesta END) AS Afec1,
       MAX(CASE WHEN p.Codigo = 'Afec2' THEN r.Respuesta END) AS Afec2,
       MAX(CASE WHEN p.Codigo = 'Decis1' THEN r.Respuesta END) AS Decis1,
       MAX(CASE WHEN p.Codigo = 'Decis2' THEN r.Respuesta END) AS Decis2,
       MAX(CASE WHEN p.Codigo = 'Soc1'  THEN r.Respuesta END) AS Soc1,
       MAX(CASE WHEN p.Codigo = 'Soc2'  THEN r.Respuesta END) AS Soc2,
       MAX(CASE WHEN p.Codigo = 'Soc3'  THEN r.Respuesta END) AS Soc3
FROM dbo.RRHH_FuncFamRespuesta r
JOIN dbo.RRHH_FuncFamPregunta p ON p.IdPregunta = r.IdPregunta
GROUP BY r.IdEmpleado
GO
