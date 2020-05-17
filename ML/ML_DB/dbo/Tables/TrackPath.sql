CREATE TABLE [dbo].[TrackPath] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Location]       VARCHAR (MAX) NOT NULL,
    [LastScanDate] DATETIME2      DEFAULT (getdate()) NOT NULL,
    [CreateDate]    DATETIME2      DEFAULT (getdate()) NOT NULL,
    [ModifyDate]    DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

