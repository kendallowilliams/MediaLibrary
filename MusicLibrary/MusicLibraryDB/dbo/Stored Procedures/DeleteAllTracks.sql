CREATE PROCEDURE [dbo].[DeleteAllTracks]
AS
	TRUNCATE TABLE track_file;
	DELETE track;
	DELETE path;
