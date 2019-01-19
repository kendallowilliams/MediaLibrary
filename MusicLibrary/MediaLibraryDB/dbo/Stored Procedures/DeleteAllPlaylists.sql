CREATE PROCEDURE [dbo].[DeleteAllPlaylists]
	@playlist_id int = NULL
AS
	DELETE playlist_track;
	DELETE playlist;
