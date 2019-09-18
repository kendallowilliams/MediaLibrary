CREATE PROCEDURE [dbo].[DeleteAllPlaylists]
	@playlist_id int = NULL
AS
	DELETE PlaylistTrack;
	DELETE playlist;
