# Informe de Avance T3

## 1. Carátula

- Universidad Privada del Norte · Ingeniería de Sistemas Computacionales
- Evolución y Configuración de Software · Sistema de gestión de asistencias
- Grupo 5 · 10 de noviembre de 2026
- Completar nombres de los integrantes, docente y sección con los datos oficiales del curso.

## 2. Resumen ejecutivo del avance

La versión `v2.6.0`, desplegada en Azure, reúne los ocho requisitos obligatorios y tres complementarios. Las versiones `v2.7.0` a `v2.9.0`, integradas en `dev`, completan los 18 RF del área usuaria: presencialidad y teletrabajo (RF-15), vacaciones (RF-07 a RF-11) y la consulta personal de vacaciones (RF-12). La `v2.9.1` corrige el cálculo de vacaciones con los datos reales de la BD (fecha de ingreso y permisos de media jornada). El proyecto web compila sin advertencias y el conjunto automatizado registra 176 pruebas aprobadas. Queda pendiente adjuntar los resultados y las capturas de las pruebas funcionales.

## 3. Avances realizados desde la T2

La matriz anterior de solicitudes se amplió con los RF de la T3. Se agregó el filtro al historial de turnos, se completaron reglas de permisos, se incorporó el registro de teletrabajo y se añadieron reportes. Después se agregaron el módulo de vacaciones y la consulta personal, y los reportes de ausencias y teletrabajo pasaron a filtrarse en la BD.

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
| RF-18 | Reportar ausencias por periodo | Consulta por rango y motivos seleccionados, filtrada en la BD |
| RF-15 | Reportar labores presenciales y teletrabajo por persona | Días con marcación en relojes presenciales y días de teletrabajo (`v2.7.0`) |
| RF-07 | Reportar vacaciones generadas | 30 días por año completo de servicio (`v2.8.0`) |
| RF-08 | Reportar vacaciones pendientes de goce | Generadas − gozadas; negativo indica adelanto (`v2.8.0`) |
| RF-09 | Reportar vacaciones truncas | 2,5 días por mes del año en curso (`v2.8.0`) |
| RF-10 | Contabilizar vacaciones desde media jornada | Por horas = 0,5 días, mínimo 4 horas (`v2.8.0`) |
| RF-11 | Generar reporte de vacaciones | Menú Vacaciones con totales, detalle y CSV (`v2.8.0`) |
| RF-12 | Consulta de vacaciones para el usuario final | Menú Mis vacaciones, solo el empleado de la propia cuenta (`v2.9.0`) |

## 8. Requerimientos pendientes

No quedan RF sin implementar. Los 18 RF requieren pruebas funcionales con datos de prueba en la aplicación desplegada. Confirmar el agrupamiento de motivos de licencia y el criterio de días calendario para teletrabajo.

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

Compilación web con cero errores y cero advertencias. Resultado de `dotnet test`: 166 aprobadas, cero fallidas y cero omitidas. Entre ellas, 13 casos de reglas de permisos, 6 de licencia anual, 2 de conteo de teletrabajo, 1 de feriados, 21 del cálculo de vacaciones, 5 de la lógica de vacaciones y 3 que comprueban que los reportes se traducen a SQL. Los resultados de pantalla con SQL Server están pendientes y se registrarán en la matriz.

## 13. Problemas técnicos encontrados

El borrado de motivos en uso chocaba con referencias de permisos. Los registros históricos de turnos y permisos pueden carecer de fecha de fin. Se controla el borrado y se usa la fecha de inicio como fin para filtrar esos registros históricos.

## 14. Riesgos pendientes

Los permisos históricos de teletrabajo 125 y 126 requieren una decisión de migración. El script nuevo debe ejecutarse en la base de destino antes de usar el módulo. Debe validarse con el área usuaria si los días calendario y el grupo de motivos de licencia coinciden con el criterio institucional. Las vacaciones dependen de que cada empleado tenga fecha de ingreso en su periodo laboral y de que los motivos de vacaciones contengan «vacación» en su descripción. La consulta personal (RF-12) requiere vincular cada usuario con su empleado en «Usuarios y roles».

## 15. Plan de cierre hacia la Evaluación Final

Revisar y fusionar `dev` en `main` mediante un PR para desplegar `v2.7.0` a `v2.9.1`. Validar los 18 RF en la aplicación desplegada con datos ficticios, registrar resultados y capturas, completar carátula y responsables y revisar las reglas asumidas.

## 16. Conclusiones

El repositorio contiene una versión compilable con turnos, permisos, teletrabajo, presencialidad, vacaciones y la consulta personal: los 8 RF obligatorios y los 10 complementarios. El historial permite rastrear implementación y pruebas automatizadas.

## 17. Recomendaciones

Usar datos ficticios para las capturas. Revisar los cambios en `develop` antes de pasar a `main`, ya que un push a esa rama activa el despliegue.

## 18. Anexos

- [Matriz de solicitudes y casos](actualizados/SOLICITUDES-DE-CAMBIO.md).
- [Guía de versionado](SEMANA-12-VERSIONADO.md).
- [Script SQL de teletrabajo](../Scripts/rf-13-crear-periodo-teletrabajo.sql).
- Capturas de la aplicación, ejecución SQL y publicación en GitHub pendientes de adjuntar.
