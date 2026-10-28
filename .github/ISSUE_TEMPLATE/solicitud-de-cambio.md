---
name: Solicitud de cambio
about: Registrar una solicitud de cambio (SC) para su análisis, aprobación y control
title: "SC-NNN: "
labels: ["solicitud-de-cambio"]
assignees: ""
---

<!-- Usa el siguiente número libre de la matriz en docs/GESTION-DEL-CAMBIO.md (ej. SC-008). -->

## 1. Registro

| Campo | Valor |
|---|---|
| Código | SC-NNN |
| Solicitante / área | |
| Fecha | dd/mm/aaaa |
| Módulo(s) | |

**Tipo de cambio**
- [ ] Correctivo
- [ ] Adaptativo
- [ ] Perfectivo
- [ ] Preventivo

**Prioridad**
- [ ] Alta
- [ ] Media
- [ ] Baja

## 2. Descripción

**Descripción del cambio:**

**Problema o necesidad que lo origina:**

**Beneficio esperado:**

**Riesgo de no realizar el cambio:**

## 3. Análisis de impacto

<!-- Nivel: Alto / Medio / Bajo / Positivo. Lo completa el analista o líder técnico. -->

| Aspecto | Cómo lo afecta | Nivel |
|---|---|---|
| Funcionalidades existentes | | |
| Código fuente | | |
| Base de datos / estructura | | |
| Pruebas | | |
| Tiempo de desarrollo | | |
| Mantenibilidad | | |
| Riesgos técnicos | | |

## 4. Decisión del comité (CCB)

<!-- Lo completa el comité. El acta queda como comentario en este issue. -->

- [ ] Aprobado
- [ ] Rechazado
- [ ] Postergado
- [ ] Aprobado con condiciones

**Justificación técnica:**

**Condiciones (si aplica):**

## 5. Plan de implementación

| # | Actividad | Responsable | Tiempo | Día |
|---|---|---|---|---|
| 1 | | | | |

**Archivos, módulos o componentes afectados:**

**Pruebas necesarias:**

**Evidencias esperadas:**

## 6. Control del cambio

- Rama: `feature/SC-NNN-descripcion`
- Commits con prefijo `SC-NNN:`
- Pull Request: `Closes #` (este issue)
- Versión / etiqueta:
- [ ] `dotnet test` en verde
- [ ] PR revisado y aprobado
- [ ] Matriz actualizada en `docs/GESTION-DEL-CAMBIO.md`
