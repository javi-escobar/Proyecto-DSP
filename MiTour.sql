------------------ SIGUE LOS PASOS PARA EJECUTAR EL QUERY Y QUE LA BASE DE DATOS Y SUS PROCEDIMIENTOS ALMACENADOS NO TENGAN PROBLEMAS --------------------------------------
------------------------------------------------------------ EJECUTAR HASTA VER UN AVISO ----------------------------------------------------------------------------------

-- Base de datos "MiTour"
USE master
GO

CREATE DATABASE MiTour;
go

USE MiTour;
go

-- Tabla de Clientes
CREATE TABLE Clientes (
    ClienteID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20),
);
go

-- Tabla de Tours
CREATE TABLE Tours (
    TourID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    TipoTour NVARCHAR(50) NOT NULL,
    Nacional BIT NOT NULL, -- Indica si el tour es nacional (1) o internacional (0)
    Transporte NVARCHAR(50) NOT NULL, -- Tipo de transporte (bus o microbús)
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    CantidadPersonas INT,
    Precio DECIMAL(10, 2) NOT NULL,
);
go

-- Tabla de Reservas
CREATE TABLE Reservas (
    ReservaID INT PRIMARY KEY IDENTITY(1,1),
    ClienteID INT REFERENCES Clientes(ClienteID),
    TourID INT REFERENCES Tours(TourID),
    FechaReserva DATE NOT NULL,
    Descuento DECIMAL(5, 2),
);
go

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
	Correo NVARCHAR(100) NOT NULL,
    Clave NVARCHAR(500) NOT NULL,
);
go

--Datos para la tabla Clientes
SET IDENTITY_INSERT Clientes ON;
GO

INSERT INTO Clientes(ClienteID, Nombre, Email, Telefono) VALUES
(1, 'Juan Hernández', 'juanher@gmail.com', '7700-2424'),
(2, 'Mónica Valladares', 'valladaresmoon@gmail.com', '2123-5678'),
(3, 'David Fernández', 'davefernandez@gmail.com', '7899-0012'),
(4, 'Lucia Alas', 'lucyalas@gmail.com', '6345-8900'),
(5 , 'María León', 'mapileon@gmail.com', '7444-2356'),
(6 , 'Irene Paredes', 'ireneparedes@gmail.com', '7471-1120'),
(7 , 'Jesús Vázques', 'jesusvazques@gmail.com', '6578-9901'),
(8 , 'Nicolás Dominguez', 'domingueznico@gmail.com', '7114-4466'),
(9 , 'Rodrigo Hernández', 'rodri5hernan@gmail.com', '6155-5014'),
(10 , 'Aitana Bonmatí', 'aitanabonmatí@gmail.com', '7089-1245')

SET IDENTITY_INSERT Clientes OFF;
GO


--Datos para la tabla Tours
SET IDENTITY_INSERT Tours ON;
GO

INSERT INTO Tours(TourID, Nombre, TipoTour, Nacional, Transporte, FechaInicio, FechaFin, Precio)
VALUES
(1, 'Viaje a Suchitoto', 'Nacional', 1, 'Bus', '2023-11-20', '2023-11-20', '25.00'),
(2, 'Conoce Antigua Guatemala', 'Internacional', 0, 'Microbus', '2023-11-20', '2023-11-22', '75.00'),
(3, 'Tour al Lago de Coatepeque', 'Nacional', 1, 'Microbus', '2023-11-13', '2023-11-15', '35.00'),
(4, 'Viaje a San Miguel', 'Nacional', 1, 'Bus', '2023-11-13', '2023-11-13', '25.00'),
(5, 'Tour a Managua y el Lago Cocibolca', 'Internacional', 0, 'Microbus', '2023-11-17', '2023-11-18', '75.00'),
(6, 'Ruta Turística hacia San Pedro Sula', 'Internacional', 0, 'Bus', '2023-11-15', '2023-11-16', '50.00'),
(7, 'Tour al Lago Güija', 'Nacional', 1, 'Microbus', '2023-12-01', '2023-12-01', '35.00'),
(8, 'Campamento en el Cerro el Pital', 'Nacional', 1, 'Bus', '2023-12-04', '2023-12-07', '25.00'),
(9, 'Conoce Guatemala', 'Internacional', 0, 'Microbus', '2023-12-02', '2023-12-03', '75.00'),
(10, 'Viaje a San José Costa Rica', 'Internacional', 0, 'Bus', '2023-12-01', '2023-12-04', '50'),
(11, 'Tour a Suchitoto', 'Nacional', 1, 'Bus', '2023-10-15', '2023-10-15', '25.00'),
(12, 'Ruta a La Libertad', 'Nacional', 1, 'Microbus', '2023-09-25', '2023-09-27', '35.00'),
(13, 'Descubre Tazumal', 'Nacional', 1, 'Bus', '2023-09-10', '2023-09-11', '25.00'),
(14, 'Tour a Copán, Honduras', 'Internacional', 0, 'Microbus', '2023-08-20', '2023-08-22', '75.00'),
(15, 'Visita a León, Nicaragua', 'Internacional', 0, 'Bus', '2023-07-12', '2023-07-15', '50.00'),
(16, 'Excursión a Antigua, Guatemala', 'Internacional', 0, 'Microbus', '2023-06-05', '2023-06-07', '75.00'),
(17, 'Recorrido por Granada, Nicaragua', 'Internacional', 0, 'Bus', '2023-05-18', '2023-05-20', '50.00'),
(18, 'Tour a San Salvador', 'Nacional', 1, 'Bus', '2023-04-02', '2023-04-02', '25.00'),
(19, 'Descubre San Vicente', 'Nacional', 1, 'Microbus', '2023-03-08', '2023-03-10', '35.00'),
(20, 'Ruta a Suchitoto', 'Nacional', 1, 'Microbus', '2023-02-15', '2023-02-15', '35.00');

SET IDENTITY_INSERT Tours OFF;
GO

-------------------------------------------------------EJECUTAR HASTA AQUÍ-------------------------------------------------------------------------------------------------------


---------------------------------------------EJECUTAR PRIMERO LA CREACIÓN DEL TRIGGER---------------------------------------------------------------------------------------

--Trigger que actualiza la cantidad de personas por tour al crear una reserva
CREATE TRIGGER ActualizarCantidadPersonasTrigger
ON Reservas
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE Tours
    SET CantidadPersonas = (
        SELECT COUNT(*)
        FROM Reservas
        WHERE Reservas.TourID = Tours.TourID
    )
    WHERE Tours.TourID IN (
        SELECT TourID
        FROM inserted
    );
END;

-------------------------------------------------------EJECUTAR HASTA AQUÍ---------------------------------------------------------------------------------------------------

---------------------------------------------EJECUTAR PRIMERO LA CREACIÓN DEL TRIGGER---------------------------------------------------------------------------------------

--Trigger que calcula el pago de la reserva según el precio del tour y el descuento 
CREATE TRIGGER CalcularDescuento
ON Reservas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Actualizar el precio con descuento para las filas afectadas
    UPDATE Reservas
    SET Descuento = (SELECT Precio FROM Tours WHERE Tours.TourID = Reservas.TourID) -
        CASE 
            WHEN DATEPART(WEEKDAY, Reservas.FechaReserva) BETWEEN 2 AND 4 THEN 0.10 -- Lunes a Jueves
            WHEN DATEPART(WEEKDAY, Reservas.FechaReserva) IN (5, 6) THEN 0.05 -- Viernes y Sábado
            ELSE 0.00 -- Domingo
        END * (SELECT Precio FROM Tours WHERE Tours.TourID = Reservas.TourID)
    FROM Reservas
    INNER JOIN INSERTED ON Reservas.ReservaID = INSERTED.ReservaID;

END;



-------------------------------------------------------EJECUTAR HASTA AQUÍ----------------------------------------------------------------------------------------------------

-----------------------------------------EJECUTAR SOLAMENTE EL INSERT DE DATOS A LA TABLA RESERVAS---------------------------------------------------------------------------

--Datos para la tabla Reservas
SET IDENTITY_INSERT Reservas ON;
GO

INSERT INTO Reservas(ReservaID, ClienteID, TourID, FechaReserva) VALUES
(1, 1, 1, '2023-11-08'),
(2, 8, 5, '2023-11-09'),
(3, 4, 1, '2023-11-08'),
(4, 7, 10, '2023-11-12'),
(5, 2, 7, '2023-11-12'),
(6, 10, 6, '2023-11-11'),
(7, 9, 2, '2023-11-10'),
(8, 1, 8, '2023-11-14'),
(9, 3, 8, '2023-11-15'),
(10, 10, 7, '2023-11-17'),
(11, 5, 3, '2023-11-18'),
(12, 2, 9, '2023-11-19'),
(13, 1, 4, '2023-11-20'),
(14, 1, 11, '2023-10-13'),
(15, 6, 12, '2023-09-20'),
(16, 2, 13, '2023-09-05'),
(17, 10, 14, '2023-08-14'),
(18, 4, 15, '2023-07-10'),
(19, 8, 16, '2023-06-01'),
(20, 3, 17, '2023-05-14'),
(21, 5, 18, '2023-03-29'),
(22, 9, 19, '2023-03-05'),
(23, 7, 20, '2023-02-13'),
(24, 9, 4, '2023-03-05'),
(25, 7, 9, '2023-02-13');

SET IDENTITY_INSERT Reservas OFF;
GO

---------------------------------------------EJECUTAR SOLAMENTE LA CREACIÓN DEL PROCEDIMIENTO--------------------------------------------------------

--Procedimientos para el login de usuarios

--Registra el usuario y comprueba que el correo ingresado no exista en la base de datos
CREATE PROC RegistrarUsuario(
@Correo varchar(100),
@Clave varchar(500),
@Registrado bit output,
@Mensaje varchar(100) output)
AS
BEGIN
	IF(NOT EXISTS (SELECT * FROM Usuarios WHERE Correo = @Correo))
	BEGIN 
		INSERT INTO Usuarios(Correo, Clave) VALUES(@Correo, @Clave)
		SET @Registrado = 1
		SET @Mensaje = 'Usuario Registrado'
	END
		ELSE
		BEGIN
		SET @Registrado = 0
		SET @Mensaje = 'Correo ya existente'
	END
END
--------------------------------------------------------- HASTA AQUÍ ----------------------------------------------------------------------------------------------------------------

------------------------------------------------- EJECUTAR PRIMERO LA CREACIÓN DEL PROCEDIMIENTO ---------------------------------------------------------------------------

--Valida el ingreso de usuario
CREATE PROC ValidarUsuario(
@Correo varchar(100),
@Clave varchar(500))
AS
BEGIN
	IF(EXISTS(SELECT * FROM Usuarios WHERE Correo = @Correo AND Clave = @Clave))
		SELECT UsuarioID FROM Usuarios WHERE Correo = @Correo AND Clave = @Clave
	ELSE
		SELECT '0'
END
---------------------------------------------- EJECUTAR EL PROCEDIMIENTO CREADO --------------------------------------------------------------------------------------------
DECLARE @Registrado bit, @Mensaje varchar(100)
EXEC RegistrarUsuario 'javi@gmail.com', '7fb8f3e5aed25cece125cbbf08df7a47ce67cc46f60d2ce0a96e283c9173488e', @Registrado output, @Mensaje output
EXEC RegistrarUsuario 'erick@gmail.com', '3106d9d44b21b4eaec16cd39851ab089666de8faa5b656e5b994224fa3dcc2f2', @Registrado output, @Mensaje output
EXEC RegistrarUsuario 'enrique@gmail.com', 'aec70b6af7d213ba80c2297dfb51df4700d03d33e5846f3aa44f13142980e77f', @Registrado output, @Mensaje output
EXEC RegistrarUsuario 'kevin@gmail.com', '85f5e10431f69bc2a14046a13aabaefc660103b6de7a84f75c4b96181d03f0b5', @Registrado output, @Mensaje output
EXEC RegistrarUsuario 'alexis@gmail.com', '8c4fd8b2c24ffcc223dbf09088bd79734e8404cd4d9e90fc418ecb490622d1ca', @Registrado output, @Mensaje output
SELECT @Registrado
SELECT @Mensaje
--------------------------------------------------------- HASTA AQUÍ --------------------------------------------------------------------------------------------------------

---------------------------------------------- EJECUTAR EL PROCEDIMIENTO CREADO --------------------------------------------------------------------------------------------

EXEC ValidarUsuario 'javi1@gmail.com', '7fb8f3e5aed25cece125cbbf08df7a47ce67cc46f60d2ce0a96e283c9173488e'

--------------------------------------------------------- HASTA AQUÍ --------------------------------------------------------------------------------------------------------

------------------------------------------------ EJECUTAR PRIMERO LA CREACIÓN DEL PROCEDIMIENTO -------------------------------------------------------------------------

-- Procedimiento para ver los tours con la mayor demanda
CREATE PROC VerToursConMayorCantidadPersonas
AS
BEGIN
    SELECT TOP 5 TourID, Nombre, CantidadPersonas
    FROM Tours
    ORDER BY CantidadPersonas DESC;
END;

--------------------------------------------------------- HASTA AQUÍ --------------------------------------------------------------------------------------------------------

--Ejemplo
EXEC VerToursConMayorCantidadPersonas



------------------------------------------------ EJECUTAR PRIMERO LA CREACIÓN DEL PROCEDIMIENTO -------------------------------------------------------------------------

-- Procedimiento para ver los tours con la menor demanda
CREATE PROC VerToursConMenorCantidadPersonas
AS
BEGIN
    SELECT TOP 5 TourID, Nombre, CantidadPersonas
    FROM Tours
    ORDER BY CantidadPersonas ASC;
END;

--------------------------------------------------------- HASTA AQUÍ --------------------------------------------------------------------------------------------------------

--Ejemplo
EXEC VerToursConMenorCantidadPersonas



------------------------------------------------ EJECUTAR PRIMERO LA CREACIÓN DEL PROCEDIMIENTO -------------------------------------------------------------------------

-- Procedimiento para ver la cantidad de reservas de un cliente en un rango de fechas
CREATE PROC VerCantidadDeReservasPorCliente
@ClienteID INT,
@FechaInicio DATE,
@FechaFin DATE
AS
BEGIN
    SELECT COUNT(*) AS CantidadDeReservas
    FROM Reservas
    WHERE ClienteID = @ClienteID
    AND FechaReserva >= @FechaInicio
    AND FechaReserva <= @FechaFin;
END;

--------------------------------------------------------- HASTA AQUÍ --------------------------------------------------------------------------------------------------------

-- Ejemplo
DECLARE @Cantidad INT;
EXEC VerCantidadDeReservasPorCliente @ClienteID = 1, @FechaInicio = '2023-11-01', @FechaFin = '2023-11-30';
SELECT @Cantidad AS CantidadDeReservas;



------------------------------------------------ EJECUTAR PRIMERO LA CREACIÓN DEL PROCEDIMIENTO -------------------------------------------------------------------------

-- Procedimiento para ver los clientes que más reservas han realizado en un rango de fechas
CREATE PROC VerClientesConMasReservas
@FechaInicio DATE,
@FechaFin DATE
AS
BEGIN
    SELECT TOP 3 C.ClienteID, C.Nombre, COUNT(R.ReservaID) AS CantidadDeReservas
    FROM Clientes C
    LEFT JOIN Reservas R ON C.ClienteID = R.ClienteID
    WHERE R.FechaReserva BETWEEN @FechaInicio AND @FechaFin
    GROUP BY C.ClienteID, C.Nombre
    ORDER BY CantidadDeReservas DESC;
END;

--------------------------------------------------------- HASTA AQUÍ --------------------------------------------------------------------------------------------------------

--Ejemplo
EXEC VerClientesConMasReservas @FechaInicio = '2023-11-01', @FechaFin = '2023-11-30';



------------------------------------------------ EJECUTAR PRIMERO LA CREACIÓN DEL PROCEDIMIENTO -------------------------------------------------------------------------

-- Procedimiento para ver los tours disponibles según la fecha actual
CREATE PROC VerToursDisponibles
AS
BEGIN
    DECLARE @FechaActual DATE = GETDATE();

    SELECT TourID, Nombre, TipoTour, Nacional, FechaInicio, FechaFin, Precio, CantidadPersonas
    FROM Tours
    WHERE FechaInicio > @FechaActual;
END;

--------------------------------------------------------- HASTA AQUÍ --------------------------------------------------------------------------------------------------------

EXEC VerToursDisponibles


------------------------------------------------ EJECUTAR PRIMERO LA CREACIÓN DEL PROCEDIMIENTO -------------------------------------------------------------------------

-- Procedimiento para ver los tours ya realizados
CREATE PROC VerToursRealizados
AS
BEGIN
    DECLARE @FechaActual DATE = GETDATE();

    SELECT TourID, Nombre, TipoTour, Nacional, FechaInicio, FechaFin, Precio, CantidadPersonas
    FROM Tours
    WHERE FechaFin < @FechaActual;
END;

--------------------------------------------------------- HASTA AQUÍ --------------------------------------------------------------------------------------------------------

EXEC VerToursRealizados;
