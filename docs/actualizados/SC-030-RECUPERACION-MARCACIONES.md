# SC-030 · Recuperación de marcaciones y asistencias sin horario

| Campo | Valor |
|---|---|
| Solicitud | SC-030 |
| Tipo | Corrección de datos (incidencia de migración) |
| Prioridad | Alta: afecta el registro de asistencia del personal |
| Fecha de registro | 11/11/2026 |
| Base de datos | `rrhhdb` (Azure SQL, servidor `sql-rrhh-anderson`) |
| Versión | `v2.9.2` |
| Estado | Lista para ejecutar; pendiente de la conformidad de RRHH (sección 11) |

## 1. Resumen

Al cargar la base de datos del sistema anterior se perdieron **104 984 marcaciones** y **1720 registros de asistencia** de 2024.

- Pertenecen a **844 empleados existentes**.
- **151 de esos empleados se quedaron sin ninguna marcación** en el sistema.
- No eran datos inválidos. Se borraron por un error en el script de carga `bd-Datos.sql`, que trató como huérfanas las filas que no tenían horario asignado.

Esta solicitud las reinserta desde el script original, sin modificar ningún registro existente. El procedimiento se puede verificar y revertir.

## 2. Antecedentes: cómo llegaron los datos a `rrhhdb`

```
bd-Datos.sql (datos del sistema anterior)         -> RRHH_Nuevo
02-migracion-desde-RRHH_Nuevo.sql (normalización) -> RRHH_Normalizado
03-datos-RRHH_Normalizado.sql (copia normalizada) -> rrhhdb (Azure)
```

El 11/11/2026 se comparó cada paso de forma automática y en modo solo lectura.

| Comparación | Resultado |
|---|---|
| `rrhhdb` frente a `03-datos-RRHH_Normalizado.sql` (77 tablas) | **Completa.** 65 tablas son idénticas. Las otras 12 solo tienen filas creadas después desde la aplicación (usuarios, permisos de 2026, bitácora, módulos de SC-009). No falta ninguna fila. Script: `Scripts/comparar-bd-con-carga-03.sql`. |
| `03-datos` frente a `bd-Datos.sql` | **Faltan marcaciones y asistencias.** El resto de las diferencias está documentado en `NORMALIZACION.md` y en `mig.Incidencia`: tablas de logística excluidas, ubigeos mal formados, personas que no son personal, etc. |

## 3. Causa

Al final de `bd-Datos.sql` hay una «limpieza de registros huérfanos» (líneas 833 841 a 833 844):

```sql
DELETE a FROM Asistencia a LEFT JOIN Horario h ON a.idHorario = h.idHorario WHERE h.idHorario IS NULL;
DELETE m FROM Marcacion  m LEFT JOIN Horario h ON m.idHorario = h.idHorario WHERE h.idHorario IS NULL;
```

La intención era borrar las filas que apuntan a un horario **inexistente**. Pero cuando `idHorario` está **vacío (NULL)**, la unión con `Horario` tampoco encuentra nada. Por eso la consulta borró también las filas que **no tenían horario asignado**, y esas no eran huérfanas:

- las 104 984 marcaciones y las 1720 asistencias pertenecen a empleados que existen (0 filas sin empleado);
- ninguna apuntaba a un horario inexistente: en todas el horario estaba vacío;
- ninguna repite una marcación o asistencia ya cargada, es decir, la misma fecha y empleado.

Como el borrado ocurrió antes de la normalización, no quedó registrado en `mig.Incidencia` ni en `NORMALIZACION.md`.

## 4. Alcance

| Dato | Marcaciones | Asistencias |
|---|---|---|
| En el sistema anterior (`bd-Datos.sql`) | 305 168 | 155 684 |
| Cargadas en `rrhhdb` | 200 184 | 153 964 |
| **Perdidas, que se recuperan** | **104 984 (34 %)** | **1720 (1 %)** |
| Empleados afectados | 844 | 407 |
| Empleados que se quedaron **sin ninguna** | **151** | 5 |
| Periodo | 01/01/2024 a 31/12/2024 | 01/01/2024 a 31/12/2024 |

Registros perdidos por mes (2024):

| Mes | Ene | Feb | Mar | Abr | May | Jun | Jul | Ago | Set | Oct | Nov | Dic |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| Marcaciones | 1046 | 754 | 2095 | 10 601 | 12 841 | 11 591 | 13 152 | 14 114 | 13 633 | 11 760 | 8258 | 5139 |
| Asistencias | 148 | 147 | 220 | 125 | 103 | 146 | 120 | 173 | 111 | 114 | 145 | 168 |

**Anexo por empleado:** [`anexos/SC-030-empleados-afectados.csv`](anexos/SC-030-empleados-afectados.csv). Separado por `;`, se abre en Excel. Tiene una fila por cada uno de los 844 empleados con:
- marcaciones recuperadas y las que ya tenía;
- asistencias recuperadas y las que ya tenía;
- si se había quedado sin ninguna marcación;
- el rango de fechas.

## 5. Tipos de marcación

Cada marcación se guarda como un número más una letra, por ejemplo `1N`.

**El número es qué marca del día es.** Coincide con las columnas de `Asistencia`:

| N.º | Marca | Hora típica en los datos |
|---|---|---|
| 1 | Entrada | 07:00 |
| 2 | Salida al refrigerio | 13:00 |
| 3 | Retorno del refrigerio | 13:00 a 14:00 |
| 4 | Salida | 16:00 a 18:00 |

**La letra es el tipo.** El sistema anterior no guardó su significado. Se dedujo comparando cada marca con el horario del empleado, en las 305 168 marcaciones:

| Tipo | Significado deducido | Evidencia |
|---|---|---|
| **N** | Normal, a la hora | El 100 % de las entradas `1N` es a la hora o antes |
| **T** | Tardanza | El 91 % de las entradas `1T` es después de la hora, con mediana de 6 min. El día típico es `1T` + `4N` |
| **X** | Tardanza fuera de tolerancia | El 100 % llega más de 10 min tarde, con mediana de 69 min |
| **M** | Marca sin reloj y sin horario, probablemente manual | Son las 104 984 recuperadas. No tienen reloj ni horario y siguen el patrón de un día normal (`1M` entrada hacia las 7:00, `4M` salida entre 16:00 y 18:00) |

> **RRHH debe confirmar estos significados** (sección 11). Hasta entonces, el catálogo los muestra con la nota «(por confirmar)».

## 6. Solución

El script `Scripts/recuperacion/recuperar-marcaciones-sin-horario.sql` se genera automáticamente desde `bd-Datos.sql` con `Scripts/recuperacion/generar-recuperacion.py`.

1. Carga las 104 984 marcaciones y las 1720 asistencias tal como estaban en el sistema anterior, en tablas temporales.
2. En una sola transacción:
   - agrega el tipo de marcación **M** al catálogo;
   - completa la descripción de N, T, X y M **solo si hoy está vacía**;
   - inserta las marcaciones con las mismas reglas que la migración original (`02-migracion`): `1M` se guarda como n.º 1, tipo M, sin reloj y sin horario;
   - inserta las asistencias con las mismas reglas: fecha sin hora y horas convertidas a `time`.
3. Muestra los totales y el conteo por tipo.

**Lo que el script no hace:**
- No borra ni modifica ninguna marcación, asistencia ni empleado existente.
- No inventa datos: cada fila viene tal cual del sistema anterior.
- Si se ejecuta dos veces no duplica nada, porque inserta solo lo que no existe.
- Si algo falla, no queda nada a medias, porque todo va en una transacción.

## 7. Impacto en el sistema

| Módulo | Efecto |
|---|---|
| Asistencia → Marcaciones | Aparecen las marcas `1M` y `4M`. Los 151 empleados que no tenían ninguna ahora muestran su registro de 2024 |
| Asistencia (registro diario) | Se agregan 1720 días, sin horario asignado |
| Reporte de presencialidad (RF-15) | **Sin cambios.** Solo cuenta marcas de los relojes elegidos y las marcas M no tienen reloj |
| Catálogo de tipos de marcación | Aparece el tipo M; N, T, X y M tienen descripción |
| Código de la aplicación | Sin cambios: `Marcacion.idHorario` y `Asistencia.idHorario` ya aceptan valores vacíos |

## 8. Procedimiento de ejecución

| Paso | Acción | Responsable |
|---|---|---|
| 1 | Revisar este documento y el anexo; dar la conformidad (sección 11) | RRHH |
| 2 | Ejecutar `Scripts/recuperacion/verificar-recuperacion.sql`. Debe decir **SIN RECUPERAR** (200 184 / 153 964) | Desarrollo |
| 3 | En SSMS: abrir `recuperar-marcaciones-sin-horario.sql` (Ctrl+O), elegir `rrhhdb` y ejecutar (F5). Tarda unos minutos | Desarrollo |
| 4 | Comprobar el resultado del script: **104 984** marcaciones y **1720** asistencias insertadas | Desarrollo |
| 5 | Ejecutar otra vez `verificar-recuperacion.sql`. Debe decir **RECUPERADO** (305 168 / 155 684) y la integridad en 0 | Desarrollo |
| 6 | En la aplicación, abrir las marcaciones de 3 empleados del anexo con «Sin ninguna marcacion antes = SI» y comprobar que aparecen | RRHH |
| 7 | Registrar la fecha de ejecución y el resultado en la sección 11 | Desarrollo |

## 9. Criterios de aceptación

- [ ] Se insertaron exactamente 104 984 marcaciones (todas de tipo M) y 1720 asistencias.
- [ ] Totales: 305 168 marcaciones y 155 684 asistencias, que son las cifras del sistema anterior.
- [ ] 0 marcaciones o asistencias de empleados inexistentes y 0 claves foráneas no confiables.
- [ ] Ningún otro conteo de `Scripts/comparar-bd-con-carga-03.sql` cambió. Ese script mostrará Marcacion +104 984, Asistencia +1720 y TipoMarcacion +1 como «SOBRAN», que es lo esperado.
- [ ] RRHH comprobó en la aplicación una muestra del anexo.

## 10. Reversión

Si RRHH no aprueba el resultado, se ejecuta `Scripts/recuperacion/revertir-recuperacion.sql`.
- Borra solo las filas recuperadas. Se identifican porque antes de la recuperación no había en `rrhhdb` ninguna marcación ni asistencia sin horario, ni el tipo M.
- Si los conteos no son exactamente 104 984 y 1720, se detiene sin borrar nada.

## 11. Conformidad del área de RRHH

**Confirmaciones solicitadas:**
- [ ] Los registros recuperados corresponden al personal y deben formar parte del historial de asistencia de 2024.
- [ ] Significado del tipo **M**: ______________________________________
- [ ] Significados de **N / T / X** (sección 5): ☐ correctos ☐ corregir: __________________

| Rol | Nombre | Firma | Fecha |
|---|---|---|---|
| Jefe(a) del área de RRHH | | | |
| Responsable del sistema / desarrollo | | | |

| Ejecución | Valor |
|---|---|
| Fecha y hora de ejecución | |
| Ejecutado por | |
| Marcaciones / asistencias insertadas | |
| Resultado de `verificar-recuperacion.sql` | |

## 12. Integridad de los archivos

| Archivo | SHA-256 |
|---|---|
| `Scripts/recuperacion/recuperar-marcaciones-sin-horario.sql` | `f4ccce625e7bf82ba815694f935cc6b6ef7aa51ea3cf37862ef8a5a8ea86a055` |
| `docs/actualizados/anexos/SC-030-empleados-afectados.csv` | `2ac931909c2b76e4649e367a3743c7e8918922ff9f01c840351e21d4eaf64af4` |

Para comprobar que nadie modificó el script entre la aprobación y la ejecución, en PowerShell:
`Get-FileHash Scripts\recuperacion\recuperar-marcaciones-sin-horario.sql -Algorithm SHA256`.
También se puede regenerar con el generador y comparar el hash.
