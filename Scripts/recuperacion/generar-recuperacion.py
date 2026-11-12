"""SC-030: genera el script de recuperación de las marcaciones y asistencias que la limpieza de huérfanos de
bd-Datos.sql borró por tener idHorario NULL (no eran huérfanas), y el anexo por empleado para RRHH.
Uso (desde la raíz del repositorio):
    python -I Scripts/recuperacion/generar-recuperacion.py D:/evo_shanty/Scripts/bd-Datos/bd-Datos.sql
        Scripts/recuperacion/recuperar-marcaciones-sin-horario.sql
        docs/actualizados/anexos/SC-030-empleados-afectados.csv
(los tres argumentos en una sola línea; el tercero, el anexo CSV, es opcional)"""
import collections, csv, re, sys

src, out = sys.argv[1], sys.argv[2]
anexo = sys.argv[3] if len(sys.argv) > 3 else None
por_empleado = collections.defaultdict(lambda: {"marc_rec": 0, "marc_conservadas": 0, "asis_rec": 0, "asis_conservadas": 0,
                                                "desde": None, "hasta": None})
fecha_de = re.compile(r"(\d{4}-\d\d-\d\d)")


def anotar(emp, clave, fecha=None):
    e = por_empleado[int(emp)]
    e[clave] += 1
    if fecha:
        e["desde"] = fecha if e["desde"] is None or fecha < e["desde"] else e["desde"]
        e["hasta"] = fecha if e["hasta"] is None or fecha > e["hasta"] else e["hasta"]
tok = re.compile(r"CAST\(N?'[^']*' AS [\w() ,]+\)|N?'(?:[^']|'')*'|NULL|-?[\d.]+")
ins = re.compile(r"INSERT \[dbo\]\.\[(Marcacion|Asistencia)\] \((.*?)\) VALUES \((.*)\)\s*$")
COL_M = ["Fecha", "Estado", "IdEmpleado", "Lugar", "idHorario"]
COL_A = ["Fecha", "IdEmpleado", "Hor_Ent", "Hor_Sal", "Alm_Sal", "Alm_Ent", "Flag_Ent", "Flag_Sal", "Estado", "idHorario", "Usuario", "PC"]
marc, asis = [], []

with open(src, encoding="utf-8-sig", errors="replace") as fh:
    for l in fh:
        if not (l.startswith("INSERT [dbo].[Marcacion]") or l.startswith("INSERT [dbo].[Asistencia]")):
            continue
        m = ins.match(l)
        cols = [c.strip(" []") for c in m.group(2).split(",")]
        vals = [x.group(0) for x in tok.finditer(m.group(3))]
        fila = dict(zip(cols, vals))
        if m.group(1) == "Marcacion":
            assert cols == COL_M and len(vals) == len(cols), l[:200]
            if fila["idHorario"] == "NULL":
                marc.append("(" + ", ".join(fila[c] for c in ["Fecha", "Estado", "IdEmpleado", "Lugar"]) + ")")
                anotar(fila["IdEmpleado"], "marc_rec", fecha_de.search(fila["Fecha"]).group(1))
            else:
                anotar(fila["IdEmpleado"], "marc_conservadas")
        else:
            assert cols == COL_A and len(vals) == len(cols), l[:200]
            if fila["idHorario"] == "NULL":
                asis.append("(" + ", ".join(fila[c] for c in COL_A if c != "idHorario") + ")")
                anotar(fila["IdEmpleado"], "asis_rec", fecha_de.search(fila["Fecha"]).group(1))
            else:
                anotar(fila["IdEmpleado"], "asis_conservadas")


def bloques(tabla, columnas, filas, tamano=1000):
    for i in range(0, len(filas), tamano):
        yield f"INSERT INTO {tabla} ({columnas}) VALUES\n" + ",\n".join(filas[i:i + tamano]) + ";"


L = [f"""/* ============================================================
   RECUPERAR MARCACIONES Y ASISTENCIAS SIN HORARIO (rrhhdb)
   La limpieza de huérfanos de bd-Datos.sql usó
       LEFT JOIN Horario ... WHERE h.idHorario IS NULL
   y así también borró las filas con idHorario NULL, que pertenecen a empleados existentes:
     - Marcacion : {len(marc)} filas (tipos 1M / 4M, sin reloj ni horario)
     - Asistencia: {len(asis)} filas
   Este script las vuelve a insertar con las mismas reglas de 02-migracion-desde-RRHH_Nuevo.sql.
   - Agrega el tipo de marcación M y completa las descripciones de N, T, X y M solo si están en NULL.
   - No borra ni cambia filas existentes; se puede ejecutar dos veces sin duplicar (NOT EXISTS por PK).
   - Todo en una transacción: si algo falla, no queda nada a medias.
   Solicitud de cambio SC-030. Justificación y procedimiento: docs/actualizados/SC-030-RECUPERACION-MARCACIONES.md
   Generado con Scripts/recuperacion/generar-recuperacion.py desde D:\\evo_shanty\\Scripts\\bd-Datos\\bd-Datos.sql.
   Ejecutar en SSMS sobre rrhhdb.
   ============================================================ */
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_NAME() <> N'rrhhdb'
BEGIN
    RAISERROR('Ejecute este script sobre la base rrhhdb.', 16, 1);
    SET NOEXEC ON;
END
GO

IF OBJECT_ID('tempdb..#marc') IS NOT NULL DROP TABLE #marc;
IF OBJECT_ID('tempdb..#asis') IS NOT NULL DROP TABLE #asis;
CREATE TABLE #marc (Fecha datetime NOT NULL, Estado nvarchar(2) NOT NULL, IdEmpleado int NOT NULL, Lugar nvarchar(10) NULL);
CREATE TABLE #asis (Fecha datetime NOT NULL, IdEmpleado int NOT NULL, Hor_Ent nvarchar(8) NULL, Hor_Sal nvarchar(8) NULL,
                    Alm_Sal nvarchar(8) NULL, Alm_Ent nvarchar(8) NULL, Flag_Ent bit NULL, Flag_Sal bit NULL, Estado bit NULL,
                    Usuario nvarchar(50) NULL, PC nvarchar(50) NULL);
GO
"""]
L += bloques("#marc", "Fecha, Estado, IdEmpleado, Lugar", marc)
L += bloques("#asis", "Fecha, IdEmpleado, Hor_Ent, Hor_Sal, Alm_Sal, Alm_Ent, Flag_Ent, Flag_Sal, Estado, Usuario, PC", asis)
L.append(f"""GO

SELECT 'Filas leídas' AS paso, (SELECT COUNT(*) FROM #marc) AS marcaciones, (SELECT COUNT(*) FROM #asis) AS asistencias,
       {len(marc)} AS marcaciones_esperadas, {len(asis)} AS asistencias_esperadas;

BEGIN TRAN;

-- Catálogo de tipos (significado deducido de los datos; confirmar con RRHH)
IF NOT EXISTS (SELECT 1 FROM dbo.TipoMarcacion WHERE Codigo = 'M')
    INSERT INTO dbo.TipoMarcacion (Codigo, Descripcion) VALUES ('M', 'Sin reloj ni horario (manual, por confirmar)');
UPDATE dbo.TipoMarcacion SET Descripcion = CASE Codigo
        WHEN 'N' THEN 'Normal (a la hora, por confirmar)'
        WHEN 'T' THEN 'Tardanza (por confirmar)'
        WHEN 'X' THEN 'Tardanza fuera de tolerancia (por confirmar)'
        WHEN 'M' THEN 'Sin reloj ni horario (manual, por confirmar)' END
WHERE Descripcion IS NULL AND Codigo IN ('N', 'T', 'X', 'M');

-- Marcacion: mismas reglas que 02-migracion (Estado '1M' -> NroMarcacion 1, TipoMarcacion 'M')
INSERT INTO dbo.Marcacion (Fecha, IdEmpleado, NroMarcacion, TipoMarcacion, CodDispositivo, idHorario)
SELECT m.Fecha, m.IdEmpleado, CAST(LEFT(m.Estado, 1) AS tinyint), SUBSTRING(m.Estado, 2, 1),
       NULLIF(LTRIM(RTRIM(m.Lugar)), ''), NULL
FROM #marc m
WHERE EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdEmpleado = m.IdEmpleado)
  AND NOT EXISTS (SELECT 1 FROM dbo.Marcacion x WHERE x.Fecha = m.Fecha AND x.IdEmpleado = m.IdEmpleado);
DECLARE @marcaciones int = @@ROWCOUNT;

-- Asistencia: mismas reglas que 02-migracion (fecha sin hora, horas texto -> time(0))
INSERT INTO dbo.Asistencia (Fecha, IdEmpleado, idHorario, Hor_Ent, Alm_Sal, Alm_Ent, Hor_Sal, Flag_Ent, Flag_Sal, Estado, Usuario, PC)
SELECT CAST(a.Fecha AS date), a.IdEmpleado, NULL,
       TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(a.Hor_Ent)), '')), TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(a.Alm_Sal)), '')),
       TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(a.Alm_Ent)), '')), TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(a.Hor_Sal)), '')),
       a.Flag_Ent, a.Flag_Sal, a.Estado, a.Usuario, a.PC
FROM #asis a
WHERE EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdEmpleado = a.IdEmpleado)
  AND NOT EXISTS (SELECT 1 FROM dbo.Asistencia x WHERE x.Fecha = CAST(a.Fecha AS date) AND x.IdEmpleado = a.IdEmpleado);
DECLARE @asistencias int = @@ROWCOUNT;

COMMIT;

SELECT 'Insertadas' AS paso, @marcaciones AS marcaciones, @asistencias AS asistencias,
       (SELECT COUNT(*) FROM dbo.Marcacion) AS total_marcaciones, (SELECT COUNT(*) FROM dbo.Asistencia) AS total_asistencias;
SELECT 'Por tipo' AS paso, t.Codigo, t.Descripcion, COUNT(m.IdEmpleado) AS marcaciones
FROM dbo.TipoMarcacion t LEFT JOIN dbo.Marcacion m ON m.TipoMarcacion = t.Codigo
GROUP BY t.Codigo, t.Descripcion ORDER BY t.Codigo;

DROP TABLE #marc;
DROP TABLE #asis;
GO
SET NOEXEC OFF;
GO
""")
with open(out, "w", encoding="utf-8-sig", newline="\r\n") as f:
    f.write("\n".join(L))
print("marcaciones", len(marc), "asistencias", len(asis))

if anexo:
    afectados = {k: v for k, v in por_empleado.items() if v["marc_rec"] or v["asis_rec"]}
    with open(anexo, "w", encoding="utf-8-sig", newline="") as f:
        w = csv.writer(f, delimiter=";")
        w.writerow(["IdEmpleado", "Marcaciones recuperadas", "Marcaciones que ya tenia", "Asistencias recuperadas",
                    "Asistencias que ya tenia", "Sin ninguna marcacion antes", "Desde", "Hasta"])
        for emp in sorted(afectados):
            v = afectados[emp]
            w.writerow([emp, v["marc_rec"], v["marc_conservadas"], v["asis_rec"], v["asis_conservadas"],
                        "SI" if v["marc_rec"] and v["marc_conservadas"] == 0 else "NO", v["desde"], v["hasta"]])
    print("anexo:", len(afectados), "empleados;",
          sum(1 for v in afectados.values() if v["marc_rec"] and v["marc_conservadas"] == 0), "sin ninguna marcacion antes")
