CREATE TABLE [dbo].[PodcastFile] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Type]        VARCHAR (128)   NOT NULL,
    [PodcastId] INT NOT NULL, 
    [PodcastItemId] INT NOT NULL,
    [Data]        VARBINARY (MAX) NOT NULL,
    [CreateDate] DATETIME2        DEFAULT (getdate()) NOT NULL,
    [ModifyDate] DATETIME2        DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_podcast_file_podcast_item] FOREIGN KEY ([PodcastItemId]) REFERENCES podcast_item([Id]),
    CONSTRAINT [FK_podcast_file_podcast] FOREIGN KEY ([PodcastId]) REFERENCES podcast([Id])
);

