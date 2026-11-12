-- Verificación de la BD para v2.7.0 a v2.9.0 (RF-07 a RF-15). SOLO LECTURA: no modifica nada.
-- Ejecutar en rrhhdb. Cada bloque devuelve una tabla con la columna "estado" (OK / REVISAR / FALTA).
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

-- 1. RF-13/14/15/16: tabla PeriodoTeletrabajo con sus columnas y restricciones
SELECT '1. PeriodoTeletrabajo' AS chequeo,
       CASE WHEN OBJECT_ID(N'dbo.PeriodoTeletrabajo', N'U') IS NULL THEN 'FALTA (ejecutar Scripts/rf-13-crear-periodo-teletrabajo.sql)' ELSE 'OK' END AS estado,
       (SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.PeriodoTeletrabajo')
          AND name IN ('Id','IdEmpleado','FechaInicio','FechaFin','Parcial')) AS columnas_de_5,
       (SELECT COUNT(*) FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'dbo.PeriodoTeletrabajo')) AS fk,
       (SELECT COUNT(*) FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.PeriodoTeletrabajo')) AS checks,
       (SELECT COUNT(*) FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.PeriodoTeletrabajo') AND type = 'UQ') AS unicos;

IF OBJECT_ID(N'dbo.PeriodoTeletrabajo', N'U') IS NOT NULL
    EXEC(N'SELECT ''1b. Periodos registrados'' AS chequeo, COUNT(*) AS periodos, SUM(CASE WHEN Parcial = 1 THEN 1 ELSE 0 END) AS parciales,
                  MIN(FechaInicio) AS desde, MAX(FechaFin) AS hasta FROM dbo.PeriodoTeletrabajo;');

-- 2. Columnas de Permiso que usan las reglas nuevas (RF-10 lee Dia, Hora_Sal y Hora_Ret)
SELECT '2. Columnas de Permiso' AS chequeo, c.name AS columna, t.name AS tipo, c.is_nullable AS acepta_null
FROM sys.columns c JOIN sys.types t ON t.user_type_id = c.user_type_id
WHERE c.object_id = OBJECT_ID(N'dbo.Permiso') AND c.name IN ('IdEmpleado','NPermiso','idMotivo','FechaInicio','FechaFin','Hora_Sal','Hora_Ret','Dia')
ORDER BY c.column_id;

-- 3. RF-07 a RF-12: motivos de vacaciones (la app los reconoce si la descripción contiene "vacación", sin tildes ni mayúsculas)
SELECT '3. Motivos de vacaciones' AS chequeo, m.idMotivo, m.Descrip_Motivo, m.Activo,
       (SELECT COUNT(*) FROM dbo.Permiso p WHERE p.idMotivo = m.idMotivo) AS permisos
FROM dbo.Motivo_Perm m
WHERE m.Descrip_Motivo COLLATE Latin1_General_CI_AI LIKE '%vacacion%'
ORDER BY m.idMotivo;

SELECT '3b. Resumen motivos de vacaciones' AS chequeo, COUNT(*) AS motivos,
       CASE WHEN COUNT(*) = 0 THEN 'FALTA: ningún motivo contiene "vacación"; el reporte no descontará días gozados' ELSE 'OK' END AS estado
FROM dbo.Motivo_Perm WHERE Descrip_Motivo COLLATE Latin1_General_CI_AI LIKE '%vacacion%';

-- 4. RF-07/RF-09: fecha de ingreso en el último periodo laboral de cada empleado
WITH ultimo AS (
    SELECT IdEmpleado, FechaIngreso, FechaSalida,
           ROW_NUMBER() OVER (PARTITION BY IdEmpleado ORDER BY NroPeriodo DESC) AS n
    FROM dbo.PeriodoLaboral)
SELECT '4. Periodo laboral' AS chequeo,
       (SELECT COUNT(*) FROM dbo.Empleado) AS empleados,
       SUM(CASE WHEN n = 1 THEN 1 ELSE 0 END) AS con_periodo,
       SUM(CASE WHEN n = 1 AND FechaIngreso IS NULL THEN 1 ELSE 0 END) AS ultimo_sin_ingreso,
       SUM(CASE WHEN n = 1 AND FechaIngreso IS NOT NULL AND (FechaSalida IS NULL OR FechaSalida > CAST(GETDATE() AS date)) THEN 1 ELSE 0 END) AS activos_calculables,
       SUM(CASE WHEN n = 1 AND FechaSalida <= CAST(GETDATE() AS date) THEN 1 ELSE 0 END) AS cesados,
       SUM(CASE WHEN n = 1 AND FechaIngreso > CAST(GETDATE() AS date) THEN 1 ELSE 0 END) AS ingreso_futuro
FROM ultimo;

-- 5. RF-10: vacaciones por horas ya registradas que no llegan a media jornada (4 h). Solo informativo:
--    la validación nueva aplica al registrar o editar, no cambia los datos históricos.
SELECT '5. Vacaciones por horas < 4 h (históricas)' AS chequeo, COUNT(*) AS permisos
FROM dbo.Permiso p JOIN dbo.Motivo_Perm m ON m.idMotivo = p.idMotivo
WHERE m.Descrip_Motivo COLLATE Latin1_General_CI_AI LIKE '%vacacion%'
  AND ISNULL(p.Dia, 0) = 0 AND p.Hora_Sal IS NOT NULL AND p.Hora_Ret IS NOT NULL
  AND DATEDIFF(MINUTE, p.Hora_Sal, p.Hora_Ret) < 240;

-- 6. RF-12: usuarios vinculados a un empleado (necesario para "Mis vacaciones")
SELECT '6. Usuarios vinculados' AS chequeo, COUNT(*) AS usuarios,
       SUM(CASE WHEN u.IdEmpleado IS NOT NULL THEN 1 ELSE 0 END) AS con_empleado,
       SUM(CASE WHEN u.IdEmpleado IS NOT NULL AND e.IdEmpleado IS NULL THEN 1 ELSE 0 END) AS empleado_inexistente
FROM dbo.Usuario u LEFT JOIN dbo.Empleado e ON e.IdEmpleado = u.IdEmpleado;

-- 7. RF-15: relojes y marcaciones con dispositivo
SELECT '7. Relojes (RF-15)' AS chequeo,
       (SELECT COUNT(*) FROM dbo.DispositivoMarcacion) AS dispositivos,
       (SELECT COUNT(*) FROM dbo.Marcacion WHERE CodDispositivo IS NOT NULL) AS marcaciones_con_reloj,
       (SELECT COUNT(*) FROM dbo.Marcacion m WHERE m.CodDispositivo IS NOT NULL
          AND NOT EXISTS (SELECT 1 FROM dbo.DispositivoMarcacion d WHERE d.Codigo = m.CodDispositivo)) AS marcaciones_reloj_desconocido;

-- 8. RF-06 y RF-18: motivos con código fijo en el código (licencia anual 70, 76, 82, 133; teletrabajo histórico 125, 126)
SELECT '8. Motivos con código fijo' AS chequeo, v.id AS idMotivo, v.uso, m.Descrip_Motivo,
       CASE WHEN m.idMotivo IS NULL THEN 'FALTA' ELSE 'OK' END AS estado
FROM (VALUES (70,'RF-06 licencia anual'),(76,'RF-06 licencia anual'),(82,'RF-06 licencia anual'),(133,'RF-06 licencia anual'),
             (125,'RF-18 excluido (teletrabajo)'),(126,'RF-18 excluido (teletrabajo)')) v(id, uso)
LEFT JOIN dbo.Motivo_Perm m ON m.idMotivo = v.id;

-- 9. Módulo Permisos en Rol_Acceso (Vacaciones usa el mismo código 0502007; no se crea uno nuevo)
SELECT '9. Opción Permisos' AS chequeo, IdSistemaOpcion, Descripcion,
       CASE WHEN IdSistemaOpcion IS NULL THEN 'FALTA' ELSE 'OK' END AS estado
FROM (SELECT 1 AS x) d LEFT JOIN dbo.SistemaOpcion o ON o.IdSistemaOpcion = '0502007';

-- 10. Índices de Permiso: los reportes nuevos filtran por idMotivo y FechaInicio
SELECT '10. Índices de Permiso' AS chequeo, i.name AS indice, i.type_desc,
       STRING_AGG(c.name, ', ') WITHIN GROUP (ORDER BY ic.key_ordinal) AS columnas
FROM sys.indexes i
JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0
JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
WHERE i.object_id = OBJECT_ID(N'dbo.Permiso')
GROUP BY i.name, i.type_desc;

-- 11. Objetos creados o modificados en los últimos 14 días
SELECT '11. Cambios recientes' AS chequeo, s.name AS esquema, o.name, o.type_desc, o.create_date, o.modify_date
FROM sys.objects o JOIN sys.schemas s ON s.schema_id = o.schema_id
WHERE o.is_ms_shipped = 0 AND o.modify_date >= DATEADD(day, -14, SYSUTCDATETIME()) AND o.type IN ('U','V','P','FN','TR')
ORDER BY o.modify_date DESC;
