# Proyecto-Evo_RRLL

Sistema de Recursos Humanos (RR.HH.) en ASP.NET Core MVC (net10.0), arquitectura por capas: Entidades, Datos y Lógica.

La aplicación trabaja sobre la base de datos normalizada **`RRHH_Normalizado`** (ver `Scripts/bd-Normalizada/NORMALIZACION.md`).

## Requisitos

- .NET 10 SDK
- SQL Server (o LocalDB)
- SSMS o `sqlcmd` para los scripts

## Cómo levantar el proyecto en local

1. Clonar el repositorio.
2. Crear la BD vacía: ejecutar `Scripts/bd-Normalizada/01-esquema-RRHH_Normalizado.sql`.
3. Cargar los datos: ejecutar `Scripts/bd-Normalizada/03-datos-RRHH_Normalizado.sql` (~160 MB, no está en GitHub; pídelo al equipo).
   Para archivos tan grandes es mejor `sqlcmd` que SSMS:
   ```
   sqlcmd -S . -E -C -i Scripts\bd-Normalizada\03-datos-RRHH_Normalizado.sql
   ```
4. Revisar la cadena de conexión en `Proyecto-Evo-RRLL/appsettings.Development.json`.
5. Abrir el proyecto en Visual Studio y presionar **F5**.
6. Iniciar sesión con el usuario creado automáticamente al arrancar:
   - **Usuario:** `admin`
   - **Contraseña:** `Admin.2026` (solo en desarrollo)

## Conexión a la base de datos

En local se usa `Proyecto-Evo-RRLL/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "RRHHNormalizado": "Server=.;Database=RRHH_Normalizado;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

Cambia `Server=` según tu caso (`(localdb)\\MSSQLLocalDB`, `NOMBRE-PC\\SQLEXPRESS`, `IP,1433`, o `User Id=...;Password=...;` en vez de `Trusted_Connection`).

En producción (Azure) la cadena y la clave inicial del admin **no** están en el repositorio: se configuran en la Web App
(`ConnectionStrings:RRHHNormalizado` y `Seguridad__AdminClave`).

## Despliegue en Azure

Recursos (grupo `RRHH`, región **Brazil South**):

- Web App `rrhh-gobierno-regional` (plan `ASP-RRHH-8913`, gratis), con implementación continua desde GitHub:
  el portal agrega el workflow en `.github/workflows/` y publica en cada push a `main`.
- Azure SQL Database `RRHH_Normalizado` con la oferta gratuita (serverless, auto-pausa),
  creada desde el portal con collation `Modern_Spanish_CI_AS`, "Allow Azure services" activado
  y una regla de firewall para la IP de quien sube la BD.

Subir la BD (desde la PC con `RRHH_Normalizado` cargada en local):

```powershell
dotnet tool install -g microsoft.sqlpackage
.\Scripts\azure\02-subir-bd.ps1 -Servidor <servidor-sql> -AdminSql <usuario> -ClaveSql '<clave>'
```

Configurar la Web App (Settings > Environment variables):

- Connection string `RRHHNormalizado`, tipo **SQLAzure**:
  `Server=tcp:<servidor-sql>.database.windows.net,1433;Database=RRHH_Normalizado;User ID=<usuario>;Password=<clave>;Encrypt=True;TrustServerCertificate=False;MultipleActiveResultSets=True;Connection Timeout=60`
- App setting `Seguridad__AdminClave`: clave inicial del usuario `admin` (se crea en el primer arranque).
- App setting `ASPNETCORE_ENVIRONMENT` = `Production`.
