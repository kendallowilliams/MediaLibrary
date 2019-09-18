CREATE TABLE [dbo].[path] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Location]       VARCHAR (256) NOT NULL,
    [LastScanDate] DATETIME      DEFAULT (getdate()) NOT NULL,
    [CreateDate]    DATETIME2      DEFAULT (getdate()) NOT NULL,
    [ModifyDate]    DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

