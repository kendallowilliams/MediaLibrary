CREATE PROCEDURE [dbo].[FindPodcastItems]
	@podcast_id INT = NULL
AS
	SELECT * FROM PodcastItem
	WHERE PodcastId = @podcast_id;