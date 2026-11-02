# EVO RRHH · Contexto de producto

> Redactado durante SC-011 a partir del brief del proyecto y de la revisión del código.
> Los puntos marcados como *supuesto* no se validaron con usuarios: confírmalos o corrígelos.

## Qué es

Sistema web interno de Recursos Humanos de un gobierno regional. Reemplaza una aplicación de escritorio
VB.NET y concentra la ficha del personal, la asistencia y las marcaciones, los permisos, la capacitación,
la salud, la familia y la vivienda, el historial laboral, la estructura orgánica y la seguridad (usuarios, roles y bitácora).

## Usuarios

- **Registradores de RR.HH.** (principal): cargan y corrigen datos todo el día. Necesitan formularios rápidos,
  mensajes de error claros y tablas fáciles de recorrer.
- **Administrador del sistema**: gestiona usuarios, roles, catálogos y desbloqueos.
- *Supuesto:* trabajan en PC de oficina con monitores de 1366 px o más y luz de oficina; el móvil es una consulta ocasional.

## Modo de la interfaz

**Operate.** Se trabaja sobre la información: la velocidad de lectura, la coherencia y los patrones conocidos
pesan más que la expresividad.

## Tono

Institucional, sobrio y confiable. Sin degradados, brillos ni adornos. Textos en español, en "usted" en las pantallas públicas
(inicio de sesión) y directos en el resto. Los errores dicen qué pasó y cómo corregirlo.

## Principios de diseño

1. **Los datos primero:** tablas densas, cifras tabulares y encabezados discretos.
2. **Un solo color de acción:** azul petróleo (`#0F4C5C`). El ocre solo marca la sección activa del menú y los iconos de apoyo.
3. **Estados claros:** verde para correcto o activo, rojo para error o bloqueado y neutro para inactivo, siempre en fondo suave con texto oscuro.
4. **Accesible:** contraste AA, foco visible y validación también sin JavaScript.

## Sistema visual (SC-011)

- Tipografía: IBM Plex Sans (Google Fonts).
- Superficies: gris piedra `#F5F4F1` de fondo, blanco para tarjetas y tablas, bordes `#DAD6CE`, radio de 6 a 8 px.
- Barra lateral sólida `#0B3440`. Modo oscuro con los mismos roles de color (`data-bs-theme="dark"`).
- Implementación: tokens y componentes en `Proyecto-Evo-RRLL/Estilos/app.css` (Tailwind 3.4 con prefijo `tw-` y sin preflight),
  compilado con `npm run css:build` en `wwwroot/css/app.css`, que se carga después de Bootstrap y `site.css`.
