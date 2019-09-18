CREATE TABLE [dbo].[playlist] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [name]        VARCHAR (150) NOT NULL,
    [create_date] DATETIME2      DEFAULT (getdate()) NOT NULL,
    [modify_date] DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([id] ASC)
);

