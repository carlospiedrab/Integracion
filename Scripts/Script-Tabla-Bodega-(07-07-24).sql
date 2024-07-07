Create Table Bodega
(
     BodegaId Int Not Null Primary Key Identity(1, 1),
     Estado Bit Not Null,
     NombreBodega NVarChar(120) Not Null,
     Descripcion NVarChar(160) Not Null
)