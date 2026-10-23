<#
  Sube la BD RRHH_Normalizado local (ya cargada con 01-esquema + 03-datos) a Azure SQL.

  Se usa un .bacpac (SqlPackage) en vez de ejecutar los .sql en Azure porque:
    - 01-esquema usa CREATE/DROP DATABASE y 03-datos usa USE y sp_MSforeachtable,
      que no existen en Azure SQL Database.
    - SqlPackage importa sobre la BD gratis ya creada (vacia) desde el portal y valida la compatibilidad.

  Requisitos:
    - SqlPackage: dotnet tool install -g microsoft.sqlpackage
    - BD RRHH_Normalizado creada vacia en el portal (oferta gratis, collation Modern_Spanish_CI_AS)
    - Regla de firewall del servidor con la IP de esta PC

  Ejemplo:
    .\02-subir-bd.ps1 -Servidor sql-rrhh-gobierno-regional -ClaveSql 'Cl4ve-SQL-Larga!'
#>
param(
    [Parameter(Mandatory)] [string] $Servidor,       # nombre del servidor SQL de Azure (con o sin .database.windows.net)
    [Parameter(Mandatory)] [string] $ClaveSql,
    [string] $AdminSql = 'evoadmin',
    [string] $ServidorLocal = '.',
    [string] $Bacpac = "$env:TEMP\RRHH_Normalizado.bacpac"
)

$ErrorActionPreference = 'Stop'
$bd = 'RRHH_Normalizado'
$servidorAzure = if ($Servidor -like '*.database.windows.net') { $Servidor } else { "$Servidor.database.windows.net" }

Write-Host "== Exportando $bd desde $ServidorLocal a $Bacpac"
SqlPackage /Action:Export `
    /SourceServerName:$ServidorLocal /SourceDatabaseName:$bd /SourceTrustServerCertificate:True `
    /TargetFile:$Bacpac /OverwriteFiles:True
if ($LASTEXITCODE -ne 0) { throw 'Fallo la exportacion' }

Write-Host "== Importando en $servidorAzure"
SqlPackage /Action:Import `
    /SourceFile:$Bacpac `
    /TargetServerName:$servidorAzure /TargetDatabaseName:$bd `
    /TargetUser:$AdminSql /TargetPassword:$ClaveSql /TargetEncryptConnection:True
if ($LASTEXITCODE -ne 0) { throw 'Fallo la importacion' }

Write-Host "== Verificando conteos en Azure"
sqlcmd -S "tcp:$servidorAzure,1433" -d $bd -U $AdminSql -P $ClaveSql -N -l 60 -W -Q @"
SET NOCOUNT ON;
SELECT t.name AS Tabla, SUM(p.rows) AS Filas
FROM sys.tables t JOIN sys.partitions p ON p.object_id = t.object_id AND p.index_id IN (0,1)
WHERE t.name IN ('Empleado','Persona','Marcacion','RegAsisDiario','Usuario')
GROUP BY t.name ORDER BY t.name;
SELECT COUNT(*) AS FKs_no_confiables FROM sys.foreign_keys WHERE is_not_trusted = 1;
"@
Write-Host "Esperado: Empleado 3335, Persona 4106, Marcacion 200184, RegAsisDiario 263520, FKs_no_confiables 0."
