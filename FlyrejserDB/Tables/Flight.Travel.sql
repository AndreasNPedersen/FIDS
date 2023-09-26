CREATE TABLE [Flight].[Travel]
(
	[Id] [INT] Identity(1,1) NOT NULL,
    [ToLocation] [nvarchar](200) NOT NULL,
    [FromLocation] [nvarchar](200) NOT NULL,
    [ArrivalDate] [DateTime2](7) NOT NULL,
    [DepartureDate] [DateTime2](7) NOT NULL,
    [FlightId] INT NOT NULL,
    CONSTRAINT [PK_FlightTravel__ID] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
