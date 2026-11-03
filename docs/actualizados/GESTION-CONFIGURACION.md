# Gestión de la configuración (Semana 8)

> Versión actualizada (noviembre 2026). El original (`docs/GESTION-CONFIGURACION.md`) describía el repositorio anterior:
> `sh4ntyfps/Proyecto-Evo_RRHH`, una rama `dev_branch`, prefijos `Fix:` y tags `v0.9-semana9`. También citaba archivos que
> aquí no existen: `bd-Datos.zip`, `bd-Normalizada`, una plantilla de PR y pruebas con SQLite.

## ¿Por qué la necesita el proyecto?

EVO-RRHH es un sistema heredado con una base de datos grande: 88 tablas, con hasta 263 000 filas en una sola. Cada push a `main` se despliega a producción en Azure.

Sin gestión de configuración:
- no se sabría qué versión del código está en producción;
- un cambio sin pruebas llegaría directo a los usuarios;
- no habría forma segura de volver atrás.

## Elementos de configuración

| Elemento | Ubicación | Se versiona | Observación |
|---|---|---|---|
| Código fuente | `Capa Entidades/`, `Capa Datos/`, `Capa Logica/`, `Proyecto-Evo-RRLL/` | Sí | Solución `Proyecto-Evo-RRLL.slnx` |
| Pruebas | `Capa Logica.Tests/` | Sí | 126 pruebas xUnit; corren en el CI |
| Pipeline | `.github/workflows/main_rrhh-gobierno-regional.yml` | Sí | Compila, prueba, publica y despliega |
| Plantilla de solicitud de cambio | `.github/ISSUE_TEMPLATE/solicitud-de-cambio.md` | Sí | |
| Scripts de BD | `Scripts/bd-evo-RRLL.sql`, `crear_bitacora.sql`, `sc-008-auditoria-datos.sql`, `azure/02-subir-bd.ps1` | Sí | El de auditoría es de solo lectura; la BD se sube a Azure con un `.bacpac` |
| Copias de la BD | `*.bacpac` | **No** | Excluidas en `.gitignore` por su tamaño |
| Configuración | `Proyecto-Evo-RRLL/appsettings.json`, `appsettings.Development.json` | Sí | **Sin claves ni cadenas con contraseña** |
| Secretos locales | `dotnet user-secrets` (`UserSecretsId` en el `.csproj`) | **No** | `Seguridad:AdminClave` en desarrollo |
| Secretos de producción | Azure Web App: la cadena de conexión `RRHHNormalizado` y el App Setting `Seguridad__AdminClave` | **No** | Fuera del repositorio |
| Estilos | `Proyecto-Evo-RRLL/Estilos/app.css`, `tailwind.config.js`, `package.json`, `package-lock.json` | Sí | Fuente de Tailwind |
| Estilos compilados | `Proyecto-Evo-RRLL/wwwroot/css/app.css` | Sí | Se versiona para que el CI no necesite Node (`npm run css:build`) |
| Dependencias de Node | `node_modules/` | **No** | Se instalan con `npm install` |
| Compilados | `bin/`, `obj/`, `publish/` | **No** | Se generan con `dotnet build/publish` |
| Documentación | `README.md`, `PRODUCT.md`, `SOLICITUDES-DE-CAMBIO.md`, `docs/` | Sí | `docs/actualizados/` tiene la versión vigente |

## Organización del repositorio

```
Capa Entidades/      dominio, contratos (IRepositorio<T>…) y atributos de validación
Capa Datos/          EF Core, RepositorioBase<T>, repositorios específicos, caché de catálogos
Capa Logica/         reglas de negocio, seguridad, exportación, paginación
Capa Logica.Tests/   pruebas automatizadas (xUnit)
Proyecto-Evo-RRLL/   ASP.NET Core MVC: controllers, vistas, servicios web, filtros, estilos
Scripts/             esquema, auditoría y subida de la BD a Azure
docs/                documentación del curso (docs/actualizados = vigente) y entregables
temas-semanas/       material de cada semana del curso
.github/             pipeline y plantilla de solicitud de cambio
```

## Control de versiones

- **Herramienta:** Git. El repositorio está en GitHub, en `AndersonBenYup/RRHH-Gobierno-Regional`.
- **Rama:** `main` es la versión estable y la que se despliega. El equipo integra directo en `main`; la protección es el pipeline, que no despliega si una prueba falla. Los *pull requests* también ejecutan las pruebas.
- **Mensajes de commit:** el código de la solicitud de cambio como prefijo, `SC-NNN: descripción`. Por ejemplo, `SC-009: Permisos por opcion (Rol_Acceso)…`. Todo commit se puede rastrear a una solicitud.
- **Etiquetas:** una por solicitud cerrada (versionado semántico):

| Tag | Contenido |
|---|---|
| `v1.1.0` | SC-008 |
| `v1.2.0` | SC-009 |
| `v1.3.0` | SC-006, SC-005 y SC-003 |
| `v1.4.0` | SC-010 |
| `v1.5.0` | SC-011 |
| `v1.6.0` | Cierre del plan de 4 semanas |

- **No se versionan** credenciales, copias de la BD, `node_modules` ni compilados.

## Control de cambios

Todo cambio sigue el flujo de [`GESTION-DEL-CAMBIO.md`](GESTION-DEL-CAMBIO.md):

1. Se registra como solicitud (SC-NNN), con la plantilla de `.github/`.
2. Se analiza el impacto y el comité lo aprueba.
3. Se implementa en commits `SC-NNN:`.
4. GitHub Actions compila, ejecuta `dotnet test` y solo si todo pasa publica en la Azure Web App `rrhh-gobierno-regional`.
5. Se actualiza la matriz y se crea el tag.

## Pruebas vinculadas a la configuración

| Tipo | Cómo se aplica |
|---|---|
| Caja blanca | `dotnet test`: 126 pruebas xUnit de reglas de negocio, seguridad, paginación, caché, exportación y modelo de EF, sin SQL Server |
| Arquitectura | `ArquitecturaTests`: ninguna capa vuelve a depender de la BD y cada clase `*Logica` se puede crear desde el contenedor |
| Caja negra | Pruebas funcionales en el navegador (Playwright) por módulo: login, listados, altas, ediciones, permisos y modo oscuro |
| Despliegue | `GET /estado` (health check) confirma que la app llega a la BD después de cada despliegue |
| Seguridad | Contraseñas con `PasswordHasher`; rol Administrador solo para los administradores de RRHH; permisos por módulo (`Rol_Acceso`); claves fuera del repositorio |
