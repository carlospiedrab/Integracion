USE DB_141102_integraciond;
GO

CREATE TABLE KardexInventario(
	KardexInventarioId INT NOT NULL IDENTITY,
	BodegaProductoId INT NOT NULL,
	Tipo NVARCHAR(50) NOT NULL,
	Detalle NVARCHAR(500) NOT NULL,
	StockAnterior INT NOT NULL,
	Cantidad INT NOT NULL,
	Costo DECIMAL NOT NULL,
	Stock INT NOT NULL,
	Total DECIMAL NOT NULL,
	UsuarioId NVARCHAR(450) NOT NULL,
	FechaRegistro DATETIME NOT NULL
)
ALTER TABLE KardexInventario
ADD CONSTRAINT PK_KardexInventario PRIMARY KEY(KardexInventarioId);

ALTER TABLE KardexInventario
ADD CONSTRAINT FK_KardexInventario_BodegaProducto FOREIGN KEY(BodegaProductoId)
REFERENCES BodegaProducto(BodegaProductoId) ON DELETE CASCADE;

ALTER TABLE KardexInventario
ADD CONSTRAINT FK_KardexInventario_AspNetUsers FOREIGN KEY(UsuarioId)
REFERENCES AspNetUsers(Id) ON DELETE CASCADE;