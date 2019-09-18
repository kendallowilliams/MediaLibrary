CREATE TABLE [dbo].[album] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Title]       VARCHAR (150) NOT NULL,
    [ArtistId]   INT           NULL,
    [Year]        INT           NULL,
    [GenreId]    INT           NULL,
    [CreateDate] DATETIME2      DEFAULT (getdate()) NOT NULL,
    [ModifyDate] DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
    FOREIGN KEY ([ArtistId]) REFERENCES [dbo].[artist] ([Id]),
    FOREIGN KEY ([GenreId]) REFERENCES [dbo].[genre] ([Id])
);

