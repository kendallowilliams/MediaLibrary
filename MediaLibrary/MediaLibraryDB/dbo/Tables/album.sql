CREATE TABLE [dbo].[album] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [title]       VARCHAR (150) NOT NULL,
    [artist_id]   INT           NULL,
    [year]        INT           NULL,
    [genre_id]    INT           NULL,
    [create_date] DATETIME2      DEFAULT (getdate()) NOT NULL,
    [modify_date] DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([id] ASC),
    FOREIGN KEY ([artist_id]) REFERENCES [dbo].[artist] ([id]),
    FOREIGN KEY ([genre_id]) REFERENCES [dbo].[genre] ([id])
);

