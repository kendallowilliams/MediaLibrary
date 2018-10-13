CREATE PROCEDURE [dbo].[FindTransactions]
	@status INT = NULL,
	@message VARCHAR(MAX) = '',
	@error_message VARCHAR(MAX) = '',
	@status_message VARCHAR(MAX) = '',
	@type INT = NULL,
	@start_date DATETIME = NULL,
	@end_date DATETIME = NULL
AS
	SELECT *
	FROM _transaction
	WHERE status = @status OR
		  message LIKE CONCAT('%',@message,'%') OR
		  error_message LIKE CONCAT('%',@error_message,'%') OR
		  status_message LIKE CONCAT('%',@status_message,'%') OR
		  type = @type OR
		  create_date BETWEEN @start_date AND @end_date
