CREATE PROCEDURE [dbo].[FindTracks]
	@title int = NULL,
	@path_id int = NULL,
	@artist_id int = NULL,
	@album_id int = NULL,
	@genre_id int = NULL,
	@year int = NULL
AS
	SELECT *
	FROM track
	WHERE title = @title OR
		  path_id = @path_id OR
		  artist_id = @artist_id OR
		  album_id = @album_id OR
		  genre_id = @genre_id OR
		  year = @year;
