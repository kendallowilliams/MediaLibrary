CREATE TABLE [dbo].[TrackFile] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
	[TrackId]	  INT			  NOT NULL,
    [Type]        VARCHAR (128)   NOT NULL,
    [Data]        VARBINARY (MAX) NOT NULL,
    [CreateDate] DATETIME2        DEFAULT (getdate()) NOT NULL,
    [ModifyDate] DATETIME2        DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_track_file_track] FOREIGN KEY ([TrackId]) REFERENCES track([Id])
);

