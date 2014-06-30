IF NOT exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AccountPermissions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[AccountPermissions] (
		[Account_ID] INT NOT NULL CONSTRAINT [FK_AccountPermissions_Accounts] FOREIGN KEY REFERENCES [dbo].[Accounts](Account_ID),
		[User_ID] INT NOT NULL CONSTRAINT [FK_AccountPermissions_Users] FOREIGN KEY REFERENCES [dbo].[Users]([User_ID]),
		CONSTRAINT [PK_AccountPermissions] PRIMARY KEY CLUSTERED (
			[Account_ID],
			[User_ID]
		) ON [PRIMARY]
	) ON [PRIMARY]
END
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ClearAccountPermissions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[ClearAccountPermissions]
GO
CREATE PROCEDURE [dbo].[ClearAccountPermissions]
@Account_ID	INT
AS
BEGIN
	DELETE FROM [AccountPermissions] WHERE [Account_ID] = @Account_ID;
END
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddAccountPermission]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[AddAccountPermission]
GO
CREATE PROCEDURE [dbo].[AddAccountPermission]
@Account_ID	INT,
@User_ID	INT
AS
BEGIN
	DELETE FROM [AccountPermissions] WHERE [Account_ID] = @Account_ID AND [User_ID] = @User_ID;
	INSERT INTO [AccountPermissions]( [Account_ID], [User_ID] ) VALUES( @Account_ID, @User_ID );
END
GO
