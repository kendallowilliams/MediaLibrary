CREATE PROCEDURE [dbo].[DeleteAllTracks]
AS
	DELETE track_file;
	DELETE track;
	DELETE path;
