USE Test
INSERT INTO Client
VALUES 
(
	0,
	'Yuliia',
	'Solomakha',
	'123456789',
	'yulsa@gmail.com'
),
(
	1,
	'Alyona',
	'Shark',
	'0663775421',
	'alyona@gmail.com'
),
(
	2,
	'Kateryna',
	'Bedenko',
	'641582345',
	'katya@gmail.com'
),
(
	3,
	'Alina',
	'Stetsenko',
	'987654321',
	'stez@gmail.com'
),
(
	4,
	'Petro',
	'Lytvynenko',
	'8677564549',
	'petro@gmail.com'
),
(
	5,
	'Dmytro',
	'Tkachenko',
	'6666666666',
	'tkach@gmail.com'
),
(
	6,
	'Vladyslav',
	'Burlaka',
	'123456098',
	'vlad@gmail.com'
),
(
	7,
	'Andriy',
	'Kryvolap',
	'123321234',
	'andriy@gmail.com'
),
(
	8,
	'Mykyta',
	'Yevtushenko',
	'098765789',
	'myk@gmail.com'
),
(
	9,
	'Yuliia',
	'Derhunova',
	'578890800',
	'derh@gmail.com'
)

INSERT INTO Airport
VALUES
(0, 'Verona Villafranca Airport', 'Verona' ),
(1, 'Barcelona-El Prat Airport', 'Barcelona'), 
(2, 'Valencia Airport', 'Valencia'), 
(3, 'Malpensa Airport', 'Milan'),
(4, 'Boryspil International Airport', 'Kyiv')

INSERT INTO Airline
VALUES
('AerostarAirlines'),
('Ryanair'),
('Volotea')

INSERT INTO Plain
VALUES
(0,'Plane1', 'Type1', 60, 'Volotea'),
(1,'Plane2', 'Type2', 50, 'Ryanair'),
(2,'Plane3', 'Type3', 40, 'Ryanair'),
(3,'Plane4', 'Type4', 50, 'AerostarAirlines'),
(4,'Plane5', 'Type5', 70, 'Ryanair' )

INSERT INTO Flight
VALUES
(0, '2023-03-27 00:00:00.000', 0, 4, '17:05:00', '15:30:00', 0),
(1, '2023-05-01 00:00:00.000', 1, 3, '15:30:00', '12:00:00', 1),
(2, '2023-04-15 00:00:00.000', 2, 2, '12:15:00', '07:40:00', 2),
(3, '2023-06-28 00:00:00.000', 3, 2, '11:20:00', '10:11:00', 3),
(4, '2023-07-28 00:00:00.000', 4, 1, '19:35:00', '14:15:00', 4)

INSERT INTO BoardingPass
VALUES
(0, 00, 22, 'T1', 'T2'),
(1, 11, 25, 'T1', 'T1'),
(2, 22, 70, 'T2', 'T1'),
(3, 33, 85, 'T1', 'T3'),
(4, 44, 12, 'T1', 'T2'),
(5, 55, 34, 'T2', 'T2'),
(6, 66, 55, 'T1', 'T1'),
(7, 77, 12, 'T2', 'T1'),
(8, 88, 11, 'T1', 'T2'),
(9, 99, 32, 'T0', 'T1')

INSERT INTO Ticket
VALUES
(0, 0, 0, 20, 0, '2023-01-15 11:45:00.000'),
(1, 0, 1, 70, 1, '2023-01-13 18:00:00.000'),
(2, 0, 4, 55, 2, '2023-01-15 08:12:12.000'),
(3, 1, 2, 76, 3, '2022-12-31 10:00:00.000'),
(4, 1, 3, 43, 4, '2023-01-07 19:45:00.000'),
(5, 2, 4, 33, 5, '2023-01-13 16:23:15.000'),
(6, 2, 1, 54, 6, '2023-01-13 16:23:18.000'),
(7, 3, 0, 55, 7, '2023-01-13 14:23:00.000'),
(8, 4, 3, 23, 8, '2023-01-04 16:35:00.000'),
(9, 5, 4, 66, 9, '2023-01-05 23:24:00.000')