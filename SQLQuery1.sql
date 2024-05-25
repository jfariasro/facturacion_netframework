use master;
go

if db_id('prueba') is not null
begin
	drop database prueba;
end
go

create database prueba;
go

use prueba;
go

create table secuencial(
idsecuencial int primary key identity,
numero int,
prefijo varchar(50)
);
go

insert into secuencial(numero, prefijo) values(0, 'FAC');
go

create procedure ObtenerSecuencial
@prefijo varchar(50), @NumeroSecuencial int output
as
begin
	update secuencial set @NumeroSecuencial = numero, numero = numero + 1 where prefijo = @prefijo
end
go

create table venta(
idventa int primary key identity,
fecha datetime2 default current_timestamp,
cliente varchar(50)
)
go

create procedure InsertarVenta
@cliente varchar(50),
@id int output
as
begin
	insert into venta(cliente) values(@cliente);
	SET @id = SCOPE_IDENTITY();
    SELECT @id AS id;
end
go

create procedure ModificarVenta
@idventa int, @cliente varchar(50)
as
begin
	update venta set cliente = @cliente where idventa = @idventa
end
go

create procedure ConsultarVenta
@idventa int
as
begin
	select * from venta where idventa = @idventa
end
go

create procedure EliminarVenta
@idventa int
as
begin
	delete from venta where idventa = @idventa
end
go

create table detalleventa(
iddetalleventa int primary key identity,
idventa int foreign key references venta(idventa),
producto varchar(50),
cantidad int,
precio decimal(10,2),
iva real,
precio_iva decimal(10,2),
total decimal(10,2),
total_iva decimal(10,2)
)
go

create procedure insertar_detalle
@idventa int,
@producto varchar(50),
@cantidad int,
@precio real,
@iva real,
@precio_iva real,
@total real,
@total_iva real
as
begin
insert into detalleventa(idventa, producto, cantidad, precio, iva, precio_iva, total, total_iva)
values(@idventa, @producto, @cantidad, @precio, @iva, @precio_iva, @total, @total_iva)
end
go

create procedure modificar_detalle
@idetalleventa int,
@idventa int,
@producto varchar(50),
@cantidad int,
@precio real,
@iva real,
@precio_iva real,
@total real,
@total_iva real
as
begin
	update detalleventa set
	idventa = @idventa, producto = @producto, cantidad = @cantidad, precio = @precio,
	iva = @iva, precio_iva = @precio_iva, total = @total, total_iva = @total_iva
end
go

create procedure consultar_detalle_venta
@idventa int
as
begin
	select dv.iddetalleventa, dv.producto, dv.cantidad, dv.precio, dv.iva, dv.precio_iva, dv.total, dv.total_iva
	from venta v join detalleventa dv on v.idventa = dv.idventa
	where dv.idventa = @idventa
end
go

create procedure eliminar_detalle
@iddetalleventa int
as
begin
	delete from detalleventa where iddetalleventa = @iddetalleventa
end
go

create procedure eliminar_toda_venta
@idventa int
as
begin
	delete from detalleventa where idventa = @idventa
end
go

create procedure consultar_venta_total
@idventa int
as
begin
	select dv.idventa, v.fecha, v.cliente, total_pago = sum(dv.total), total_pago_iva = sum(dv.total_iva)
	from venta v join detalleventa dv on v.idventa = dv.idventa
	where dv.idventa = @idventa
	group by dv.idventa, v.fecha, v.cliente
end
go

