# Gestión del cambio (Semana 9)

Resumen de la exposición `Grupo5_Gestion_del_Cambio.pptx` aplicada a este repositorio.

## Flujo

```
Solicitud ─► Análisis ─► Evaluación de impacto ─► Aprobación / rechazo (CCB)
   ─► Planificación ─► Implementación (rama + PR) ─► Verificación (dotnet test + prueba funcional) ─► Registro y cierre
```

| Rol | Función |
|---|---|
| Solicitante | Usuario, cliente o integrante que detecta la necesidad y registra la solicitud |
| Gestor de cambios | Revisa las solicitudes, coordina el análisis, mantiene la matriz y comunica las decisiones |
| Comité de Control de Cambios (CCB) | Jefe de proyecto, líder técnico y representante del usuario: aprueba, rechaza o difiere |
| Analista / Líder técnico | Evalúa el impacto: componentes, base de datos, esfuerzo, riesgos y dependencias |
| Desarrollador | Implementa el cambio aprobado en su rama y abre el *pull request* |
| QA / Tester | Verifica con pruebas funcionales y de regresión antes de integrar |
| Gestor de configuración | Controla ramas y versiones; integra a `main` y etiqueta la versión |

## Formato de solicitud de cambio

Plantilla en GitHub: `.github/ISSUE_TEMPLATE/solicitud-de-cambio.md`.

| Campo | Contenido |
|---|---|
| Código | SC-NNN |
| Solicitante / fecha | |
| Tipo | Correctivo · Adaptativo · Perfectivo · Preventivo |
| Descripción | Qué se pide y por qué |
| Módulos y capas afectadas | |
| Impacto | Alto · Medio · Bajo, con justificación |
| Decisión | Aprobado · Rechazado · Diferido |
| Verificación | Pruebas que demuestran el cambio |

## Matriz de cambios del proyecto

| Código | Tipo | Descripción | Módulo | Prioridad | Estado |
|---|---|---|---|---|---|
| SC-001 | Correctivo | Validar fechas, horas y solapamiento al registrar permisos (`PermisoLogica` hoy no valida) | Permisos | Alta | Cerrada en SC-008 · `v1.1.0` |
| SC-002 | Preventivo | Bloquear la cuenta tras intentos fallidos de inicio de sesión (`SeguridadController.Login` no los limita) | Seguridad | Alta | Cerrada en SC-009 · `v1.2.0` |
| SC-003 | Perfectivo | Exportar el registro de asistencia a CSV reutilizando `ExportadorTablas` / `ExportadorCsv` | Asistencia | Media | Cerrada · `v1.3.0` |
| SC-004 | Adaptativo | Leer la cadena de conexión desde variable de entorno en producción (la clave del admin ya se lee de `Seguridad__AdminClave`) | Configuración | Alta | Cerrada |
| SC-005 | Perfectivo | Filtrar y paginar permisos en la BD (`PermisoController.Index` trae toda la tabla y filtra en memoria) | Permisos | Baja | Cerrada · `v1.3.0` |
| SC-006 | Perfectivo | Arquitectura en capas con contratos: cambiar de BD solo toca `Capa Datos` | Todas las capas | Media | Cerrada (reimplementada) · `v1.3.0` |
| SC-007 | Perfectivo | Rediseño de la interfaz (login, inicio y layout) con Tailwind CSS | Vistas | Media | Cerrada con corrección: se hizo con Bootstrap + `site.css`, sin Tailwind |
| SC-008 | Correctivo | Validar todos los formularios en frontend y backend: DataAnnotations, `ModelState` y reglas de negocio (retoma SC-001) | Todos los módulos | Alta | Cerrada · `v1.1.0` |
| SC-009 | Preventivo | Endurecer la seguridad: clave fuera del repositorio, cabeceras HTTP, política de contraseñas y bloqueo de cuenta (completa SC-002) | Seguridad | Alta | Cerrada · `v1.2.0` |
| SC-010 | Preventivo | Pruebas automatizadas con xUnit y paso `dotnet test` en el pipeline de Azure | Todas las capas / CI | Media | Aprobado (Semana 4) |
| SC-011 | Perfectivo | Rediseño visual coherente en todos los módulos con Tailwind CSS junto a Bootstrap (corrige el alcance de SC-007) | Vistas | Media | Registrada |

**Evidencia de cierre de SC-004:** `appsettings.json` no tiene cadena de conexión; en Azure la Web App define la *Connection string* `RRHHNormalizado` y el *App setting* `Seguridad__AdminClave`. `Program.cs` se detiene con un mensaje claro si falta la cadena.

**Evidencia de cierre de SC-008 (`v1.1.0`):**
- Atributos reutilizables `[Dni]` y `[FechaNoAnteriorA]` en `Capa Entidades/Validacion`; `StringLength` con el largo real de cada columna de `RRHH_Normalizado`; `Required`/`Range` en los campos de los formularios.
- Reglas en la Capa Lógica (`Capa Logica/Reglas`): fechas, horas y solapamiento de permisos (cierra SC-001), DNI de 8 dígitos, RUC de 11 y documento duplicado.
- `ModelState.IsValid` en todos los POST de alta y edición; el catálogo genérico valida las DataAnnotations antes de guardar.
- Vistas con `asp-validation-for`, validación en el navegador (jquery.validate en español, rangos de fechas/horas, DNI) y la misma regla en el servidor (probado con JavaScript desactivado).
- `Scripts/sc-008-auditoria-datos.sql` (solo lectura). En la BD local encontró 40 pares de permisos cruzados, 41 capacitaciones y 11 estudios con fechas invertidas, 5 RUC mal formados y 3 feriados sin motivo.
- Pruebas: `Capa Logica.Tests` (xUnit), 41 en verde.

**Evidencia de cierre de SC-009 (`v1.2.0`, completa SC-002):**
- La clave del administrador ya no está en el repositorio (`appsettings.Development.json`, README ni la página de Ayuda, que la mostraba a cualquiera). Solo se lee de `Seguridad:AdminClave` (user-secrets en local, App setting en Azure).
- Bloqueo de cuenta: 5 intentos fallidos en 15 minutos la bloquean 15 minutos (`ReglasBloqueo`, `BloqueoCuentaLogica`). Los intentos quedan en la bitácora (`LoginFallido`, `CuentaBloqueada`, `LoginBloqueado`, `DesbloquearCuenta`); no hubo cambio de esquema, así que no hace falta un script previo al despliegue. El administrador desbloquea desde *Mantenimiento › Usuarios*.
- Política de contraseñas (`PoliticaClave`): mínimo 8 caracteres, letra y número, sin incluir el login.
- Cabeceras CSP, `X-Frame-Options`, `X-Content-Type-Options`, `Referrer-Policy` y `Permissions-Policy`; cookies `HttpOnly`, `SameSite=Strict` y solo HTTPS en producción.
- Correcciones encontradas al probar: el límite global contaba los archivos estáticos (un usuario recibía 429 a las ~10 páginas), el límite de login era uno solo para todos los usuarios (ahora es por IP) y el rol `ADMINISTRADOR` de la BD no coincidía con `[Authorize(Roles = "Administrador")]`.
- Pendiente (fuera del alcance): permisos por opción con `Rol_Acceso`.
- Pruebas: `ReglasBloqueoTests` y `PoliticaClaveTests` (57 pruebas en verde en total).

**Evidencia de cierre de SC-006, SC-005 y SC-003 (`v1.3.0`):**
- SC-006: contratos `IRepositorio<T>` y específicos en `Capa Entidades/Contratos`; `RepositorioBase<T>` reemplaza las 80 clases `XxxData` duplicadas (−2.900 líneas); `LogicaBase<T>` y `Capa Logica.csproj` sin referencia a `Capa Datos`; `Program.cs` pasa de ~160 `AddScoped` a `AgregarCapaDatos()` + `AgregarCapaLogica()`. `ArquitecturaTests` impide volver a acoplar las capas.
- SC-005: `IPermisoRepositorio.ListarPaginado` filtra y pagina en SQL Server (`OFFSET/FETCH`, 25 por página sobre 26.120 permisos); el siguiente `NPermiso` se calcula con `MAX` en la BD.
- SC-003: `IExportadorTabla` / `ExportadorCsv` / `ExportadorTablas` en `Capa Logica/Exportacion`; `Asistencia/Exportar` con los filtros de la pantalla. La pantalla de asistencia de un mes pasó de más de 30 s a menos de 1 s (el rango se filtra en la BD).
- Pruebas: 75 en verde (`ArquitecturaTests`, `ExportacionTests` y las anteriores).




## Análisis de impacto

| SC | Entidades | Datos | Lógica | Controllers | Vistas | BD | Config. | Impacto | Riesgo |
|---|:-:|:-:|:-:|:-:|:-:|:-:|:-:|---|---|
| SC-001 | | | ● | ● | ● | | | Medio | Bajo |
| SC-002 | ● | ● | ● | ● | ● | ● | | Alto | Alto |
| SC-003 | | | | ● | ● | | | Bajo | Bajo |
| SC-004 | | | | | | | ● | Medio | Medio |
| SC-005 | ● | ● | ● | ● | ● | | | Medio | Medio |
| SC-006 | ● | ● | ● | ● | | | ● | Alto | Medio |
| SC-007 | | | | | ● | | ● | Medio | Bajo |

- **SC-002** es el más crítico: agrega columnas a la tabla `Usuario`, así que el script SQL se despliega antes que el código.
- **SC-001 y SC-005** modifican el mismo `PermisoController`: se planifican juntos para evitar conflictos de *merge*.
- **SC-005** necesita un contrato nuevo en `Capa Entidades/Contratos` (por ejemplo `IPermisoRepositorio`) con la consulta filtrada, igual que `IAsistenciaRepositorio`.
- **SC-006** tocó las capas sin cambiar el comportamiento observable. *Mitigación:* las pruebas existentes siguen en verde y `ArquitecturaTests` impide que una capa vuelva a depender de la BD.

## Correcciones cerradas antes de usar este proceso

- Alta de beneficiarios en Familia/Salud guardaba siempre N.º 0 (el segundo fallaba).
- Formulario de Permiso daba error 500 (checkbox `bool?`).

## Posibles cambios futuros (sin solicitud registrada)

- Base normalizada `RRHH_Normalizado` (`Scripts/bd-Normalizada`): solo está preparada como opción. Si se decide usarla, se registra como nueva solicitud; gracias a SC-006 solo cambiarían `Capa Datos` y la cadena de conexión.

## Buenas prácticas del equipo

- Ningún cambio se integra sin solicitud registrada y *pull request* revisado.
- Un cambio por rama (`feature/SC-NNN-descripcion`) y commits con el código de la SC.
- `dotnet test` debe pasar antes de integrar.
- Al cerrar, se actualiza el estado en esta matriz.
