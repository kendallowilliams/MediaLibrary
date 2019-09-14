CREATE PROCEDURE [dbo].[FindAlbums]
	@title VARCHAR(MAX) = NULL,
	@artist_id INT = NULL,
	@year INT = NULL,
	@genre_id INT = NULL
AS
	SELECT * FROM album
	WHERE title = @title OR 
		  artist_id = @artist_id OR 
		  year = @year OR 
		  genre_id = @genre_id;