BEGIN TRANSACTION;

BEGIN TRY
    DECLARE @CustomerId INT;
    DECLARE @CustomerAddressId INT;
    DECLARE @StoreAddressId INT;
    DECLARE @StoreId INT;
    DECLARE @CourierId INT;

    IF NOT EXISTS (SELECT 1 FROM Customers WHERE Email = 'customer.demo@deliveryapp.com')
    BEGIN
        INSERT INTO Customers (FullName, Email, PhoneNumber)
        VALUES ('Customer Demo', 'customer.demo@deliveryapp.com', '+33610000000');
    END;

    SELECT @CustomerId = Id
    FROM Customers
    WHERE Email = 'customer.demo@deliveryapp.com';

    IF NOT EXISTS (
        SELECT 1
        FROM Addresses
        WHERE Street = '25 Rue de Rivoli' AND City = 'Paris' AND ZipCode = '75004'
    )
    BEGIN
        INSERT INTO Addresses (Street, City, ZipCode, CustomerId)
        VALUES ('25 Rue de Rivoli', 'Paris', '75004', @CustomerId);
    END;

    SELECT TOP 1 @CustomerAddressId = Id
    FROM Addresses
    WHERE Street = '25 Rue de Rivoli' AND City = 'Paris' AND ZipCode = '75004'
    ORDER BY Id DESC;

    IF NOT EXISTS (
        SELECT 1
        FROM Addresses
        WHERE Street = '10 Boulevard Voltaire' AND City = 'Paris' AND ZipCode = '75011'
    )
    BEGIN
        INSERT INTO Addresses (Street, City, ZipCode, CustomerId)
        VALUES ('10 Boulevard Voltaire', 'Paris', '75011', NULL);
    END;

    SELECT TOP 1 @StoreAddressId = Id
    FROM Addresses
    WHERE Street = '10 Boulevard Voltaire' AND City = 'Paris' AND ZipCode = '75011'
    ORDER BY Id DESC;

    IF NOT EXISTS (SELECT 1 FROM Stores WHERE Name = 'Burger Street')
    BEGIN
        INSERT INTO Stores (Name, Category, AddressId)
        VALUES ('Burger Street', 'Fast Food', @StoreAddressId);
    END;

    SELECT @StoreId = Id
    FROM Stores
    WHERE Name = 'Burger Street';

    IF NOT EXISTS (SELECT 1 FROM Products WHERE Name = 'Smash Burger' AND StoreId = @StoreId)
    BEGIN
        INSERT INTO Products (Name, Price, StoreId)
        VALUES ('Smash Burger', 13.90, @StoreId);
    END;

    IF NOT EXISTS (SELECT 1 FROM Products WHERE Name = 'Crispy Fries' AND StoreId = @StoreId)
    BEGIN
        INSERT INTO Products (Name, Price, StoreId)
        VALUES ('Crispy Fries', 4.90, @StoreId);
    END;

    IF NOT EXISTS (SELECT 1 FROM Products WHERE Name = 'Iced Tea 33cl' AND StoreId = @StoreId)
    BEGIN
        INSERT INTO Products (Name, Price, StoreId)
        VALUES ('Iced Tea 33cl', 3.20, @StoreId);
    END;

    IF NOT EXISTS (SELECT 1 FROM Couriers WHERE PhoneNumber = '+33620000000')
    BEGIN
        INSERT INTO Couriers (FullName, PhoneNumber, Vehicle, IsAvailable)
        VALUES ('Courier Demo', '+33620000000', 1, 1);
    END;

    SELECT @CourierId = Id
    FROM Couriers
    WHERE PhoneNumber = '+33620000000';

    SELECT
        @CustomerId AS CustomerId,
        @CustomerAddressId AS CustomerAddressId,
        @StoreId AS StoreId,
        @CourierId AS CourierId;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;
