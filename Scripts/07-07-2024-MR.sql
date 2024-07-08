CREATE TABLE BodegaProducto(
	BodegaProductoId INT NOT NULL IDENTITY,
	BodegaId INT NOT NULL,
	ProductoId INT NOT NULL,
	Cantidad INT NOT NULL
);

ALTER TABLE BodegaProducto
ADD CONSTRAINT PK_BodegaProducto PRIMARY KEY(BodegaProductoId);

ALTER TABLE BodegaProducto
ADD CONSTRAINT FK_BodegaProducto_BodegaId FOREIGN KEY(BodegaId)
REFERENCES Bodega(BodegaId) ON DELETE CASCADE;

ALTER TABLE BodegaProducto
ADD CONSTRAINT FK_BodegaProducto_ProductoId FOREIGN KEY(ProductoId)
REFERENCES Producto(ProductoId) ON DELETE CASCADE;


INSERT INTO BodegaProducto(BodegaId, ProductoId, Cantidad)
VALUES (1,1,5), (1,2,3);