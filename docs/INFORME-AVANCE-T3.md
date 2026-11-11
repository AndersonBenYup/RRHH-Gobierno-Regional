# Informe de Avance T3

## 1. Carátula

- Universidad Privada del Norte · Ingeniería de Sistemas Computacionales
- Evolución y Configuración de Software · Sistema de gestión de asistencias
- Grupo 5 · 10 de noviembre de 2026
- Completar nombres de los integrantes, docente y sección con los datos oficiales del curso.

## 2. Resumen ejecutivo del avance

La versión intermedia desplegada en Azure reúne los ocho requisitos obligatorios y tres complementarios. El proyecto web compila y el conjunto automatizado registra 135 pruebas aprobadas. Queda pendiente adjuntar los resultados y las capturas de las pruebas funcionales.

## 3. Avances realizados desde la T2

La matriz anterior de solicitudes se amplió con los RF de la T3. Se agregó el filtro al historial de turnos, se completaron reglas de permisos, se incorporó el registro de teletrabajo y se añadieron reportes.

## 4. Actualización del diagnóstico técnico

Horarios Temporales ya almacenaba periodos por persona. La regla de permisos consideraba las horas y permitía dos permisos el mismo día. Los motivos históricos de teletrabajo estaban en el catálogo de permisos. La reconstrucción deja separados el registro de teletrabajo y las ausencias.

## 5. Base técnica reconstruida o recuperada

La solución `Proyecto-Evo-RRLL.slnx` integra la aplicación web, las capas de Entidades, Lógica, Datos y las pruebas. La aplicación utiliza Azure SQL. Se ejecutó `Scripts/rf-13-crear-periodo-teletrabajo.sql` en `rrhhdb` y una consulta posterior confirmó la existencia de `dbo.PeriodoTeletrabajo`.

## 6. Arquitectura o estructura actual del sistema

ASP.NET Core MVC recibe y valida las solicitudes. La capa lógica aplica las reglas y depende de contratos de repositorio; la capa de datos usa EF Core y SQL Server. La vista de teletrabajo utiliza el permiso de acceso del módulo Permisos.

## 7. Requerimientos implementados

| RF | Función | Estado del avance |
| --- | --- | --- |
| RF-01 | Registrar turnos por periodos de fechas | Existente, revisado en código |
| RF-02 | Consultar el histórico de turnos | Filtro por empleado y rango |
| RF-03 | Impedir dos permisos para la misma persona y día | Regla aplicada en alta y edición |
| RF-04 | Agregar motivos de permiso | Existente, revisado en código |
| RF-05 | Eliminar motivos de permiso | Baja de motivos sin referencias y aviso si están en uso |
| RF-06 | Validar licencias que se otorgan una vez al año | Regla y pruebas; criterio por confirmar |
| RF-13 | Registrar periodos de teletrabajo | Alta por persona, fechas y modalidad |
| RF-14 | Reportar días de teletrabajo | Total de días calendario únicos por empleado |
| RF-16 | Reportar quiénes realizan teletrabajo total o parcial | Reporte por rango y modalidad |
| RF-17 | Validar feriados en el reporte general | Conteo de fechas únicas en Registro de Asistencia |
| RF-18 | Reportar ausencias por periodo | Consulta por rango y motivos seleccionados |

## 8. Requerimientos pendientes

RF-07 a RF-12 y RF-15 quedan fuera de la selección complementaria de esta versión. Los 11 RF de la tabla requieren pruebas funcionales con datos de prueba. Confirmar el agrupamiento de motivos de licencia y el criterio de días calendario para teletrabajo.

## 9. Evidencias de refactorización aplicada

| Refactorización | Antes | Después | Principio y beneficio |
| --- | --- | --- | --- |
| Repositorio genérico | `afcd88c^`: clases CRUD por entidad, como `Capa Datos/Data/AFPData.cs` | `afcd88c`: `RepositorioBase<T>` centraliza el CRUD | DRY. Reduce operaciones repetidas |
| Separación de responsabilidades del controlador de empleados | `fc8d71d^`: listado y catálogos dentro de `EmpleadoController` | `fc8d71d`: `EmpleadoListadoServicio` y `EmpleadoCatalogosServicio` | Responsabilidad única. Controlador más pequeño |
| Dependencias entre capas | `2284cea^`: lógica acoplada a la implementación de acceso | `2284cea`: `LogicaBase<T>` y lógica sobre contratos; `c808e85` verifica la arquitectura | Inversión de dependencias. La lógica se prueba sin depender de EF Core |

Para ver el código antes y después de cada cambio, ejecutar `git show <commit>`.

## 10. Matriz de control de cambios actualizada

Ver [solicitudes y trazabilidad](actualizados/SOLICITUDES-DE-CAMBIO.md) para los RF, ramas, commits, casos y resultados. Completar responsables con los nombres de quienes realizaron o verificaron cada cambio.

## 11. Evidencias de versionado y repositorio

La integración inicial en `develop` se hizo localmente. Tras la reescritura de `main`, los cambios se reaplicaron en `dev` y llegaron a `main` mediante el [PR #1](https://github.com/AndersonBenYup/RRHH-Gobierno-Regional/pull/1). GitHub Actions mostró compilación y despliegue correctos para `49e516a`, commit identificado por `v2.6.0`. La etiqueta `v1.7.0` identifica el historial anterior. La matriz distingue los hashes originales y los publicados. Queda por adjuntar evidencia de contribuciones individuales.

## 12. Casos de prueba ejecutados

Compilación web con cero errores y cero advertencias. Resultado de `dotnet test`: 135 aprobadas, cero fallidas y cero omitidas. Entre ellas, 13 casos de reglas de permisos, 6 de licencia anual, 2 de conteo de teletrabajo y 1 de feriados. Los resultados de pantalla con SQL Server están pendientes y se registrarán en la matriz.

## 13. Problemas técnicos encontrados

El borrado de motivos en uso chocaba con referencias de permisos. Los registros históricos de turnos y permisos pueden carecer de fecha de fin. Se controla el borrado y se usa la fecha de inicio como fin para filtrar esos registros históricos.

## 14. Riesgos pendientes

Los permisos históricos de teletrabajo 125 y 126 requieren una decisión de migración. El script nuevo debe ejecutarse en la base de destino antes de usar el módulo. Debe validarse con el área usuaria si los días calendario y el grupo de motivos de licencia coinciden con el criterio institucional.

## 15. Plan de cierre hacia la Evaluación Final

Validar los 11 RF en la aplicación desplegada con datos ficticios. Registrar resultados y capturas, completar carátula y responsables y revisar las reglas asumidas. Continuar con los RF de vacaciones y el reporte de presencialidad en la siguiente etapa.

## 16. Conclusiones

El repositorio contiene una versión intermedia compilable con turnos, permisos, teletrabajo y tres reportes complementarios. El historial permite rastrear implementación y pruebas automatizadas.

## 17. Recomendaciones

Usar datos ficticios para las capturas. Revisar los cambios en `develop` antes de pasar a `main`, ya que un push a esa rama activa el despliegue.

## 18. Anexos

- [Matriz de solicitudes y casos](actualizados/SOLICITUDES-DE-CAMBIO.md).
- [Guía de versionado](SEMANA-12-VERSIONADO.md).
- [Script SQL de teletrabajo](../Scripts/rf-13-crear-periodo-teletrabajo.sql).
- Capturas de la aplicación, ejecución SQL y publicación en GitHub pendientes de adjuntar.
