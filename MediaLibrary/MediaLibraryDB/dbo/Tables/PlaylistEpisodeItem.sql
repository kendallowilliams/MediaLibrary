CREATE TABLE [dbo].[PlaylistEpisode]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [PlaylistId] INT NOT NULL, 
    [EpisodeId] INT NOT NULL, 
    [CreateDate] DATETIME2 DEFAULT (getdate()) NOT NULL, 
    [ModifyDate] DATETIME2 DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_playlist_episode_episode] FOREIGN KEY ([EpisodeId]) REFERENCES Episode([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_playlist_episode_playlist] FOREIGN KEY ([PlaylistId]) REFERENCES Playlist([Id]) ON DELETE CASCADE
)
