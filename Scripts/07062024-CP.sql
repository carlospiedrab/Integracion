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


INSERT INTO Proveedor (Nombre, Descripcion, Direccion, Telefono, Email, Estado)
Values ('Proveedor de Microsoft','Proveedor de Productos y Servicios de Microsoft','352 Broadway ave, New York', '325-989-9896','support@microsoftes.es',1);

INSERT INTO Proveedor (Nombre, Descripcion, Direccion, Telefono, Email, Estado)
Values ('Proveedor de Mac','Proveedor de Productos y Servicios de Mac','600 Broadway ave, New York', '323-856-9686','support@mac.es',1);