CREATE TABLE [dbo].[Series] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Title]       VARCHAR (256) NOT NULL,
    [CreateDate] DATETIME2      DEFAULT (getdate()) NOT NULL,
    [ModifyDate] DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
);