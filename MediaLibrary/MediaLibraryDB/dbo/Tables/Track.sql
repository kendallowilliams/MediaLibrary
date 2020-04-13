CREATE TABLE [dbo].[Track] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [FileName]   VARCHAR (256) NOT NULL,
    [PathId]     INT           NULL,
    [Title]       VARCHAR (150) NOT NULL,
    [AlbumId]    INT           NULL,
    [GenreId]    INT           NULL,
    [ArtistId]   INT           NULL,
    [Position]    INT           NULL,
    [Year]        INT           NULL,
    [Duration]    DECIMAL (18)  NOT NULL,
    [Progress]    INT DEFAULT (0) NOT NULL,
    [PlayCount]  INT           DEFAULT (0) NOT NULL,
    [LastPlayedDate] DATETIME2      NULL,
    [CreateDate] DATETIME2      DEFAULT (getdate()) NOT NULL,
    [ModifyDate] DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
    FOREIGN KEY ([AlbumId]) REFERENCES [dbo].[Album] ([Id]) ON DELETE SET NULL,
    FOREIGN KEY ([ArtistId]) REFERENCES [dbo].[Artist] ([Id]) ON DELETE SET NULL,
    FOREIGN KEY ([PathId]) REFERENCES [dbo].[TrackPath] ([Id]) ON DELETE CASCADE,
	FOREIGN KEY ([GenreId]) REFERENCES [dbo].[Genre] ([Id]) ON DELETE SET NULL
);

