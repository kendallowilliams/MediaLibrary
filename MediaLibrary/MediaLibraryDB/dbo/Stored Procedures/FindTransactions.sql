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
	FROM [Transaction]
	WHERE status = @status OR
		  message LIKE CONCAT('%',@message,'%') OR
		  ErrorMessage LIKE CONCAT('%',@error_message,'%') OR
		  StatusMessage LIKE CONCAT('%',@status_message,'%') OR
		  type = @type OR
		  CreateDate BETWEEN @start_date AND @end_date
