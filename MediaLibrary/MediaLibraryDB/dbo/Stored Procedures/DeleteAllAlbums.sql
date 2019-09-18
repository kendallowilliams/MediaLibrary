CREATE PROCEDURE [dbo].[DeleteAllAlbums]
AS
	UPDATE track SET AlbumId = NULL;
	DELETE album;
