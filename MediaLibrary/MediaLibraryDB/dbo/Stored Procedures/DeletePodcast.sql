CREATE PROCEDURE [dbo].[DeletePodcast]
	@PodcastId int = NULL
AS
	DECLARE @count int = 0;

	DELETE PodcastFile
	WHERE PodcastId = @PodcastId;

	SET @count = @count + @@ROWCOUNT;

	DELETE PodcastItem
	WHERE PodcastId = @PodcastId;

	SET @count = @count + @@ROWCOUNT;

	DELETE podcast
	WHERE id = @PodcastId;

	SET @count = @count + @@ROWCOUNT;

	SELECT CASE WHEN @count > 0 THEN 1 ELSE 0 END;
