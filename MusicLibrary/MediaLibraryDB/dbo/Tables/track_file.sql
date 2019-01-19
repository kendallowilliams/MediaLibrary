CREATE TABLE [dbo].[track_file] (
    [id]          INT             IDENTITY (1, 1) NOT NULL,
	[track_id]	  INT			  NOT NULL,
    [type]        VARCHAR (128)   NOT NULL,
    [data]        VARBINARY (MAX) NOT NULL,
    [create_date] DATETIME        DEFAULT (getdate()) NOT NULL,
    [modify_date] DATETIME        DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([id] ASC), 
    CONSTRAINT [FK_track_file_track] FOREIGN KEY (track_id) REFERENCES track(id)
);

