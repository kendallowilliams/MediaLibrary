CREATE PROCEDURE [dbo].[FindAlbums]
	@title VARCHAR(MAX) = NULL,
	@artist_id INT = NULL,
	@year INT = NULL,
	@genre_id INT = NULL
AS
	SELECT * FROM album
	WHERE title = @title OR 
		  ArtistId = @artist_id OR 
		  year = @year OR 
		  GenreId = @genre_id;