CREATE PROCEDURE [dbo].[DeleteAllGenres]
AS
	UPDATE track SET GenreId = NULL;
	UPDATE album SET GenreId = NULL;
	DELETE genre;
