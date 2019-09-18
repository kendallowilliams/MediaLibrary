CREATE TABLE [dbo].[podcast] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [title]            VARCHAR (150) NOT NULL,
    [url]              VARCHAR (MAX) NOT NULL,
    [image_url]        VARCHAR (MAX) NULL,
	[description]	   VARCHAR (MAX) NULL,
    [author]		   VARCHAR (MAX) NULL,
    [last_update_date] DATETIME2      NOT NULL,
    [create_date]      DATETIME2      DEFAULT (getdate()) NOT NULL,
    [modify_date]      DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([id] ASC)
);

