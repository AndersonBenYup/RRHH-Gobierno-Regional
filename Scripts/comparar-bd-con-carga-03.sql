-- Comparación SOLO LECTURA: lo que cargó 03-datos-RRHH_Normalizado.sql frente a lo que hay hoy en rrhhdb.
-- Generado automáticamente desde ese script (conteos de su bloque final y fechas leídas de sus INSERT).
-- Solo usa SELECT y variables de tabla (@...); no escribe en ninguna tabla de la BD.
-- Si ya se ejecutó Scripts/recuperar-marcaciones-sin-horario.sql, es normal que salga SOBRAN en:
--   Marcacion +104984 (tipo M), Asistencia +1720 y TipoMarcacion +1. Esas filas venían en bd-Datos.sql y
--   la limpieza de huérfanos de ese script las había borrado por tener idHorario NULL.
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

-- 1. Filas por tabla: esperadas (script) frente a cargadas (BD)
DECLARE @esperado TABLE (Tabla sysname PRIMARY KEY, Esperado int);
INSERT INTO @esperado (Tabla, Esperado) VALUES
  (N'[dbo].[AFP]', 4),
  (N'[dbo].[AguaViv]', 5),
  (N'[dbo].[AlumbradoViv]', 5),
  (N'[dbo].[Asistencia]', 153964),
  (N'[dbo].[Bitacora]', 0),
  (N'[dbo].[Capacitacion]', 2177),
  (N'[dbo].[Cargo]', 185),
  (N'[dbo].[Comportamiento]', 292),
  (N'[dbo].[ConservacionViv]', 4),
  (N'[dbo].[Departamento]', 25),
  (N'[dbo].[Discapacidad]', 6),
  (N'[dbo].[DispositivoMarcacion]', 4),
  (N'[dbo].[Distrito]', 2009),
  (N'[dbo].[Empleado]', 3335),
  (N'[dbo].[Empleado_Area]', 35884),
  (N'[dbo].[EstadoAsistencia]', 45),
  (N'[dbo].[EstadoCivil]', 6),
  (N'[dbo].[EstadoEstudio]', 6),
  (N'[dbo].[EstructOrganiz]', 1433),
  (N'[dbo].[EstudiosRealizado]', 1082),
  (N'[dbo].[ExcretasViv]', 5),
  (N'[dbo].[ExpLaboral]', 316),
  (N'[dbo].[Familiar]', 966),
  (N'[dbo].[Horario]', 70),
  (N'[dbo].[HorarioTemporal]', 11353),
  (N'[dbo].[Local]', 27),
  (N'[dbo].[Marcacion]', 200184),
  (N'[dbo].[MaterialViv]', 7),
  (N'[dbo].[MotivoBaja]', 5),
  (N'[dbo].[Motivo_Perm]', 140),
  (N'[dbo].[Nacionalidad]', 1),
  (N'[dbo].[PeriodoLaboral]', 882),
  (N'[dbo].[Permiso]', 26120),
  (N'[dbo].[Persona]', 4106),
  (N'[dbo].[Provincia]', 195),
  (N'[dbo].[RRHH_AcudeEnferm]', 5),
  (N'[dbo].[RRHH_Asegurado]', 2),
  (N'[dbo].[RRHH_AspSocio]', 628),
  (N'[dbo].[RRHH_DinamicaFamiliar]', 628),
  (N'[dbo].[RRHH_Feriado]', 221),
  (N'[dbo].[RRHH_FuncFamPregunta]', 9),
  (N'[dbo].[RRHH_FuncFamRespuesta]', 0),
  (N'[dbo].[RRHH_RelacHermano]', 4),
  (N'[dbo].[RRHH_RelacPH]', 5),
  (N'[dbo].[RRHH_RelacPareja]', 4),
  (N'[dbo].[RRHH_Salud]', 628),
  (N'[dbo].[RRHH_TipoFamilia]', 4),
  (N'[dbo].[RegAsisDiario]', 263520),
  (N'[dbo].[RegimenPension]', 3),
  (N'[dbo].[Resolucion]', 803),
  (N'[dbo].[ResponsableXUO]', 125),
  (N'[dbo].[Rol]', 15),
  (N'[dbo].[Rol_Acceso]', 0),
  (N'[dbo].[Rotacion]', 51),
  (N'[dbo].[ServicioViv]', 4),
  (N'[dbo].[SistemaOpcion]', 12),
  (N'[dbo].[TenenciaViv]', 7),
  (N'[dbo].[TipoComportamiento]', 2),
  (N'[dbo].[TipoDocID]', 3),
  (N'[dbo].[TipoEstudio]', 6),
  (N'[dbo].[TipoFamiliar]', 7),
  (N'[dbo].[TipoInstitucion]', 2),
  (N'[dbo].[TipoMarcacion]', 3),
  (N'[dbo].[TipoPermiso]', 3),
  (N'[dbo].[TipoResolucion]', 7),
  (N'[dbo].[TipoSangre]', 6),
  (N'[dbo].[TipoTrabajador]', 7),
  (N'[dbo].[TipoViv]', 7),
  (N'[dbo].[TitulosEmpleado]', 152),
  (N'[dbo].[UbicacionViv]', 4),
  (N'[dbo].[Usuario]', 95),
  (N'[dbo].[Usuario_Rol]', 93),
  (N'[dbo].[VinculoAsegurado]', 2),
  (N'[dbo].[ViveCon]', 10),
  (N'[dbo].[Vivienda]', 634),
  (N'[dbo].[ViviendaServicio]', 0),
  (N'[mig].[Incidencia]', 2202);

SELECT '1. Filas por tabla' AS chequeo, e.Tabla, e.Esperado,
       CASE WHEN OBJECT_ID(e.Tabla, N'U') IS NULL THEN NULL ELSE ISNULL(SUM(p.rows), 0) END AS En_BD,
       CASE WHEN OBJECT_ID(e.Tabla, N'U') IS NULL THEN NULL ELSE ISNULL(SUM(p.rows), 0) - e.Esperado END AS Diferencia,
       CASE WHEN OBJECT_ID(e.Tabla, N'U') IS NULL THEN 'NO EXISTE'
            WHEN ISNULL(SUM(p.rows), 0) = e.Esperado THEN 'OK'
            WHEN ISNULL(SUM(p.rows), 0) < e.Esperado THEN 'FALTAN'
            ELSE 'SOBRAN' END AS Estado
FROM @esperado e
LEFT JOIN sys.partitions p ON p.object_id = OBJECT_ID(e.Tabla) AND p.index_id IN (0, 1)
GROUP BY e.Tabla, e.Esperado
ORDER BY CASE WHEN OBJECT_ID(e.Tabla, N'U') IS NULL THEN 0 WHEN ISNULL(SUM(p.rows), 0) < e.Esperado THEN 1
              WHEN ISNULL(SUM(p.rows), 0) > e.Esperado THEN 2 ELSE 3 END, e.Tabla;

-- 2. Rango de fechas: script frente a BD
DECLARE @fechas TABLE (Tabla sysname, Columna sysname, Desde_script date, Hasta_script date, Con_fecha_script int);
INSERT INTO @fechas VALUES
  (N'Permiso', N'FechaInicio', '2024-01-01', '2024-12-31', 26120),
  (N'Marcacion', N'Fecha', '2024-01-01', '2024-12-31', 200184),
  (N'Asistencia', N'Fecha', '2024-01-01', '2024-12-31', 153964),
  (N'RegAsisDiario', N'Fecha', '2024-05-01', '2024-12-31', 263520),
  (N'PeriodoLaboral', N'FechaIngreso', '1963-11-11', '2011-08-16', 881),
  (N'PeriodoLaboral', N'FechaSalida', '1980-10-30', '2011-10-27', 559),
  (N'Empleado', N'FechaIngreso', '1970-01-01', '2010-02-17', 633),
  (N'HorarioTemporal', N'FechaInicio', '2011-10-25', '2026-02-28', 11353),
  (N'RRHH_Feriado', N'Fecha', '2010-07-28', '2026-01-02', 221),
  (N'Resolucion', N'FechaResolucion', '1961-08-11', '2012-12-07', 803),
  (N'Capacitacion', N'FechaInicio', '1996-10-02', '3009-03-14', 2177);

DECLARE @rangos TABLE (Tabla sysname, Columna sysname, Desde_bd date, Hasta_bd date, Con_fecha_bd int);
DECLARE @t sysname, @c sysname, @sql nvarchar(max);
DECLARE cur CURSOR LOCAL FAST_FORWARD FOR SELECT Tabla, Columna FROM @fechas;
OPEN cur; FETCH NEXT FROM cur INTO @t, @c;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF COL_LENGTH(N'dbo.' + @t, @c) IS NOT NULL
    BEGIN
        SET @sql = N'SELECT @t, @c, MIN(CAST(' + QUOTENAME(@c) + N' AS date)), MAX(CAST(' + QUOTENAME(@c) + N' AS date)), COUNT('
                 + QUOTENAME(@c) + N') FROM dbo.' + QUOTENAME(@t);
        INSERT INTO @rangos EXEC sp_executesql @sql, N'@t sysname, @c sysname', @t, @c;
    END
    FETCH NEXT FROM cur INTO @t, @c;
END
CLOSE cur; DEALLOCATE cur;

SELECT '2. Rango de fechas' AS chequeo, f.Tabla, f.Columna, f.Desde_script, r.Desde_bd, f.Hasta_script, r.Hasta_bd,
       f.Con_fecha_script, r.Con_fecha_bd,
       CASE WHEN r.Tabla IS NULL THEN 'NO EXISTE'
            WHEN r.Con_fecha_bd = f.Con_fecha_script AND r.Desde_bd = f.Desde_script AND r.Hasta_bd = f.Hasta_script THEN 'OK'
            WHEN r.Con_fecha_bd < f.Con_fecha_script THEN 'FALTAN'
            ELSE 'DISTINTO' END AS Estado
FROM @fechas f LEFT JOIN @rangos r ON r.Tabla = f.Tabla AND r.Columna = f.Columna
ORDER BY f.Tabla, f.Columna;

-- 3. Filas por año en movimientos y fechas de ingreso (se listan los movimientos y cualquier año con diferencia)
DECLARE @anios TABLE (Tabla sysname, Columna sysname, Anio int, En_script int);
INSERT INTO @anios VALUES
  (N'Empleado', N'FechaIngreso', 1970, 3),
  (N'Empleado', N'FechaIngreso', 1971, 1),
  (N'Empleado', N'FechaIngreso', 1973, 1),
  (N'Empleado', N'FechaIngreso', 1974, 1),
  (N'Empleado', N'FechaIngreso', 1976, 2),
  (N'Empleado', N'FechaIngreso', 1979, 3),
  (N'Empleado', N'FechaIngreso', 1980, 18),
  (N'Empleado', N'FechaIngreso', 1982, 10),
  (N'Empleado', N'FechaIngreso', 1983, 8),
  (N'Empleado', N'FechaIngreso', 1984, 2),
  (N'Empleado', N'FechaIngreso', 1985, 5),
  (N'Empleado', N'FechaIngreso', 1986, 37),
  (N'Empleado', N'FechaIngreso', 1987, 20),
  (N'Empleado', N'FechaIngreso', 1988, 13),
  (N'Empleado', N'FechaIngreso', 1989, 14),
  (N'Empleado', N'FechaIngreso', 1990, 9),
  (N'Empleado', N'FechaIngreso', 1992, 4),
  (N'Empleado', N'FechaIngreso', 1997, 2),
  (N'Empleado', N'FechaIngreso', 2003, 8),
  (N'Empleado', N'FechaIngreso', 2004, 2),
  (N'Empleado', N'FechaIngreso', 2005, 8),
  (N'Empleado', N'FechaIngreso', 2006, 1),
  (N'Empleado', N'FechaIngreso', 2007, 28),
  (N'Empleado', N'FechaIngreso', 2008, 260),
  (N'Empleado', N'FechaIngreso', 2009, 143),
  (N'Empleado', N'FechaIngreso', 2010, 30),
  (N'Asistencia', N'Fecha', 2024, 153964),
  (N'Marcacion', N'Fecha', 2024, 200184),
  (N'PeriodoLaboral', N'FechaIngreso', 1963, 1),
  (N'PeriodoLaboral', N'FechaIngreso', 1964, 3),
  (N'PeriodoLaboral', N'FechaIngreso', 1966, 1),
  (N'PeriodoLaboral', N'FechaIngreso', 1970, 4),
  (N'PeriodoLaboral', N'FechaIngreso', 1971, 3),
  (N'PeriodoLaboral', N'FechaIngreso', 1972, 4),
  (N'PeriodoLaboral', N'FechaIngreso', 1973, 15),
  (N'PeriodoLaboral', N'FechaIngreso', 1974, 10),
  (N'PeriodoLaboral', N'FechaIngreso', 1975, 2),
  (N'PeriodoLaboral', N'FechaIngreso', 1976, 12),
  (N'PeriodoLaboral', N'FechaIngreso', 1977, 8),
  (N'PeriodoLaboral', N'FechaIngreso', 1978, 6),
  (N'PeriodoLaboral', N'FechaIngreso', 1979, 9),
  (N'PeriodoLaboral', N'FechaIngreso', 1980, 40),
  (N'PeriodoLaboral', N'FechaIngreso', 1981, 5),
  (N'PeriodoLaboral', N'FechaIngreso', 1982, 14),
  (N'PeriodoLaboral', N'FechaIngreso', 1983, 20),
  (N'PeriodoLaboral', N'FechaIngreso', 1984, 11),
  (N'PeriodoLaboral', N'FechaIngreso', 1985, 16),
  (N'PeriodoLaboral', N'FechaIngreso', 1986, 59),
  (N'PeriodoLaboral', N'FechaIngreso', 1987, 52),
  (N'PeriodoLaboral', N'FechaIngreso', 1988, 26),
  (N'PeriodoLaboral', N'FechaIngreso', 1989, 45),
  (N'PeriodoLaboral', N'FechaIngreso', 1990, 22),
  (N'PeriodoLaboral', N'FechaIngreso', 1991, 8),
  (N'PeriodoLaboral', N'FechaIngreso', 1992, 7),
  (N'PeriodoLaboral', N'FechaIngreso', 1993, 6),
  (N'PeriodoLaboral', N'FechaIngreso', 1994, 13),
  (N'PeriodoLaboral', N'FechaIngreso', 1995, 6),
  (N'PeriodoLaboral', N'FechaIngreso', 1996, 3),
  (N'PeriodoLaboral', N'FechaIngreso', 1997, 11),
  (N'PeriodoLaboral', N'FechaIngreso', 1998, 4),
  (N'PeriodoLaboral', N'FechaIngreso', 1999, 12),
  (N'PeriodoLaboral', N'FechaIngreso', 2000, 7),
  (N'PeriodoLaboral', N'FechaIngreso', 2001, 3),
  (N'PeriodoLaboral', N'FechaIngreso', 2002, 5),
  (N'PeriodoLaboral', N'FechaIngreso', 2003, 18),
  (N'PeriodoLaboral', N'FechaIngreso', 2004, 15),
  (N'PeriodoLaboral', N'FechaIngreso', 2005, 21),
  (N'PeriodoLaboral', N'FechaIngreso', 2006, 16),
  (N'PeriodoLaboral', N'FechaIngreso', 2007, 37),
  (N'PeriodoLaboral', N'FechaIngreso', 2008, 216),
  (N'PeriodoLaboral', N'FechaIngreso', 2009, 16),
  (N'PeriodoLaboral', N'FechaIngreso', 2010, 46),
  (N'PeriodoLaboral', N'FechaIngreso', 2011, 23),
  (N'Permiso', N'FechaInicio', 2024, 26120),
  (N'RegAsisDiario', N'Fecha', 2024, 263520);

DECLARE @anios_bd TABLE (Tabla sysname, Columna sysname, Anio int, En_bd int);
DECLARE cur2 CURSOR LOCAL FAST_FORWARD FOR SELECT DISTINCT Tabla, Columna FROM @anios;
OPEN cur2; FETCH NEXT FROM cur2 INTO @t, @c;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF COL_LENGTH(N'dbo.' + @t, @c) IS NOT NULL
    BEGIN
        SET @sql = N'SELECT @t, @c, YEAR(' + QUOTENAME(@c) + N'), COUNT(*) FROM dbo.' + QUOTENAME(@t)
                 + N' WHERE ' + QUOTENAME(@c) + N' IS NOT NULL GROUP BY YEAR(' + QUOTENAME(@c) + N')';
        INSERT INTO @anios_bd EXEC sp_executesql @sql, N'@t sysname, @c sysname', @t, @c;
    END
    FETCH NEXT FROM cur2 INTO @t, @c;
END
CLOSE cur2; DEALLOCATE cur2;

SELECT '3. Filas por año' AS chequeo, COALESCE(a.Tabla, b.Tabla) AS Tabla, COALESCE(a.Columna, b.Columna) AS Columna,
       COALESCE(a.Anio, b.Anio) AS Anio, ISNULL(a.En_script, 0) AS En_script, ISNULL(b.En_bd, 0) AS En_bd,
       ISNULL(b.En_bd, 0) - ISNULL(a.En_script, 0) AS Diferencia
FROM @anios a FULL JOIN @anios_bd b ON b.Tabla = a.Tabla AND b.Columna = a.Columna AND b.Anio = a.Anio
WHERE ISNULL(a.En_script, 0) <> ISNULL(b.En_bd, 0)
   OR COALESCE(a.Tabla, b.Tabla) IN (N'Permiso', N'Marcacion', N'Asistencia', N'RegAsisDiario')
ORDER BY Tabla, Columna, Anio;

-- 4. Integridad: movimientos de empleados que no existen y FKs no confiables o deshabilitadas
SELECT '4. Integridad' AS chequeo,
       (SELECT COUNT(*) FROM dbo.Permiso x WHERE NOT EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdEmpleado = x.IdEmpleado)) AS permisos_sin_empleado,
       (SELECT COUNT(*) FROM dbo.Marcacion x WHERE NOT EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdEmpleado = x.IdEmpleado)) AS marcaciones_sin_empleado,
       (SELECT COUNT(*) FROM dbo.Asistencia x WHERE NOT EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdEmpleado = x.IdEmpleado)) AS asistencias_sin_empleado,
       (SELECT COUNT(*) FROM dbo.PeriodoLaboral x WHERE NOT EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdEmpleado = x.IdEmpleado)) AS periodos_sin_empleado,
       (SELECT COUNT(*) FROM sys.foreign_keys WHERE is_not_trusted = 1) AS fks_no_confiables,
       (SELECT COUNT(*) FROM sys.foreign_keys WHERE is_disabled = 1) AS fks_deshabilitadas;
