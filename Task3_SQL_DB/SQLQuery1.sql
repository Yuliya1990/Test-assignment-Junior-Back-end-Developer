SELECT Ticket.id as TicketId, Ticket.TotalPrice, Ticket.PurchaseDate,
Client.id as ClientId, Client.Name, Client.Surname, Client.email, Client.Phone,
Flight.Date, Flight.TimeArrive, Flight.TimeDeparture, Dep.Name as DepartureAirport, Ar.Name as ArrivalAirport
FROM (((Client INNER JOIN Ticket ON Client.id = Ticket.FK_Client)
INNER JOIN Flight ON Ticket.FK_Flight = Flight.id)
INNER JOIN Airport as Ar ON Flight.FK_ArrivalAirport = Ar.id)
INNER JOIN Airport as Dep ON Flight.FK_DerartureAirport = Dep.id