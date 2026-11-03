# Product

<!-- impeccable:product-schema 1 -->

## Platform

web

## Users

- **Registradores de RR.HH.** (usuario principal): pasan la jornada cargando y corrigiendo datos del personal.
  Para eso necesitan formularios rápidos, mensajes de error claros y tablas fáciles de recorrer.
- **Administrador del sistema:** gestiona usuarios, roles, catálogos y desbloqueos de cuenta.
- No hay otros usuarios. Los jefes de área, los empleados y auditoría no entran al sistema (confirmado).
- Trabajan en PC de oficina con monitores de 1366 px o más, en horario de oficina (confirmado).
  El móvil se usa solo para alguna consulta ocasional.

## Product Purpose

Sistema web interno de Recursos Humanos de un gobierno regional. Reemplaza una aplicación de escritorio en VB.NET
y reúne en un solo lugar:

- la ficha del personal;
- la asistencia y las marcaciones;
- los permisos y los feriados;
- la capacitación;
- la salud, la familia y la vivienda;
- el historial laboral;
- la estructura orgánica;
- la seguridad: usuarios, roles y bitácora.

Tiene éxito cuando RR.HH. registra y consulta sin errores ni demoras, y cuando cada cambio queda trazado.

## Positioning

Herramienta institucional de uso interno. No compite con otros productos: su valor es cubrir toda la ficha de cada
empleado sobre la base de datos real de la institución (`RRHH_Normalizado`), con las reglas de negocio validadas tanto en el
navegador como en el servidor.

## Operating Context

- Lo usa la oficina de RR.HH. todo el día. Las tareas típicas son buscar un empleado, abrir su ficha, registrar o corregir
  un permiso, revisar la asistencia de un rango de fechas y exportarla a CSV.
- Cada cambio entra como una solicitud de cambio numerada. Los commits llevan el prefijo `SC-NNN:` y la matriz de cambios
  está en `docs/GESTION-DEL-CAMBIO.md`.
- Cada push a `main` corre las pruebas y despliega a Azure (Web App `rrhh-gobierno-regional` y Azure SQL).

## Capabilities and Constraints

- **Stack:**
  - ASP.NET Core MVC sobre .NET 10.
  - EF Core 10 con SQL Server, en capas: Entidades, Datos, Lógica y Web.
  - Bootstrap 5 junto a Tailwind 3.4, este último con prefijo `tw-` y sin preflight.
- **CSP estricta:** solo recursos propios y Google Fonts; no se usan CDNs de scripts.
- **Datos heredados:** hay registros antiguos que no cumplen las validaciones nuevas, y se corrigen al editarlos.
  El esquema de la BD no se cambia sin una solicitud de cambio.
- **Idioma:** español. En la BD el rol del administrador figura como `ADMINISTRADOR`.
- **Pendiente:** permisos por opción del menú (`Rol_Acceso`).

## Brand Commitments

- Tono institucional, sobrio y confiable.
- En las pantallas públicas (inicio de sesión) se trata al usuario de "usted"; en el resto, los textos son directos.
- Los errores dicen qué pasó y cómo corregirlo.
- Dirección "Institucional sobrio", elegida en SC-011: azul petróleo como color de acción e Public Sans.
  El detalle está implementado en `Proyecto-Evo-RRLL/Estilos/app.css`.

## Evidence on Hand

- Datos reales en la BD local y en Azure. Por ejemplo, hay unos 26 000 permisos y 11 600 registros de asistencia exportables.
- Documentación del proyecto en `docs/` (entregables, gestión del cambio, arquitectura) y en `temas-semanas/`
  (exposiciones semanales).
- No hay un logo oficial ni un manual de marca del gobierno regional en el repositorio. No hay que inventarlos ni
  simular un escudo institucional.

## Product Principles

1. **Los datos primero:** las pantallas sirven para leer y corregir información rápido; la expresividad viene después.
2. **Ningún dato inválido entra:** cada regla se valida en el navegador y se repite en el servidor.
3. **Todo queda trazado:** los accesos y los cambios sensibles se registran en la bitácora.
4. **Mínimo privilegio sobre datos sensibles:** solo se muestra lo que la tarea necesita.

## Accessibility & Inclusion

- **WCAG 2.x nivel AA**, como estándar exigido: contraste AA, foco visible, navegación con teclado y formularios que
  validan también con JavaScript desactivado.
- **Protección de datos personales (Ley 29733):**
  - El DNI, la salud y la familia son datos sensibles.
  - No se exponen en URLs, exportaciones ni pantallas públicas más allá de lo necesario.
  - Las claves nunca se muestran ni se versionan.
