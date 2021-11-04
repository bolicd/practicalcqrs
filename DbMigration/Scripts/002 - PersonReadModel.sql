BEGIN TRANSACTION
CREATE TABLE [dbo].[PersonReadModel](
    [Id] [int] NOT NULL IDENTITY ,
    [FirstName] [nvarchar] (128) NULL,
	[LastName] [nvarchar] (128) NULL,
    [Street] [nvarchar] (255) NULL,
    [City] [nvarchar] (255) NULL,
    [Country] [nvarchar] (255) NULL,
    [ZipCode] [nvarchar] (50) NULL,
    [UpdatedAt] [datetime] NOT NULL,
	[Sequence] [int] NOT NULL,
	[EventId] [nvarchar](128) NOT NULL,
    [AggregateId] [nvarchar](255) NOT NULL
)

GO
COMMIT