# Gestión de la configuración (Semana 8)

## ¿Por qué la necesita el proyecto?

EVO-RRHH lo trabajan varias personas a la vez, sobre un sistema heredado y con una base de datos grande. Sin gestión de configuración no se sabe qué versión de los scripts corresponde a qué versión del código, dos personas pueden editar el mismo archivo sin enterarse, y no hay forma segura de volver atrás si algo se rompe.

## Elementos de configuración

| Elemento | Ubicación | Se versiona | Observación |
|---|---|---|---|
| Código fuente | `Capa Entidades/`, `Capa Datos/`, `Capa Logica/`, `Proyecto-Evo-RRLL/` | Sí | |
| Pruebas | `Capa Logica.Tests/` | Sí | Se ejecutan con `dotnet test` |
| Script de esquema | `Scripts/bd-evo-RRLL.sql`, `Scripts/crear_bitacora.sql` | Sí | |
| Datos de la BD | `Scripts/bd-Datos.zip` | Sí, comprimido | El `.sql` descomprimido pesa 172 MB: supera el límite de GitHub (100 MB) |
| BD normalizada | `Scripts/bd-Normalizada/01..02`, `generar-03-datos.ps1` | Sí | `03-datos-*.sql` (156 MB) se regenera con el script o se distribuye comprimido |
| Configuración | `Proyecto-Evo-RRLL/appsettings.json` | Sí | Sin claves: la de producción va en la variable `Seguridad__AdminClave` |
| Configuración local | `appsettings.Development.json` | Sí | Solo clave de desarrollo |
| Documentación | `README.md`, `docs/`, `NORMALIZACION.md` | Sí | |
| Compilados | `bin/`, `obj/`, `publish/` | **No** | Excluidos en `.gitignore`; se generan con `dotnet build/publish` |

## Organización del repositorio

```
Capa Entidades/      dominio + contratos (sin dependencias)
Capa Datos/          EF Core, repositorios, numeración
Capa Logica/         reglas de negocio
Capa Logica.Tests/   pruebas automatizadas
Proyecto-Evo-RRLL/   ASP.NET Core MVC (controllers, vistas)
Scripts/             esquema y datos de la BD
docs/                documentación del curso y del proyecto
.github/             plantillas de solicitud de cambio y de pull request
```

## Control de versiones

- **Herramienta:** Git. El repositorio está en GitHub: `sh4ntyfps/Proyecto-Evo_RRHH`.
- **Ramas:**
  - `main`: versión estable y entregable.
  - `dev_branch`: integración.
  - Una rama por persona o por tarea (`andersonBenites`, `refactor/srp-dry`…).
- **Mensajes de commit:** un prefijo que indique el tipo de cambio, como ya se usa en el historial:
  - `Fix:` corrección (mantenimiento correctivo).
  - `Modulo X:` funcionalidad nueva.
  - `refactor(...)`: refactorización.
  - `Seguridad:`, `Rendimiento:`, `Doc:`.
- **Etiquetas:** una por entrega semanal (`v0.9-semana9`).
- **No se versionan** credenciales, archivos de más de 100 MB ni compilados.

## Control de cambios

Todo cambio sigue el flujo de [`GESTION-DEL-CAMBIO.md`](GESTION-DEL-CAMBIO.md):

1. Se registra como solicitud (*issue*) con la plantilla de `.github/`.
2. Se trabaja en una rama.
3. Se integra por *pull request*, con revisión de un compañero y `dotnet test` en verde.

## Pruebas vinculadas a la configuración

| Tipo | Cómo se aplica |
|---|---|
| Caja blanca | `dotnet test`: xUnit con SQLite en memoria (numeración, transacciones, ficha social, exportación, arquitectura) |
| Caja negra | Pruebas funcionales manuales por módulo: login, listados, altas y ediciones |
| Caja gris | `RegistroCapasTests` verifica que cada controller pueda construirse con el contenedor real |
| Seguridad | Contraseñas con `PasswordHasher`, roles por política `SoloAdministradores` y la clave del admin fuera del código |
