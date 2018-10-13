CREATE PROCEDURE [dbo].[DeleteAllArtists]
AS
	UPDATE track SET artist_id = NULL;
	DELETE artist;