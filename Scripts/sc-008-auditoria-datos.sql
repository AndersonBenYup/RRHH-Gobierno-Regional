/* ============================================================
   SC-008 · Auditoría de datos existentes (SOLO LECTURA)
   ------------------------------------------------------------
   Detecta los registros que ya están en la BD y que NO cumplen
   las nuevas validaciones. No modifica nada.

   Cuándo ejecutarlo: antes de desplegar SC-008 en Azure
   (condición 4 del CCB). Los registros listados se podrán seguir
   viendo, pero al editarlos el sistema pedirá corregirlos.

   Cómo: Query editor de la BD RRHH_Normalizado en el portal de
   Azure, o:  sqlcmd -S <servidor> -d RRHH_Normalizado -f 65001 -i este_archivo

   Resultado en la BD local (02/11/2026): 40 pares de permisos que se
   cruzan, 41 capacitaciones y 11 estudios con fechas invertidas,
   5 RUC mal formados y 3 feriados sin motivo. 0 DNI inválidos.
   ============================================================ */
SET NOCOUNT ON;

PRINT '== 1. Resumen';
SELECT 'Personas con DNI que no tiene 8 dígitos' AS Regla,
       COUNT(*) AS Registros
FROM Persona
WHERE TipoDocID = 1
  AND NumDocID IS NOT NULL
  AND LTRIM(RTRIM(NumDocID)) NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
UNION ALL
SELECT 'Personas con RUC que no tiene 11 dígitos', COUNT(*)
FROM Persona
WHERE TipoDocID = 2
  AND NumDocID IS NOT NULL
  AND LTRIM(RTRIM(NumDocID)) NOT LIKE REPLICATE('[0-9]', 11)
UNION ALL
SELECT 'Personas con tipo de documento y sin número', COUNT(*)
FROM Persona
WHERE TipoDocID IS NOT NULL AND LTRIM(RTRIM(ISNULL(NumDocID, ''))) = ''
UNION ALL
SELECT 'Números de documento repetidos (grupos)', COUNT(*)
FROM (SELECT LTRIM(RTRIM(NumDocID)) AS Doc
      FROM Persona
      WHERE LTRIM(RTRIM(ISNULL(NumDocID, ''))) <> ''
      GROUP BY LTRIM(RTRIM(NumDocID))
      HAVING COUNT(*) > 1) d
UNION ALL
SELECT 'Permisos con fecha fin anterior a la de inicio', COUNT(*)
FROM Permiso WHERE FechaFin < FechaInicio
UNION ALL
SELECT 'Permisos sin fecha de inicio', COUNT(*)
FROM Permiso WHERE FechaInicio IS NULL
UNION ALL
SELECT 'Permisos sin motivo', COUNT(*)
FROM Permiso WHERE idMotivo IS NULL
UNION ALL
SELECT 'Permisos con retorno no posterior a la salida (mismo día)', COUNT(*)
FROM Permiso
WHERE Hora_Sal IS NOT NULL AND Hora_Ret IS NOT NULL AND Hora_Ret <= Hora_Sal
  AND ISNULL(FechaFin, FechaInicio) = FechaInicio
UNION ALL
SELECT 'Pares de permisos que se cruzan (mismo empleado)', COUNT(*)
FROM Permiso a
JOIN Permiso b ON b.IdEmpleado = a.IdEmpleado AND b.NPermiso > a.NPermiso
WHERE a.FechaInicio IS NOT NULL AND b.FechaInicio IS NOT NULL
  AND a.FechaInicio <= ISNULL(b.FechaFin, b.FechaInicio)
  AND b.FechaInicio <= ISNULL(a.FechaFin, a.FechaInicio)
  AND NOT (   ISNULL(a.Dia, 0) = 0 AND ISNULL(b.Dia, 0) = 0
          AND a.Hora_Sal IS NOT NULL AND a.Hora_Ret IS NOT NULL
          AND b.Hora_Sal IS NOT NULL AND b.Hora_Ret IS NOT NULL
          AND ISNULL(a.FechaFin, a.FechaInicio) = a.FechaInicio
          AND ISNULL(b.FechaFin, b.FechaInicio) = b.FechaInicio
          AND (a.Hora_Ret <= b.Hora_Sal OR b.Hora_Ret <= a.Hora_Sal))
UNION ALL
SELECT 'Capacitaciones con fin anterior al inicio', COUNT(*)
FROM Capacitacion WHERE FechaFin < FechaInicio
UNION ALL
SELECT 'Estudios con término anterior al inicio', COUNT(*)
FROM EstudiosRealizado WHERE FechaTermino < FechaInicio
UNION ALL
SELECT 'Periodos laborales con salida anterior al ingreso', COUNT(*)
FROM PeriodoLaboral WHERE FechaSalida < FechaIngreso
UNION ALL
SELECT 'Experiencias laborales con egreso anterior al ingreso', COUNT(*)
FROM ExpLaboral WHERE FechaEgreso < FechaIngreso
UNION ALL
SELECT 'Horarios temporales con fin anterior al inicio', COUNT(*)
FROM HorarioTemporal WHERE FechaFin < FechaInicio
UNION ALL
SELECT 'Feriados sin motivo', COUNT(*)
FROM RRHH_Feriado WHERE LTRIM(RTRIM(ISNULL(Motivo, ''))) = '';

PRINT '== 2. Detalle: DNI inválidos';
SELECT IdPersona, Nombres, Apellido_Paterno, NumDocID
FROM Persona
WHERE TipoDocID = 1
  AND NumDocID IS NOT NULL
  AND LTRIM(RTRIM(NumDocID)) NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
ORDER BY IdPersona;

PRINT '== 3. Detalle: documentos repetidos';
SELECT LTRIM(RTRIM(NumDocID)) AS NumDocID, COUNT(*) AS Veces,
       STRING_AGG(CAST(IdPersona AS varchar(12)), ', ') AS IdPersonas
FROM Persona
WHERE LTRIM(RTRIM(ISNULL(NumDocID, ''))) <> ''
GROUP BY LTRIM(RTRIM(NumDocID))
HAVING COUNT(*) > 1
ORDER BY Veces DESC;

PRINT '== 4. Detalle: permisos con fechas u horas inválidas';
SELECT IdEmpleado, NPermiso, FechaInicio, FechaFin, Hora_Sal, Hora_Ret
FROM Permiso
WHERE FechaFin < FechaInicio
   OR FechaInicio IS NULL
   OR (Hora_Sal IS NOT NULL AND Hora_Ret IS NOT NULL AND Hora_Ret <= Hora_Sal
       AND ISNULL(FechaFin, FechaInicio) = FechaInicio)
ORDER BY IdEmpleado, NPermiso;
