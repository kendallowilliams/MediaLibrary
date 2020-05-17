CREATE TABLE [dbo].[Genre] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (150) NOT NULL,
    [CreateDate] DATETIME2      DEFAULT (getdate()) NOT NULL,
    [ModifyDate] DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

