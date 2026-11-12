-- Verificación 2 (SOLO LECTURA): fecha de ingreso y forma de registro de los permisos de vacaciones.
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

-- A. ¿De dónde sacar la fecha de ingreso? Empleado.FechaIngreso frente a PeriodoLaboral, por estado del empleado
WITH ultimo AS (
    SELECT IdEmpleado, FechaIngreso, FechaSalida,
           ROW_NUMBER() OVER (PARTITION BY IdEmpleado ORDER BY NroPeriodo DESC) AS n
    FROM dbo.PeriodoLaboral)
SELECT 'A. Fecha de ingreso' AS chequeo,
       CASE e.Estado WHEN 1 THEN 'Activo' WHEN 0 THEN 'Inactivo' ELSE 'Sin estado' END AS estado_empleado,
       COUNT(*) AS empleados,
       SUM(CASE WHEN e.FechaIngreso IS NOT NULL THEN 1 ELSE 0 END) AS con_ingreso_en_empleado,
       SUM(CASE WHEN u.FechaIngreso IS NOT NULL THEN 1 ELSE 0 END) AS con_ingreso_en_periodo,
       SUM(CASE WHEN e.FechaIngreso IS NULL AND u.FechaIngreso IS NULL THEN 1 ELSE 0 END) AS sin_ninguna,
       SUM(CASE WHEN e.FechaIngreso IS NOT NULL AND u.FechaIngreso IS NOT NULL AND e.FechaIngreso <> u.FechaIngreso THEN 1 ELSE 0 END) AS fechas_distintas,
       SUM(CASE WHEN u.FechaSalida IS NOT NULL AND u.FechaSalida <= CAST(GETDATE() AS date) THEN 1 ELSE 0 END) AS periodo_cerrado
FROM dbo.Empleado e
LEFT JOIN ultimo u ON u.IdEmpleado = e.IdEmpleado AND u.n = 1
GROUP BY e.Estado
ORDER BY e.Estado DESC;

-- A2. Ejemplos de empleados activos con fechas distintas (para ver cuál es la correcta)
WITH ultimo AS (
    SELECT IdEmpleado, NroPeriodo, FechaIngreso, FechaSalida,
           ROW_NUMBER() OVER (PARTITION BY IdEmpleado ORDER BY NroPeriodo DESC) AS n
    FROM dbo.PeriodoLaboral)
SELECT TOP (10) 'A2. Fechas distintas (activos)' AS chequeo, e.IdEmpleado, e.FechaIngreso AS ingreso_empleado,
       u.NroPeriodo, u.FechaIngreso AS ingreso_periodo, u.FechaSalida AS salida_periodo
FROM dbo.Empleado e JOIN ultimo u ON u.IdEmpleado = e.IdEmpleado AND u.n = 1
WHERE e.Estado = 1 AND e.FechaIngreso <> u.FechaIngreso
ORDER BY e.IdEmpleado;

-- B. ¿Cómo se registran los permisos de vacaciones? (por día, por horas, duración)
SELECT 'B. Forma de registro' AS chequeo, m.idMotivo, m.Descrip_Motivo,
       COUNT(*) AS permisos,
       SUM(CASE WHEN p.Dia = 1 THEN 1 ELSE 0 END) AS dia_1,
       SUM(CASE WHEN ISNULL(p.Dia, 0) = 0 AND p.Hora_Sal IS NOT NULL AND p.Hora_Ret IS NOT NULL THEN 1 ELSE 0 END) AS por_horas,
       SUM(CASE WHEN p.Hora_Sal IS NULL OR p.Hora_Ret IS NULL THEN 1 ELSE 0 END) AS sin_horas,
       SUM(CASE WHEN p.FechaFin IS NOT NULL AND p.FechaFin > p.FechaInicio THEN 1 ELSE 0 END) AS varios_dias,
       AVG(CASE WHEN p.Hora_Sal IS NOT NULL AND p.Hora_Ret IS NOT NULL THEN DATEDIFF(MINUTE, p.Hora_Sal, p.Hora_Ret) END) AS minutos_promedio,
       MIN(CASE WHEN p.Hora_Sal IS NOT NULL AND p.Hora_Ret IS NOT NULL THEN DATEDIFF(MINUTE, p.Hora_Sal, p.Hora_Ret) END) AS minutos_min,
       MAX(CASE WHEN p.Hora_Sal IS NOT NULL AND p.Hora_Ret IS NOT NULL THEN DATEDIFF(MINUTE, p.Hora_Sal, p.Hora_Ret) END) AS minutos_max,
       MIN(p.FechaInicio) AS desde, MAX(p.FechaInicio) AS hasta
FROM dbo.Permiso p JOIN dbo.Motivo_Perm m ON m.idMotivo = p.idMotivo
WHERE m.Descrip_Motivo COLLATE Latin1_General_CI_AI LIKE '%vacacion%'
GROUP BY m.idMotivo, m.Descrip_Motivo
ORDER BY m.idMotivo;

-- C. Ejemplos de cada motivo (5 por motivo) para ver los campos tal como están
WITH ej AS (
    SELECT p.*, ROW_NUMBER() OVER (PARTITION BY p.idMotivo ORDER BY p.FechaInicio DESC) AS n
    FROM dbo.Permiso p JOIN dbo.Motivo_Perm m ON m.idMotivo = p.idMotivo
    WHERE m.Descrip_Motivo COLLATE Latin1_General_CI_AI LIKE '%vacacion%')
SELECT 'C. Ejemplos' AS chequeo, idMotivo, IdEmpleado, NPermiso, FechaInicio, FechaFin, Hora_Sal, Hora_Ret, Dia, Retorno, Obs
FROM ej WHERE n <= 5 ORDER BY idMotivo, n;

-- D. Tamaño de Permiso (para decidir si los reportes necesitan un índice por motivo y fecha)
SELECT 'D. Tamaño de Permiso' AS chequeo, COUNT(*) AS filas FROM dbo.Permiso;
