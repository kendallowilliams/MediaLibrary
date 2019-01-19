CREATE PROCEDURE [dbo].[DeleteAllPodcasts]
AS
	TRUNCATE TABLE podcast_file;
	DELETE podcast_item;
	DELETE podcast;
