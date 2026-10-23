/* ============================================================
   crear_bitacora.sql
   Crea la tabla [Bitacora] usada por la auditoria de actividad
   (BitacoraData / Vista "Bitacora de actividad").
   Es seguro de ejecutar varias veces (IF NOT EXISTS).
   ============================================================ */

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Bitacora' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
	CREATE TABLE [dbo].[Bitacora](
		[IdBitacora] [int] IDENTITY(1,1) NOT NULL,
		[Fecha] [datetime] NOT NULL,
		[Usuario] [varchar](100) NULL,
		[Accion] [varchar](50) NULL,
		[Entidad] [varchar](100) NULL,
		[Detalle] [text] NULL,
		CONSTRAINT [PK_Bitacora] PRIMARY KEY CLUSTERED ([IdBitacora] ASC)
	);
END
GO