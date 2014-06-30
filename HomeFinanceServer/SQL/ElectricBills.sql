IF NOT exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ElectricBills]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[ElectricBills] (
		[ElectricBill_ID] INT IDENTITY (1, 1) NOT NULL CONSTRAINT [PK_ElectricBills] PRIMARY KEY,
		[TotalUsedKWh] FLOAT NOT NULL,
		[TotalPriceElectricity] FLOAT NOT NULL,
		[TotalPriceGroundFee] FLOAT NOT NULL,
		[GuestLastReadingTicks] BIGINT NOT NULL,
		[GuestLastReadingKWh] FLOAT NOT NULL,
		[GuestCurrentReadingTicks] BIGINT NOT NULL,
		[GuestCurrentReadingKWh] FLOAT NOT NULL,
		[GuestPartInGroundFee] INT NOT NULL,
		[OCR] VARCHAR(255) NULL,
		[CreatedDate] DATETIME NOT NULL CONSTRAINT [DF_ElectricBills_CreatedDate] DEFAULT(GETDATE()),
		[CreatedByUser_ID] INT NOT NULL CONSTRAINT [FK_ElectricBills_Users] FOREIGN KEY REFERENCES [dbo].[Users]([User_ID])
	) ON [PRIMARY]
END
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetElectricBills]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetElectricBills]
GO
CREATE PROCEDURE [dbo].[GetElectricBills]
AS
BEGIN
	SELECT * FROM [ElectricBills] ORDER BY [ElectricBill_ID];
END
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateElectricBill]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[UpdateElectricBill]
GO
CREATE PROCEDURE [dbo].[UpdateElectricBill]
@ElectricBill_ID INT OUTPUT,
@TotalUsedKWh FLOAT,
@TotalPriceElectricity FLOAT,
@TotalPriceGroundFee FLOAT,
@GuestLastReadingTicks BIGINT,
@GuestLastReadingKWh FLOAT,
@GuestCurrentReadingTicks BIGINT,
@GuestCurrentReadingKWh FLOAT,
@GuestPartInGroundFee INT,
@OCR VARCHAR(255),
@CreatedByUser_ID INT
AS
BEGIN
	IF @ElectricBill_ID IS NULL OR @ElectricBill_ID = 0
	 BEGIN
		INSERT INTO [ElectricBills]( TotalUsedKWh, TotalPriceElectricity, TotalPriceGroundFee, GuestLastReadingTicks, GuestLastReadingKWh, GuestCurrentReadingTicks, GuestCurrentReadingKWh, GuestPartInGroundFee, OCR, CreatedDate, CreatedByUser_ID )
		VALUES( @TotalUsedKWh, @TotalPriceElectricity, @TotalPriceGroundFee, @GuestLastReadingTicks, @GuestLastReadingKWh, @GuestCurrentReadingTicks, @GuestCurrentReadingKWh, @GuestPartInGroundFee, @OCR, GETDATE(), @CreatedByUser_ID );
		SELECT @ElectricBill_ID = @@IDENTITY;
	 END
	ELSE
	 BEGIN
		UPDATE [ElectricBills]
			SET
				[TotalUsedKWh] = @TotalUsedKWh,
				[TotalPriceElectricity] = @TotalPriceElectricity,
				[TotalPriceGroundFee] = @TotalPriceGroundFee,
				[GuestLastReadingTicks] = @GuestLastReadingTicks,
				[GuestLastReadingKWh] = @GuestLastReadingKWh,
				[GuestCurrentReadingTicks] = @GuestCurrentReadingTicks,
				[GuestCurrentReadingKWh] = @GuestCurrentReadingKWh,
				[GuestPartInGroundFee] = @GuestPartInGroundFee,
				[OCR] = @OCR,
				[CreatedDate] = GETDATE(),
				[CreatedByUser_ID] = @CreatedByUser_ID
			WHERE
				[ElectricBill_ID] = @ElectricBill_ID;
	 END
END
GO