# Cambios en la arquitectura

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

## Abierto a la extensión (OCP) — SC-003

- `IExportadorTabla`, `ExportadorCsv` y `ExportadorTablas` en `Capa Logica/Exportacion`. Los usan Empleado,
  Catálogo y Asistencia. Un formato nuevo (Excel, PDF) es otra clase registrada en `AgregarCapaLogica`.

## Escalabilidad — SC-005 y SC-003

- Permisos: filtro y paginación en SQL Server (`OFFSET/FETCH`) con `_Paginador.cshtml` reutilizable.
- Asistencia: el rango de fechas se filtra en la BD.

## Pruebas — SC-010

- `Capa Logica.Tests` (xUnit): reglas de validación, bloqueo y política de claves, exportación, modelo de EF y
  arquitectura. No necesita SQL Server.
- El workflow de GitHub Actions ejecuta `dotnet test` antes de publicar. Si una prueba falla, no se despliega a Azure.

## Pendiente

- Permisos por opción (`Rol_Acceso`).
- Caché de catálogos y health checks.
- Paginación en la BD para el resto de listados grandes (marcaciones, registro diario).
