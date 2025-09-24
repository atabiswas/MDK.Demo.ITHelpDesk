USE [ITHelpDesk]
GO

/****** Object: Table [dbo].[User] Script Date: 9/24/2025 1:45:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User] (
    [Id]        INT            NOT NULL,
    [FirstName] NVARCHAR (50)  NOT NULL,
    [LastName]  NVARCHAR (50)  NOT NULL,
    [EmailId]   NVARCHAR (100) NOT NULL
);


