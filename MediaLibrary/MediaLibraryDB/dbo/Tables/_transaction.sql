CREATE TABLE [dbo].[_transaction] (
    [id]             INT      IDENTITY (1, 1) NOT NULL,
    [status]         INT      NOT NULL,
    [message]        TEXT     NULL,
    [status_message] TEXT     NULL,
    [error_message]  TEXT     NULL,
    [type]           INT      NOT NULL,
    [create_date]    DATETIME2 DEFAULT (getdate()) NOT NULL,
    [modify_date]    DATETIME2 DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([id] ASC)
);

