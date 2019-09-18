CREATE TABLE [dbo].[Transaction] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [Status]         INT      NOT NULL,
    [Message]        TEXT     NULL,
    [StatusMessage] TEXT     NULL,
    [ErrorMessage]  TEXT     NULL,
    [Type]           INT      NOT NULL,
    [CreateDate]    DATETIME2 DEFAULT (getdate()) NOT NULL,
    [ModifyDate]    DATETIME2 DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

