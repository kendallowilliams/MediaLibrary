CREATE PROCEDURE [dbo].[DeleteAllArtists]
AS
	UPDATE track SET ArtistId = NULL;
	DELETE artist;