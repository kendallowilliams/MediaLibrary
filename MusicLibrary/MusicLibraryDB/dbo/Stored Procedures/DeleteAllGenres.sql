CREATE PROCEDURE [dbo].[DeleteAllGenres]
AS
	UPDATE track SET genre_id = NULL;
	UPDATE album SET genre_id = NULL;
	DELETE genre;
