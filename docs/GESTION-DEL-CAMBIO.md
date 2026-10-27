# Gestión del cambio (Semana 9)

Resumen de la exposición `Grupo5_Gestion_del_Cambio.pptx` aplicada a este repositorio.

## Flujo

```
Solicitud ─► Análisis ─► Evaluación de impacto ─► Aprobación / rechazo (CCB)
   ─► Planificación ─► Implementación (rama + PR) ─► Verificación (dotnet test + prueba funcional) ─► Registro y cierre
```

| Rol | Función |
|---|---|
| Solicitante | Usuario, cliente o integrante que detecta la necesidad y registra la solicitud |
| Gestor de cambios | Revisa las solicitudes, coordina el análisis, mantiene la matriz y comunica las decisiones |
| Comité de Control de Cambios (CCB) | Jefe de proyecto, líder técnico y representante del usuario: aprueba, rechaza o difiere |
| Analista / Líder técnico | Evalúa el impacto: componentes, base de datos, esfuerzo, riesgos y dependencias |
| Desarrollador | Implementa el cambio aprobado en su rama y abre el *pull request* |
| QA / Tester | Verifica con pruebas funcionales y de regresión antes de integrar |
| Gestor de configuración | Controla ramas y versiones; integra a `main` y etiqueta la versión |

## Formato de solicitud de cambio

Plantilla en GitHub: `.github/ISSUE_TEMPLATE/solicitud-de-cambio.md`.

| Campo | Contenido |
|---|---|
| Código | SC-NNN |
| Solicitante / fecha | |
| Tipo | Correctivo · Adaptativo · Perfectivo · Preventivo |
| Descripción | Qué se pide y por qué |
| Módulos y capas afectadas | |
| Impacto | Alto · Medio · Bajo, con justificación |
| Decisión | Aprobado · Rechazado · Diferido |
| Verificación | Pruebas que demuestran el cambio |

## Matriz de cambios del proyecto

| Código | Tipo | Descripción | Módulo | Prioridad | Estado |
|---|---|---|---|---|---|
| SC-001 | Correctivo | Validar fechas, horas y solapamiento al registrar permisos (`PermisoLogica` hoy no valida) | Permisos | Alta | Aprobado |
| SC-002 | Preventivo | Bloquear la cuenta tras intentos fallidos de inicio de sesión (`SeguridadController.Login` no los limita) | Seguridad | Alta | Aprobado |
| SC-003 | Perfectivo | Exportar el registro de asistencia a CSV reutilizando `ExportadorTablas` / `ExportadorCsv` | Asistencia | Media | Aprobado |
| SC-004 | Adaptativo | Leer la cadena de conexión desde variable de entorno en producción (la clave del admin ya se lee de `Seguridad__AdminClave`) | Configuración | Media | Diferido |
| SC-005 | Perfectivo | Filtrar y paginar permisos en la BD (`PermisoController.Index` trae toda la tabla y filtra en memoria) | Permisos | Baja | En evaluación |
| SC-006 | Perfectivo | Arquitectura en capas con contratos: cambiar de BD solo toca `Capa Datos` | Todas las capas | Media | Implementado |
| SC-007 | Perfectivo | Rediseño de la interfaz (login, inicio y layout) con Tailwind CSS | Vistas | Media | Implementado |

## Análisis de impacto

| SC | Entidades | Datos | Lógica | Controllers | Vistas | BD | Config. | Impacto | Riesgo |
|---|:-:|:-:|:-:|:-:|:-:|:-:|:-:|---|---|
| SC-001 | | | ● | ● | ● | | | Medio | Bajo |
| SC-002 | ● | ● | ● | ● | ● | ● | | Alto | Alto |
| SC-003 | | | | ● | ● | | | Bajo | Bajo |
| SC-004 | | | | | | | ● | Medio | Medio |
| SC-005 | ● | ● | ● | ● | ● | | | Medio | Medio |
| SC-006 | ● | ● | ● | ● | | | ● | Alto | Medio |
| SC-007 | | | | | ● | | ● | Medio | Bajo |

- **SC-002** es el más crítico: agrega columnas a la tabla `Usuario`, así que el script SQL se despliega antes que el código.
- **SC-001 y SC-005** modifican el mismo `PermisoController`: se planifican juntos para evitar conflictos de *merge*.
- **SC-005** necesita un contrato nuevo en `Capa Entidades/Contratos` (por ejemplo `IPermisoRepositorio`) con la consulta filtrada, igual que `IAsistenciaRepositorio`.
- **SC-006** tocó las capas sin cambiar el comportamiento observable. *Mitigación:* las pruebas existentes siguen en verde y `ArquitecturaTests` impide que una capa vuelva a depender de la BD.

## Correcciones cerradas antes de usar este proceso

- Alta de beneficiarios en Familia/Salud guardaba siempre N.º 0 (el segundo fallaba).
- Formulario de Permiso daba error 500 (checkbox `bool?`).

## Posibles cambios futuros (sin solicitud registrada)

- Base normalizada `RRHH_Normalizado` (`Scripts/bd-Normalizada`): solo está preparada como opción. Si se decide usarla, se registra como nueva solicitud; gracias a SC-006 solo cambiarían `Capa Datos` y la cadena de conexión.

## Buenas prácticas del equipo

- Ningún cambio se integra sin solicitud registrada y *pull request* revisado.
- Un cambio por rama (`feature/SC-NNN-descripcion`) y commits con el código de la SC.
- `dotnet test` debe pasar antes de integrar.
- Al cerrar, se actualiza el estado en esta matriz.
