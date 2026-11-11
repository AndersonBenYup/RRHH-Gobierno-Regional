-- Ejecutar en la base de datos normalizada antes de activar RF-13.
IF OBJECT_ID(N'dbo.PeriodoTeletrabajo', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PeriodoTeletrabajo (
        Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        IdEmpleado int NOT NULL REFERENCES dbo.Empleado(IdEmpleado),
        FechaInicio date NOT NULL,
        FechaFin date NOT NULL,
        Parcial bit NOT NULL,
        CONSTRAINT CK_PeriodoTeletrabajo_Fechas CHECK (FechaFin >= FechaInicio),
        CONSTRAINT UQ_PeriodoTeletrabajo_Registro UNIQUE (IdEmpleado, FechaInicio, FechaFin, Parcial)
    );
END;
