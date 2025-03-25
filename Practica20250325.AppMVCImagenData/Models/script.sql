-- Crear la base de datos Practica20250325DB
CREATE DATABASE Practica20250325IDataDB;
GO

-- Usar la base de datos recién creada
USE Practica20250325IDataDB;
GO

-- Crear la tabla Productos
CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(255) NOT NULL,
    Descripcion TEXT,
    Precio DECIMAL(10, 2) NOT NULL,
    ImagenBytes VARBINARY(MAX) -- Cambiado a VARBINARY(MAX)
);
GO