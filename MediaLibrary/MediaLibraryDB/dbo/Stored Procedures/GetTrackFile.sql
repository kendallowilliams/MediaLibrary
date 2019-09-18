CREATE PROCEDURE [dbo].[GetTrackFile]
	@track_id int = 0
AS
	SELECT *
	FROM TrackFile
	WHERE TrackId = @track_id;
