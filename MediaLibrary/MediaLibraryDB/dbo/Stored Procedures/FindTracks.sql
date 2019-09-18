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
		  PathId = @path_id OR
		  ArtistId = @artist_id OR
		  AlbumId = @album_id OR
		  GenreId = @genre_id OR
		  year = @year;
