CREATE TABLE [dbo].[podcast] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [title]            VARCHAR (150) NOT NULL,
    [url]              VARCHAR (MAX) NOT NULL,
    [last_update_date] DATETIME      NOT NULL,
    [create_date]      DATETIME      DEFAULT (getdate()) NOT NULL,
    [modify_date]      DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([id] ASC)
);

