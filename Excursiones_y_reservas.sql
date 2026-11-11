-- Insertamos excursiones de prueba con distintas fechas y estados
INSERT INTO Excursiones (NombreExcursion, FechaExcursion, Precio, Cupo, IdEstado, Descripcion, DuracionDias, ImagenUrl, Borrado)
VALUES 
('Cataratas del Iguazú', '2026-11-15 10:00:00', 15000.00, 20, 1, 'Excursión programada a futuro.', 3, '/images/default-excursion.png', 0),
('Salta y sus Cerros', '2026-11-10 09:00:00', 12000.00, 0, 1, 'Excursión programada próxima a cumplirse (menos de 48hs).', 4, '/images/default-excursion.png', 0),
('Glaciar Perito Moreno', '2026-10-01 08:00:00', 35000.00, 15, 2, 'Excursión ya completada (ideal para probar calificaciones).', 5, '/images/default-excursion.png', 0),
('Tierra del Fuego Express', '2026-11-05 08:00:00', 25000.00, 10, 3, 'Excursión cancelada/suspendida por la administración.', 2, '/images/default-excursion.png', 0),
('Aventura en la Cordillera de los Andes', '2026-11-25 09:00:00', 18500.00, 25, 1, 'Trekking de alta montaña con guías especializados y equipo incluido.', 2, '/images/default-excursion.png', 0),
('Valle de la Luna y Ischigualasto', '2026-12-02 08:30:00', 22000.00, 30, 1, 'Recorrido por formaciones geológicas únicas y paisajes prehistóricos.', 3, '/images/default-excursion.png', 0),
('Ruta del Vino en Mendoza', '2026-12-10 10:00:00', 31000.00, 15, 1, 'Visita a bodegas exclusivas con cata de los mejores malbecs de la región.', 2, '/images/default-excursion.png', 0),
('Sierras de Córdoba y Alta Gracia', '2026-12-18 08:00:00', 14500.00, 40, 1, 'Paseo histórico y natural por los rincones más hermosos de las sierras.', 1, '/images/default-excursion.png', 0),
('Avistaje de Ballenas en Puerto Madryn', '2027-01-15 07:30:00', 45000.00, 20, 1, 'Experiencia inolvidable de navegación y contacto con la fauna marina.', 4, '/images/default-excursion.png', 0),
('Esteros del Iberá Salvaje', '2027-02-05 09:00:00', 38000.00, 12, 1, 'Safari fotográfico en lancha para avistar carpinchos, yacarés y aves autóctonas.', 3, '/images/default-excursion.png', 0);


-- asociamos las reservas al ID de usuario 2 (el segundo usuario / cliente)

INSERT INTO Reservas (IdExcursion, IdUsuario, CantidadLugares, FechaReserva, TotalAbonado)
VALUES 
-- 1. Reserva Programada normal (con más de 48 hs) -> Permite Modificar o Cancelar
((SELECT TOP 1 IdExcursion FROM Excursiones WHERE NombreExcursion = 'Cataratas del Iguazú'), 2, 2, GETDATE(), 30000.00),

-- 2. Reserva Programada próxima (a menos de 48 hs) -> Muestra el aviso de plazo vencido
((SELECT TOP 1 IdExcursion FROM Excursiones WHERE NombreExcursion = 'Salta y sus Cerros'), 2, 3, GETDATE(), 36000.00),

-- 3. Reserva de excursión Completada (IdEstado = 2) -> Habilita el botón de Calificar
((SELECT TOP 1 IdExcursion FROM Excursiones WHERE NombreExcursion = 'Glaciar Perito Moreno'), 2, 1, '2026-09-25 14:00:00', 35000.00),

-- 4. Reserva de excursión Cancelada/Suspendida por el Admin (IdEstado = 3 o Borrado = 1) -> Muestra alerta roja y permite quitarla
((SELECT TOP 1 IdExcursion FROM Excursiones WHERE NombreExcursion = 'Tierra del Fuego Express'), 2, 2, '2026-10-20 11:30:00', 50000.00);