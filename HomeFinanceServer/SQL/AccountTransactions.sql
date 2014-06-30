IF NOT exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AccountTransactions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[AccountTransactions] (
		[AccountTransaction_ID] INT IDENTITY (1, 1) NOT NULL CONSTRAINT [PK_AccountTransactions] PRIMARY KEY,
		[Account_ID] INT NOT NULL CONSTRAINT [FK_AccountTransations_Accounts] FOREIGN KEY REFERENCES [dbo].[Accounts](Account_ID),
		[Amount] FLOAT NOT NULL,
		[User_ID] INT NOT NULL CONSTRAINT [PK_AccountTransations_Users] FOREIGN KEY REFERENCES [dbo].[Users]([User_ID]),
		[Date] DATETIME NOT NULL CONSTRAINT [DF_AccountTransations_Date] DEFAULT(GETDATE()),
		[Comment] VARCHAR(MAX) NULL
	) ON [PRIMARY]
END
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAccountTransactions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetAccountTransactions]
GO
CREATE PROCEDURE [dbo].[GetAccountTransactions]
@Account_ID	INT
AS
BEGIN
	SELECT * FROM [AccountTransactions] WHERE [Account_ID] = @Account_ID ORDER BY [Date];
END
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateAccountTransaction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[UpdateAccountTransaction]
GO
CREATE PROCEDURE [dbo].[UpdateAccountTransaction]
@AccountTransaction_ID	INT OUTPUT,
@Account_ID	INT,
@Amount	FLOAT,
@User_ID	INT,
@Date	DATETIME,
@Comment	VARCHAR(MAX)
AS
BEGIN
	IF @AccountTransaction_ID IS NULL OR @AccountTransaction_ID = 0
	 BEGIN
		INSERT INTO [AccountTransactions]( [Account_ID], [Amount], [User_ID], [Date], [Comment] ) VALUES( @Account_ID, @Amount, @User_ID, @Date, @Comment );
		SELECT @AccountTransaction_ID = @@IDENTITY;
	 END
	ELSE
	 BEGIN
		UPDATE [AccountTransactions]
			SET
				[Account_ID] = @Account_ID,
				[Amount] = @Amount,
				[User_ID] = @User_ID,
				[Date] = @Date,
				[Comment] = @Comment
			WHERE
				[AccountTransaction_ID] = @AccountTransaction_ID;
	 END
END
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteAccountTransaction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[DeleteAccountTransaction]
GO
CREATE PROCEDURE [dbo].[DeleteAccountTransaction]
@AccountTransaction_ID	INT
AS
BEGIN
	DELETE FROM [AccountTransactions] WHERE [AccountTransaction_ID] = @AccountTransaction_ID;
END
GO
