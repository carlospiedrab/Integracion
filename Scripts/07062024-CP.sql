-- Tabla de Proveedor
CREATE TABLE Proveedor (
 ProveedorId int NOT NULL IDENTITY,
 Nombre nvarchar(120) NOT NULL,
 Descripcion nvarchar(200) NOT NULL,
 Direccion nvarchar(180) NOT NULL,
 Telefono nvarchar(40) NOT NULL, 
 Email nvarchar(100) NOT NULL, 
 Estado bit NOT NULL,
 );

ALTER TABLE Proveedor
ADD Constraint PK_Proveedor PRIMARY KEY (ProveedorId);
