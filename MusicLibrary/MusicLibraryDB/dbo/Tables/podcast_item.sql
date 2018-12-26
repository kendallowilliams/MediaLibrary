CREATE TABLE [dbo].[podcast_item] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [title]        VARCHAR (150) NOT NULL,
    [description]  VARCHAR (MAX) NULL,
    [length]       INT           NULL,
    [url]          VARCHAR (MAX) NOT NULL,
    [podcast_id]   INT           NOT NULL,
    [publish_date] DATETIME      NOT NULL,
    [create_date]  DATETIME      DEFAULT (getdate()) NOT NULL,
    [modify_date]  DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([id] ASC),
    FOREIGN KEY ([podcast_id]) REFERENCES [dbo].[podcast] ([id])
);

