CREATE TABLE [dbo].[track] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [file_name]   VARCHAR (256) NOT NULL,
    [path_id]     INT           NULL,
    [title]       VARCHAR (150) NOT NULL,
    [album_id]    INT           NULL,
    [genre_id]    INT           NULL,
    [artist_id]   INT           NULL,
    [position]    INT           NULL,
    [year]        INT           NULL,
    [duration]    DECIMAL (18)  NOT NULL,
    [play_count]  INT           DEFAULT ((0)) NOT NULL,
    [create_date] DATETIME      DEFAULT (getdate()) NOT NULL,
    [modify_date] DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([id] ASC),
    FOREIGN KEY ([album_id]) REFERENCES [dbo].[album] ([id]),
    FOREIGN KEY ([artist_id]) REFERENCES [dbo].[artist] ([id]),
    FOREIGN KEY ([path_id]) REFERENCES [dbo].[path] ([id])
);

