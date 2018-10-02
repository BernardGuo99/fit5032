CREATE TABLE [dbo].[Branch] (
    [branchId]  INT             IDENTITY (1, 1) NOT NULL,
    [name]      NVARCHAR (100)  NOT NULL,
    [address]   NVARCHAR (100)  NOT NULL,
    [suburb]    NVARCHAR (100)  NOT NULL,
    [state]     NVARCHAR (100)  NOT NULL,
    [postcode]  NCHAR (4)       NOT NULL,
    [latitude]  NUMERIC (10, 8) NOT NULL,
    [longitude] NUMERIC (11, 8) NOT NULL,
    [open]      NCHAR (7)       NOT NULL,
    [close]     NCHAR (7)       NOT NULL,
    PRIMARY KEY CLUSTERED ([branchId] ASC),
	CONSTRAINT CK_Latitude CHECK (Latitude >= -90 AND Latitude <= 90),
	CONSTRAINT CK_Longtitude CHECK (Longitude >= -180 AND Longitude <= 180),
	CONSTRAINT uq_location UNIQUE (address, suburb, state)
);