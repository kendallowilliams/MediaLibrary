CREATE TABLE [dbo].[PlaylistPodcastItem]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [PlaylistId] INT NOT NULL, 
    [PodcastItemId] INT NOT NULL, 
    [CreateDate] DATETIME2 DEFAULT (getdate()) NOT NULL, 
    [ModifyDate] DATETIME2 DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_playlist_podcastitem_podcastitem] FOREIGN KEY ([PodcastItemId]) REFERENCES PodcastItem([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_playlist_podcastitem_playlist] FOREIGN KEY ([PlaylistId]) REFERENCES Playlist([Id]) ON DELETE CASCADE
)
