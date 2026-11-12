# Semana 12 · Gestión de versiones

El trabajo inicial se integró localmente en `develop`. Cuando se reescribió `main`, se reaplicaron nueve commits de código y se consolidó la documentación en `dev`, a partir de `9fed9df`. El [PR #1](https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/pull/1) integró `dev` a `main` en `49e516a`. Los hashes cambiaron al reaplicar los commits. La matriz registra ambos.

Para los próximos cambios, el flujo es `feature → dev → main`.

1. Crear una rama nueva desde `dev` al empezar cada funcionalidad. Usar nombres como `feature/registro-vacaciones` e incluir RF y SC en el PR.
2. Abrir el PR hacia `dev` con el caso de prueba y su evidencia. Integrarlo después de la revisión.
3. Abrir un PR de `dev` a `main` al preparar una versión. Actualizar el changelog y etiquetar el commit desplegado.

`develop`, `integracion/develop-sobre-main`, `docs/semana12-versionado` y las ramas `feature/*` publicadas para la T3 quedan como referencias. Las siete ramas que se reservaron para RF-07 a RF-12 y RF-15 son puntos de partida del historial anterior. Crear sus ramas de trabajo desde `dev` al empezar.

El flujo de Azure se ejecuta en pushes y pull requests a `main` y por lanzamiento manual. Los pull requests ejecutan la compilación sin desplegar. Un push a `main` activa el despliegue.

## Referencias históricas

Estas ramas `historial/*` se crean al adoptar el flujo para localizar evidencia existente. Su fecha de creación no representa la fecha de implementación ni acredita pull requests anteriores.

| Rama | Referencia | Evidencia |
| --- | --- | --- |
| `historial/SC-001-validar-permisos` | `1273fc0` | Reglas en la capa lógica y pruebas, dentro de SC-008 |
| `historial/SC-002-bloqueo-cuenta` | `ef9506b` | Bloqueo por intentos fallidos, dentro de SC-009 |
| `historial/SC-003-exportar-asistencia` | `32fecf6` | Exportación a CSV |
| `historial/SC-004-configuracion-azure` | `9b93a9e` | Cierre de la solicitud y actualización de la matriz |
| `historial/SC-005-paginar-permisos` | `d5f0f51` | Filtrado y paginación en base de datos |
| `historial/SC-006-contratos-y-capas` | `c808e85` | Pruebas de arquitectura tras separar las capas |
| `historial/SC-007-interfaz` | `v2.4.0` | Referencia al rediseño implementado en SC-011 |
| `historial/SC-008-validaciones` | `v2.1.0` | Versión de validaciones |
| `historial/SC-009-seguridad` | `v2.2.0` | Versión de seguridad |
| `historial/SC-010-pruebas` | `v2.3.1` | Versión de pruebas e integración |
| `historial/SC-011-rediseno-visual` | `v2.4.0` | Versión del rediseño visual |

## Solicitudes T3

Se reserva un código por RF para revisar lo existente y completar solo las brechas. Todas las solicitudes parten en estado **por verificar**. Registrar responsable, componente, commit, caso ejecutado, resultado y evidencia al revisar cada una. RF-03 tiene como antecedente `1273fc0`: comprobar también dos permisos con horas distintas para la misma persona y fecha.

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
