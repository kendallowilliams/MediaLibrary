CREATE PROCEDURE [dbo].[FindArtists]
	@name VARCHAR(MAX) = NULL
AS
	SELECT * FROM artist
	WHERE name = @name;