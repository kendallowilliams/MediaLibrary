CREATE PROCEDURE [dbo].[DeleteAllTracks]
AS
	TRUNCATE TABLE TrackFile;
	DELETE track;
	DELETE path;
