> Versión actualizada (noviembre 2026), con el cierre de `v1.6.0` en la sección 6. El original está en la raíz del repositorio (`SOLICITUDES-DE-CAMBIO.md`).

# Solicitudes de cambio (SC-001 a SC-007): estado real

## Seguimiento T3 · SC-028 (RF-17) · Validar feriados en el reporte general

- **Rama y commit:** `feature/feriados-en-reporte-general`, `7b5b1b1`.
- **Componente:** Registro de Asistencia, resumen de feriados en pantalla. Cuenta cada fecha una vez dentro del rango y muestra las fechas incluidas. El total es independiente de los empleados mostrados.
- **CP-RF17-01:** se comprobaron ambas fechas límite, fechas externas, fechas duplicadas y valores nulos. Prueba automatizada aprobada. Captura del reporte pendiente.

## Seguimiento T3 · SC-029 (RF-18) · Reportar ausencias por periodo

- **Rama y commit:** `feature/reporte-de-ausencias-por-periodo`, `0b7a16f`.
- **Componente:** Permisos → Reporte de ausencias. Consulta permisos por fecha y motivos seleccionados en el catálogo. Incluye periodos que cruzan los límites y excluye los códigos históricos de teletrabajo 125 y 126.
- **CP-RF18-01:** consultar del 10 al 15 con vacaciones del 8 al 10 y licencia del 16 al 18. Al seleccionar ambos motivos, solo aparece la primera persona.
- **Estado:** compilación correcta. Pendiente ejecutar el caso funcional y guardar evidencia.

## Seguimiento T3

### SC-013 · RF-02 · Consultar el histórico de turnos

- **Estado inicial:** parcial. El historial permitía filtrar por empleado.
- **Rama:** `feature/historico-de-turnos`. **Implementación:** `8f07a49`.
- **Componente:** Asistencia → Horarios Temporales.
- **Regla:** incluye los turnos que se cruzan con el rango, con fechas límite incluidas. Cada límite es opcional. Para registros sin fecha de fin se usa la fecha de inicio como fin al filtrar.
- **Verificación técnica:** el proyecto web compiló con cero errores y cero advertencias.
- **Estado:** implementado, pendiente de validación funcional y evidencia de pantalla.

| Caso | Datos de prueba | Resultado esperado | Estado |
| --- | --- | --- | --- |
| CP-RF02-01 | Buscar al empleado A del 10 al 15 de noviembre. Turnos de A del 8 al 10, del 15 al 16 y del 16 al 18. Turno de B del 12 al 13. | Aparecen los primeros dos turnos de A. | Pendiente |
| CP-RF02-02 | Buscar desde el 15 de noviembre con un turno del 15 sin fecha de fin y otro terminado el 14. | Aparece el turno del 15. | Pendiente |
| CP-RF02-03 | Ingresar desde el 16 hasta el 15 de noviembre. | Se muestra el mensaje de rango inválido. | Pendiente |
## Seguimiento T3 · SC-014 (RF-03)

- **Solicitud:** impedir que una persona tenga dos permisos en un mismo día. Antecedente: SC-008.
- **Rama y commit:** `feature/evitar-permisos-duplicados`, `8065889`.
- **Componente:** `ReglasPermiso.Validar`, invocado desde el alta y la edición de permisos.
- **Estado:** implementado. La regla rechaza fechas compartidas incluso si las horas son distintas. En edición se excluye el propio permiso.
- **Pruebas y evidencia:** las 13 pruebas de `ReglasPermisoTests` pasaron, incluido el caso de regresión CP-RF03-01 (mismo empleado y día, de 08:00 a 10:00 y de 15:00 a 16:00). También se verificaron los casos de otro empleado, días consecutivos y edición del mismo registro. Queda pendiente la evidencia funcional en pantalla.
## Seguimiento T3 · SC-015 (RF-04) · Agregar motivos de permiso

- **Estado:** implementación existente, confirmada mediante revisión del código. Pendiente de prueba funcional.
- **Rama de seguimiento:** `feature/agregar-motivos-de-permiso`. **Commit de referencia:** `0f441ee`.
- **Componente:** Permisos → Motivos de permiso → Nuevo. `CatalogoController.Nuevo` requiere el rol Administrador y guarda mediante `CatalogoRepositorio.Crear` en el catálogo `Motivo_Perm`.
- **CP-RF04-01:** en una base de prueba, ingresar como Administrador y crear un motivo con descripción «Motivo de prueba T3» y los demás campos obligatorios válidos. Se espera un mensaje de éxito, el motivo visible en el catálogo tras recargar y su disponibilidad al registrar un permiso.
- **Resultado obtenido y evidencia:** pendientes de ejecución. Adjuntar captura del catálogo y del selector de motivos usando datos de prueba.
## Seguimiento T3 · SC-016 (RF-05) · Eliminar motivos de permiso

- **Estado:** implementado, pendiente de validación funcional con datos de prueba.
- **Rama y commit:** `feature/eliminar-motivos-de-permiso`, `73719bb`.
- **Componentes:** `CatalogoRepositorio.Eliminar` y `CatalogoController.Eliminar`.
- **Regla:** el administrador puede eliminar un motivo sin referencias. Si está asociado a un permiso, se conserva y se muestra un mensaje comprensible.
- **Verificación técnica:** compilación web correcta, cero errores y advertencias.

| Caso | Datos | Resultado esperado | Resultado obtenido y evidencia |
| --- | --- | --- | --- |
| CP-RF05-01 | Motivo de prueba sin permisos asociados | Desaparece del catálogo y se muestra éxito | Pendiente de ejecución |
| CP-RF05-02 | Motivo de prueba asociado a un permiso | El motivo y el permiso se conservan; se muestra el aviso de motivo en uso | Pendiente de ejecución |
## Seguimiento T3 · SC-017 (RF-06) · Validar licencias que se otorgan una sola vez al año

- **Rama y commit:** `feature/licencias-una-vez-al-ano`, `543d70d`.
- **Componente:** `PermisoLogica.Validar` y `ReglaLicenciaAnual`, utilizados al registrar y editar permisos.
- **Regla implementada para revisión:** los motivos 70, 76, 82 y 133 comparten un límite por empleado y año calendario de la fecha de inicio. Se excluye el permiso que se está editando.
- **CP-RF06-01 a CP-RF06-06:** 6 pruebas automatizadas correctas. Cubren repetición del motivo, otro código del grupo, año distinto, empleado distinto, edición del propio registro y un motivo ajeno al grupo.
- **Estado:** implementado según el supuesto indicado. Pendiente de confirmación del área usuaria y de evidencia funcional en la aplicación con una base de prueba.
## Seguimiento T3 · SC-012 (RF-01) · Registrar turnos por periodos de fechas

- **Estado:** implementación existente, verificada en el código. Pendiente de prueba funcional.
- **Rama de seguimiento:** `feature/registro-de-turnos`. **Referencias:** `0f441ee` (alta) y `312f4aa` (validación del formulario).
- **Componente:** Asistencia → Horarios Temporales → Nuevo horario. `AsistenciaController.HorarioTemporalNuevo` guarda el horario asignado y las fechas del empleado en `HorarioTemporal`.
- **CP-RF01-01:** registrar un horario de prueba del 10 al 15 de noviembre. Verificar que aparezca en la lista del empleado con el horario y ambas fechas tras recargar.
- **CP-RF01-02:** intentar registrar el mismo horario con fecha final anterior a la inicial. Verificar que se muestre el mensaje de validación y que el registro no aparezca.
- **Resultado obtenido y evidencia:** pendientes de ejecución con una base de prueba.
## Seguimiento T3 · Teletrabajo

| Solicitud | Rama y commit | Implementación | Caso y evidencia |
| --- | --- | --- | --- |
| SC-024 · RF-13 · Registrar periodos de teletrabajo | `feature/registro-de-teletrabajo`, `88c7765` | Permisos → Registrar teletrabajo. Guarda empleado, fechas y modalidad en `PeriodoTeletrabajo`. | CP-RF13-01: crear un periodo válido, comprobar su persistencia y rechazar fechas invertidas. Pendiente de ejecución con SQL Server. |
| SC-025 · RF-14 · Reportar días de teletrabajo | `feature/reporte-de-dias-de-teletrabajo`, `6e4f31e` | Reporte por empleado y rango. Cuenta días calendario únicos y recorta los periodos al rango. | CP-RF14-01 y CP-RF14-02: recorte de rangos, solapamientos y cero días sin registros. Pruebas automatizadas aprobadas; captura pendiente. |
| SC-027 · RF-16 · Reportar quiénes realizan teletrabajo total o parcial | `feature/reporte-de-teletrabajo-total-y-parcial`, `4c92370` | El reporte permite consultar todos los empleados, muestra el nombre y distingue modalidad total y parcial. | CP-RF16-01: registrar un periodo total para A y uno parcial para B; consultar cada modalidad y comprobar que solo aparezca la persona correspondiente. Compilación correcta; prueba funcional pendiente. |

Antes de la prueba funcional, ejecutar `Scripts/rf-13-crear-periodo-teletrabajo.sql` en una base de prueba. Los motivos históricos 125 y 126 quedan en el catálogo de permisos. El reporte utiliza la tabla nueva y su criterio de días calendario incluye fines de semana y feriados. Validar ese criterio con el área usuaria antes de publicar.

Revisión del repositorio **`sh4ntyfps/Proyecto-Evo_RRHH`**, rama `dev_branch`, commit `a04f97e` (2 de noviembre de 2026).

Este documento explica las 7 solicitudes de cambio que se presentaron en la exposición de la Semana 9 (`Grupo5_EVO-RRHH_Semanas_1a9.pptx` y `temas-semanas/Grupo5_Gestion_del_Cambio.pptx`) y registradas en `docs/GESTION-DEL-CAMBIO.md`. Para cada una se revisó el código y se indica si realmente se cumplió. Sirve como base para la nueva revisión que se hace en el repositorio de despliegue en Azure, donde algunas solicitudes se reabren.

---

## 1. Resumen

| Código | Tipo | Cambio solicitado | Estado declarado (Semana 9) | Estado real en el código | ¿Reabrir? |
|---|---|---|---|---|---|
| SC-001 | Correctivo | Validar fechas, horas y solapamiento al registrar permisos | Aprobado | ❌ **No cumplida** | Sí |
| SC-002 | Preventivo | Bloquear la cuenta tras intentos fallidos de inicio de sesión | Aprobado | ⚠️ **Parcial** (solo límite de peticiones por IP, sin bloqueo de cuenta) | Sí |
| SC-003 | Perfectivo | Exportar el registro de asistencia a CSV | Aprobado | ❌ **No cumplida** | Sí |
| SC-004 | Adaptativo | Leer la cadena de conexión desde variable de entorno en producción | Diferido | ⚠️ **Parcial** (lo permite ASP.NET Core, pero falta configurarlo para Azure) | Sí, por el despliegue |
| SC-005 | Perfectivo | Filtrar y paginar permisos en la base de datos | En evaluación | ❌ **No cumplida** | Sí |
| SC-006 | Perfectivo | Arquitectura en capas con contratos (cambiar de BD solo toca `Capa Datos`) | Implementado | ✅ **Cumplida** | No |
| SC-007 | Perfectivo | Rediseño de la interfaz con Tailwind CSS | Implementado | ✅ **Cumplida**, con matices (convive con Bootstrap) | No, salvo que se pida quitar Bootstrap |

**Balance:** 2 cumplidas, 2 parciales y 3 sin implementar. Las únicas marcadas como "Implementado" en la exposición (SC-006 y SC-007) sí se cumplieron. Las tres aprobadas (SC-001, SC-002 y SC-003) nunca pasaron a implementarse.

---

## 2. Proceso con el que se gestionaron

Según `docs/GESTION-DEL-CAMBIO.md` y la exposición de la Semana 9, todo cambio sigue 8 pasos:

```
Solicitud → Análisis → Evaluación de impacto → Aprobación / rechazo (CCB)
→ Planificación → Implementación (rama + PR) → Verificación (dotnet test) → Registro y cierre
```

- Plantilla de solicitud: `.github/ISSUE_TEMPLATE/solicitud-de-cambio.md`
- Plantilla de PR: `.github/pull_request_template.md` (exige `dotnet test` en verde y no subir credenciales) — *existía en el repositorio anterior; en el actual el control lo hace el pipeline, que no despliega si `dotnet test` falla*
- Convención: una rama por cambio (`feature/SC-NNN-descripcion`) y commits con el código de la SC.

**Observación:** en el historial de Git (56 commits) **ningún commit menciona un código SC-NNN** y no existen ramas `feature/SC-*`. Las solicitudes se documentaron, pero el proceso de trazabilidad (rama → PR → cierre) no se aplicó. Además, la matriz de `docs/GESTION-DEL-CAMBIO.md` no se actualizó después de la exposición.

---

## 3. Detalle por solicitud

### SC-001 · Validar fechas de permisos (Correctivo · Alta)

**Qué se pidió:** al registrar o editar un permiso, validar que la fecha de fin no sea anterior a la de inicio, que las horas de salida y retorno tengan sentido y que no se solape con otro permiso del mismo empleado.

**Capas afectadas:** Lógica, Controllers y Vistas. Impacto medio, riesgo bajo.

**Qué hay en el código:**
- `Capa Logica/Logica/PermisoLogica.cs` solo hereda de `LogicaBase<Permiso>` y agrega `Obtener(...)`. **No tiene ninguna validación.**
- `PermisoController.Nuevo` / `Editar` llaman directamente a `Registrar` / `Modificar` sin revisar fechas.
- La entidad `Permiso` no tiene `[Required]` ni `IValidatableObject`. Las horas (`Hora_Sal`, `Hora_Ret`) son `string?` sin formato.
- `Views/Permiso/Form.cshtml` solo usa `type="date"` y no tiene `min` ni comprobaciones en el cliente.

**Veredicto:** ❌ **No cumplida.** Hoy se puede guardar un permiso con fin antes que inicio o solapado con otro.

**Para cerrarla:** agregar en `PermisoLogica` un método de validación (fin ≥ inicio, retorno > salida, sin solapamiento con los permisos del mismo `IdEmpleado`), mostrar los errores en `ModelState` y añadir pruebas en `Capa Logica.Tests`.

---

### SC-002 · Bloqueo tras intentos fallidos (Preventivo · Alta)

**Qué se pidió:** bloquear la cuenta de usuario después de N intentos fallidos de inicio de sesión.

**Capas afectadas:** todas, incluida la BD (nuevas columnas en `Usuario`). Impacto alto, riesgo alto. El script SQL debe desplegarse antes que el código.

**Qué hay en el código:**
- ✅ Commit `b6e340e`: *Rate limiting* en `Program.cs`. Se aceptan 5 intentos por minuto en el endpoint `login` (política `AddFixedWindowLimiter("login")`) y 100 peticiones por minuto por usuario a nivel global. Al pasarse del límite se responde 429.
- ✅ `SeguridadController.Login` tiene `[EnableRateLimiting("login")]`.
- ❌ La entidad `Usuario` no tiene contador de intentos ni fecha de bloqueo, y no hay script SQL que lo agregue.
- ❌ Cuando la clave es incorrecta, no se registra nada: solo se guarda en la bitácora el acceso correcto (`IniciarSesion`).

**Veredicto:** ⚠️ **Parcial.** El límite de peticiones frena la fuerza bruta rápida, pero **no bloquea la cuenta**: un atacante puede probar 5 claves por minuto sin límite de tiempo, o usar varias IP.

**Para cerrarla:** agregar `IntentosFallidos` y `BloqueadoHasta` a `Usuario` (con script en `Scripts/bd-Normalizada`), incrementarlos en el login fallido, rechazar el acceso mientras dure el bloqueo, registrar los fallos en la bitácora y permitir que el administrador desbloquee la cuenta.

---

### SC-003 · Exportar asistencia a CSV (Perfectivo · Media)

**Qué se pidió:** exportar el registro de asistencia a CSV reutilizando `ExportadorTablas` / `ExportadorCsv` (diseño OCP de la Semana 5).

**Capas afectadas:** Controllers y Vistas. Impacto bajo, riesgo bajo.

**Qué hay en el código:**
- La infraestructura existe: `IExportadorTabla`, `ExportadorCsv` y `ExportadorTablas` en `Capa Logica/Exportacion/`, con pruebas en `ExportacionTests.cs`.
- Solo la usan `EmpleadoController.Exportar` y `CatalogoController.Exportar`.
- `AsistenciaController` **no tiene acción `Exportar`** y su vista no tiene botón de exportación.

**Veredicto:** ❌ **No cumplida.** Es la más fácil de cerrar.

**Para cerrarla:** copiar el patrón de `EmpleadoController.Exportar` en `AsistenciaController` (respetando los filtros de empleado y fechas) y agregar el botón "Exportar CSV" en `Views/Asistencia/Index.cshtml`.

---

### SC-004 · Cadena de conexión por variable de entorno (Adaptativo · Media)

**Qué se pidió:** que en producción la cadena de conexión no se lea de `appsettings.json` sino de una variable de entorno, igual que la clave del administrador (`Seguridad__AdminClave`).

**Estado en la Semana 9:** Diferido.

**Qué hay en el código:**
- `Program.cs` usa `builder.Configuration.GetConnectionString("RRHHNormalizado")`. ASP.NET Core ya permite sobrescribirla con la variable `ConnectionStrings__RRHHNormalizado` o con *Connection strings* en Azure App Service.
- `appsettings.json` solo tiene una cadena local (`Server=.;Trusted_Connection=True`) sin credenciales. ✅
- Hay un cambio local sin confirmar en `appsettings.json`: la clave `RRHHNuevo` pasó a llamarse `RRHHNormalizado` para que coincida con `Program.cs`. **Sin ese cambio, la aplicación del commit actual no encuentra la cadena.**
- No hay validación: si falta la cadena, `GetConnectionString(...)!` pasa `null` y la app falla sin un mensaje claro. Tampoco hay documentación del despliegue ni `appsettings.Production.json`.

**Veredicto:** ⚠️ **Parcial.** Es técnicamente posible gracias al framework, pero no se hizo como cambio controlado. Con el despliegue en Azure, esta solicitud se vuelve obligatoria.

**Para cerrarla:** confirmar el cambio de `appsettings.json`, fallar con un mensaje claro si falta la cadena, configurarla en Azure como *Connection string* (o en Key Vault) y documentarlo en el README.

---

### SC-005 · Filtrar y paginar permisos en la BD (Perfectivo · Baja)

**Qué se pidió:** que `PermisoController.Index` no traiga toda la tabla para filtrar en memoria. Se debe consultar en la BD con un contrato nuevo (`IPermisoRepositorio`), igual que `IAsistenciaRepositorio`. En la Semana 2 se presentó como **deuda técnica**.

**Capas afectadas:** Entidades, Datos, Lógica, Controllers y Vistas. Impacto medio, riesgo medio.

**Qué hay en el código:**
- `PermisoController.Index` sigue haciendo `(await _permisoLogica.Listar()).Where(...).OrderByDescending(...)`, es decir, trae **todos** los permisos y los filtra en memoria.
- No existe `IPermisoRepositorio`.
- La paginación del servidor (`Paginacion.TamanoPagina`) solo se aplica en `EmpleadoController`. La mejora del commit `05268dc` es una paginación **en el navegador** (`site.js`), así que la tabla completa igual viaja desde la BD.

**Veredicto:** ❌ **No cumplida.**

**Para cerrarla:** crear `IPermisoRepositorio.Listar(idEmpleado, pagina, tamano)` en `Capa Entidades/Contratos`, implementarlo en `Capa Datos` con `Where/Skip/Take` sobre EF Core y usarlo desde `PermisoLogica`. Conviene planificarla junto con SC-001 porque las dos modifican `PermisoController`.

---

### SC-006 · Arquitectura en capas con contratos (Perfectivo · Media)

**Qué se pidió:** separar el sistema en capas con interfaces para que cambiar de base de datos solo afecte a `Capa Datos`.

**Qué hay en el código (commits `ed3787d` y `c3c1aa9`):**
- Contratos `IRepositorio<T>`, `IUnidadDeTrabajo` y otros en `Capa Entidades/Contratos`.
- `RepositorioBase<T>` y `LogicaBase<T>` eliminan el CRUD repetido. `ConfiguracionCapaDatos` / `AgregarCapaDatos` es el único lugar que conoce EF Core.
- `Capa Logica.csproj` solo referencia a `Capa Entidades`.
- `ArquitecturaTests` comprueba que Entidades no depende de nadie ni de EF, que Lógica no depende de Datos ni de EF, que las entidades no tienen atributos de mapeo y que los Controllers y las vistas no usan tipos de `Capa Datos`.
- **`dotnet test` ejecutado en esta revisión: 41 de 41 pruebas correctas.**
- Lo demuestra en la práctica que la aplicación ya funciona sobre `RRHH_Normalizado`, que en la Semana 9 era solo un "posible cambio futuro".

**Veredicto:** ✅ **Cumplida.**

---

### SC-007 · Rediseño de la interfaz con Tailwind CSS (Perfectivo · Media)

**Qué se pidió:** rediseñar el login, el inicio y el layout con Tailwind CSS.

**Qué hay en el código (commits `fb070b8` y `f4815e8`):**
- `package.json` con `tailwindcss 3.4.17` y los scripts `css:build` y `css:watch`, además de `tailwind.config.js`.
- Fuente en `Estilos/app.css` y salida compilada en `wwwroot/css/app.css` (versionada, por lo que no hace falta Node para ejecutar la app).
- `_Layout.cshtml` carga `~/css/app.css`.

**Matiz:** Bootstrap 5 y Bootstrap Icons **siguen cargados** en el layout, y unas 75 vistas usan clases de Bootstrap (`btn`, `form-control`, `col-md-*`). Tailwind se usa para darle a Bootstrap el aspecto de la marca, no para reemplazarlo. Está documentado así en `Estilos/app.css`.

**Veredicto:** ✅ **Cumplida** tal como se pidió (login, inicio y layout). Si en la nueva revisión se pide "solo Tailwind", habría que abrir otra SC para quitar Bootstrap, porque no fue parte del alcance original.

---

## 4. Otros cambios hechos sin SC registrada

Estos cambios están en el código pero no tienen SC en la matriz. Conviene regularizarlos en la nueva revisión:

| Cambio | Evidencia | Relación con las SC |
|---|---|---|
| Migración a la BD `RRHH_Normalizado` | `Program.cs`, `Scripts/bd-Normalizada/` | La matriz decía "si se decide usarla, se registra como nueva solicitud" y no se registró |
| Límite de peticiones (100/min global, 5/min en login) | commit `b6e340e` | Cubre parte de SC-002 |
| Paginación en el navegador estilo Google | commit `05268dc` (`site.js`) | Cubre en apariencia SC-005, pero no en la BD |
| Constantes `EstadosEmpleado`, `ConstantesSeguridad`, `Paginacion` | commit `358c807` | Clean Code |
| Correcciones: beneficiarios con N.º 0, error 500 en Permiso/Familiar/Periodo, "Feriados del mes" en 0, clave del admin visible en Ayuda | `docs/CAMBIOS-ARQUITECTURA.md` | Correctivos cerrados antes del proceso |

---

## 5. Recomendación para la nueva revisión (repositorio de Azure)

1. **Reabrir:** SC-001, SC-002, SC-003 y SC-005, con el estado y lo que falta descrito arriba.
2. **Subir la prioridad de SC-004 a Alta:** es requisito para desplegar en Azure (cadena de conexión y `Seguridad__AdminClave` como configuración de App Service).
3. **Cerrar** SC-006 y SC-007 con la evidencia de este documento.
4. **Orden sugerido:** SC-004 → SC-003 (rápida) → SC-001 + SC-005 (juntas, mismo controller) → SC-002 (requiere script de BD antes que el código).
5. **Aplicar la trazabilidad que faltó:** rama `feature/SC-NNN-...`, commits con el código SC, PR con `dotnet test` en verde y actualizar la matriz al cerrar.

---

## 6. Estado en el repositorio de Azure (`AndersonBenYup/RRHH-Gobierno-Regional`, 02/11/2026)

Esta repo partió de un *snapshot* más antiguo que el revisado arriba: tampoco tenía SC-006 (contratos, pruebas) ni SC-007 (Tailwind).
Se siguió la numeración de la exposición de la Semana 11 (`temas-semanas/Grupo5_Semana11_Solicitud_de_Cambio_SC-008 (2).pptx`).
Todos los commits usan el prefijo `SC-NNN:` y cada cierre tiene su etiqueta.

| Código | Resultado | Versión |
|---|---|---|
| SC-001 | Cerrada dentro de SC-008 (fechas, horas y solapamiento de permisos) | `v1.1.0` |
| SC-002 | Cerrada dentro de SC-009 (bloqueo de cuenta por intentos fallidos) | `v1.2.0` |
| SC-003 | Cerrada: exportación de asistencia a CSV con `IExportadorTabla` | `v1.3.0` |
| SC-004 | Cerrada formalmente: cadena por configuración de Azure y error claro si falta | — |
| SC-005 | Cerrada: filtro y paginación de permisos en la BD. Ampliada en `v1.6.0`: paginador reutilizable, paginación en BD de Personal, Capacitación, Familia y Salud, marcaciones y registro diario filtrados en BD, caché de catálogos y health check `/estado` | `v1.3.0` · `v1.6.0` |
| SC-006 | Reimplementada: contratos, `RepositorioBase`/`LogicaBase`, Lógica sin dependencia de Datos y `ArquitecturaTests`. En `v1.6.0`: `EmpleadoController` partido en servicios (18 → 5 dependencias), `IOptions` y sin sufijos `Param` | `v1.3.0` · `v1.6.0` |
| SC-007 | Registro corregido: se había hecho con Bootstrap; el rediseño con Tailwind se hizo en SC-011 | — |
| SC-008 | Cerrada: validaciones en frontend y backend | `v1.1.0` |
| SC-009 | Cerrada: seguridad (clave fuera del repo, cabeceras, política de claves y bloqueo). Permisos por opción (`Rol_Acceso`) en `v1.6.0` | `v1.2.0` · `v1.6.0` |
| SC-010 | Cerrada: xUnit y `dotnet test` en el pipeline (75 pruebas en `v1.4.0`, 126 en `v1.6.0`) | `v1.4.0` |
| SC-011 | Cerrada: rediseño visual institucional con Tailwind junto a Bootstrap; después, animaciones sutiles, Public Sans y transición entre páginas | `v1.5.0` |

Detalle y evidencia de cada una: [`GESTION-DEL-CAMBIO.md`](GESTION-DEL-CAMBIO.md).

**Pendiente después de `v1.6.0`:** paginación en la BD de Historial laboral y del índice de Vivienda.
