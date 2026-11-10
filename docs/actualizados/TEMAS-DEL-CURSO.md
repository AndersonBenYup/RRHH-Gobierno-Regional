# Temas del curso aplicados en EVO-RRHH

> Versión actualizada (noviembre 2026). Reemplaza a `docs/TEMAS-DEL-CURSO.md`, que venía del repositorio anterior y citaba
> clases que aquí no existen (`Numeracion`, `FichaSocialLogica`, `PersonaExtensiones`, `IUnidadDeTrabajo`, pruebas con SQLite).
> Cada nombre de este documento se comprobó en el código.

**Curso:** Evolución y Configuración de Software, semanas 2 a 11.
**Proyecto:** sistema web de RR.HH. de un gobierno regional. Es ASP.NET Core MVC sobre .NET 10 y EF Core 10, con la base de datos en Azure SQL (`RRHH_Normalizado`).

| Semana | Tema | Dónde está la evidencia |
|---|---|---|
| 2 | Mantenimiento del software | [Tipos aplicados](#semana-2--mantenimiento-del-software) |
| 3 | Ingeniería inversa | [`INGENIERIA-INVERSA.md`](INGENIERIA-INVERSA.md) |
| 4 | Refactorización, SRP y DRY | [Antes y después](#semana-4--refactorización-srp-y-dry) |
| 5 | Principio Abierto-Cerrado | [Exportación y paginador](#semana-5--principio-abierto-cerrado) |
| 7 | Inversión de Control | [Contratos e inyección](#semana-7--inversión-de-control) |
| 8 | Gestión de la configuración | [`GESTION-CONFIGURACION.md`](GESTION-CONFIGURACION.md) |
| 9 | Gestión del cambio | [`GESTION-DEL-CAMBIO.md`](GESTION-DEL-CAMBIO.md), con la plantilla `.github/ISSUE_TEMPLATE/solicitud-de-cambio.md` |
| 11 | Taller de solicitudes de cambio | [Semana 11](#semana-11--taller-de-gestión-de-cambios), basada en `temas-semanas/Grupo5_Semana11_Solicitud_de_Cambio_SC-008 (2).pptx` |
| — | Clean Code, arquitectura y pruebas | [Arquitectura](#arquitectura-por-capas) · [Pruebas](#pruebas) |

---

## Arquitectura por capas

```
Proyecto-Evo-RRLL (controllers, vistas, servicios web) ──► Capa Logica ──► Capa Entidades (entidades + contratos)
          │                                                                       ▲
          └──────────────────────► Capa Datos (EF Core, SQL Server) ──────────────┘ implementa los contratos
```

- **Regla de dependencia:** todo apunta a `Capa Entidades`. `Capa Logica.csproj` no referencia a `Capa Datos` ni a EF Core.
- **Composición en `Program.cs`:**
  - `AgregarCapaDatos(cadena)` (`Capa Datos/ConfiguracionCapaDatos.cs`);
  - `AgregarCapaLogica()` (`Capa Logica/ConfiguracionCapaLogica.cs`).
- **`ArquitecturaTests` comprueba en cada `dotnet test`:**
  - Entidades no depende de ninguna capa ni de EF;
  - Lógica no depende de Datos ni de EF;
  - los contratos viven en `Capa Entidades`;
  - toda clase `*Logica` se puede crear desde el contenedor;
  - ningún controller usa tipos de `Capa Datos`.

---

## Semana 2 – Mantenimiento del software

Cada tipo de mantenimiento tiene al menos una solicitud de cambio real, implementada y etiquetada:

| Tipo | Solicitud | Qué se hizo |
|---|---|---|
| Correctivo | SC-008 (`v2.1.0`) | Validación en navegador y servidor de todos los formularios. Las reglas de permisos (fechas, horas, solapamiento) cierran SC-001 |
| Correctivo | `v2.5.0` | La app se colgaba después de abrir las páginas con más datos (`MultipleActiveResultSets`). Marcaciones daba error a los 35 s |
| Adaptativo | SC-004 | La cadena de conexión y la clave del admin salen de la configuración de Azure, no del código |
| Perfectivo | SC-003, SC-005, SC-011 | Exportación a CSV, paginación en la BD y rediseño visual |
| Preventivo | SC-009, SC-010 | Bloqueo de cuenta, cabeceras de seguridad, permisos por opción, y `dotnet test` en el pipeline |

**Deuda técnica conocida:**
- Historial laboral y el índice de Vivienda aún traen las tablas completas.
- Los formularios cargan la lista entera de empleados (3 335) en un desplegable.

---

## Semana 3 – Ingeniería inversa

Ver [`INGENIERIA-INVERSA.md`](INGENIERIA-INVERSA.md). Contiene:
- el reconocimiento funcional;
- la BD: 88 tablas, 84 claves foráneas y 6 vistas;
- un diagrama E-R inferido;
- un diccionario de datos de las tablas clave;
- la matriz módulos ↔ tablas;
- los hallazgos.

---

## Semana 4 – Refactorización, SRP y DRY

| Antes (problema) | Después (solución) | Principio |
|---|---|---|
| 80 clases `XxxData` repetían el mismo CRUD | `RepositorioBase<T>` (`Capa Datos/RepositorioBase.cs`): el CRUD se escribe una sola vez. Solo hay repositorios propios donde hay consultas específicas (`Capa Datos/Repositorios`) | DRY |
| 77 clases `XxxLogica` repetían Listar/Crear/Actualizar/Eliminar | `LogicaBase<T>` (`Capa Logica/LogicaBase.cs`) | DRY |
| `EmpleadoController` con 18 dependencias y 321 líneas: listado, exportación, catálogos y detalle | 5 dependencias y 184 líneas. `Services/EmpleadoListadoServicio.cs` arma el listado y la exportación; `Services/EmpleadoCatalogosServicio.cs` maneja los catálogos y el detalle | SRP |
| Validaciones repetidas o ausentes en los controllers | `Capa Logica/Reglas/ReglasPermiso.cs` y `ReglasPersona.cs`, más los atributos `[Dni]` y `[FechaNoAnteriorA]` (`Capa Entidades/Validacion`) | SRP y DRY |
| Paginación escrita a mano en cada vista | `Views/Shared/_Paginador.cshtml` con `Capa Logica/Paginacion/VentanaPaginas.cs` | DRY |
| `Program.cs` con unos 160 `AddScoped` | `AgregarCapaDatos()` y `AgregarCapaLogica()` registran todo por convención | SRP |

---

## Semana 5 – Principio Abierto-Cerrado

**Exportación** (`Capa Logica/Exportacion/Exportadores.cs`):
- `IExportadorTabla` es el contrato, `ExportadorCsv` la implementación y `ExportadorTablas` elige por formato.
- Agregar Excel o PDF es una clase nueva más una línea en `AgregarCapaLogica`; ningún controller cambia.
- `ExportacionTests` lo demuestra con un exportador TSV de prueba (`ExportadorTsv`).

**Registro por convención:** una tabla nueva solo necesita su entidad y su `XxxLogica`. `RepositorioBase<>` se registra como genérico abierto y las clases `*Logica` se descubren por reflexión.

**Paginación:** cualquier listado se pagina en la BD con `IRepositorio<T>.ListarPagina(filtro, orden, página, tamaño)` y se dibuja con `_Paginador`, sin modificar la base.

**Permisos por opción:** un módulo nuevo es una línea en `ReglasAcceso.Modulos`. El filtro, el menú y la pantalla de permisos lo toman automáticamente.

---

## Semana 7 – Inversión de Control

- **Contratos** en `Capa Entidades/Contratos`:
  - `IRepositorio<T>`;
  - `IPermisoRepositorio`, `IPersonaRepositorio`, `IUsuarioRepositorio`, `IAsistenciaRepositorio`;
  - `IMarcacionRepositorio`, `IRegAsisDiarioRepositorio`;
  - `IBitacoraRepositorio`, `ISecuenciaService`, `ICatalogoRepositorio`.
- **La lógica recibe contratos por constructor:**
  - `PermisoLogica(IPermisoRepositorio)`;
  - `BloqueoCuentaLogica(IBitacoraRepositorio, TimeProvider, OpcionesSeguridad)`;
  - `AccesoLogica(IRepositorio<Rol_Acceso>)`.

  El contenedor de ASP.NET Core entrega la implementación.
- **El tiempo se inyecta** (`TimeProvider`), así el bloqueo de cuenta se prueba sin esperar 15 minutos.
- **La configuración se inyecta** con `IOptions<OpcionesSeguridad>` (sección `Seguridad`): clave inicial, intentos y minutos de bloqueo, validados al arrancar.
- **Pruebas sin base de datos:** `OpcionesSeguridadTests` usa una bitácora en memoria y un reloj fijo.

---

## Semana 8 – Gestión de la configuración

Ver [`GESTION-CONFIGURACION.md`](GESTION-CONFIGURACION.md): elementos de configuración, repositorio, convención de commits `SC-NNN:`, tags `v2.1.0`–`v2.5.0`, pipeline y secretos.

## Semana 9 – Gestión del cambio

Ver [`GESTION-DEL-CAMBIO.md`](GESTION-DEL-CAMBIO.md): flujo, roles, formato, matriz SC-001 a SC-011 con su evidencia y análisis de impacto.

## Semana 11 – Taller de gestión de cambios

**Registro de solicitudes** (pptx de la Semana 11):

| Código | Solicitud | Tipo | Prioridad | Decisión | Estado |
|---|---|---|---|---|---|
| SC-008 | Validaciones en frontend y backend (retoma SC-001) | Correctivo | Alta | Aprobada con condiciones | Cerrada, `v2.1.0` |
| SC-009 | Seguridad: clave fuera del repo, cabeceras, política de claves y bloqueo (completa SC-002) | Preventivo | Alta | Aprobada | Cerrada, `v2.2.0`; permisos por opción en `v2.5.0` |
| SC-010 | Pruebas xUnit y `dotnet test` en el pipeline | Preventivo | Media | Aprobada | Cerrada, `v2.3.1` (126 pruebas) |

**Solicitud principal:** SC-008. Se eligió porque era la brecha más grande del diagnóstico: solo había 2 DataAnnotations y `ModelState` se revisaba en 15 de 89 POST.

**Condiciones del comité y cómo se cumplieron:**

| Condición | Cómo se cumplió |
|---|---|
| Trabajar por módulos | Empezó por Empleado, Permiso y Asistencia, y luego siguió con el resto |
| Sin cambios de esquema | Solo DataAnnotations y reglas en código |
| Auditar los datos antes | `Scripts/sc-008-auditoria-datos.sql`, de solo lectura |
| Una prueba por regla | `ReglasPermisoTests` (12), `ReglasPersonaTests` (9) y `AtributosValidacionTests` (6) |

**Plan ejecutado:** el de 4 semanas de `docs/entregables`, con su estado actualizado en [`entregables/`](entregables/).

---

## Clean Code

- Nombres del dominio en español y consistentes: `Listar`, `Obtener`, `Crear`, `Actualizar`, `Eliminar`, `Validar`, `ListarPagina`.
- Sin sufijos de ruido en los parámetros: en `v2.5.0` se quitó `...Param` de 81 clases. Por ejemplo, `IdEmpleadoParam` pasó a `idEmpleado`.
- Reglas de negocio como funciones puras y cortas (`ReglasPermiso`, `ReglasPersona`, `ReglasBloqueo`, `ReglasAcceso`, `PoliticaClave`, `VentanaPaginas`) que se prueban sin base de datos.

## Pruebas

`Capa Logica.Tests` (xUnit) tiene **126 pruebas**, que corren en cada push con GitHub Actions y no necesitan SQL Server:

| Archivo | Qué cubre |
|---|---|
| `Validacion/ReglasPermisoTests.cs` | Fechas, horas y solapamiento de permisos |
| `Validacion/ReglasPersonaTests.cs` | DNI, RUC y documento duplicado |
| `Validacion/AtributosValidacionTests.cs` | `[Dni]`, `[FechaNoAnteriorA]` y largos |
| `Seguridad/SeguridadTests.cs` | Bloqueo de cuenta y política de contraseñas |
| `Seguridad/ReglasAccesoTests.cs` | Permisos por opción y quién es administrador de RRHH |
| `Seguridad/OpcionesSeguridadTests.cs` | Configuración del bloqueo con `IOptions` |
| `Paginacion/VentanaPaginasTests.cs` | Números del paginador (máximo 7) |
| `Datos/CacheCatalogosTests.cs` | Caché e invalidación de catálogos |
| `Datos/ModeloEfTests.cs` | Nulabilidad del modelo de EF frente a la BD |
| `Exportacion/ExportacionTests.cs` | CSV (BOM y fórmulas) y extensión OCP |
| `Arquitectura/ArquitecturaTests.cs` | Reglas de dependencia entre capas |
