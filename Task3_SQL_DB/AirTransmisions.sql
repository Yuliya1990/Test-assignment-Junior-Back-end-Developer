CREATE TABLE "Client"(
    "id" INT NOT NULL,
    "Name" NCHAR(50) NOT NULL,
    "Surname" NCHAR(50) NOT NULL,
    "Phone" NCHAR(10) NOT NULL,
    "email" NCHAR(50) NOT NULL
);
ALTER TABLE
    "Client" ADD CONSTRAINT "client_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "client_phone_unique" ON
    "Client"("Phone");
CREATE UNIQUE INDEX "client_email_unique" ON
    "Client"("email");
CREATE TABLE "Ticket"(
    "id" INT NOT NULL,
    "FK_Client" INT NOT NULL,
    "FK_Flight" INT NOT NULL,
    "TotalPrice" MONEY NOT NULL,
    "FK_BoardingPass" INT NULL,
    "PurchaseDate" DATETIME NOT NULL
);
ALTER TABLE
    "Ticket" ADD CONSTRAINT "ticket_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "ticket_fk_boardingpass_id_unique" ON
    "Ticket"("FK_BoardingPass", "id");
CREATE TABLE "Airline"("Name" NVARCHAR(50) NOT NULL);
ALTER TABLE
    "Airline" ADD CONSTRAINT "airline_name_primary" PRIMARY KEY("Name");
CREATE TABLE "Plain"(
    "id" INT NOT NULL,
    "Name" NVARCHAR(50) NOT NULL,
    "Type" NVARCHAR(50) NOT NULL,
    "Seats" INT NOT NULL,
    "FK_Airline" NVARCHAR(50) NOT NULL
);
ALTER TABLE
    "Plain" ADD CONSTRAINT "plain_id_primary" PRIMARY KEY("id");
CREATE TABLE "Flight"(
    "id" INT NOT NULL,
    "Date" DATETIME NOT NULL,
    "FK_DerartureAirport" INT NOT NULL,
    "FK_ArrivalAirport" INT NOT NULL,
    "TimeArrive" TIME NOT NULL,
    "TimeDeparture" TIME NOT NULL,
    "FK_Plain" INT NOT NULL
);
ALTER TABLE
    "Flight" ADD CONSTRAINT "flight_id_primary" PRIMARY KEY("id");
CREATE TABLE "BoardingPass"(
    "id" INT NOT NULL,
    "BoardingNumber" INT NOT NULL,
    "SeatNumber" INT NOT NULL,
    "TerminalDeparture" NVARCHAR(10) NOT NULL,
    "TerminalArrive" NVARCHAR(10) NOT NULL
);
ALTER TABLE
    "BoardingPass" ADD CONSTRAINT "boardingpass_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "boardingpass_boardingnumber_unique" ON
    "BoardingPass"("BoardingNumber");
CREATE TABLE "Airport"(
    "id" INT NOT NULL,
    "Name" NVARCHAR(50) NOT NULL,
    "City" NVARCHAR(50) NOT NULL
);
ALTER TABLE
    "Airport" ADD CONSTRAINT "airport_id_primary" PRIMARY KEY("id");
ALTER TABLE
    "Plain" ADD CONSTRAINT "plain_fk_airline_foreign" FOREIGN KEY("FK_Airline") REFERENCES "Airline"("Name");
ALTER TABLE
    "Flight" ADD CONSTRAINT "flight_fk_derartureairport_foreign" FOREIGN KEY("FK_DerartureAirport") REFERENCES "Airport"("id");
ALTER TABLE
    "Flight" ADD CONSTRAINT "flight_fk_arrivalairport_foreign" FOREIGN KEY("FK_ArrivalAirport") REFERENCES "Airport"("id");
ALTER TABLE
    "Flight" ADD CONSTRAINT "flight_fk_plain_foreign" FOREIGN KEY("FK_Plain") REFERENCES "Plain"("id");
ALTER TABLE
    "Ticket" ADD CONSTRAINT "ticket_fk_client_foreign" FOREIGN KEY("FK_Client") REFERENCES "Client"("id");
ALTER TABLE
    "Ticket" ADD CONSTRAINT "ticket_fk_flight_foreign" FOREIGN KEY("FK_Flight") REFERENCES "Flight"("id");
ALTER TABLE
    "Ticket" ADD CONSTRAINT "ticket_fk_boardingpass_foreign" FOREIGN KEY("FK_BoardingPass") REFERENCES "BoardingPass"("id");