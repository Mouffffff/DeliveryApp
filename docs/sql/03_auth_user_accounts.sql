/*
03_auth_user_accounts.sql

A executer dans DeliveryAppDb avant d'utiliser l'auth JWT.
*/

SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID('dbo.UserAccounts', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.UserAccounts
        (
            Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_UserAccounts PRIMARY KEY,
            Email NVARCHAR(256) NOT NULL,
            PasswordHash NVARCHAR(512) NOT NULL,
            DisplayName NVARCHAR(150) NOT NULL,
            [Role] NVARCHAR(20) NOT NULL,
            IsActive BIT NOT NULL CONSTRAINT DF_UserAccounts_IsActive DEFAULT (1),
            CustomerId INT NULL,
            CourierId INT NULL,
            CreatedAt DATETIME2(7) NOT NULL CONSTRAINT DF_UserAccounts_CreatedAt DEFAULT (GETUTCDATE()),
            LastLoginAt DATETIME2(7) NULL
        );

        ALTER TABLE dbo.UserAccounts
            ADD CONSTRAINT FK_UserAccounts_Customers_CustomerId
                FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(Id) ON DELETE SET NULL;

        ALTER TABLE dbo.UserAccounts
            ADD CONSTRAINT FK_UserAccounts_Couriers_CourierId
                FOREIGN KEY (CourierId) REFERENCES dbo.Couriers(Id) ON DELETE SET NULL;

        CREATE UNIQUE INDEX UX_UserAccounts_Email
            ON dbo.UserAccounts(Email);

        CREATE UNIQUE INDEX UX_UserAccounts_CustomerId
            ON dbo.UserAccounts(CustomerId)
            WHERE CustomerId IS NOT NULL;

        CREATE UNIQUE INDEX UX_UserAccounts_CourierId
            ON dbo.UserAccounts(CourierId)
            WHERE CourierId IS NOT NULL;
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

SELECT TOP 5 *
FROM dbo.UserAccounts
ORDER BY Id DESC;
GO
