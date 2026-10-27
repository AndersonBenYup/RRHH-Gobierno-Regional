# Temas del curso aplicados en EVO-RRHH

Curso: **Evolución y Configuración de Software** (semanas 2 a 9). Para cada tema se indica dónde está la evidencia en el repositorio.

El detalle de cada cambio de código (qué se hizo, por qué y para qué sirve) está en [`CAMBIOS-ARQUITECTURA.md`](CAMBIOS-ARQUITECTURA.md).

| Semana | Tema | Evidencia |
|---|---|---|
| 2 | Mantenimiento del software | [Tipos de mantenimiento aplicados](#semana-2--mantenimiento-del-software) |
| 3 | Ingeniería inversa | [Matriz módulos ↔ tablas](#semana-3--ingeniería-inversa) · `Scripts/bd-Normalizada/NORMALIZACION.md` |
| 4 | Refactorización, SRP y DRY | `RepositorioBase`, `LogicaBase`, `Numeracion`, `FichaSocialLogica`, `PersonaExtensiones` |
| 5 | Principio Abierto-Cerrado | `Exportacion/` (`IExportadorTabla`), registro automático de capas, `Numeracion` |
| 7 | Inversión de Control | `Capa Entidades/Contratos/`, `EmpleadoServicio`, `ConfiguracionCapaDatos`, `RegistroCapas` |
| 8 | Gestión de la configuración | [`GESTION-CONFIGURACION.md`](GESTION-CONFIGURACION.md) |
| 9 | Gestión del cambio | [`GESTION-DEL-CAMBIO.md`](GESTION-DEL-CAMBIO.md) · plantillas en `.github/` |
| — | Clean Code, arquitectura limpia, pruebas | [Arquitectura](#arquitectura-limpia-por-capas) · `Capa Logica.Tests` |

---

## Arquitectura limpia por capas

```
Proyecto-Evo-RRLL (controllers, vistas) ──► Capa Logica ──► Capa Entidades (entidades + contratos)
                                                                   ▲
                                          Capa Datos (EF, SQL) ────┘ implementa los contratos
```

**Regla de dependencia:** todo apunta hacia `Capa Entidades`. La lógica de negocio no conoce la base de datos, así que cambiar de BD (por ejemplo a `RRHH_Normalizado`) solo toca `Capa Datos`. `ArquitecturaTests` comprueba esta regla en cada `dotnet test`:

- `Capa Entidades` no depende de ninguna capa ni de Entity Framework.
- `Capa Logica` no referencia `Capa Datos` ni Entity Framework.
- Las entidades no llevan atributos de mapeo (`[Table]`, `[Key]`, `[Column]`).
- Ningún controller ni view model recibe tipos de `Capa Datos`.

---

## Semana 2 – Mantenimiento del software

| Tipo | Cambio en el proyecto |
|---|---|
| Correctivo | Alta de beneficiarios en Familia/Salud: antes se guardaba siempre con N.º 0 y el segundo fallaba. Ahora la Capa Datos lo numera por empleado. |
| Correctivo | Checkboxes de Permiso (`bool?` con `asp-for`) que daban error 500 al abrir el formulario. |
| Adaptativo | Clave del administrador inicial leída de configuración (variable de entorno `Seguridad__AdminClave` en producción). |
| Perfectivo | Bitácora de actividad, exportación a CSV y dashboard. |
| Perfectivo | Reestructuración por capas con contratos, para que un cambio de BD no se propague a la lógica ni a la interfaz (SC-006). |
| Preventivo | Pruebas `ArquitecturaTests`, que fallan si alguna capa vuelve a depender de la base de datos. |

**Deuda técnica conocida** (pendiente, registrada para gestionarla):

- Varias pantallas cargan tablas completas en memoria (`Listar()` y luego filtran en C#). Con `Marcacion` (~200 mil filas) esto es lento.
- Las vistas reciben entidades del dominio directamente en lugar de view models dedicados.

---

## Semana 3 – Ingeniería inversa

Análisis de la base de datos (tablas, claves, dependencias funcionales y formas normales): `Scripts/bd-Normalizada/NORMALIZACION.md`.

**Matriz módulos ↔ tablas**, obtenida de las dependencias que recibe cada controller:

| Módulo (controller) | Tablas que usa |
|---|---|
| Asistencia | Asistencia, Marcacion, RegAsisDiario, HorarioTemporal, Horario, Empleado, Persona |
| Capacitacion | Capacitacion, EstudiosRealizado, TitulosEmpleado, Institucion, TipoEstudio, EstadoEstudio, TipoInstitucion |
| Catalogo | 37 catálogos editables (ver `CatalogoLogica`), Bitacora |
| Comportamiento | Comportamiento, TipoComportamiento |
| Empleado | Empleado, Persona, Cargo, EstructOrganiz, Local, Horario, TipoTrabajador, RegimenPension, TipoDocID, AFP, EstadoCivil, TipoSangre, Discapacidad, ViveCon, Nacionalidad |
| EstructuraOrganiz | EstructOrganiz, ResponsableXUO, Empleado_Area, Local |
| Familia | Familiar, RRHH_DinamicaFamiliar, RRHH_FuncFam, Persona, TipoFamiliar, RRHH_TipoFamilia, RRHH_RelacPareja, RRHH_RelacPH, RRHH_RelacHermano, EstadoCivil, Discapacidad, TipoSangre |
| FamiliaSalud | Empleado, Persona, Familiar, RRHH_Salud, RRHH_DinamicaFamiliar, RRHH_AspSocio, RRHH_FuncFam, RRHH_Asegurado, RRHH_AcudeEnferm y catálogos de familia |
| Feriado | RRHH_Feriado |
| HistorialLaboral | PeriodoLaboral, ExpLaboral, Rotacion, Resolucion, TipoResolucion, TipoInstitucion, EstructOrganiz |
| Home (dashboard) | Empleado, Persona, Asistencia, EstructOrganiz, Cargo, RRHH_Feriado |
| Inconveniente | Inconveniente |
| Permiso | Permiso, Motivo_Perm |
| Salud | RRHH_Salud, RRHH_Asegurado, RRHH_AcudeEnferm, Persona, EstadoCivil, MotivoBaja |
| Seguridad / SeguridadAdmin | Usuario, Usuario_Rol, Rol, Rol_Acceso, SistemaOpcion, Bitacora |
| Vivienda | Vivienda y sus 8 catálogos (TipoViv, TenenciaViv, UbicacionViv, AguaViv, AlumbradoViv, ConservacionViv, ExcretasViv, MaterialViv) |

---

## Semana 4 – Refactorización, SRP y DRY

| Antes | Después | Principio |
|---|---|---|
| Cada clase `*Data` repetía el CRUD | `RepositorioBase<T>`: el CRUD está escrito una sola vez | DRY |
| Cada clase `*Logica` repetía Listar/Crear/Actualizar/Eliminar | `LogicaBase<T>` | DRY |
| 26 clases calculaban "máximo + 1" cada una con su propio método | `Capa Datos/Numeracion.cs`: una tabla de reglas | DRY y SRP |
| `FamiliaSaludController` abría la transacción y copiaba campos de la ficha social | `FichaSocialLogica.Guardar`: el controller solo atiende la petición HTTP | SRP |
| Nombre completo armado en varios lugares | `PersonaExtensiones.NombreCompleto()` | DRY |
| Las entidades sabían cómo se llaman las tablas | El mapeo vive en `EvoRRLDbContext` (Capa Datos) | SRP |

## Semana 5 – Principio Abierto-Cerrado

- **Exportación** (`Capa Logica/Exportacion`): `IExportadorTabla` es el contrato y `ExportadorTablas` elige la implementación por formato. Agregar Excel o PDF es una clase nueva más una línea de registro; ningún controller cambia. Lo demuestra la prueba `ExportadorTablasTests` con un formato TSV ficticio.
- **Registro automático de capas:** una tabla nueva solo agrega su `*Data` y su `*Logica`. `ConfiguracionCapaDatos` y `RegistroCapas` las registran sin editarse.
- **Cambio de base de datos:** la lógica queda cerrada a la modificación y la Capa Datos es el punto de extensión.

## Semana 7 – Inversión de Control

- **Contratos** en `Capa Entidades/Contratos`: `IRepositorio<T>`, `IUnidadDeTrabajo`, `IEmpleadoData`, `IPersonaData`, `IUsuarioRepositorio`, `IAsistenciaRepositorio`, `IBitacoraRepositorio`, `ICatalogoRepositorio`.
- **La lógica recibe contratos por constructor.** Ejemplo: `PermisoLogica(IRepositorio<Permiso>, IUnidadDeTrabajo)`. El contenedor de ASP.NET Core entrega la implementación.
- **Raíz de composición:** `Program.cs` → `AgregarCapaDatos(...)` + `AgregarServiciosDeAplicacion()`.
- **Pruebas sin base de datos:** `EmpleadoServicioTests` usa `EmpleadoDataFalso` y `PersonaDataFalso`.

## Clean Code y pruebas

- Nombres del dominio en español y consistentes: `Listar`, `Obtener`, `Crear`, `Actualizar`, `Eliminar`, `Registrar`. Los parámetros no llevan sufijos de ruido (`idEmpleado`, no `IdEmpleadoParam`).
- Funciones cortas que hacen una sola cosa.
- **Pruebas de caja blanca** (`dotnet test`, xUnit + SQLite en memoria): numeración, alta de empleado, ficha social, exportación, registro de dependencias y reglas de arquitectura.
