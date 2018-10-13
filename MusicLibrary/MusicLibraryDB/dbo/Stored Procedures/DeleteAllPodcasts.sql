CREATE PROCEDURE [dbo].[DeleteAllPodcasts]
AS
	DELETE podcast_file;
	DELETE podcast_item;
	DELETE podcast;
