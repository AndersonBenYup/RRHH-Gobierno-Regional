# Semana 12 · Gestión de versiones

El flujo se adopta el 10 de noviembre de 2026 a partir de `v1.6.0` (`7d81d5b`).

1. Crear una rama `feature/<funcionalidad>` desde `develop`, por ejemplo `feature/historico-de-turnos`. Registrar los códigos RF y SC en la matriz y el pull request. Si la funcionalidad ya tiene rama, actualizarla desde `develop` antes de empezar.
2. Registrar el cambio con un mensaje descriptivo, por ejemplo `SC-014 (RF-03): validar permisos duplicados por día`. Enlazar el caso de prueba y su evidencia en el pull request a `develop`.
3. Integrar en `develop` tras revisar el cambio y su evidencia. Cuando haya una versión aprobada, abrir un pull request de `develop` a `main`, actualizar el changelog y etiquetar el commit publicado.

El flujo de Azure se ejecuta en pushes y pull requests a `main` y por lanzamiento manual. Los pull requests ejecutan la compilación sin desplegar. Un push a `main` activa el despliegue.

## Referencias históricas

Estas ramas `historial/*` se crean al adoptar el flujo para localizar evidencia existente. Su fecha de creación no representa la fecha de implementación ni acredita pull requests anteriores.

| Rama | Referencia | Evidencia |
| --- | --- | --- |
| `historial/SC-001-validar-permisos` | `22d7c6f` | Reglas en la capa lógica y pruebas, dentro de SC-008 |
| `historial/SC-002-bloqueo-cuenta` | `586d90a` | Bloqueo por intentos fallidos, dentro de SC-009 |
| `historial/SC-003-exportar-asistencia` | `0705852` | Exportación a CSV |
| `historial/SC-004-configuracion-azure` | `548c4c2` | Cierre de la solicitud y actualización de la matriz |
| `historial/SC-005-paginar-permisos` | `2fba606` | Filtrado y paginación en base de datos |
| `historial/SC-006-contratos-y-capas` | `230c91c` | Pruebas de arquitectura tras separar las capas |
| `historial/SC-007-interfaz` | `v1.5.0` | Referencia al rediseño implementado en SC-011 |
| `historial/SC-008-validaciones` | `v1.1.0` | Versión de validaciones |
| `historial/SC-009-seguridad` | `v1.2.0` | Versión de seguridad |
| `historial/SC-010-pruebas` | `v1.4.0` | Versión de pruebas e integración |
| `historial/SC-011-rediseno-visual` | `v1.5.0` | Versión del rediseño visual |

## Solicitudes T3

Se reserva un código por RF para revisar lo existente y completar solo las brechas. Todas las solicitudes parten en estado **por verificar**. Registrar responsable, componente, commit, caso ejecutado, resultado y evidencia al revisar cada una. RF-03 tiene como antecedente `22d7c6f`: comprobar también dos permisos con horas distintas para la misma persona y fecha.

| Solicitud | RF | Alcance | Prioridad T3 |
| --- | --- | --- | --- |
| SC-012 | RF-01 | Registrar turnos por periodos | Obligatorio |
| SC-013 | RF-02 | Consultar histórico de turnos. Implementado, pendiente de prueba funcional | Obligatorio |
| SC-014 | RF-03 | Impedir dos permisos el mismo día | Obligatorio |
| SC-015 | RF-04 | Agregar motivos de permiso | Obligatorio |
| SC-016 | RF-05 | Eliminar motivos de permiso | Obligatorio |
| SC-017 | RF-06 | Validar licencias otorgadas una vez al año | Obligatorio |
| SC-018 | RF-07 | Reportar vacaciones generadas | Complementario |
| SC-019 | RF-08 | Reportar vacaciones pendientes | Complementario |
| SC-020 | RF-09 | Reportar vacaciones truncas | Complementario |
| SC-021 | RF-10 | Contabilizar vacaciones de media jornada | Complementario |
| SC-022 | RF-11 | Generar reporte de vacaciones | Complementario |
| SC-023 | RF-12 | Consultar vacaciones pendientes y usadas | Complementario |
| SC-024 | RF-13 | Registrar periodos de teletrabajo | Obligatorio |
| SC-025 | RF-14 | Reportar días de teletrabajo | Obligatorio |
| SC-026 | RF-15 | Reportar presencialidad y teletrabajo por persona | Complementario |
| SC-027 | RF-16 | Reportar teletrabajo total y parcial | Complementario |
| SC-028 | RF-17 | Validar feriados en el reporte general | Complementario |
| SC-029 | RF-18 | Reportar ausencias por periodo | Complementario |

Las ramas `feature/` usan nombres de funcionalidad. La T3 exige los ocho RF obligatorios y al menos tres complementarios. Elegir los tres tras revisar los módulos y su evidencia.

Para el informe, usar la lista de ramas, el gráfico de commits, las etiquetas y el changelog. Adjuntar los enlaces de pull requests y las pruebas ejecutadas cuando existan. Registrar las contribuciones con los autores y evidencias reales.
