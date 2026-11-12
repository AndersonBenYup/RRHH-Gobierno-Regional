-- SC-030: verificación SOLO LECTURA, antes y después de ejecutar recuperar-marcaciones-sin-horario.sql en rrhhdb.
-- Antes : Marcacion 200184, Asistencia 153964, tipo M sin filas (o inexistente).
-- Después: Marcacion 305168, Asistencia 155684, tipo M con 104984 filas.
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT 'Totales' AS chequeo,
       (SELECT COUNT(*) FROM dbo.Marcacion) AS marcaciones,
       (SELECT COUNT(*) FROM dbo.Asistencia) AS asistencias,
       (SELECT COUNT(*) FROM dbo.Marcacion WHERE TipoMarcacion = 'M') AS marcaciones_tipo_M,
       (SELECT COUNT(*) FROM dbo.Marcacion WHERE idHorario IS NULL) AS marcaciones_sin_horario,
       (SELECT COUNT(*) FROM dbo.Asistencia WHERE idHorario IS NULL) AS asistencias_sin_horario,
       CASE WHEN (SELECT COUNT(*) FROM dbo.Marcacion) = 305168 AND (SELECT COUNT(*) FROM dbo.Asistencia) = 155684
            THEN 'RECUPERADO'
            WHEN (SELECT COUNT(*) FROM dbo.Marcacion) = 200184 AND (SELECT COUNT(*) FROM dbo.Asistencia) = 153964
            THEN 'SIN RECUPERAR'
            ELSE 'REVISAR' END AS estado;

SELECT 'Tipos de marcación' AS chequeo, t.Codigo, t.Descripcion, COUNT(m.IdEmpleado) AS marcaciones
FROM dbo.TipoMarcacion t LEFT JOIN dbo.Marcacion m ON m.TipoMarcacion = t.Codigo
GROUP BY t.Codigo, t.Descripcion ORDER BY t.Codigo;

SELECT 'Marcaciones por mes y tipo (2024)' AS chequeo, FORMAT(Fecha, 'yyyy-MM') AS mes,
       SUM(CASE WHEN TipoMarcacion = 'M' THEN 1 ELSE 0 END) AS tipo_M,
       SUM(CASE WHEN TipoMarcacion <> 'M' THEN 1 ELSE 0 END) AS otros_tipos
FROM dbo.Marcacion WHERE Fecha >= '2024-01-01' AND Fecha < '2025-01-01'
GROUP BY FORMAT(Fecha, 'yyyy-MM') ORDER BY mes;

SELECT 'Empleados activos sin ninguna marcación en 2024' AS chequeo, COUNT(*) AS empleados
FROM dbo.Empleado e
WHERE e.Estado = 1
  AND NOT EXISTS (SELECT 1 FROM dbo.Marcacion m WHERE m.IdEmpleado = e.IdEmpleado AND m.Fecha >= '2024-01-01' AND m.Fecha < '2025-01-01');

SELECT 'Integridad' AS chequeo,
       (SELECT COUNT(*) FROM dbo.Marcacion x WHERE NOT EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdEmpleado = x.IdEmpleado)) AS marcaciones_sin_empleado,
       (SELECT COUNT(*) FROM dbo.Asistencia x WHERE NOT EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdEmpleado = x.IdEmpleado)) AS asistencias_sin_empleado,
       (SELECT COUNT(*) FROM sys.foreign_keys WHERE is_not_trusted = 1) AS fks_no_confiables;
