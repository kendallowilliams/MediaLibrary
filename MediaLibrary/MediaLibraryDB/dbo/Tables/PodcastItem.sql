CREATE TABLE [dbo].[PodcastItem] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Title]        VARCHAR (150) NOT NULL,
    [Description]  VARCHAR (MAX) NULL,
    [Length]       INT           NULL,
    [Url]          VARCHAR (MAX) NOT NULL,
	[File]		VARCHAR(MAX) NULL,
    [PodcastId]   INT           NOT NULL,
    [PublishDate] DATETIME2      NOT NULL,
	[PlayCount] INT  DEFAULT (0) NOT NULL,
    [CreateDate]  DATETIME2      DEFAULT (getdate()) NOT NULL,
    [ModifyDate]  DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
    FOREIGN KEY ([PodcastId]) REFERENCES [dbo].[Podcast] ([Id]) ON DELETE CASCADE
);

