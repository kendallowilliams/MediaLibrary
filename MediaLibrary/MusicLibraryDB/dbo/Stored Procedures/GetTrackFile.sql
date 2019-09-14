CREATE PROCEDURE [dbo].[GetTrackFile]
	@track_id int = 0
AS
	SELECT *
	FROM track_file
	WHERE track_id = @track_id;
