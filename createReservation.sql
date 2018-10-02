CREATE TABLE [dbo].[Reservation]
(
	[reservationId] INT NOT NULL PRIMARY KEY, 
    [branchId] INT NOT NULL, 
	[customerId] INT NOT NULL,
    [date] DATETIME NOT NULL, 
	constraint fk_reservation_branch foreign key (branchId) references [dbo].[Branch] (branchId)
)
