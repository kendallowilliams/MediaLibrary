CREATE TABLE [dbo].[playlist_track]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [playlist_id] INT NOT NULL, 
    [track_id] INT NOT NULL, 
    [create_date] DATETIME2 DEFAULT (getdate()) NOT NULL, 
    [modify_date] DATETIME2 DEFAULT (getdate()) NOT NULL, 
    CONSTRAINT [FK_playlist_track_track] FOREIGN KEY (track_id) REFERENCES track([Id]), 
    CONSTRAINT [FK_playlist_track_playlist] FOREIGN KEY (playlist_id) REFERENCES playlist([Id])
)
