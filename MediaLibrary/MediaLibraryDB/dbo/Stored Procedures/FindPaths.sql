CREATE PROCEDURE [dbo].[FindPaths]
	@location VARCHAR(256) = NULL
AS
	SELECT *
	FROM path
	WHERE location = @location;