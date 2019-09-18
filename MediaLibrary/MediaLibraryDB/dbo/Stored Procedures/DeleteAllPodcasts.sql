CREATE PROCEDURE [dbo].[DeleteAllPodcasts]
AS
	TRUNCATE TABLE PodcastFile;
	DELETE PodcastItem;
	DELETE podcast;
