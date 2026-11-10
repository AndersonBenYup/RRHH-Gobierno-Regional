# Documentos actualizados (noviembre 2026, `v2.5.0`)

Esta carpeta tiene las versiones corregidas de la documentación del curso y de los entregables.
Los **originales no se modificaron**: `docs/*.md`, `docs/entregables/` y `SOLICITUDES-DE-CAMBIO.md` en la raíz quedan como el diagnóstico inicial.

| Documento | Qué cambió frente al original |
|---|---|
| [`TEMAS-DEL-CURSO.md`](TEMAS-DEL-CURSO.md) | Reescrito. El original citaba clases del repositorio anterior que aquí no existen (`Numeracion`, `FichaSocialLogica`, `PersonaExtensiones`, `IUnidadDeTrabajo`, pruebas con SQLite). Se agregaron las semanas 9 y 11 y la tabla de pruebas |
| [`INGENIERIA-INVERSA.md`](INGENIERIA-INVERSA.md) | **Nuevo** (Semana 3): reconocimiento funcional, BD (88 tablas), diagrama E-R, diccionario de datos, matriz módulos ↔ tablas y hallazgos |
| [`GESTION-CONFIGURACION.md`](GESTION-CONFIGURACION.md) | Reescrito con el repositorio, las ramas, los commits `SC-NNN:`, los tags `v1.x`, los secretos y las pruebas reales |
| [`GESTION-DEL-CAMBIO.md`](GESTION-DEL-CAMBIO.md) | Impacto de SC-008 a SC-011, evidencia de `v2.5.0` y flujo de trabajo real (commits a `main` con el CI como control) |
| [`CAMBIOS-ARQUITECTURA.md`](CAMBIOS-ARQUITECTURA.md) | Se suman los permisos por opción, la paginación genérica, el caché, el health check, `IOptions` y los servicios de Personal |
| [`SOLICITUDES-DE-CAMBIO.md`](SOLICITUDES-DE-CAMBIO.md) | La sección 6 incluye lo cerrado en `v2.5.0` |
| [`entregables/TierList_EVO-RRHH.xlsx`](entregables/TierList_EVO-RRHH.xlsx) | Estado actual: 29 ✔ y 1 ◐ (paginación en la BD: faltan Historial laboral y Vivienda) |
| [`entregables/Plan_EVO-RRHH_4semanas.docx`](entregables/Plan_EVO-RRHH_4semanas.docx) | Conserva el diagnóstico y agrega "5. Estado al cierre" y "6. Riesgos: cómo terminaron" |

## Pendiente para cerrar el plan

1. **Antes del push**, en Azure:
   - confirmar que el rol del usuario `admin` es `ADMINISTRADOR` del sistema `0500000` (o `Administrador`);
   - quitar `MultipleActiveResultSets=True` de la cadena `RRHHNormalizado`, si la tiene;
   - cambiar `Seguridad__AdminClave` y la clave del usuario `admin`.
2. `git push origin main --follow-tags` y revisar que GitHub Actions termine en verde.
3. Probar en Azure: login, `/estado`, Personal, Permisos y Permisos del rol.
4. Pendiente de código: paginación en la BD de Historial laboral y del índice de Vivienda.
