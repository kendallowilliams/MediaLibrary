CREATE TABLE [dbo].[path] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [location]       VARCHAR (256) NOT NULL,
    [last_scan_date] DATETIME      DEFAULT (getdate()) NOT NULL,
    [create_date]    DATETIME2      DEFAULT (getdate()) NOT NULL,
    [modify_date]    DATETIME2      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([id] ASC)
);

