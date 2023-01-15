SELECT TOP 5 Ticket.FK_Client, COUNT(Ticket.FK_Client) as Tickets
FROM Ticket
GROUP BY Ticket.FK_Client
ORDER BY Tickets DESC