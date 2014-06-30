IF NOT exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Users] (
		[User_ID] INT IDENTITY (1, 1) NOT NULL CONSTRAINT [PK_Users] PRIMARY KEY,
		[Username] VARCHAR(255) NULL,
		[CreatedDate] DATETIME NOT NULL CONSTRAINT [DF_Users_CreatedDate] DEFAULT(GETDATE()),
		[LastLoggedOn] [datetime] NULL
	) ON [PRIMARY]
END
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetUser]
GO
CREATE PROCEDURE [dbo].[GetUser]
@Username	VARCHAR(255)
AS
BEGIN
	IF NOT EXISTS(SELECT * FROM [Users] WHERE [Username] = @Username)
	 BEGIN
		INSERT INTO [Users]( [Username] ) VALUES( @Username );
	 END
	UPDATE [Users] SET [LastLoggedOn] = GETDATE() WHERE [Username] = @Username;
	SELECT * FROM [Users] WHERE [Username] = @Username;
END
GO