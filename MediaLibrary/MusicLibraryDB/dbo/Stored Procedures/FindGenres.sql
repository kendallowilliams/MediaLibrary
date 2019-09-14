CREATE PROCEDURE [dbo].[FindGenres]
	@name VARCHAR(MAX) = NULL
AS
	SELECT * 
	FROM genre
	WHERE name = @name;
