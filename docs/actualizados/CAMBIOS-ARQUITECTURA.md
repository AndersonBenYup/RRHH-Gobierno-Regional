# Cambios en la arquitectura

> Versión actualizada (noviembre 2026), con lo agregado en `v1.6.0`. El original está en `docs/CAMBIOS-ARQUITECTURA.md`.

Qué hay en el código hoy (repositorio de despliegue en Azure), por qué y en qué solicitud de cambio se hizo.
Este documento reemplaza la versión heredada del repositorio anterior, que mencionaba piezas que aquí no
existían (SC-010: "alinear la documentación con el código").

## Capas e interfaces (DIP / IoC) — SC-006

```
Proyecto-Evo-RRLL (web) ──► Capa Logica ──► Capa Entidades (entidades + contratos)
          │                                        ▲
          └──────────► Capa Datos (EF Core) ───────┘
```

- **Contratos** en `Capa Entidades/Contratos`: `IRepositorio<T>` y los específicos `IPermisoRepositorio`,
  `IPersonaRepositorio`, `IUsuarioRepositorio`, `IAsistenciaRepositorio`, `IBitacoraRepositorio`,
  `ISecuenciaService` e `ICatalogoRepositorio`.
- **`RepositorioBase<T>`** (`Capa Datos`) implementa el CRUD una sola vez. Reemplazó a las 80 clases `XxxData`
  que repetían el mismo código. Solo hay repositorios propios donde hay consultas específicas (`Capa Datos/Repositorios`).
- **`ConfiguracionCapaDatos.AgregarCapaDatos()`** es el único lugar que conoce EF Core y SQL Server.
  Cambiar de base de datos solo toca `Capa Datos`.
- **`LogicaBase<T>`** (`Capa Logica`) recibe `IRepositorio<T>`. `Capa Logica.csproj` **no referencia** a `Capa Datos`.
- **`ConfiguracionCapaLogica.AgregarCapaLogica()`** registra todas las clases `*Logica`.
- **Controllers delgados:** `EmpleadoController` pasó de 18 dependencias a 5. El listado y la exportación están en `Services/EmpleadoListadoServicio`; los catálogos del formulario y el detalle, en `Services/EmpleadoCatalogosServicio`.
- **Configuración con `IOptions`:** `OpcionesSeguridad` (sección `Seguridad`) se valida al arrancar y llega a `BloqueoCuentaLogica` por constructor.
  `Program.cs` pasó de ~160 `AddScoped` a dos líneas.
- **`ArquitecturaTests`** falla si Entidades o Lógica dependen de Datos o de EF, si un controller usa `Capa_Datos`
  o si alguna clase no se puede crear desde el contenedor.

## Validaciones — SC-008 (retoma SC-001)

- Atributos reutilizables en `Capa Entidades/Validacion`: `[Dni]` (8 dígitos) y `[FechaNoAnteriorA]`.
- `StringLength` en las entidades con el largo real de cada columna de `RRHH_Normalizado`; `Required`/`Range`
  en los campos de los formularios. `EvoRRLDbContext` mantiene anulables las columnas que lo son en la BD.
- Reglas de negocio puras en `Capa Logica/Reglas` (`ReglasPermiso`, `ReglasPersona`), expuestas como
  `PermisoLogica.Validar` y `PersonaLogica.Validar`.
- `ModelState.IsValid` en todos los POST de alta y edición. Los mensajes de las reglas se pasan con
  `ModelStateExtensiones.AgregarErrores`.
- En el navegador: `asp-validation-for`, jquery.validate en español (`wwwroot/js/validacion-es.js`) y rangos de
  fechas y horas en `site.js`. El servidor repite cada regla (funciona con JavaScript desactivado).

## Seguridad — SC-009 (completa SC-002)

- La clave del admin solo viene de la configuración (`Seguridad:AdminClave`).
- Bloqueo de cuenta con `ReglasBloqueo` y `BloqueoCuentaLogica`, usando los eventos de la bitácora (sin cambio de esquema).
- `PoliticaClave`, cabeceras de seguridad y cookies endurecidas en `Program.cs`. Límites de peticiones por usuario y por IP.
- **Permisos por opción (`v1.6.0`):**
  - `Capa Logica/Seguridad/ReglasAcceso` decide quién es administrador de RRHH y qué módulos ve cada rol (`Rol_Acceso`);
  - `AccesoLogica` lo calcula al iniciar sesión y deja un claim por módulo;
  - `Filtros/AccesoModuloFilter` responde 403 (`Views/Shared/SinAcceso`);
  - el menú usa `User.PuedeVer(...)` (`Seguridad/AccesoModulos.cs`);
  - las 12 opciones de módulo se crean al arrancar si faltan.
- `ILogger` registra en el login los accesos correctos, los fallidos y los bloqueos.

## Abierto a la extensión (OCP) — SC-003

- `IExportadorTabla`, `ExportadorCsv` y `ExportadorTablas` en `Capa Logica/Exportacion`. Los usan Empleado,
  Catálogo y Asistencia. Un formato nuevo (Excel, PDF) es otra clase registrada en `AgregarCapaLogica`.

## Escalabilidad — SC-005 y SC-003

- Paginación genérica en la BD: `IRepositorio<T>.ListarPagina(filtro, orden, página, tamaño)` (`OFFSET/FETCH`). Se usa en Personal, Permisos, Capacitación, Familia y Salud.
- Paginador reutilizable:
  - `Views/Shared/_Paginador.cshtml` en el servidor y `site.js` en el navegador generan el mismo HTML;
  - máximo 7 números (`Capa Logica/Paginacion/VentanaPaginas`), filas por página e "Ir a página".
- Asistencia, marcaciones y registro diario se filtran en la BD (`IMarcacionRepositorio`, `IRegAsisDiarioRepositorio`).
- Caché de catálogos: `Capa Datos/CacheCatalogos` (`IMemoryCache`, 10 min). Se invalida al crear, editar o eliminar.
- Health check: `GET /estado` (`Salud/BaseDatosHealthCheck`).
- Sin `MultipleActiveResultSets` en la cadena de conexión: con MARS la app se colgaba después de las páginas con más datos.

## Pruebas — SC-010

- `Capa Logica.Tests` (xUnit) tiene 126 pruebas y no necesita SQL Server. Cubre:
  - reglas de validación;
  - bloqueo, política de claves, opciones de seguridad y permisos por opción;
  - paginación y caché;
  - exportación, modelo de EF y arquitectura.
- El workflow de GitHub Actions ejecuta `dotnet test` antes de publicar. Si una prueba falla, no se despliega a Azure.

## Pendiente

- Paginación en la BD de Historial laboral (4 tablas en pestañas) y del índice de Vivienda.
- Reemplazar los desplegables de 3 335 empleados por una búsqueda con autocompletado.
- Permisos por acción dentro de cada módulo (hoy el permiso es por módulo).
