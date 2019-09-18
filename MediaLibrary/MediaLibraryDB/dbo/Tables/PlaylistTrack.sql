CREATE TABLE [dbo].[PlaylistTrack]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [PlaylistId] INT NOT NULL, 
    [TrackId] INT NOT NULL, 
    [CreateDate] DATETIME2 DEFAULT (getdate()) NOT NULL, 
    [ModifyDate] DATETIME2 DEFAULT (getdate()) NOT NULL, 
    CONSTRAINT [FK_playlist_track_track] FOREIGN KEY ([TrackId]) REFERENCES track([Id]), 
    CONSTRAINT [FK_playlist_track_playlist] FOREIGN KEY ([PlaylistId]) REFERENCES playlist([Id])
)
