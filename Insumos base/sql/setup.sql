/* ============================================================================
   SETUP - SISTEMA DE RECURSOS HUMANOS (GRLL)
   ============================================================================
   Este script configura el entorno SQL para poder probar el sistema.

   Requisitos: SQL Server LocalDB (o SQL Server Express) con la base "CMI".
   Si no tienes el respaldo CMI_Backup_QA.bak, solicitalo al equipo o usa una
   base "CMI" restaurada. Este script crea el LOGIN SQL y el usuario que la
   aplicacion necesita para autenticarse.

   ============================================================================
   PASO 1 - Crear (o asegurar) la base CMI
   ----------------------------------------------------------------------------
   La base "CMI" debe existir. Normalmente se restaura desde CMI_Backup_QA.bak:

     RESTORE DATABASE [CMI] FROM DISK = 'C:\ruta\CMI_Backup_QA.bak'
     WITH MOVE 'CMI_Data' TO 'C:\SQLData\MSSQLLocalDB\CMI.mdf',
          MOVE 'CMI_Log'  TO 'C:\SQLData\MSSQLLocalDB\CMI_log.ldf', RECOVERY;

   (Adjusta las rutas segun tu instalacion.)

   ============================================================================
   PASO 2 - Crear el LOGIN SQL y el usuario (credenciales que pide la app)
   ============================================================================
*/
USE [master];
GO

-- Elimina el login si ya existe (para poder recrear)
IF SUSER_ID('user_1') IS NOT NULL
    DROP LOGIN [user_1];
GO

-- Crea el login SQL con la contrasena del entorno de prueba
CREATE LOGIN [user_1]
    WITH PASSWORD = 'Usuario_Qa_2024',
         DEFAULT_DATABASE = [CMI],
         CHECK_POLICY = OFF,
         CHECK_EXPIRATION = OFF;
GO

-- Crea el usuario en la base CMI y le da permisos amplios
USE [CMI];
GO
IF USER_ID('user_1') IS NOT NULL
    DROP USER [user_1];
GO
CREATE USER [user_1] FOR LOGIN [user_1];
GO
ALTER ROLE db_owner ADD MEMBER [user_1];
GO

/* ============================================================================
   PASO 3 - Asignar EL ROL DEL SISTEMA (0500000) al usuario user_1
   ----------------------------------------------------------------------------
   La aplicacion (Codsys = '0500000') solo concede acceso si el usuario tiene
   un rol cuyo IdSistema = '0500000'. El rol 13 (ADMINISTRADOR) corresponde a
   ese sistema. Insertamos la relacion si no existe.
   ============================================================================
*/
USE [CMI];
GO
IF NOT EXISTS (SELECT 1 FROM Usuario_Rol WHERE IdUsuario = 1 AND IdRol = 13)
BEGIN
    -- Asegura que user_1 (IdUsuario=1) tenga el rol 13 (ADMINISTRADOR 0500000)
    INSERT INTO Usuario_Rol (IdUsuario, IdRol) VALUES (1, 13);
END
GO

/* ============================================================================
   VERIFICACION
   ----------------------------------------------------------------------------
   Deberia devolver una fila (acceso concedido):
   SELECT * FROM GRLL_Usuario_Acceso('0500000', 'user_1');
   ============================================================================
*/

PRINT 'Setup completado. Login: user_1 / Usuario_Qa_2024';
GO
