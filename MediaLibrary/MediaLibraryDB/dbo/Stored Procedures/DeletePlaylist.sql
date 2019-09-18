CREATE PROCEDURE [dbo].[DeletePlaylist]
	@playlist_id int = NULL
AS
	DELETE PlaylistTrack
	WHERE PlaylistId = @playlist_id;

	DELETE playlist
	WHERE id = @playlist_id;
