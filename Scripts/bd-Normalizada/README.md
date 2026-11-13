# BD normalizada `RRHH_Normalizado` (desde v2.0.0)

La app usa esta base de datos desde la **v2.0.0**. La v1.0.0 (repositorio anterior) usaba `RRHH_Nuevo`, la base sin normalizar. Su esquema sigue en `Scripts/bd-evo-RRLL.sql`, solo como referencia.

| Archivo | En el repo | Qué hace |
|---|---|---|
| `01-esquema-RRHH_Normalizado.sql` | Sí | Crea `RRHH_Normalizado` vacía: 77 tablas de RR.HH. (incluye `PeriodoTeletrabajo`, RF-13) y `mig.Incidencia`, 6 vistas y 84 FKs |
| `NORMALIZACION.md` | Sí | Qué forma normal viola cada tabla original y cómo se corrigió |
| `03-datos-RRHH_Normalizado.sql` | **No** | Carga los datos ya normalizados: 714 771 filas, ~156 MB |

`03-datos` no está en git porque pesa más que el límite de 100 MB de GitHub (está en `.gitignore`) y se entrega aparte. Para usarlo, cópialo en esta carpeta.

## Cómo crear la BD

```
1. 01-esquema-RRHH_Normalizado.sql   -> crea RRHH_Normalizado vacía
2. 03-datos-RRHH_Normalizado.sql     -> carga los datos (~100 s)
3. ..\azure\02-subir-bd.ps1          -> (opcional) la sube a Azure SQL con un .bacpac
```

`NORMALIZACION.md` también describe una ruta larga, que parte de `RRHH_Nuevo` y usa `02-migracion-desde-RRHH_Nuevo.sql`. Ese script solo está en la rama `dev_branch` del repositorio anterior (`sh4ntyfps/Proyecto-Evo_RRHH`). No hace falta para levantar el sistema.

`PeriodoTeletrabajo` (RF-13) ya viene en `01-esquema`. Si tu BD local se creó antes y no la tiene, **no vuelvas a correr `01-esquema`** (borra la BD y sus datos): ejecuta solo [rf-13-crear-periodo-teletrabajo.sql](../rf-13-crear-periodo-teletrabajo.sql), que es idempotente.

## Conexión local

En desarrollo la app usa `appsettings.Development.json`:

```
Server=.;Database=RRHH_Normalizado;Trusted_Connection=True;TrustServerCertificate=True
```

(instancia por defecto `MSSQLSERVER` con autenticación de Windows). Se arranca con el perfil `http` o `https` de `launchSettings.json`, que fija `ASPNETCORE_ENVIRONMENT=Development`. En Azure la cadena viene de la Connection string `RRHHNormalizado` de la Web App.
