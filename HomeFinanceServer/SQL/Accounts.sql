IF NOT exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Accounts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Accounts] (
		[Account_ID] INT IDENTITY (1, 1) NOT NULL CONSTRAINT [PK_Accounts] PRIMARY KEY,
		[Name] VARCHAR(255) NOT NULL,
		[CreatedDate] DATETIME NOT NULL CONSTRAINT [DF_Accounts_CreatedDate] DEFAULT(GETDATE()),
		[CreatedByUser_ID] INT NOT NULL CONSTRAINT [FK_Accounts_Users] FOREIGN KEY REFERENCES [dbo].[Users]([User_ID])
	) ON [PRIMARY]
END
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAccounts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetAccounts]
GO
CREATE PROCEDURE [dbo].[GetAccounts]
@User_ID	INT
AS
BEGIN
	--SELECT Accounts.*, (SELECT SUM( AccountTransactions.Amount ) FROM AccountTransactions WHERE AccountTransactions.Account_ID = Accounts.Account_ID) AS Balance FROM Accounts
	SELECT Accounts.* FROM Accounts
	INNER JOIN AccountPermissions ON AccountPermissions.Account_ID = Accounts.Account_ID
	WHERE AccountPermissions.User_ID = @User_ID
	ORDER BY Accounts.Name;
END
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateAccount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[UpdateAccount]
GO
CREATE PROCEDURE [dbo].[UpdateAccount]
@Account_ID	INT OUTPUT,
@Name	VARCHAR(255),
@User_ID	INT
AS
BEGIN
	IF @Account_ID IS NULL OR @Account_ID = 0
	 BEGIN
		INSERT INTO Accounts( [Name], [CreatedDate], [CreatedByUser_ID] ) VALUES( @Name, GETDATE(), @User_ID );
		SELECT @Account_ID = @@IDENTITY;
	 END
	ELSE
	 BEGIN
		UPDATE Accounts
			SET
				[Name] = @Name
			WHERE
				[Account_ID] = @Account_ID;
	 END
END
GO
