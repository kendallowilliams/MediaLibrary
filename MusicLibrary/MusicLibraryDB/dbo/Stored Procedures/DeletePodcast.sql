CREATE PROCEDURE [dbo].[DeletePodcast]
	@podcast_id int = NULL
AS
	DELETE podcast_file
	WHERE podcast_id = @podcast_id;

	DELETE podcast_item
	WHERE podcast_id = @podcast_id;

	DELETE podcast
	WHERE id = @podcast_id;
