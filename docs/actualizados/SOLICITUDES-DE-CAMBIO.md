> Versión actualizada (noviembre 2026), con el cierre de `v1.6.0` en la sección 6. El original está en la raíz del repositorio (`SOLICITUDES-DE-CAMBIO.md`).

# Solicitudes de cambio (SC-001 a SC-007): estado real

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
