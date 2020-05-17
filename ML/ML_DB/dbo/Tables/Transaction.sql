CREATE TABLE [dbo].[Transaction] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [Status]         INT      NOT NULL,
    [Message]        NVARCHAR(MAX)     NULL,
    [StatusMessage] NVARCHAR(MAX)     NULL,
    [ErrorMessage]  NVARCHAR(MAX)     NULL,
    [Type]           INT      NOT NULL,
    [CreateDate]    DATETIME2 DEFAULT (getdate()) NOT NULL,
    [ModifyDate]    DATETIME2 DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

