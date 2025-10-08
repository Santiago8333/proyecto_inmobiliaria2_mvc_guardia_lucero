-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 09-10-2025 a las 00:59:55
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
(18, 9, 20, 0, '5000.00', '10000.00', '10000.00', '2025-10-04', '2025-10-01', '2025-11-30', NULL, 2, 'admin@admin', NULL, 1),
(19, 10, 22, 0, '5000.00', '15000.00', '0.00', '2025-10-05', '2025-07-01', '2025-09-30', NULL, 3, 'admin@admin', NULL, 1);

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
  `Portada` varchar(255) DEFAULT NULL,
  `Creado_por` varchar(255) DEFAULT NULL,
  `Desactivado_por` varchar(255) DEFAULT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id_inmueble`, `Id_propietario`, `Uso`, `Tipo`, `Direccion`, `Ambiente`, `Precio`, `Longitud`, `Latitud`, `Fecha_creacion`, `Portada`, `Creado_por`, `Desactivado_por`, `Estado`) VALUES
(20, 15, 'Residencial', 'depósito', 'La Punta', 2, '1457.00', -33.233951948327196, -6.623027863042181e15, '2025-10-04', NULL, 'admin@admin', '', 1),
(21, 16, 'Comercial', 'depósito', 'La Punta ,modulo 1, Manzana 108, casa 10', 2, '1457.00', -3.3233951948327196e16, -6.623027863042181e15, '2025-10-05', NULL, 'admin@admin', NULL, 1),
(22, 17, 'Residencial', 'depósito', 'San Luis', 2, '1457.00', -3.3233951948327196e16, -66.23027863042181, '2025-10-05', NULL, 'admin@admin', NULL, 1);

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
  `Creado_por` varchar(255) DEFAULT NULL,
  `Fecha_creacion` date NOT NULL DEFAULT current_timestamp(),
  `Estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id_inquilino`, `Nombre`, `Apellido`, `Email`, `Telefono`, `Dni`, `Creado_por`, `Fecha_creacion`, `Estado`) VALUES
(8, 'Lopexz', 'Pedro', 'Pedro@Pedro', '2668433443', '90122', 'admin@admin', '2025-10-04', 1),
(9, 'Jose', 'Jose', 'jose@jose', '2668433443', '673242', 'admin@admin', '2025-10-04', 1),
(10, 'Pablo', 'Pablo', 'Pablo@Pablo', '6019521325', '25476', 'admin@admin', '2025-10-05', 1);

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
  `Creado_por` varchar(255) DEFAULT NULL,
  `Anulado_por` varchar(255) DEFAULT NULL,
  `Fecha_creacion` datetime NOT NULL DEFAULT current_timestamp(),
  `Estado` tinyint(1) NOT NULL DEFAULT 1
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
  `Creado_por` varchar(255) DEFAULT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1,
  `Fecha_creacion` date NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id_propietario`, `Dni`, `Nombre`, `Apellido`, `Email`, `Telefono`, `Creado_por`, `Estado`, `Fecha_creacion`) VALUES
(15, '90122', 'Pepe2', 'Pepe', 'Pepe@Pepe', '2663992222', 'admin@admin', 1, '2025-10-04'),
(16, '34546777', 'jose', 'Kilo', 'Kilo@Kilo', '26645', 'admin@admin', 1, '2025-10-04'),
(17, '4366634', 'Lopexz', 'Luis', 'Luis@Luis', '2668433443', 'test@test', 1, '2025-10-04'),
(18, '34546777', 'Lopexz', 'Pepe2', 'pepe2@pepe', '777777777777777', 'admin@admin', 1, '2025-10-07');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `Id_usuario` int(11) NOT NULL,
  `Nombre` varchar(255) NOT NULL,
  `Apellido` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `AvatarUrl` varchar(255) DEFAULT NULL,
  `Rol` int(11) NOT NULL,
  `RolNombre` varchar(255) NOT NULL,
  `Fecha_creacion` datetime NOT NULL DEFAULT current_timestamp(),
  `Clave` varchar(255) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`Id_usuario`, `Nombre`, `Apellido`, `Email`, `AvatarUrl`, `Rol`, `RolNombre`, `Fecha_creacion`, `Clave`, `Estado`) VALUES
(1, 'admin', 'admin', 'admin@admin', '/images/avatars/1d76a2bc-e3af-4969-b773-277e59321dca.png', 1, 'Administrador', '2025-09-25 17:26:22', 'Hh5PJIf2spPKILQESYS5hbDBI9MT0YpCqENeX/PwcPE=', 1),
(2, 'jose2', 'Pepe2', 'jose@jose', '/images/avatars/79cba695-2e7c-4709-a7ea-3293632bb0dc.jpg', 2, 'Empleado', '2025-09-28 19:26:52', 'Hh5PJIf2spPKILQESYS5hbDBI9MT0YpCqENeX/PwcPE=', 1),
(3, 'Lopexz', 'Kilo', 'test@test', '/images/avatars/default-avatar.png', 2, 'Empleado', '2025-09-28 21:49:31', 'Hh5PJIf2spPKILQESYS5hbDBI9MT0YpCqENeX/PwcPE=', 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`Id_contrato`),
  ADD KEY `fk_inmuebles_contrato` (`Id_inmueble`),
  ADD KEY `fk_inquilinos_contrato` (`Id_inquilino`);

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
  ADD PRIMARY KEY (`Id_multa`),
  ADD KEY `FK_CONTRATO_MULTA` (`Id_contrato`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id_pago`),
  ADD KEY `FK_PAGOS_CONTRATO` (`Id_contrato`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id_propietario`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id_usuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `Id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id_inmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `multas`
--
ALTER TABLE `multas`
  MODIFY `Id_multa` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id_pago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id_propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `fk_inmuebles_contrato` FOREIGN KEY (`Id_inmueble`) REFERENCES `inmuebles` (`Id_inmueble`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_inquilinos_contrato` FOREIGN KEY (`Id_inquilino`) REFERENCES `inquilinos` (`Id_inquilino`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `fk_propietarios_inmuebles` FOREIGN KEY (`Id_propietario`) REFERENCES `propietarios` (`id_propietario`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `multas`
--
ALTER TABLE `multas`
  ADD CONSTRAINT `FK_CONTRATO_MULTA` FOREIGN KEY (`Id_contrato`) REFERENCES `contratos` (`Id_contrato`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `FK_PAGOS_CONTRATO` FOREIGN KEY (`Id_contrato`) REFERENCES `contratos` (`Id_contrato`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
