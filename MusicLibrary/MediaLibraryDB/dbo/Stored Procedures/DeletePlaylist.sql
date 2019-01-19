CREATE PROCEDURE [dbo].[DeletePlaylist]
	@playlist_id int = NULL
AS
	DELETE playlist_track
	WHERE playlist_id = @playlist_id;

	DELETE playlist
	WHERE id = @playlist_id;
