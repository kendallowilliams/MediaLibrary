CREATE TABLE [dbo].[podcast_file] (
    [id]          INT             IDENTITY (1, 1) NOT NULL,
    [type]        VARCHAR (128)   NOT NULL,
    [podcast_id] INT NOT NULL, 
    [podcast_item_id] INT NOT NULL,
    [data]        VARBINARY (MAX) NOT NULL,
    [create_date] DATETIME        DEFAULT (getdate()) NOT NULL,
    [modify_date] DATETIME        DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([id] ASC),
    CONSTRAINT [FK_podcast_file_podcast_item] FOREIGN KEY (podcast_item_id) REFERENCES podcast_item(id),
    CONSTRAINT [FK_podcast_file_podcast] FOREIGN KEY (podcast_id) REFERENCES podcast(id)
);

