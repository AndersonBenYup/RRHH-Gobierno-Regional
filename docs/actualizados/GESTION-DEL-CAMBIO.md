# Gestión del cambio (Semanas 9 y 11)

> Versión actualizada (noviembre 2026). El original está en `docs/GESTION-DEL-CAMBIO.md`. Esta versión agrega el impacto de SC-008 a SC-011, la evidencia de `v2.5.0` y corrige lo que ya no coincide con el repositorio.

Resumen de la exposición `Grupo5_Gestion_del_Cambio.pptx` aplicada a este repositorio.

## Flujo

```
Solicitud ─► Análisis ─► Evaluación de impacto ─► Aprobación / rechazo (CCB)
   ─► Planificación ─► Implementación (commits SC-NNN en main) ─► Verificación (dotnet test en CI + prueba funcional) ─► Registro, tag y cierre
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
| SC-001 | Correctivo | Validar fechas, horas y solapamiento al registrar permisos (`PermisoLogica` hoy no valida) | Permisos | Alta | Cerrada en SC-008 · `v2.1.0` |
| SC-002 | Preventivo | Bloquear la cuenta tras intentos fallidos de inicio de sesión (`SeguridadController.Login` no los limita) | Seguridad | Alta | Cerrada en SC-009 · `v2.2.0` |
| SC-003 | Perfectivo | Exportar el registro de asistencia a CSV reutilizando `ExportadorTablas` / `ExportadorCsv` | Asistencia | Media | Cerrada · `v2.3.0` |
| SC-004 | Adaptativo | Leer la cadena de conexión desde variable de entorno en producción (la clave del admin ya se lee de `Seguridad__AdminClave`) | Configuración | Alta | Cerrada |
| SC-005 | Perfectivo | Filtrar y paginar permisos en la BD (`PermisoController.Index` trae toda la tabla y filtra en memoria) | Permisos | Baja | Cerrada · `v2.3.0` |
| SC-006 | Perfectivo | Arquitectura en capas con contratos: cambiar de BD solo toca `Capa Datos` | Todas las capas | Media | Cerrada (reimplementada) · `v2.3.0` |
| SC-007 | Perfectivo | Rediseño de la interfaz (login, inicio y layout) con Tailwind CSS | Vistas | Media | Cerrada con corrección: se hizo con Bootstrap + `site.css`, sin Tailwind |
| SC-008 | Correctivo | Validar todos los formularios en frontend y backend: DataAnnotations, `ModelState` y reglas de negocio (retoma SC-001) | Todos los módulos | Alta | Cerrada · `v2.1.0` |
| SC-009 | Preventivo | Endurecer la seguridad: clave fuera del repositorio, cabeceras HTTP, política de contraseñas y bloqueo de cuenta (completa SC-002) | Seguridad | Alta | Cerrada · `v2.2.0` |
| SC-010 | Preventivo | Pruebas automatizadas con xUnit y paso `dotnet test` en el pipeline de Azure | Todas las capas / CI | Media | Cerrada · `v2.3.1` |
| SC-011 | Perfectivo | Rediseño visual coherente en todos los módulos con Tailwind CSS junto a Bootstrap (corrige el alcance de SC-007) | Vistas | Media | Cerrada · `v2.4.0` |

**Evidencia de cierre de SC-004:** `appsettings.json` no tiene cadena de conexión; en Azure la Web App define la *Connection string* `RRHHNormalizado` y el *App setting* `Seguridad__AdminClave`. `Program.cs` se detiene con un mensaje claro si falta la cadena.

**Evidencia de cierre de SC-008 (`v2.1.0`):**
- Atributos reutilizables `[Dni]` y `[FechaNoAnteriorA]` en `Capa Entidades/Validacion`; `StringLength` con el largo real de cada columna de `RRHH_Normalizado`; `Required`/`Range` en los campos de los formularios.
- Reglas en la Capa Lógica (`Capa Logica/Reglas`): fechas, horas y solapamiento de permisos (cierra SC-001), DNI de 8 dígitos, RUC de 11 y documento duplicado.
- `ModelState.IsValid` en todos los POST de alta y edición; el catálogo genérico valida las DataAnnotations antes de guardar.
- Vistas con `asp-validation-for`, validación en el navegador (jquery.validate en español, rangos de fechas/horas, DNI) y la misma regla en el servidor (probado con JavaScript desactivado).
- `Scripts/sc-008-auditoria-datos.sql` (solo lectura). En la BD local encontró 40 pares de permisos cruzados, 41 capacitaciones y 11 estudios con fechas invertidas, 5 RUC mal formados y 3 feriados sin motivo.
- Pruebas: `Capa Logica.Tests` (xUnit), 41 en verde.

**Evidencia de cierre de SC-009 (`v2.2.0`, completa SC-002):**
- La clave del administrador ya no está en el repositorio (`appsettings.Development.json`, README ni la página de Ayuda, que la mostraba a cualquiera). Solo se lee de `Seguridad:AdminClave` (user-secrets en local, App setting en Azure).
- Bloqueo de cuenta: 5 intentos fallidos en 15 minutos la bloquean 15 minutos (`ReglasBloqueo`, `BloqueoCuentaLogica`). Los intentos quedan en la bitácora (`LoginFallido`, `CuentaBloqueada`, `LoginBloqueado`, `DesbloquearCuenta`); no hubo cambio de esquema, así que no hace falta un script previo al despliegue. El administrador desbloquea desde *Mantenimiento › Usuarios*.
- Política de contraseñas (`PoliticaClave`): mínimo 8 caracteres, letra y número, sin incluir el login.
- Cabeceras CSP, `X-Frame-Options`, `X-Content-Type-Options`, `Referrer-Policy` y `Permissions-Policy`; cookies `HttpOnly`, `SameSite=Strict` y solo HTTPS en producción.
- Correcciones encontradas al probar: el límite global contaba los archivos estáticos (un usuario recibía 429 a las ~10 páginas), el límite de login era uno solo para todos los usuarios (ahora es por IP) y el rol `ADMINISTRADOR` de la BD no coincidía con `[Authorize(Roles = "Administrador")]`.
- Permisos por opción con `Rol_Acceso`: quedó pendiente en `v2.2.0` y se completó en `v2.5.0` (ver más abajo).
- Pruebas: `ReglasBloqueoTests` y `PoliticaClaveTests` (57 pruebas en verde en total).

**Evidencia de cierre de SC-006, SC-005 y SC-003 (`v2.3.0`):**
- SC-006: contratos `IRepositorio<T>` y específicos en `Capa Entidades/Contratos`; `RepositorioBase<T>` reemplaza las 80 clases `XxxData` duplicadas (−2.900 líneas); `LogicaBase<T>` y `Capa Logica.csproj` sin referencia a `Capa Datos`; `Program.cs` pasa de ~160 `AddScoped` a `AgregarCapaDatos()` + `AgregarCapaLogica()`. `ArquitecturaTests` impide volver a acoplar las capas.
- SC-005: `IPermisoRepositorio.ListarPaginado` filtra y pagina en SQL Server (`OFFSET/FETCH`, 25 por página sobre 26.120 permisos); el siguiente `NPermiso` se calcula con `MAX` en la BD.
- SC-003: `IExportadorTabla` / `ExportadorCsv` / `ExportadorTablas` en `Capa Logica/Exportacion`; `Asistencia/Exportar` con los filtros de la pantalla. La pantalla de asistencia de un mes pasó de más de 30 s a menos de 1 s (el rango se filtra en la BD).
- Pruebas: 75 en verde (`ArquitecturaTests`, `ExportacionTests` y las anteriores).

**Evidencia de cierre de SC-010 (`v2.3.1`):**
- `.github/workflows/main_rrhh-gobierno-regional.yml`: restaura y compila la solución, ejecuta `dotnet test` (75 pruebas, sin SQL Server) y sube el reporte `.trx`. Si falla, no se publica ni se despliega. Los PR a `main` también ejecutan las pruebas.
- Se actualizó `System.Security.Cryptography.Pkcs` a 9.0.21 (vulnerabilidad alta en la 9.0.11).
- `docs/CAMBIOS-ARQUITECTURA.md` reescrito según el código actual.

**Evidencia de cierre de SC-011 (`v2.4.0`):**
- Tailwind 3.4 junto a Bootstrap (prefijo `tw-`, sin preflight): `Estilos/app.css` → `npm run css:build` → `wwwroot/css/app.css` (versionado; la app no necesita Node).
- Sistema visual institucional: azul petróleo y gris piedra, Public Sans, sin degradados; tablas, formularios con errores de validación, badges, paginador, barra lateral y modo oscuro coherentes. Contexto en `PRODUCT.md`.
- Login rediseñado y centrado; gráficos del inicio con la paleta institucional; se eliminó la pantalla de carga que dejaba un logo encima del contenido; los formularios ya no repiten cada error dos veces.
- `node_modules` deja de estar en git (1590 archivos incompletos).
- Revisado en escritorio (1366 px), móvil (390 px, sin desborde horizontal) y modo oscuro.


**Evidencia de `v2.5.0` (cierre del plan de 4 semanas):**
- SC-005 (amplía):
  - paginador reutilizable (`Views/Shared/_Paginador.cshtml` + `site.js`, máximo 7 números, filas por página e "Ir a página");
  - paginación en la BD de Personal, Permisos, Capacitación (3,09 MB → 347 KB), Familia y Salud (`IRepositorio.ListarPagina`);
  - marcaciones (200 184 filas) y registro diario (263 520) filtrados en la BD: de error a los 35 s a ~1 s;
  - caché de catálogos (`CacheCatalogos`) y health check `GET /estado`.
- SC-006 (amplía):
  - `EmpleadoController` pasa de 18 dependencias a 5 (`EmpleadoListadoServicio`, `EmpleadoCatalogosServicio`);
  - `OpcionesSeguridad` con `IOptions`, validada al arrancar;
  - `ILogger` en el login;
  - sin el sufijo `Param` en 81 clases de la Capa Lógica.
- SC-009 (completa), permisos por opción:
  - `ReglasAcceso` y `AccesoModuloFilter` (403);
  - menú filtrado y 12 `SistemaOpcion` sembradas;
  - los administradores de SIA, SIAB y Consejo Regional dejan de ser administradores de RRHH.
- SC-011 (amplía): animaciones sutiles con `prefers-reduced-motion`, tipografía Public Sans y transición entre páginas.
- Corrección de configuración: sin `MultipleActiveResultSets` (la app se colgaba después de las páginas con más datos).
- Pruebas: 126 en verde.

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
| SC-008 | ● | ● | ● | ● | ● | | | Alto | Medio |
| SC-009 | | ● | ● | ● | ● | ● (datos) | ● | Alto | Alto |
| SC-010 | | | | | | | ● | Medio | Bajo |
| SC-011 | | | | | ● | | ● | Medio | Bajo |

- **SC-002 / SC-009**: se planteó agregar columnas a `Usuario`. Al implementarlo, los intentos fallidos se guardaron en la bitácora y **no hubo cambio de esquema**. Los permisos por opción solo insertan filas en `SistemaOpcion` al arrancar.
- **SC-001 y SC-005** modifican el mismo `PermisoController`: se planifican juntos para evitar conflictos de *merge*.
- **SC-005** necesita un contrato nuevo en `Capa Entidades/Contratos` (por ejemplo `IPermisoRepositorio`) con la consulta filtrada, igual que `IAsistenciaRepositorio`.
- **SC-008** afecta los datos heredados: un registro antiguo que no cumple las reglas se debe corregir al editarlo. `Scripts/sc-008-auditoria-datos.sql` (solo lectura) mide cuántos son.
- **SC-009** cambia quién es administrador. Antes de desplegar se confirma en Azure que el rol del usuario `admin` es `ADMINISTRADOR` del sistema `0500000` (o `Administrador`).
- **SC-006** tocó las capas sin cambiar el comportamiento observable. *Mitigación:* las pruebas existentes siguen en verde y `ArquitecturaTests` impide que una capa vuelva a depender de la BD.

## Correcciones cerradas antes de usar este proceso

- Alta de beneficiarios en Familia/Salud guardaba siempre N.º 0 (el segundo fallaba).
- Formulario de Permiso daba error 500 (checkbox `bool?`).

## Posibles cambios futuros (sin solicitud registrada)

- **Paginación en la BD** de Historial laboral (4 tablas en pestañas) y del índice de Vivienda.
- **Desplegables de empleados:** hoy llevan 3 335 opciones en cada formulario (unos 300 KB por página). Se podrían reemplazar por una búsqueda con autocompletado.
- **Permisos por acción** (ver/crear/editar/eliminar) dentro de cada módulo; hoy el permiso es por módulo.

## Buenas prácticas del equipo

- Ningún cambio se integra sin una solicitud registrada (SC-NNN).
- Commits con el prefijo de la SC: `SC-NNN: descripción`. El equipo trabaja directo en `main`, y cada push despliega a Azure solo si `dotnet test` pasa en GitHub Actions. Los *pull requests* también corren las pruebas.
- Un tag por versión con versionado semántico (`v2.0.0` … `v2.5.0`); el detalle está en `CHANGELOG.md`.
- Al cerrar, se actualiza el estado en esta matriz.
