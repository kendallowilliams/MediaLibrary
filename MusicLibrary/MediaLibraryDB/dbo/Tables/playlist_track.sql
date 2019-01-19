﻿CREATE TABLE [dbo].[playlist_track]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [playlist_id] INT NOT NULL, 
    [track_id] INT NOT NULL, 
    [create_date] DATETIME DEFAULT (getdate()) NOT NULL, 
    [modify_date] DATETIME DEFAULT (getdate()) NOT NULL, 
    CONSTRAINT [FK_playlist_track_track] FOREIGN KEY (track_id) REFERENCES track(id), 
    CONSTRAINT [FK_playlist_track_playlist] FOREIGN KEY (playlist_id) REFERENCES playlist(id)
)
