CREATE TABLE [dbo].[Podcast] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Title]            VARCHAR (150) NOT NULL,
    [Url]              VARCHAR (MAX) NOT NULL,
    [ImageUrl]        VARCHAR (MAX) NULL,
	[Description]	   VARCHAR (MAX) NULL,
    [Author]		   VARCHAR (MAX) NULL,
    [LastUpdateDate] DATETIME2      NOT NULL,
    [CreateDate]      DATETIME2      DEFAULT (getdate()) NOT NULL,
    [ModifyDate]      DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

