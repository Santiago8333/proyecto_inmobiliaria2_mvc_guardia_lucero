-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 09-09-2025 a las 20:05:03
-- Versión del servidor: 10.4.27-MariaDB
-- Versión de PHP: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `proyecto_inmobiliaria_guardia_lucero`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `Id_contrato` int(11) NOT NULL,
  `Id_inquilino` int(11) NOT NULL,
  `Id_inmueble` int(11) NOT NULL,
  `Contrato_completado` tinyint(1) NOT NULL DEFAULT 0,
  `Monto` decimal(10,2) NOT NULL,
  `Monto_total` decimal(10,2) NOT NULL,
  `Monto_a_pagar` decimal(10,2) NOT NULL,
  `Fecha_creacion` date NOT NULL DEFAULT current_timestamp(),
  `Fecha_desde` date NOT NULL,
  `Fecha_hasta` date NOT NULL,
  `Fecha_final` date DEFAULT NULL,
  `Meses` int(11) NOT NULL,
  `Creado_por` varchar(255) DEFAULT NULL,
  `Terminado_por` varchar(255) DEFAULT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`Id_contrato`, `Id_inquilino`, `Id_inmueble`, `Contrato_completado`, `Monto`, `Monto_total`, `Monto_a_pagar`, `Fecha_creacion`, `Fecha_desde`, `Fecha_hasta`, `Fecha_final`, `Meses`, `Creado_por`, `Terminado_por`, `Estado`) VALUES
(2, 5, 13, 0, '5000.00', '15000.00', '15000.00', '2025-09-08', '2025-09-01', '2025-12-01', NULL, 3, NULL, NULL, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `Id_inmueble` int(11) NOT NULL,
  `Id_propietario` int(11) NOT NULL,
  `Uso` varchar(255) NOT NULL,
  `Tipo` varchar(255) NOT NULL,
  `Direccion` varchar(255) NOT NULL,
  `Ambiente` int(11) NOT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Longitud` double NOT NULL,
  `Latitud` double NOT NULL,
  `Fecha_creacion` date NOT NULL DEFAULT current_timestamp(),
  `Estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id_inmueble`, `Id_propietario`, `Uso`, `Tipo`, `Direccion`, `Ambiente`, `Precio`, `Longitud`, `Latitud`, `Fecha_creacion`, `Estado`) VALUES
(13, 8, 'Residencial', 'local', 'La Punta f', 7, '77777.00', -3.3233951948327196e16, -66.23027863042181, '2025-09-02', 1),
(14, 3, 'Comercial', 'casa', 'La Punta', 3, '1457.00', -3.3233951948327196e16, -66.23027863042181, '2025-09-02', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `Id_inquilino` int(11) NOT NULL,
  `Nombre` varchar(255) NOT NULL,
  `Apellido` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Telefono` varchar(255) NOT NULL,
  `Dni` varchar(255) NOT NULL,
  `Fecha_creacion` date NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id_inquilino`, `Nombre`, `Apellido`, `Email`, `Telefono`, `Dni`, `Fecha_creacion`) VALUES
(4, 'Test', 'test', 'test@test', '2663992222', '34546777', '2025-08-24'),
(5, 'Jose', 'Kilo', 'Mateo@Mateo', '2663992222', '34546777', '2025-08-25'),
(6, 'jose', 'Kilo', 'pepe2@pepe', '2668433443', '4366634', '2025-08-28');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `multas`
--

CREATE TABLE `multas` (
  `Id_multa` int(11) NOT NULL,
  `Id_contrato` int(11) NOT NULL,
  `Razon_multa` varchar(255) NOT NULL,
  `Monto` decimal(10,2) NOT NULL,
  `Fecha` date NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id_pago` int(11) NOT NULL,
  `Id_contrato` int(11) NOT NULL,
  `Detalle` varchar(255) NOT NULL,
  `Monto` decimal(10,2) NOT NULL,
  `Creado_por` varchar(255) NOT NULL,
  `Anulado_por` int(255) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id_propietario` int(11) NOT NULL,
  `Dni` varchar(255) NOT NULL,
  `Nombre` varchar(255) NOT NULL,
  `Apellido` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Telefono` varchar(255) NOT NULL,
  `Fecha_creacion` date NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id_propietario`, `Dni`, `Nombre`, `Apellido`, `Email`, `Telefono`, `Fecha_creacion`) VALUES
(2, '6855324234', 'Pepe', 'Jose', 'jose@jose', '2667464546', '2025-08-18'),
(3, '4565464565', 'Test', 'Test', 'Test@Test', '123244554', '2025-08-18'),
(4, '678234243', 'jose', 'Pepe', 'admin@admin', '26645434324', '0001-01-01'),
(5, '768678', 'Lopexz', 'Pepe', 'pepe@pepe', '266456456546455', '2025-08-19'),
(8, '90122', 'Lopexz', 'Pepe', 'test@test', '2668433443', '2025-08-24'),
(12, '90122', 'jose', 'Pepe', 'martintoledo@gmail.com', '2664545565', '2025-08-28');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`Id_contrato`),
  ADD KEY `fk_inquilinos_contrato` (`Id_inquilino`),
  ADD KEY `fk_inmuebles_contrato` (`Id_inmueble`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`Id_inmueble`),
  ADD KEY `fk_propietarios_inmuebles` (`Id_propietario`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`Id_inquilino`);

--
-- Indices de la tabla `multas`
--
ALTER TABLE `multas`
  ADD PRIMARY KEY (`Id_multa`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id_pago`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id_propietario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `Id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id_inmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `multas`
--
ALTER TABLE `multas`
  MODIFY `Id_multa` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id_pago` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id_propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `fk_inmuebles_contrato` FOREIGN KEY (`Id_inmueble`) REFERENCES `inmuebles` (`Id_inmueble`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_inquilinos_contrato` FOREIGN KEY (`Id_inquilino`) REFERENCES `inquilinos` (`Id_inquilino`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `fk_propietarios_inmuebles` FOREIGN KEY (`Id_propietario`) REFERENCES `propietarios` (`id_propietario`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
