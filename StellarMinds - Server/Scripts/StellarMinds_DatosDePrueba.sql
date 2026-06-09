/* =============================================================================
   StellarMinds - Script de datos de prueba (inserts)
   -----------------------------------------------------------------------------
   - Base de datos: StellarMindsDb (SQL Server / .\SQLEXPRESS o LocalDB)
   - Requiere que las tablas YA existan (correr la API una vez aplica las
     migraciones de EF Core, o usar Update-Database).
   - El script es re-ejecutable: borra los datos existentes en orden seguro de
     claves foráneas y vuelve a insertar con IDs fijos (SET IDENTITY_INSERT).
   - Cumple el requisito de "al menos 10 registros por tabla":
        Usuarios 12 · Equipos 16 · ObjetosCelestes 12 · Prestamos 12 ·
        Observaciones 12 · Auditorias 12
   - Mapeo de enums (se guardan como int):
        TipoUsuario:        0=Administrador 1=Coordinador 2=Socio
        EstadoPrestamo:     0=EN_PRESTAMO   1=DEVUELTO
        TipoMontura:        0=Ecuatorial    1=Altazimutal 2=Hibrida
        TipoSensor:         0=CMOS          1=CCD
        TipoObjetoCeleste:  0=Planeta 1=Galaxia 2=Nebulosa 3=Estrella
        TipoAccionAuditoria:0=PRESTAMO      1=DEVOLUCION
   - Equipos usa herencia TPH (una sola tabla con columna Discriminator:
        'Telescopio' | 'Montura' | 'Camara' | 'Ocular').
   - CODIFICACIÓN: el archivo está en UTF-8. En SSMS abre y ejecuta normal.
     Si se corre por consola con sqlcmd, usar el flag -f 65001 para no corromper
     los acentos, p.ej.:
        sqlcmd -S ".\SQLEXPRESS" -E -d StellarMindsDb -f 65001 -i StellarMinds_DatosDePrueba.sql
   ============================================================================= */

USE StellarMindsDb;
GO

SET NOCOUNT ON;
GO

/* -----------------------------------------------------------------------------
   1) Limpieza en orden seguro de FK (hijas primero)
   ----------------------------------------------------------------------------- */
DELETE FROM dbo.Auditorias;
DELETE FROM dbo.Observaciones;
DELETE FROM dbo.Prestamos;
DELETE FROM dbo.Usuarios;
DELETE FROM dbo.ObjetosCelestes;
DELETE FROM dbo.Equipos;
GO

/* =============================================================================
   2) USUARIOS (12)  -> Admin: 1,11 · Coordinador: 2,5,6 · Socio: 3,4,7,8,9,10,12
   Pass de todos: 1234!
   ============================================================================= */
SET IDENTITY_INSERT dbo.Usuarios ON;
INSERT INTO dbo.Usuarios
    (Id, NombreCompleto_Nombre, NombreCompleto_Apellido, Direccion_Calle1, Direccion_Calle2, Telefono, Email, Username, Pass, TipoUsuario)
VALUES
    (1,  N'fabricio',  N'ferreira', N'Av. Principal 1000', N'Montevideo', 99000001, N'fabricioferreira1169@stellar.com', N'fabricio', N'1234!', 0),
    (2,  N'María',     N'González', N'Bulevar Artigas 1234', N'Montevideo', 99000002, N'maria@stellar.com',  N'maria1',  N'1234!', 1),
    (3,  N'Juan',      N'Pérez',    N'Calle 18 2500',       N'Montevideo', 99000003, N'JuanPerez@stellar.com', N'juan1', N'1234!', 2),
    (4,  N'Ana',       N'Soto',     N'Av. Italia 4500',     N'Montevideo', 99000004, N'AnaSoto@stellar.com', N'ana1',   N'1234!', 2),
    (5,  N'Carlos',    N'Díaz',     N'Calle Coord 2',       N'Montevideo', 99200002, N'coord2@stellar.com',  N'coord2', N'1234!', 1),
    (6,  N'Laura',     N'Méndez',   N'Calle Coord 3',       N'Montevideo', 99200003, N'coord3@stellar.com',  N'coord3', N'1234!', 1),
    (7,  N'Pedro',     N'Sosa',     N'Calle Socio 1',       N'Montevideo', 99300001, N'socio1@stellar.com',  N'socio1', N'1234!', 2),
    (8,  N'Lucía',     N'Rama',     N'Calle Socio 2',       N'Montevideo', 99300002, N'socio2@stellar.com',  N'socio2', N'1234!', 2),
    (9,  N'Diego',     N'Núñez',    N'Calle Socio 3',       N'Montevideo', 99300003, N'socio3@stellar.com',  N'socio3', N'1234!', 2),
    (10, N'Sofía',     N'Vera',     N'Calle Socio 4',       N'Montevideo', 99300004, N'socio4@stellar.com',  N'socio4', N'1234!', 2),
    (11, N'Martín',    N'Rey',      N'Calle Admin 2',       N'Montevideo', 99100002, N'admin2@stellar.com',  N'admin2', N'1234!', 0),
    (12, N'Valentina', N'Cruz',     N'Calle Socio 5',       N'Montevideo', 99300005, N'socio5@stellar.com',  N'socio5', N'1234!', 2);
SET IDENTITY_INSERT dbo.Usuarios OFF;
GO

/* =============================================================================
   3) EQUIPOS (16)  -> Telescopios 1-5 · Monturas 6-9 · Cámaras 10-13 · Oculares 14-16
   TPH: cada subtipo inserta solo sus columnas; el resto queda NULL.
   ============================================================================= */
SET IDENTITY_INSERT dbo.Equipos ON;

-- Telescopios (Discriminator='Telescopio') columnas: Apertura, RelacionFocal, DistanciaFocal, Peso
INSERT INTO dbo.Equipos (Id, Marca, Modelo, Cantidad, Discriminator, Apertura, RelacionFocal, DistanciaFocal, Peso)
VALUES
    (1, N'Celestron',   N'NexStar 8SE',      2, N'Telescopio', 203.2, 10.0, 2032, 5),
    (2, N'Sky-Watcher', N'Explorer 150P',    3, N'Telescopio', 150.0,  5.0,  750, 4),
    (3, N'Meade',       N'LX90',             1, N'Telescopio', 203.2, 10.0, 2000, 6),
    (4, N'Celestron',   N'AstroMaster 130EQ',2, N'Telescopio', 130.0,  5.0,  650, 3),
    (5, N'Orion',       N'SkyQuest XT8',     1, N'Telescopio', 203.2,  5.9, 1200, 9);

-- Monturas (Discriminator='Montura') columnas: Tipo, PesoMaximo, Goto ([Goto] es palabra reservada en T-SQL)
INSERT INTO dbo.Equipos (Id, Marca, Modelo, Cantidad, Discriminator, Tipo, PesoMaximo, [Goto])
VALUES
    (6, N'Sky-Watcher', N'EQ6-R Pro',       1, N'Montura', 0, 20, 1),
    (7, N'Celestron',   N'AVX',             2, N'Montura', 0, 13, 1),
    (8, N'Sky-Watcher', N'AZ-GTi',          2, N'Montura', 1,  5, 1),
    (9, N'iOptron',     N'SkyGuider Pro',   1, N'Montura', 2,  3, 0);

-- Cámaras (Discriminator='Camara') columnas: Sensor, Resolucion, PixelSize
INSERT INTO dbo.Equipos (Id, Marca, Modelo, Cantidad, Discriminator, Sensor, Resolucion, PixelSize)
VALUES
    (10, N'ZWO',   N'ASI294MC', 3, N'Camara', 0, 11.70, 4.63),
    (11, N'ZWO',   N'ASI183MM', 2, N'Camara', 0, 20.18, 2.40),
    (12, N'Canon', N'EOS Ra',   1, N'Camara', 0, 30.30, 5.36),
    (13, N'QHY',   N'QHY268M',  1, N'Camara', 1, 26.00, 3.76);

-- Oculares (Discriminator='Ocular') columnas: Diametro, AnguloVisual
INSERT INTO dbo.Equipos (Id, Marca, Modelo, Cantidad, Discriminator, Diametro, AnguloVisual)
VALUES
    (14, N'Celestron', N'Plossl 25mm',  4, N'Ocular', 31.70, 52.00),
    (15, N'Tele Vue',  N'Nagler 13mm',  2, N'Ocular', 31.70, 82.00),
    (16, N'Baader',    N'Hyperion 8mm', 3, N'Ocular', 31.70, 68.00);

SET IDENTITY_INSERT dbo.Equipos OFF;
GO

/* =============================================================================
   4) OBJETOS CELESTES (12)
   ============================================================================= */
SET IDENTITY_INSERT dbo.ObjetosCelestes ON;
INSERT INTO dbo.ObjetosCelestes (Id, Nombre, Tipo, Magnitud)
VALUES
    (1,  N'Júpiter',                       0, -2.20),
    (2,  N'Saturno',                       0,  0.46),
    (3,  N'Marte',                         0, -1.00),
    (4,  N'Venus',                         0, -4.40),
    (5,  N'Galaxia de Andrómeda (M31)',    1,  3.44),
    (6,  N'Galaxia del Remolino (M51)',    1,  8.40),
    (7,  N'Nebulosa de Orión (M42)',       2,  4.00),
    (8,  N'Nebulosa del Anillo (M57)',     2,  8.80),
    (9,  N'Polaris',                       3,  1.98),
    (10, N'Sirio',                         3, -1.46),
    (11, N'Betelgeuse',                    3,  0.42),
    (12, N'Neptuno',                       0,  7.80);
SET IDENTITY_INSERT dbo.ObjetosCelestes OFF;
GO

/* =============================================================================
   5) PRESTAMOS (12)
   Estado: 0=EN_PRESTAMO 1=DEVUELTO. VisualId puede ser NULL (cámara/ocular opcional).
   Casos de prueba:
     - RF08 (mes/año + atraso) sobre Juan (Id 3): P1..P4
     - Aislamiento RF08: Ana (Id 4) P5 -> Juan NO debe verlo
     - RF09 (socios por telescopio 2): P6,P7 (socio1 repetido -> 1 sola vez),P8,P9
   ============================================================================= */
SET IDENTITY_INSERT dbo.Prestamos ON;
INSERT INTO dbo.Prestamos (Id, Fecha_Inicio, Fecha_Fin, TelescopioId, MonturaId, VisualId, Estado, UsuarioId)
VALUES
    -- [Juan=3] MAYO 2026 atrasado (fin pasado y sigue EN_PRESTAMO)
    (1,  '2026-05-01', '2026-05-10', 1, 6, 10,   0, 3),
    -- [Juan=3] MAYO 2026 devuelto
    (2,  '2026-05-15', '2026-05-20', 1, 6, NULL, 1, 3),
    -- [Juan=3] JUNIO 2026 atrasado
    (3,  '2026-06-01', '2026-06-02', 1, 6, NULL, 0, 3),
    -- [Juan=3] JUNIO 2026 vigente NO atrasado (fin futuro)
    (4,  '2026-06-01', '2026-06-30', 1, 6, 10,   0, 3),
    -- [Ana=4] JUNIO 2026 (otro socio: aislamiento RF08)
    (5,  '2026-06-03', '2026-06-25', 1, 6, NULL, 0, 4),
    -- RF09 sobre Telescopio 2 (Explorer 150P):
    (6,  '2026-04-01', '2026-04-10', 2, 6, NULL, 1, 7),  -- socio1
    (7,  '2026-04-15', '2026-04-20', 2, 6, NULL, 1, 7),  -- socio1 otra vez -> NO duplicar en RF09
    (8,  '2026-04-05', '2026-04-12', 2, 6, NULL, 1, 3),  -- Juan
    (9,  '2026-04-18', '2026-04-28', 2, 7, NULL, 1, 4),  -- Ana
    -- Préstamos extra:
    (10, '2026-05-05', '2026-05-12', 4, 7, 14,   1, 8),  -- socio2 (devuelto, con ocular)
    (11, '2026-06-05', '2026-06-28', 5, 8, 11,   0, 9),  -- socio3 (vigente, con cámara)
    (12, '2026-06-01', '2026-06-04', 2, 8, NULL, 0, 10); -- socio4 (atrasado)
SET IDENTITY_INSERT dbo.Prestamos OFF;
GO

/* =============================================================================
   6) OBSERVACIONES (12)
   RF10 ranking (desc por cantidad): Júpiter 4, Saturno 3, luego 1 c/u.
   Los objetos NO observados (Andrómeda M51, Nebulosa Anillo, Polaris, Betelgeuse,
   Neptuno) no deben aparecer en el ranking.
   ============================================================================= */
SET IDENTITY_INSERT dbo.Observaciones ON;
INSERT INTO dbo.Observaciones (Id, Fecha, MotivoIA, ObjetoCelesteId, PrestamoId, ResultadoIA)
VALUES
    (1,  '2026-05-02', N'Observación de prueba precargada (RF10).', 1,  1,  N'ADECUADO'),     -- Júpiter
    (2,  '2026-05-16', N'Observación de prueba precargada (RF10).', 1,  2,  N'ADECUADO'),     -- Júpiter
    (3,  '2026-06-02', N'Observación de prueba precargada (RF10).', 1,  4,  N'ADECUADO'),     -- Júpiter
    (4,  '2026-06-03', N'Observación de prueba precargada (RF10).', 2,  4,  N'ADECUADO'),     -- Saturno
    (5,  '2026-06-05', N'Observación de prueba precargada (RF10).', 2,  5,  N'ADECUADO'),     -- Saturno
    (6,  '2026-06-06', N'Magnitud alta para el equipo prestado.',   7,  4,  N'NO ADECUADO'),  -- Orión
    (7,  '2026-06-07', N'Observación de prueba precargada (RF10).', 3,  5,  N'ADECUADO'),     -- Marte
    (8,  '2026-06-10', N'Observación de prueba precargada (RF10).', 4,  11, N'ADECUADO'),     -- Venus
    (9,  '2026-06-12', N'Objeto débil para apertura disponible.',   5,  11, N'NO ADECUADO'),  -- Andrómeda
    (10, '2026-05-08', N'Observación de prueba precargada (RF10).', 10, 10, N'ADECUADO'),     -- Sirio
    (11, '2026-06-14', N'Observación de prueba precargada (RF10).', 1,  11, N'ADECUADO'),     -- Júpiter
    (12, '2026-06-15', N'Observación de prueba precargada (RF10).', 2,  11, N'ADECUADO');     -- Saturno
SET IDENTITY_INSERT dbo.Observaciones OFF;
GO

/* =============================================================================
   7) AUDITORIAS (12)  -> RF06: cada alta de préstamo genera PRESTAMO(0) y cada
   devolución DEVOLUCION(1). UsuarioId = coordinador que realizó la acción.
   ============================================================================= */
SET IDENTITY_INSERT dbo.Auditorias ON;
INSERT INTO dbo.Auditorias (Id, Accion, FechaHora, PrestamoId, UsuarioId)
VALUES
    (1,  0, '2026-05-01T09:00:00', 1,  2),  -- PRESTAMO  P1  (coord María)
    (2,  0, '2026-05-15T09:00:00', 2,  2),  -- PRESTAMO  P2
    (3,  1, '2026-05-20T16:00:00', 2,  2),  -- DEVOLUCION P2
    (4,  0, '2026-06-01T09:00:00', 3,  5),  -- PRESTAMO  P3  (coord Carlos)
    (5,  0, '2026-06-01T10:00:00', 4,  5),  -- PRESTAMO  P4
    (6,  0, '2026-06-03T09:00:00', 5,  2),  -- PRESTAMO  P5
    (7,  0, '2026-04-01T09:00:00', 6,  6),  -- PRESTAMO  P6  (coord Laura)
    (8,  1, '2026-04-10T16:00:00', 6,  6),  -- DEVOLUCION P6
    (9,  0, '2026-04-15T09:00:00', 7,  6),  -- PRESTAMO  P7
    (10, 1, '2026-04-20T16:00:00', 7,  6),  -- DEVOLUCION P7
    (11, 0, '2026-05-05T09:00:00', 10, 5),  -- PRESTAMO  P10
    (12, 1, '2026-05-12T16:00:00', 10, 5);  -- DEVOLUCION P10
SET IDENTITY_INSERT dbo.Auditorias OFF;
GO

/* -----------------------------------------------------------------------------
   8) Reajuste de los contadores IDENTITY para que las próximas altas de la app
      continúen después del último Id insertado.
   ----------------------------------------------------------------------------- */
DBCC CHECKIDENT ('dbo.Usuarios',        RESEED, 12);
DBCC CHECKIDENT ('dbo.Equipos',         RESEED, 16);
DBCC CHECKIDENT ('dbo.ObjetosCelestes', RESEED, 12);
DBCC CHECKIDENT ('dbo.Prestamos',       RESEED, 12);
DBCC CHECKIDENT ('dbo.Observaciones',   RESEED, 12);
DBCC CHECKIDENT ('dbo.Auditorias',      RESEED, 12);
GO

PRINT 'Datos de prueba cargados correctamente.';
GO
