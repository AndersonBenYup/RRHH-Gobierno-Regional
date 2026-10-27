# Cambios en la arquitectura

Qué se cambió, por qué y para qué sirve.

## Capas e interfaces (IoC / DIP)

- **Se crearon las interfaces** `IRepositorio<T>`, `IUnidadDeTrabajo` y otras en `Capa Entidades/Contratos`. Las clases de `Capa Datos` (`PermisoData`, `UsuarioData`…) las implementan y las de `Capa Logica` solo reciben la interfaz. Así, cambiar de base de datos solo afecta a `Capa Datos`.
- **Se creó `ConfiguracionCapaDatos`**, el único lugar donde la aplicación conoce la base de datos (**IoC**).
- **Se crearon las pruebas `ArquitecturaTests`**, que fallan si una capa vuelve a depender de la base de datos.

## Código repetido (DRY)

- **Se creó la clase base `RepositorioBase<T>`.** Las 77 clases `*Data` repetían el mismo CRUD; ahora heredan de ella.
- **Se creó la clase base `LogicaBase<T>`**, por el mismo motivo, para las clases `*Logica`.
- **Se creó `PersonaExtensiones.NombreCompleto()`.** El nombre completo se armaba a mano en 10 lugares; ahora se arma en uno solo.
- **El registro de clases en `Program.cs` pasó a ser automático:** 154 líneas manuales menos.

## Una responsabilidad por clase (SRP)

- **Se creó la clase `Numeracion`.** El cálculo "máximo + 1" para generar IDs estaba copiado en 27 lugares de 14 controllers; ahora vive en un solo lugar de `Capa Datos`.
- **Las reglas de negocio pasaron de los controllers a la lógica:** `EmpleadoLogica.RegistrarConPersona`, `FichaSocialLogica`, `CatalogoLogica`. El controller solo recibe la petición y muestra el resultado.

## Abierto a la extensión (OCP)

- **Se creó la interfaz `IExportadorTabla`**, implementada por `ExportadorCsv`. La usan `EmpleadoController` y `CatalogoController` para exportar. Para agregar Excel o PDF basta con crear una clase nueva, sin modificar las que ya existen.

## Inversión de control (IoC)

- **`EmpleadoServicio` ahora depende de `IEmpleadoData` e `IPersonaData`** y no de clases concretas. Se puede probar sin base de datos, con clases falsas.

## Números mágicos → constantes

- **Se crearon `EstadosEmpleado`** (`"A"`, `"I"`), **`ConstantesSeguridad`** (`"Administrador"`, `"0500000"`) y **`Paginacion`** (`25`). Antes esos valores estaban escritos a mano en varios archivos; ahora tienen un nombre claro y se cambian en un solo lugar (**SRP**, Clean Code).

## Clean Code

- **Se eliminaron 26 dependencias** que se inyectaban en los controllers y nunca se usaban.
- **Se renombraron los campos `_xxxData` a `_xxxLogica`**, porque el nombre no correspondía a su tipo.

## Correcciones

- **Script de BD:** se eliminó una FK duplicada y se agregó una FK que faltaba.
- **Admin inicial:** se creaba con un sistema inexistente (`"GRLL"`) y perdía sus permisos al iniciar sesión.
- **Formularios:** los de Permiso, Familiar y Periodo laboral daban error 500.
- **Panel de inicio:** "Feriados del mes" siempre mostraba 0.
- **Ayuda:** la página mostraba la contraseña del administrador.

## Pruebas y configuración

- **Se creó el proyecto `Capa Logica.Tests`** (xUnit): 41 pruebas que no necesitan SQL Server.
- **Se ordenó la configuración:**
  - `publish_test/` salió del repositorio.
  - La clave del admin salió de `appsettings.json`.
  - Los estilos ahora usan Tailwind.
