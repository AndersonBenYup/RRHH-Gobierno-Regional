# Normalización de la BD de RR.HH. — `RRHH_Normalizado`

Este documento explica **qué se normalizó, por qué y hasta qué forma normal llega cada tabla** en la nueva base `RRHH_Normalizado`, construida a partir de `RRHH_Nuevo`.

> `RRHH_Nuevo` **no se modifica**. Sigue siendo la fuente de datos y la BD que usa la aplicación hoy. `RRHH_Normalizado` es una BD nueva e independiente.

---

## 1. Cómo se construye

```
1. Scripts/bd-evo-RRLL.sql                          -> crea RRHH_Nuevo (esquema original)
2. Scripts/bd-Datos/bd-Datos.sql                    -> carga RRHH_Nuevo (sin cambios)
3. Scripts/bd-Normalizada/01-esquema-RRHH_Normalizado.sql   -> crea RRHH_Normalizado vacía
4. Scripts/bd-Normalizada/02-migracion-desde-RRHH_Nuevo.sql -> copia y transforma los datos
```

**Ruta corta (sin `RRHH_Nuevo`):** si solo se necesita la BD normalizada, basta con dos scripts:

```
1. Scripts/bd-Normalizada/01-esquema-RRHH_Normalizado.sql   -> crea RRHH_Normalizado vacía
2. Scripts/bd-Normalizada/03-datos-RRHH_Normalizado.sql     -> carga los datos ya normalizados (~156 MB, ~100 s)
```

`03-datos-RRHH_Normalizado.sql` es el equivalente normalizado de `bd-Datos.sql`: tiene los mismos INSERT ya en el formato nuevo (714,771 filas en 77 tablas). Mantiene los Id originales con `IDENTITY_INSERT`, carga con las FKs deshabilitadas y al final las vuelve a habilitar `WITH CHECK`, así que quedan confiables. Termina verificando los conteos. Se genera con `generar-03-datos.ps1` a partir de una `RRHH_Normalizado` cargada por la ruta 1–4. Hay que regenerarlo si cambian el esquema o la migración.

- **Los scripts de datos originales no se tocan.** Toda la transformación se hace con SQL (`INSERT … SELECT`) de una BD a la otra.
- La migración corre en **una sola transacción**: si algo falla, no queda nada a medias.
- Las FKs están activas durante la carga, así que quedan **confiables** (`is_not_trusted = 0`). Esto es distinto de `bd-Datos.sql`, que carga con `NOCHECK`.
- Cada dato que se corrigió, movió o descartó queda registrado en la tabla **`mig.Incidencia`**.
- Para repetir el proceso basta con ejecutar otra vez los pasos 3 y 4. El paso 4 se niega a correr si la BD ya tiene datos.

**Resultado verificado** (SQL Server 2025, unos 7 s): 76 tablas + 6 vistas + 84 FKs, todas confiables. Los conteos de origen y destino coinciden en las 25 tablas verificadas.

| Tabla | Filas | | Tabla | Filas |
|---|---:|---|---|---:|
| RegAsisDiario | 263,520 | | Persona (solo RRHH) | 4,106 |
| Marcacion | 200,184 | | Empleado | 3,335 |
| Asistencia | 153,964 | | Distrito | 2,009 |
| Empleado_Area | 35,884 | | Familiar | 966 |
| Permiso | 26,120 | | Resolucion | 803 |

---

## 2. Repaso rápido de las formas normales

| Forma | Regla (resumida) | Síntoma típico cuando se viola |
|---|---|---|
| **1FN** | Cada celda guarda **un solo valor atómico** y no hay grupos repetidos. | `Tel1, Tel2, Tel3`; `'1N'` que en realidad son dos datos. |
| **2FN** | 1FN + ningún atributo depende de **solo una parte** de una clave compuesta. | En `Familiar(IdEmpleado, IdPersona)`, el sexo depende solo de `IdPersona`. |
| **3FN** | 2FN + ningún atributo no clave depende de **otro atributo no clave** (dependencia transitiva). | `Empleado → IdPersona → Sexo`. |
| **FNBC** | Todo determinante es clave candidata (una 3FN más estricta). | Casi siempre coincide con 3FN cuando las claves son simples. |
| **4FN** | FNBC + no hay **dos hechos multivaluados independientes** en la misma tabla. | `Vivienda(Id, Servicio, Material)` mezclando dos listas sin relación. |
| **5FN** | 4FN + la tabla no se puede partir en tablas más chicas sin perder información. | Relaciones ternarias que en realidad eran binarias. |

> **Matiz importante:** las formas normales hablan de dependencias **dentro de una misma tabla**. Guardar un dato que se puede **calcular desde otra tabla** (por ejemplo, `NumHijos` cuando los hijos ya están en `Familiar`) no viola técnicamente ninguna FN, pero es **redundancia entre tablas** y causa las mismas inconsistencias. En este trabajo también se eliminó, y se marca aparte como "dato derivado".

---

## 3. Veredicto global

| | `RRHH_Nuevo` (antes) | `RRHH_Normalizado` (después) |
|---|---|---|
| Forma normal del modelo | **No llegaba a 1FN** en varias tablas; violaciones de 2FN y 3FN en el núcleo (`Empleado`, `Familiar`, `Asegurado`) | **FNBC en todas las tablas**; las tablas de relación muchos-a-muchos llegan a **4FN/5FN** |
| FKs | 76 tablas, muchas FKs faltantes (por ejemplo `Empleado → Persona`) | 84 FKs, todas confiables |
| Datos derivados guardados | `NumHijos`, `LaboraInstitucion`, `NroTrabajaUO`, `PromedMensSueld` | Se calculan en vistas |
| Tablas ajenas a RR.HH. | 10 tablas de logística/planeamiento | Excluidas (siguen en `RRHH_Nuevo`) |

### Forma normal alcanzada, por tabla

| Tabla | Antes | Después | Cambio principal |
|---|---|---|---|
| **Persona** | FNBC (*) | **FNBC** | Recibe los atributos personales que estaban repetidos en 3 tablas; clave candidata `(TipoDocID, NumDocID)` garantizada |
| **Empleado** | ✗ 1FN y ✗ 3FN | **FNBC** | Sale lo personal (→ Persona), las resoluciones (→ Resolucion), el área (→ Empleado_Area) y la ficha social (→ RRHH_AspSocio) |
| **Familiar** | ✗ **2FN** | **FNBC** | Sale todo lo que depende solo de `IdPersona` |
| **RRHH_Asegurado** | ✗ 3FN | **FNBC** | Sale `Sexo`/`EstadoCivil`; `VinculoFamiliar` pasa a catálogo |
| **ResponsableXUO** | ✗ 3FN | **FNBC** | Sale `AreaOrganizacional` e `idDependeDe` (copias del maestro) |
| **RRHH_FuncFam** | ✗ **1FN** | **4FN** → `RRHH_FuncFamRespuesta` | 9 columnas → filas |
| **RRHH_AspSocio** | ✗ 1FN | **FNBC** | Sale el grupo `idPregunta1..3` |
| **Vivienda** | ✗ 1FN | **FNBC** + `ViviendaServicio` (**4FN**) | Columnas bit de servicios → filas |
| **Marcacion** | ✗ **1FN** | **FNBC** | `Estado '1N'` → `NroMarcacion` + `TipoMarcacion` |
| **Ubigeo** | FNBC (*) | **FNBC** → `Departamento` / `Provincia` / `Distrito` | 3 niveles separados; 1,069 códigos basura eliminados |
| **EstructOrganiz** | FNBC + derivados | **FNBC** | Salen los derivados y las columnas de presupuesto |
| **Resolucion** | FNBC | **FNBC** | Recibe las resoluciones que eran columnas en Empleado |
| **TitulosEmpleado** | FNBC con clave mal elegida | **FNBC** | PK propia + `UNIQUE(IdEmpleado, NumColegiatura)` |
| **RegAsisDiario** | FNBC, dominio incorrecto | **FNBC** | `MinTarde` texto → `SegundosTarde int`; `Estado` → catálogo |
| **Rol_Acceso** | ✗ 1FN (sospecha) | **5FN** | Se elimina `Permiso char(8)` (cadena codificada, sin uso) |
| **SistemaOpcion** | FNBC | **FNBC** | Se eliminan `ConClave`/`Clave` (contraseñas en texto plano) |
| **Empleado_Area** | 5FN | **5FN** | Recibe el área que estaba en Empleado |
| **Usuario_Rol**, **ViviendaServicio**, **RRHH_FuncFamRespuesta** | — | **4FN/5FN** | Tablas de relación, clave = todas sus columnas de relación |
| Catálogos (40) | FNBC | **FNBC** | Solo tipos, `NOT NULL` y `IDENTITY` |
| Resto (Asistencia, Permiso, PeriodoLaboral, Capacitacion, ExpLaboral, EstudiosRealizado, Comportamiento, Rotacion, HorarioTemporal, Horario, RRHH_Salud, RRHH_DinamicaFamiliar, Usuario, Rol) | FNBC con FKs faltantes y tipos incorrectos | **FNBC** | FKs completas, tipos correctos (`date`, `time`, numéricos) |

(*) Formalmente cumplía la forma normal, pero tenía problemas de integridad o calidad. Se explican abajo.

---

## 4. Detalle de cada normalización

Cada caso trae la **evidencia encontrada en los datos reales** de `RRHH_Nuevo`. No son suposiciones.

### 4.1 `Familiar` — violaba **2FN** → ahora FNBC

**Problema.** La clave es compuesta: `(IdEmpleado, IdPersona)`. Pero `FamSexo`, `IdEstadoCivil`, `IdTipoSangre`, `IdDiscapacidad`, `NumAutogenSalud`, `FamOcupacion`, `FamGradoInstruc`, `FamLugTrab`, `Procedencia`, `Enfermedad` y `TiempoEnfermedad` describen **a la persona**, no a la relación con el empleado. Dependen solo de `IdPersona`, es decir, de una parte de la clave. Esa es la definición exacta de violar la 2FN.

**Evidencia.** 31 personas son familiares de **más de un** empleado (por ejemplo, hermanos que trabajan en la institución). Como sus datos están copiados en cada fila, ya divergieron: 1 persona tiene dos sexos distintos, 3 tienen dos estados civiles y 17 tienen dos ocupaciones.

**Solución.**
```
Familiar(IdEmpleado, IdPersona, IdTipoFam, ViveCasa)      <- solo lo que depende del PAR
Persona(IdPersona, Sexo, IdEstadoCivil, Ocupacion, ...)   <- lo que depende de la persona
```
`ViveCasa` se queda porque depende del par: la misma persona puede vivir con un hermano y no con el otro. `LaboraInstitucion` se elimina porque es **derivable** (el familiar tiene un vínculo laboral activo). Ahora se calcula en `vw_FamiliarDetalle`. Había **16 filas** donde el valor guardado contradecía la realidad.

### 4.2 `Empleado` — violaba **1FN y 3FN** → ahora FNBC

`Empleado` en realidad modela un **vínculo laboral**: **107 personas tienen entre 2 y 6 registros** de empleado (reingresos). Por eso `IdPersona` **no** es clave en `Empleado`, y todo lo que dependa de la persona genera una dependencia transitiva:

```
IdEmpleado -> IdPersona -> Sexo, EstadoCivil, TipoSangre, Nacionalidad, Discapacidad,
                           NumAutogenSalud, NumRUC, Brevete, LibretaMilitar, Profesion,
                           GradoInstruccion, Foto
```

**Evidencia de anomalías:** de las 107 personas con varios vínculos, 11 tienen estados civiles distintos y 3 tienen tipos de sangre distintos según el registro.

| Problema | Forma violada | Solución |
|---|---|---|
| Datos personales (lista de arriba) | **3FN** (transitiva vía `IdPersona`) | Se mueven a `Persona` |
| `NumResolCese/Fecha…`, `NumResol20530/Fecha…`, `NumResolPenMen/Fecha…`, `NumResoIngreEsta/FechaResolIngreEsta/FechaIngreEstado`, `NumResoIngreInstitu/FechaResoIngreInstitu` | **1FN** (grupo repetido "resolución + fecha") | Pasan a filas de `Resolucion` con 2 tipos nuevos: *Ingreso al Estado* e *Ingreso a la Institución*. Se agrega `FechaVigencia`. Resultado: 646 + 145 filas. Las de cese, 20530 y pensión mensual estaban vacías en todas las filas. |
| `Year` + `idAreaOrganiz` | Redundancia con `Empleado_Area` (historial) | El área va al historial: se agregaron **551** combinaciones que solo estaban en `Empleado`. El área vigente se obtiene con `vw_EmpleadoAreaActual`. |
| `NumHijos` | Dato derivado de `Familiar` | Se calcula en `vw_EmpleadoResumenFamiliar`. En **33** empleados el valor guardado no coincidía con los hijos registrados. |
| `AspecSalud, AspecEducacion, AspecRecreacion, AspecPsico, AspecSocial, DiagnosSocial, ObsFam` | No es una FN: cohesión (partición vertical) | Pasan a `RRHH_AspSocio`, que ya era la ficha social 1:1 |
| `Userid` (uniqueidentifier) | FK colgante hacia `aspnet_Users`, tabla eliminada | Se elimina (3 filas con valor, registradas en `mig.Incidencia`) |
| `IdSeguro` | FK hacia una tabla inexistente; 100 % NULL | Se elimina |
| `Estado char(1)`, `CargaFam varchar`, `Foto image` | Dominio y tipos | `bit`, `tinyint`, `varbinary(max)` en `Persona` |

**Decisión de diseño:** los datos previsionales (`IdAFP`, `NroAFP`, `IdRegimenPen`) y `CtaCorriente` **se quedan en Empleado**. La planilla los reporta por vínculo laboral y pueden cambiar entre un vínculo y otro. Con esa regla de negocio dependen de `IdEmpleado` y no violan la 3FN.

### 4.3 `RRHH_Asegurado` — violaba **3FN** → ahora FNBC

`Sexo` e `idEstadoCivil` dependen de `idPersona` (que no es clave), así que es una transitiva. Se mueven a `Persona`. `VinculoFamiliar` era **texto libre** ('Hijo', 'Gestante'…), así que pasa al catálogo nuevo `VinculoAsegurado`. No se reutiliza `TipoFamiliar` porque los vínculos de EsSalud (gestante, hijo mayor incapacitado…) no son lo mismo que el parentesco.

### 4.4 `ResponsableXUO` — violaba **3FN** → ahora FNBC

`AreaOrganizacional` e `idDependeDe` dependen de `(Year, idAreaOrganiz)`, que no es la clave de la tabla (la clave es `IdResponsable`). Eran copias del maestro `EstructOrganiz`.
**Evidencia:** en **las 125 filas** el nombre del área ya era distinto al del maestro. Es la anomalía de actualización clásica. Se eliminan las dos columnas.

### 4.5 `RRHH_FuncFam` — violaba **1FN** → `RRHH_FuncFamRespuesta` (4FN)

`Com1, Com2, Afec1, Afec2, Decis1, Decis2, Soc1, Soc2, Soc3` son 9 columnas del **mismo dominio** (respuesta a una pregunta del test de funcionamiento familiar). Es un grupo repetido: agregar una pregunta obligaría a hacer un `ALTER TABLE`.

```
RRHH_FuncFamPregunta(IdPregunta, Codigo, Dimension, Orden)     -- 9 filas
RRHH_FuncFamRespuesta(IdEmpleado, IdPregunta, Respuesta)       -- 1 fila por respuesta
```
La vista `vw_RRHH_FuncFam` devuelve la forma antigua por compatibilidad.
**Dato:** las 628 encuestas de origen tenían **las 9 respuestas en NULL**, por eso la tabla nueva queda vacía. No se perdió nada.

### 4.6 `RRHH_AspSocio` — violaba **1FN** → FNBC

`idPregunta1`, `idPregunta2`, `idPregunta3` son un grupo repetido y además **sin catálogo de preguntas**: no había a qué apuntar. Estaban en NULL en el 100 % de las filas, así que se eliminan. Si se necesitan, se modelan igual que el test de funcionamiento familiar (4.5).

### 4.7 `Vivienda` — violaba **1FN** → FNBC + `ViviendaServicio` (4FN)

`Telefono`, `Cable`, `Internet` y `Otros` (bits) son una lista de servicios guardada como columnas. Ahora están en el catálogo `ServicioViv` y en la tabla `ViviendaServicio(IdVivienda, IdServicio)`.
Además, como **4FN**, los servicios van en su propia tabla y no mezclados con otros datos multivaluados de la vivienda. Así se evita el producto cartesiano de hechos independientes.
**Dato:** ninguna vivienda tenía servicios marcados (0 filas).

### 4.8 `Marcacion` — violaba **1FN** → FNBC

`Estado` guardaba **dos datos en un campo**: `'1N'` = marca n.º **1** + tipo **N**. Valores reales: `1N, 2N, 3N, 4N, 1T, 2T, 3T, 1X`. No es atómico: no se puede filtrar "todas las marcas n.º 4" sin usar `LEFT()`.

```
Marcacion(..., NroMarcacion tinyint, TipoMarcacion char(1) FK, CodDispositivo FK, ...)
```
`Lugar` (texto: `'*'`, `'53423'`, `'44013'`, `'44021'`) era el código del reloj marcador. Pasa al catálogo `DispositivoMarcacion`.

### 4.9 `Ubigeo` → `Departamento` / `Provincia` / `Distrito`

Formalmente estaba en FNBC, porque `IdUbigeo` y `(Departamento, Provincia, Distrito)` son claves candidatas. Pero tenía dos problemas serios:

1. **Mezclaba 3 entidades en una tabla:** había filas de departamento (`130000`), de provincia (`130100`) y de distrito (`130101`). Una FK a `Ubigeo` no podía exigir que fuera un distrito.
2. **1,069 códigos basura:** el mismo ubigeo aparecía sin el cero inicial (`10101` además de `010101`). Hasta los códigos de dos dígitos estaban como `'1 '` en vez de `'01'`.

```
Departamento(CodDepartamento)
Provincia(CodDepartamento, CodProvincia)              FK -> Departamento
Distrito(CodDepartamento, CodProvincia, CodDistrito,  FK -> Provincia
         IdUbigeo AS Dep+Prov+Dist PERSISTED)         PK = IdUbigeo
```
`IdUbigeo` es una **columna calculada**: el código de 6 dígitos ya no se escribe a mano y no puede contradecir a sus partes. `vw_Ubigeo` da compatibilidad con la tabla anterior.

### 4.10 `EstructOrganiz` — FNBC, se limpian derivados y columnas ajenas

- La clave `(Year, idAreaOrganiz)` es correcta: **8 áreas cambian de nombre entre años**, así que la tabla está versionada por año y cada atributo depende de la clave completa.
- `NroTrabajaUO` es derivado y se calcula en `vw_EstructOrganizDotacion`. `PromedMensSueld` depende de planillas que no existen en esta BD.
- Las columnas de presupuesto y logística (`PresupAnual`, `IdPresupuesto`, `InversPublic`, `ApruebaPedido`, `MontoAprobacion`, `PresupCompra`, `PresupUtilizado`) quedan fuera del dominio de RR.HH.
- `IdEmpleado` (jefe): **16 filas** apuntaban a empleados inexistentes. Se dejan en NULL y se registran.

### 4.11 `Persona` — se convierte en el centro del modelo

- Recibe todos los atributos personales que estaban en `Empleado`, `Familiar` y `RRHH_Asegurado`.
- **Consolidación:** cuando las fuentes discrepaban, se tomó el valor con esta prioridad: vínculo laboral más reciente > registro como familiar > registro como asegurado. Hubo **488 discrepancias**, todas en `mig.Incidencia` con los valores originales. La mayoría (NumAutogenSalud, RUC, libreta militar) se explican porque los datos de prueba están anonimizados por fila.
- **Clave candidata garantizada:** `UNIQUE (TipoDocID, NumDocID)`. Antes había solo un índice no único.
- **Dominio:** solo migran las **4,106 personas de RR.HH.** Las otras 4,195 eran sobre todo personas jurídicas o proveedores de logística (RUC) y siguen en `RRHH_Nuevo`.
- **Ubigeos inválidos:** 9 referencias vacías o que no eran un distrito se dejan en NULL.
- Se eliminan `Fax` y `FonoCentroLab` (100 % vacías).

### 4.12 Otros ajustes

| Tabla | Cambio | Motivo |
|---|---|---|
| `TitulosEmpleado` | PK `IdTitulo` + `UNIQUE(IdEmpleado, NumColegiatura)`; `IdEmpleado NOT NULL` | Con la PK solo en `NumColegiatura`, dos profesionales de distintos colegios con el mismo número chocaban |
| `RegAsisDiario` | `MinTarde varchar` (texto `'00:05:30'`) → `SegundosTarde int`; `Estado` → FK a `EstadoAsistencia` (45 códigos) | Dominio: un "minuto" guardado como texto, con segundos (9,745 filas tenían segundos ≠ 0) |
| `EstudiosRealizado` | `[AñoInicio]/[AñoTermino] datetime` → `FechaInicio/FechaTermino date`; PK reordenada `(IdEmpleado, IdTipoEstudios, Correlativo)` | Guardaban fechas, no años; nombres sin `ñ` |
| `Rol_Acceso` | Se elimina `Permiso char(8)` | Cadena codificada (no atómica) que la aplicación no usa. Que exista la fila ya significa "tiene acceso" |
| `SistemaOpcion` | Se eliminan `ConClave`, `Clave` | Contraseñas en texto plano (todas vacías) |
| `Motivo_Perm` | `CHECK (TipoModalidad IN ('C','P'))`, `idTipoPermiso NOT NULL` | Restricción de dominio |
| `Horario`, `Asistencia`, `Permiso` | Horas `char(8)` → `time(0)` | Tipo correcto |
| Fechas en general | `datetime` → `date` donde la hora no significa nada | Por ejemplo `FechaNacimiento` traía `20:35:21.960` en las 8,301 filas |
| Todas | Restricciones con nombre, `ANSI_NULLS ON`, sin `image`/`text` | Estándar y tipos vigentes |
| Claves simples | `IDENTITY` | Evita el `MAX()+1` (condición de carrera con dos usuarios a la vez) |

---

## 5. Decisiones conscientes: lo que **no** se normalizó y por qué

Un modelo normalizado no es el que parte todo hasta el extremo. Estas decisiones se tomaron a propósito:

| Caso | Decisión | Justificación |
|---|---|---|
| `Horario(Ingreso1, Salida1, Ingreso2, Salida2)` y `Asistencia(Hor_Ent, Alm_Sal, Alm_Ent, Hor_Sal)` | **Se mantienen como columnas** | La jornada partida tiene exactamente **2 tramos con significado distinto** (mañana y retorno de almuerzo), no una lista de largo variable. No es un grupo repetido en sentido estricto, y pasarlos a filas complicaría todo el cálculo de asistencia sin ganar nada. |
| `RegAsisDiario` | **Se mantiene** | Es una tabla de **resultados de proceso** (resumen diario calculado). Se acepta como dato derivado intencional por rendimiento. Debe regenerarse, no editarse a mano. |
| `Empleado.FechaIngreso` vs `PeriodoLaboral` | **Se mantienen las dos** | En 144 empleados la fecha no coincide con ningún periodo, así que **no es derivable**: es otro dato (la fecha de registro del vínculo). |
| `Empleado.CargaFam` | **Se mantiene** (como `tinyint`) | Es la carga **declarada**, no el número de familiares registrados: difiere en 46 casos. `NumHijos`, en cambio, sí era derivable y se eliminó. |
| `EstructOrganiz.IdEmpleado` y `ResponsableXUO` | **Se mantienen los dos** | Uno es el jefe del área en el año; el otro, el historial de responsables con fechas. Se parecen, pero no se derivan uno del otro. |
| `EstudiosRealizado`, `TitulosEmpleado`, `Capacitacion`, `ExpLaboral` ligadas a `Empleado` | **Se mantiene** (pendiente) | En rigor la formación pertenece a la **persona**, no al vínculo. Moverlas a `Persona` es la evolución natural, pero cambia mucho la aplicación. Queda como siguiente paso. |
| `Persona.GradoInstruccion`, `Familiar` → ocupación | **Texto libre** | Los valores reales mezclan conceptos ('SU CASA', 'EMPLEADO' dentro de grado de instrucción). Pasarlos a catálogo requiere una **limpieza manual** previa con el área usuaria. |
| `Permiso.Autorizacion` | **Texto libre** | Debería ser una FK al empleado que autoriza, pero el dato es un nombre escrito a mano y no se puede mapear de forma confiable. |
| `EstadoAsistencia`, `TipoMarcacion`, `DispositivoMarcacion` | Catálogos creados **sin descripción** | Los códigos (`F`, `FR`, `TLT`, `PV`, `N`, `T`, `X`…) se sacaron de los datos, pero su significado lo tiene que confirmar el área de RR.HH. |

---

## 6. Tablas excluidas (fuera del dominio RR.HH.)

Venían de los módulos de logística y planeamiento del CMI. **No se migran** y siguen disponibles en `RRHH_Nuevo`:

`Familia` (1,321 filas), `UnidadMedida` (254), `TipoRecurso` (117), `TipoDoc` (64), `TipoTransaccion` (12), `TipoMovimiento` (8), `TipoMoneda` (5), `Limitacion` (11), `Institucion` (1), `Inconveniente` (0).

---

## 7. Auditoría de datos: `mig.Incidencia`

Cada transformación con efecto en los datos quedó registrada (2,202 registros):

| Tabla | Acción | Registros | Qué significa |
|---|---|---:|---|
| Ubigeo | DESCARTADO | 1,069 | Códigos sin cero inicial, duplicados de uno válido |
| Empleado | MOVIDO | 551 + 1 | Áreas que pasaron al historial; resumen de resoluciones movidas |
| Persona | CONSOLIDADO | 488 | Atributo con valores distintos entre fuentes (se guarda cuál se eligió) |
| Empleado | DERIVADO | 33 | `NumHijos` guardado ≠ hijos registrados |
| Familiar | DERIVADO | 16 | `LaboraInstitucion` guardado ≠ calculado |
| EstructOrganiz | ANULADO | 16 | Jefe de área inexistente |
| Persona | ANULADO | 9 | Ubigeo vacío o que no es un distrito |
| Empleado | DESCARTADO | 3 | `Userid` hacia `aspnet_Users` |
| (varias) | NO MIGRADO / DESCARTADO / DERIVADO | 16 | Resúmenes de tablas y columnas excluidas |

```sql
-- ver el detalle
SELECT * FROM RRHH_Normalizado.mig.Incidencia ORDER BY Tabla, Accion;
```

---

## 8. Vistas (reemplazan columnas derivadas)

| Vista | Reemplaza |
|---|---|
| `vw_EmpleadoAreaActual` | `Empleado.Year` / `Empleado.idAreaOrganiz` |
| `vw_EmpleadoResumenFamiliar` | `Empleado.NumHijos` (+ número de familiares y carga declarada) |
| `vw_FamiliarDetalle` | `Familiar.LaboraInstitucion` |
| `vw_EstructOrganizDotacion` | `EstructOrganiz.NroTrabajaUO` |
| `vw_Ubigeo` | La antigua tabla `Ubigeo` (solo distritos válidos) |
| `vw_RRHH_FuncFam` | La forma antigua de `RRHH_FuncFam` (9 columnas) |

---

## 9. Impacto en la aplicación (para cuando se migre)

La aplicación **sigue apuntando a `RRHH_Nuevo`** y no se rompe nada. Para pasarla a `RRHH_Normalizado` habrá que ajustar:

1. **Entidades EF** (`Capa Entidades`): `Empleado`, `Persona`, `Familiar`, `RRHH_Asegurado`, `ResponsableXUO`, `EstructOrganiz`, `Marcacion`, `RegAsisDiario`, `EstudiosRealizado`, `TitulosEmpleado`, `Rol_Acceso`, `SistemaOpcion`. Reemplazar `Ubigeo` por `Distrito`/`vw_Ubigeo` y `RRHH_FuncFam` por `RRHH_FuncFamRespuesta`. Agregar `Departamento`, `Provincia`, `VinculoAsegurado`, `ServicioViv`, `ViviendaServicio`, `EstadoAsistencia`, `TipoMarcacion` y `DispositivoMarcacion`.
2. **Quitar del `DbContext`** las 10 entidades excluidas (`Familia`, `UnidadMedida`, `Inconveniente`, `Limitacion`, `Institucion`, `TipoDoc`, …).
3. **Generación de Id:** 14 controladores calculan el Id con `Max()+1`. Con `IDENTITY` hay que dejar que la BD lo asigne (en EF: `[DatabaseGenerated(DatabaseGeneratedOption.Identity)]`).
4. **Formularios de Empleado y Familiar:** los datos personales ahora se guardan en `Persona`.
5. **Cadena de conexión:** `Database=RRHH_Normalizado` en `appsettings.json`.
