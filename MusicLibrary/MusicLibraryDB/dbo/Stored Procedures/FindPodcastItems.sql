CREATE PROCEDURE [dbo].[FindPodcastItems]
	@podcast_id INT = NULL
AS
	SELECT * FROM podcast_item
	WHERE podcast_id = @podcast_id;