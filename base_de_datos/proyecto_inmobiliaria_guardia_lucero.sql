-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 25-08-2025 a las 01:34:14
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
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `Id_inquilino` int(11) NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `Apellido` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Telefono` varchar(255) NOT NULL,
  `Dni` varchar(255) NOT NULL,
  `Fecha_creacion` date NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id_inquilino`, `nombre`, `Apellido`, `Email`, `Telefono`, `Dni`, `Fecha_creacion`) VALUES
(3, 'Lopexz', 'Kilo', 'pepe@pepe', '6019521325', '90122', '2025-08-24'),
(4, 'Test', 'test', 'test@test', '2663992222', '34546777', '2025-08-24');

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
(1, '34546777', 'Modificar', 'Modificar', 'Modificar@Modificar', '76666', '2025-08-18'),
(2, '6855324234', 'Pepe', 'Jose', 'jose@jose', '2667464546', '2025-08-18'),
(3, '4565464565', 'Test', 'Test', 'Test@Test', '123244554', '2025-08-18'),
(4, '678234243', 'jose', 'Pepe', 'admin@admin', '26645434324', '0001-01-01'),
(5, '768678', 'Lopexz', 'Pepe', 'pepe@pepe', '266456456546455', '2025-08-19'),
(8, '90122', 'Lopexz', 'Pepe', 'test@test', '2668433443', '2025-08-24');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`Id_inquilino`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id_propietario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id_propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
