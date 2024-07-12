/* Tabla OrdenCompra */
CREATE TABLE OrdenCompra (
    OrdenCompraId INT NOT NULL IDENTITY,
    ProveedorId INT NOT NULL,
    FechaIngreso DATETIME NOT NULL,
    UsuarioId NVARCHAR(450) NOT NULL,
    TotalOrden DECIMAL(18,2) NOT NULL       
);
-- Restricción de clave primaria
ALTER TABLE OrdenCompra ADD CONSTRAINT Pk_OrdenCompra PRIMARY KEY (OrdenCompraId);

-- Restricciones de claves foraneas
ALTER TABLE OrdenCompra 
ADD CONSTRAINT Fk_OrdenCompra_Proveedor_ProveedorId
FOREIGN KEY (ProveedorId) 
REFERENCES Proveedor (ProveedorId) ON DELETE CASCADE;

ALTER TABLE OrdenCompra 
ADD CONSTRAINT Fk_OrdenCompra_AspNetUsers_Id
FOREIGN KEY (UsuarioId) 
REFERENCES dbo.AspNetUsers (Id) ON DELETE CASCADE;

/* Tabla OrdenCompraDetalle */
CREATE TABLE OrdenCompraDetalle (
    OrdenCompraDetalleId INT NOT NULL IDENTITY,
    OrdenCompraId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    Costo DECIMAL NOT NULL,
    Subtotal AS CAST((Costo * Cantidad) AS DECIMAL(18,2)) PERSISTED
);
-- Restricción de clave primaria
ALTER TABLE OrdenCompraDetalle ADD CONSTRAINT Pk_OrdenCompraDetalle PRIMARY KEY (OrdenCompraDetalleId);

-- Restricciones de claves foraneas
ALTER TABLE OrdenCompraDetalle 
ADD CONSTRAINT Fk_OrdenCompraDetalle_OrdenCompra_OrdenCompraId
FOREIGN KEY (OrdenCompraId) 
REFERENCES OrdenCompra (OrdenCompraId) ON DELETE CASCADE;

ALTER TABLE OrdenCompraDetalle 
ADD CONSTRAINT Fk_OrdenCompraDetalle_ProductoProveedor_ProductoProveedorId
FOREIGN KEY (ProductoId) 
REFERENCES ProductoProveedor (ProductoProveedorId);

-- Restricción para que columna Cantidad no acepte valores negativos
ALTER TABLE OrdenCompraDetalle ADD CONSTRAINT CHK_CantidadPositiva CHECK (Cantidad >= 0 );

/* Desencadenadores para actualizar el valor de la columna TotalOrden de la tabla OrdenCompra */

-- Desencadenador cuando se inserta o se actualiza un registro en la tabla OrdenCompraDetalle
CREATE TRIGGER trg_UpdateTotalOrden_InsertUpdate
ON OrdenCompraDetalle
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Actualizar el TotalOrden en la tabla OrdenCompra
    UPDATE oc
    SET oc.TotalOrden = (
        SELECT SUM(d.Costo * d.Cantidad)
        FROM OrdenCompraDetalle d
        WHERE d.OrdenCompraId = oc.OrdenCompraId
    )
    FROM OrdenCompra oc
    INNER JOIN inserted i ON oc.OrdenCompraId = i.OrdenCompraId;
END;

-- Desencadenador cuando se elimina un registro en la tabla OrdenCompraDetalle
CREATE TRIGGER trg_UpdateTotalOrden_Delete
ON OrdenCompraDetalle
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Actualizar el TotalOrden en la tabla OrdenCompra
    UPDATE oc
    SET oc.TotalOrden = (
        SELECT SUM(d.Costo * d.Cantidad)
        FROM OrdenCompraDetalle d
        WHERE d.OrdenCompraId = oc.OrdenCompraId
    )
    FROM OrdenCompra oc
    INNER JOIN deleted d ON oc.OrdenCompraId = d.OrdenCompraId;
END;





