USE [ITHelpDesk]
GO

/****** Object: Table [dbo].[Ticket] Script Date: 9/24/2025 1:44:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ticket] (
    [Id]             INT             NOT NULL,
    [Title]          NVARCHAR (200)  NOT NULL,
    [Description]    NVARCHAR (2000) NULL,
    [StatusId]       SMALLINT        NOT NULL,
    [AssignedUserId] SMALLINT        NULL,
    [CreatedUserId]  SMALLINT        NULL,
    [CreatedAt]      DATETIME        NOT NULL,
    [ModifiedAt]     DATETIME        NULL
);


