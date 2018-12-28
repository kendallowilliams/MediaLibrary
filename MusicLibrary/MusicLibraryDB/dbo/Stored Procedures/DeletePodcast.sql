CREATE PROCEDURE [dbo].[DeletePodcast]
	@podcast_id int = NULL
AS
	DECLARE @count int = 0;

	DELETE podcast_file
	WHERE podcast_id = @podcast_id;

	SET @count = @count + @@ROWCOUNT;

	DELETE podcast_item
	WHERE podcast_id = @podcast_id;

	SET @count = @count + @@ROWCOUNT;

	DELETE podcast
	WHERE id = @podcast_id;

	SET @count = @count + @@ROWCOUNT;

	SELECT CASE WHEN @count > 0 THEN 1 ELSE 0 END;
