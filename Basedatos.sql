IF DB_ID('LoginBCryptDB') IS NULL
    CREATE DATABASE LoginBCryptDB;
GO

USE LoginBCryptDB;
GO

CREATE TABLE Usuarios (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario   NVARCHAR(50)  NOT NULL UNIQUE,
    NombreCompleto  NVARCHAR(100) NOT NULL,
    PasswordHash    NVARCHAR(100) NOT NULL,
    FechaRegistro   DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE IntentosLogin (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario   NVARCHAR(50) NOT NULL,
    Exitoso         BIT          NOT NULL,   -- 1 = exitoso, 0 = fallido
    Fecha           DATETIME     NOT NULL DEFAULT GETDATE()
);
GO