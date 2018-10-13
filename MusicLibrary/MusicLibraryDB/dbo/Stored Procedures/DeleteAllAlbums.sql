CREATE PROCEDURE [dbo].[DeleteAllAlbums]
AS
	UPDATE track SET album_id = NULL;
	DELETE album;
