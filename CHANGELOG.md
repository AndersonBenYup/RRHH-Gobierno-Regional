# Changelog

Todos los cambios importantes de EVO-RRHH, el sistema de Recursos Humanos del Gobierno Regional.

El formato sigue [Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/). Las versiones usan [versionado semántico](https://semver.org/lang/es/):
- **MAYOR:** cambio incompatible, como otro esquema de BD u otra arquitectura que obliga a migrar.
- **MENOR:** funcionalidad nueva compatible con la versión anterior.
- **PARCHE:** corrección, pruebas, CI o dependencias, sin funcionalidad nueva.

Cada versión 2.x tiene su tag en este repositorio. El código de cada solicitud de cambio (`SC-NNN`) remite a [`SOLICITUDES-DE-CAMBIO.md`](SOLICITUDES-DE-CAMBIO.md) y a [`docs/actualizados/GESTION-DEL-CAMBIO.md`](docs/actualizados/GESTION-DEL-CAMBIO.md).

## [Sin publicar]

## [2.10.0] - 2026-11-11

### Agregado
- SC-031: **desplegables con buscador**. Encima de cada desplegable de empleado aparece un cuadro «Buscar por nombre o N.º de documento». La búsqueda por nombre no distingue tildes ni mayúsculas. Lo mismo pasa con cualquier desplegable de más de 20 opciones, que se busca por su texto, como distritos o cargos.
  - Funciona en todas las pantallas sin cambiar las vistas: registro de teletrabajo, permisos, reportes, asistencia, historial laboral, familia, etc.
  - Enter elige la primera coincidencia y Esc limpia. La opción elegida nunca se pierde al filtrar.
  - El documento se busca en la BD con `BusquedaEmpleadoLogica` y `GET BuscarEmpleado/PorDocumento`. Solo se devuelven Id de empleado: los documentos no se envían a la página.
  - Un desplegable se excluye con `data-sin-buscador` o se fuerza con `data-buscador`.

### Cambiado
- `<Version>2.10.0</Version>`.

Pruebas: 187 en verde.


## [2.9.2] - 2026-11-11

Corrección de datos SC-030. El código de la aplicación no cambia.

### Corregido
- SC-030: recuperación de **104 984 marcaciones y 1720 asistencias de 2024** que la limpieza de huérfanos de `bd-Datos.sql` borró por tener `idHorario` NULL (`LEFT JOIN ... WHERE h.idHorario IS NULL` también elimina los NULL). Pertenecen a 844 empleados existentes; 151 se habían quedado sin ninguna marcación.

### Agregado
- `Scripts/recuperacion/`: generador reproducible, script de recuperación (transaccional, no duplica y no modifica filas existentes), verificación de solo lectura y reversión controlada.
- Tipo de marcación `M` (marca sin reloj ni horario) y descripciones de N, T y X, marcadas como «por confirmar» por RRHH.
- `docs/actualizados/SC-030-RECUPERACION-MARCACIONES.md`, con causa, evidencia, alcance, procedimiento, criterios de aceptación y acta de conformidad de RRHH. Anexo por empleado en `docs/actualizados/anexos/`.
- `Scripts/comparar-bd-con-carga-03.sql`: comparación de solo lectura de `rrhhdb` con la carga normalizada.

### Cambiado
- `<Version>2.9.2</Version>`.


## [2.9.1] - 2026-11-11

Corrección del cálculo de vacaciones tras revisar los datos reales de `rrhhdb` (solo lectura, `Scripts/verificar-bd-v2.9.0*.sql`).

### Corregido
- RF-07/RF-09 (SC-018, SC-020): la fecha de ingreso ya no depende solo de `PeriodoLaboral`, que existe para 833 de 3338 empleados y, en muchos activos, termina hace años (datos migrados). Un activo usa su último periodo si sigue abierto y, si no, `Empleado.FechaIngreso`; ya no se le trata como cesado por un periodo antiguo cerrado. Un inactivo usa su último periodo cerrado.
- El reporte avisa cuántos empleados se omiten por no tener fecha de ingreso vigente.
- RF-10 (SC-021): los permisos «1/2 JORNADA VACACIONES» están registrados con `Dia = 1` y sin horas; ahora descuentan 0,5 por día por su motivo y no un día completo. El detalle muestra los días que descuenta cada permiso.

Pruebas: 176 en verde.


## [2.9.0] - 2026-11-11

Con esta versión están implementados los 18 RF de la Evaluación T3 (8 obligatorios y 10 complementarios).

### Agregado
- RF-12 (SC-023): menú **Mis vacaciones** para todos los usuarios. Cada uno ve solo el saldo y los goces del empleado vinculado a su cuenta (`Usuario.IdEmpleado`); si la cuenta no está vinculada, se explica cómo hacerlo en «Usuarios y roles».

### Cambiado
- Informe de avance T3 y matriz de solicitudes con SC-018 a SC-023, SC-026 y la ampliación de SC-005.
- `<Version>2.9.0</Version>`.

Pruebas: 166 en verde.

## [2.8.0] - 2026-11-11

Vacaciones (RF-07 a RF-11). Regla verificada con el área usuaria: 30 días calendario por cada año completo de servicio desde la fecha de ingreso; es la misma para los D. Leg. 276, 1057 (Ley 29849) y 713.

### Agregado
- RF-07 (SC-018): vacaciones generadas, 30 días por año cumplido desde `PeriodoLaboral.FechaIngreso` (último periodo del empleado; corregido en 2.9.1).
- RF-08 (SC-019): pendientes de goce = generadas − gozadas. Un saldo negativo indica vacaciones adelantadas.
- RF-09 (SC-020): truncas, 2,5 días por mes completo del año en curso y la parte proporcional de los días restantes. Para un cesado se calculan a su fecha de salida.
- RF-10 (SC-021): un permiso de vacaciones por horas cuenta 0,5 días y debe cubrir al menos media jornada (4 horas); si no, el alta o la edición del permiso muestra el error.
- RF-11 (SC-022): menú **Vacaciones** (módulo Permisos, `0502007`) con fecha de corte, filtro por empleado, cesados, totales, detalle por empleado y exportación CSV.
- `CalculoVacaciones` (reglas puras) y `VacacionesLogica`. Los motivos de vacaciones se reconocen por su descripción («vacación»), sin fijar códigos.

### Cambiado
- `<Version>2.8.0</Version>`.

Pruebas: 166 en verde (21 del cálculo y 5 de la lógica de vacaciones).

## [2.7.0] - 2026-11-11

### Agregado
- RF-15 (SC-026): en el reporte de teletrabajo, días con marcación en los relojes presenciales elegidos junto a los días de teletrabajo del empleado. El conteo de marcaciones se hace en la BD y requiere acceso a Asistencia.

### Cambiado
- Los reportes de ausencias (RF-18) y de teletrabajo (RF-14, RF-15 y RF-16) filtran fechas, motivos, empleado y modalidad en la BD con `IPermisoRepositorio` e `IPeriodoTeletrabajoRepositorio`; antes traían toda la tabla (SC-005).
- `<Version>2.7.0</Version>` en `Proyecto-Evo-RRLL.csproj`.

Pruebas: 138 en verde (3 nuevas comprueban con `ToQueryString()` que EF traduce los filtros a SQL).

## [2.6.0] - 2026-11-11

### Agregado
- RF-02, RF-03, RF-05 y RF-06: historial de turnos por fechas y validaciones de permisos, motivos y licencias.
- RF-13, RF-14 y RF-16: registro y reporte de teletrabajo.
- RF-17 y RF-18: resumen de feriados y reporte de ausencias por periodo.
- Este `CHANGELOG.md`, con la historia del sistema desde la v1.0.0.
- Esquema de la BD normalizada en el repositorio: `Scripts/bd-Normalizada/01-esquema-RRHH_Normalizado.sql`, `NORMALIZACION.md` y un README con el orden de ejecución. Los datos (`03-datos`, 156 MB) quedan fuera por el límite de GitHub.
- `<Version>2.5.0</Version>` en `Proyecto-Evo-RRLL.csproj`: la aplicación sabe qué versión es.

### Cambiado
- Los tags pasan de `v1.1.0`–`v1.6.0` a `v2.1.0`–`v2.5.0`. El cambio a la BD normalizada es incompatible con la v1, así que este repositorio empieza en la 2.0.0. La antigua `v1.4.0` pasa a `v2.3.1` porque era un parche.

## [2.5.0] - 2026-11-02

Cierre del plan de 4 semanas.

### Agregado
- Permisos por opción con `Rol_Acceso` (SC-009): `ReglasAcceso` y `AccesoModuloFilter` (responde 403), menú filtrado según el rol y 12 `SistemaOpcion` sembradas.
- Paginador compacto y reutilizable (`_Paginador.cshtml`) en todas las tablas: hasta 7 números, filas por página e "Ir a página" (SC-005).
- Paginación en la BD de Personal, Permisos, Capacitación (3,09 MB → 347 KB), Familia y Salud (SC-005).
- Caché en memoria de los catálogos pequeños (`CacheCatalogos`) (SC-005).
- Health check `GET /estado` con la conexión a la BD (SC-005).
- Animaciones sutiles en inicio, menú, tablas, tema, login y avisos, que respetan `prefers-reduced-motion`; transición entre páginas (SC-011).

### Cambiado
- `EmpleadoController` pasa de 18 dependencias a 5, con `EmpleadoListadoServicio` y `EmpleadoCatalogosServicio` (SC-006).
- Configuración de seguridad con `IOptions<OpcionesSeguridad>`, validada al arrancar, y `ILogger` en el login (SC-006).
- Se quita el sufijo `Param` de los parámetros en 81 clases de la Capa Lógica (SC-006).
- Tipografía Public Sans (SC-011).
- Documentación del curso alineada con el código en `docs/actualizados/` (SC-010).

### Corregido
- La app se colgaba después de abrir las páginas con más datos: se quitó `MultipleActiveResultSets`.
- Marcaciones (200 184 filas) y registro diario (263 520) se filtran en la BD: antes daban error a los 35 s, ahora tardan ~1 s.

### Seguridad
- Los administradores de otros sistemas (SIA, SIAB, Consejo Regional) dejan de ser administradores de RRHH.

Pruebas: 126 en verde.

## [2.4.0] - 2026-11-02

Rediseño visual (SC-011).

### Agregado
- Tailwind CSS 3.4 junto a Bootstrap (prefijo `tw-`, sin preflight). La fuente está en `Estilos/app.css` y el CSS compilado se versiona, así que la app no necesita Node.
- `PRODUCT.md` con el contexto de diseño: usuarios, restricciones y accesibilidad.

### Cambiado
- Sistema visual institucional (azul petróleo y gris piedra, sin degradados) aplicado a tablas, formularios, badges, barra lateral y modo oscuro.
- Inicio de sesión y gráficos del inicio rediseñados.

### Corregido
- El resumen de validación ya no repite cada error dos veces.
- Se eliminó la pantalla de carga que dejaba el logo encima del contenido.

### Eliminado
- `node_modules` del repositorio (1590 archivos).

## [2.3.1] - 2026-11-02

Pruebas automatizadas en el pipeline (SC-010).

### Agregado
- `dotnet test` en el pipeline de GitHub Actions hacia Azure. Si una prueba falla, no se publica ni se despliega. Los *pull requests* a `main` también ejecutan las pruebas.

### Seguridad
- `System.Security.Cryptography.Pkcs` de 9.0.11 a 9.0.21 (vulnerabilidad alta).

### Cambiado
- `docs/CAMBIOS-ARQUITECTURA.md` reescrito según el código.

## [2.3.0] - 2026-11-02

Arquitectura con contratos (SC-006), paginación en la BD (SC-005) y exportación (SC-003).

### Agregado
- Exportar el registro de asistencia a CSV con los filtros de la pantalla: `IExportadorTabla`, `ExportadorCsv` (SC-003).
- `ArquitecturaTests`: falla si una capa vuelve a depender de otra que no debe (SC-006).

### Cambiado
- Contratos `IRepositorio<T>` y repositorios específicos en `Capa Entidades/Contratos` (SC-006).
- `RepositorioBase<T>` genérico reemplaza las 80 clases `XxxData` duplicadas, con 2900 líneas menos (SC-006).
- `LogicaBase<T>`; la Capa Lógica ya no depende de Capa Datos (SC-006).
- `Program.cs` pasa de ~160 `AddScoped` a `AgregarCapaDatos()` y `AgregarCapaLogica()` (SC-006).
- Los permisos se filtran y paginan en SQL Server con `OFFSET/FETCH` (26 120 permisos) (SC-005).

### Corregido
- La asistencia de un mes tardaba más de 30 s; ahora menos de 1 s porque el rango se filtra en la BD.

Pruebas: 75 en verde.

## [2.2.0] - 2026-11-02

Seguridad (SC-009, cierra SC-002).

### Agregado
- Bloqueo de cuenta: 5 intentos fallidos en 15 minutos la bloquean 15 minutos. Los intentos quedan en la bitácora y el administrador desbloquea desde *Mantenimiento › Usuarios*.
- Política de contraseñas: mínimo 8 caracteres, con letra y número, sin incluir el login.
- Cabeceras de seguridad (CSP, `X-Frame-Options`, `X-Content-Type-Options`, `Referrer-Policy`, `Permissions-Policy`) y cookies `HttpOnly`, `SameSite=Strict` y solo HTTPS.

### Seguridad
- La clave del administrador sale del repositorio, del README y de la página de Ayuda. Ahora se lee de `Seguridad:AdminClave` (user-secrets en local, App setting en Azure).

### Corregido
- El límite de peticiones contaba los archivos estáticos: un usuario recibía 429 a las ~10 páginas.
- El límite de login era uno solo para todos los usuarios; ahora es por IP.
- El rol `ADMINISTRADOR` de la BD no coincidía con `[Authorize(Roles = "Administrador")]`.

Pruebas: 57 en verde.

## [2.1.0] - 2026-11-02

Validaciones en frontend y backend (SC-008, cierra SC-001).

### Agregado
- Atributos reutilizables `[Dni]` y `[FechaNoAnteriorA]`. `StringLength` con el largo real de cada columna y `Required`/`Range` en los formularios.
- Reglas de negocio en la Capa Lógica: fechas, horas y solapamiento de permisos; DNI de 8 dígitos; RUC de 11; documento duplicado.
- `ModelState.IsValid` en todos los POST de alta y edición.
- Validación en el navegador con mensajes en español (`asp-validation-for`, jquery.validate).
- `Scripts/sc-008-auditoria-datos.sql` (solo lectura) para encontrar datos existentes que no cumplen las reglas.
- Proyecto de pruebas `Capa Logica.Tests` (xUnit), con 41 pruebas.

### Corregido
- EF respeta los NULL de la BD aunque la entidad tenga `[Required]`.

## [2.0.0] - 2026-10-28

**Cambio mayor e incompatible con la v1.0.0.** La BD y la arquitectura son otras. Viene de las ramas `dev_branch` y `deploy/azure-normalizada` del repositorio anterior (15 al 23 de octubre).

### Cambiado
- **BD `RRHH_Nuevo` → `RRHH_Normalizado`**, normalizada a 3FN/FNBC (4FN donde aplica). Se corrigieron, entre otras, `Empleado` (violaba 1FN y 3FN), `Familiar` (2FN), `Vivienda` y `Marcacion` (1FN), y `Ubigeo` se separó en `Departamento`, `Provincia` y `Distrito`. `Persona` pasa a ser el centro del modelo. Detalle en `Scripts/bd-Normalizada/NORMALIZACION.md`.
- Arquitectura reestructurada según lo visto en las primeras 9 semanas del curso: cambiar de BD solo toca `Capa Datos`.
- Clase de constantes en lugar de textos sueltos.
- Frontend mejorado y paginación con títulos en los botones y hasta 10 páginas visibles.

### Agregado
- Despliegue continuo a Azure Web App con GitHub Actions y la BD en Azure SQL (`Scripts/azure/02-subir-bd.ps1`).
- Plantilla de solicitud de cambio y material de la Semana 11.

### Seguridad
- La cadena de conexión sale del repositorio. En Azure se define como *Connection string* `RRHHNormalizado`, y `Program.cs` se detiene con un mensaje claro si falta (SC-004).

## [1.0.0] - 2026-10-16

Primera versión completa, en el repositorio anterior [`sh4ntyfps/Proyecto-Evo_RRHH`](https://github.com/sh4ntyfps/Proyecto-Evo_RRHH) (commit `b6e340e`). Usa la BD sin normalizar `RRHH_Nuevo` (76 tablas, `Scripts/bd-evo-RRLL.sql`). Esa versión no tiene tag; el número se asignó después para ordenar la historia.

### Agregado
- **07-sep:** capas Entidades, Datos y Lógica con la capa web ASP.NET Core MVC; scripts de la BD; modo oscuro, buscador, página de Ayuda; diseño con barra lateral y dashboard con gráficos (Chart.js).
- **08-sep:** página 404 personalizada y política de privacidad.
- **09-sep:** módulos Capacitación, Permisos, Historial laboral, Asistencia (marcaciones, registro diario, horarios), Seguridad (usuarios, roles, permisos por opción) y Empleado (ficha completa con foto).
- **10 al 11-sep:** módulos Salud y Familia.
- **17-sep:** alta de empleados; módulos Vivienda, Feriados, Estructura orgánica, Comportamiento, Inconvenientes y Catálogos; bitácora de actividad; exportación de catálogos y empleados a CSV.
- **16-oct:** límite de peticiones: 100 por minuto por usuario y 5 por minuto en el login (responde 429).

### Cambiado
- **17-sep:** IDs generados bajo transacción serializable con reintentos; paginación de empleados; dashboard sin cargar toda la tabla de asistencias; filtro global de excepciones con log.
- **21 al 24-sep:** la Capa Lógica se conecta a los controllers; escape de campos CSV centralizado.

### Seguridad
- **17-sep:** validación de catálogos, roles de administrador y clave de seed configurable.

[Sin publicar]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.10.0...HEAD
[2.10.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.9.2...v2.10.0
[2.9.2]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.9.1...v2.9.2
[2.9.1]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.9.0...v2.9.1
[2.9.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.8.0...v2.9.0
[2.8.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.7.0...v2.8.0
[2.7.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.6.0...v2.7.0
[2.6.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.5.0...v2.6.0
[2.5.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.4.0...v2.5.0
[2.4.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.3.1...v2.4.0
[2.3.1]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.3.0...v2.3.1
[2.3.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.2.0...v2.3.0
[2.2.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.1.0...v2.2.0
[2.1.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/compare/v2.0.0...v2.1.0
[2.0.0]: https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/releases/tag/v2.0.0
[1.0.0]: https://github.com/sh4ntyfps/Proyecto-Evo_RRHH/tree/b6e340e42e6f2f052d9bde185d60a04faec595a4
