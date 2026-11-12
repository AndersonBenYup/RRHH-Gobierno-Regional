# BD normalizada `RRHH_Normalizado` (desde v2.0.0)

La app usa esta base de datos desde la **v2.0.0**. La v1.0.0 (repositorio anterior) usaba `RRHH_Nuevo`, la base sin normalizar. Su esquema sigue en `Scripts/bd-evo-RRLL.sql`, solo como referencia.

| Archivo | En el repo | Qué hace |
|---|---|---|
| `01-esquema-RRHH_Normalizado.sql` | Sí | Crea `RRHH_Normalizado` vacía: 76 tablas de RR.HH. y `mig.Incidencia`, 6 vistas y 84 FKs |
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

Para probar el registro y reporte de teletrabajo, ejecutar [rf-13-crear-periodo-teletrabajo.sql](../rf-13-crear-periodo-teletrabajo.sql) sobre la base normalizada ya creada.
