CREATE TABLE [dbo].[PlaylistTrack]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [PlaylistId] INT NOT NULL, 
    [TrackId] INT NOT NULL, 
    [CreateDate] DATETIME2 DEFAULT (getdate()) NOT NULL, 
    [ModifyDate] DATETIME2 DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_playlist_track_track] FOREIGN KEY ([TrackId]) REFERENCES Track([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_playlist_track_playlist] FOREIGN KEY ([PlaylistId]) REFERENCES Playlist([Id]) ON DELETE CASCADE
)
