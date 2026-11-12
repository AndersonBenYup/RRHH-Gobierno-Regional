/* ============================================================
   SC-030: REVERTIR la recuperación de marcaciones y asistencias sin horario (rrhhdb).
   Usar solo si RRHH no aprueba la recuperación.
   Antes de la recuperación no había en rrhhdb marcaciones ni asistencias con idHorario NULL, ni el tipo M:
   las filas recuperadas se identifican por eso (2024, idHorario NULL; las marcaciones además tipo M y sin reloj).
   El script se detiene sin borrar nada si los conteos no son exactamente 104984 y 1720
   (por ejemplo, si después se registraron otras filas sin horario desde la aplicación).
   ============================================================ */
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_NAME() <> N'rrhhdb'
BEGIN
    RAISERROR('Ejecute este script sobre la base rrhhdb.', 16, 1);
    SET NOEXEC ON;
END
GO

DECLARE @m int = (SELECT COUNT(*) FROM dbo.Marcacion
                  WHERE TipoMarcacion = 'M' AND idHorario IS NULL AND CodDispositivo IS NULL
                    AND Fecha >= '2024-01-01' AND Fecha < '2025-01-01');
DECLARE @a int = (SELECT COUNT(*) FROM dbo.Asistencia
                  WHERE idHorario IS NULL AND Fecha >= '2024-01-01' AND Fecha < '2025-01-01');

IF @m <> 104984 OR @a <> 1720
BEGIN
    SELECT 'NO SE REVIRTIÓ' AS resultado, @m AS marcaciones_encontradas, 104984 AS esperadas,
           @a AS asistencias_encontradas, 1720 AS esperadas;
    RAISERROR('Los conteos no coinciden con la recuperación de SC-030; revise antes de borrar.', 16, 1);
    RETURN;
END

BEGIN TRAN;
DELETE FROM dbo.Marcacion
WHERE TipoMarcacion = 'M' AND idHorario IS NULL AND CodDispositivo IS NULL
  AND Fecha >= '2024-01-01' AND Fecha < '2025-01-01';
DELETE FROM dbo.Asistencia
WHERE idHorario IS NULL AND Fecha >= '2024-01-01' AND Fecha < '2025-01-01';
IF NOT EXISTS (SELECT 1 FROM dbo.Marcacion WHERE TipoMarcacion = 'M')
    DELETE FROM dbo.TipoMarcacion WHERE Codigo = 'M';
COMMIT;

SELECT 'REVERTIDO' AS resultado, (SELECT COUNT(*) FROM dbo.Marcacion) AS marcaciones, (SELECT COUNT(*) FROM dbo.Asistencia) AS asistencias;
GO
SET NOEXEC OFF;
GO
