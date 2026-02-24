BEGIN TRANSACTION;

BEGIN TRY
    DECLARE @CustomerId INT;
    DECLARE @StoreId INT;
    DECLARE @AddressId INT;
    DECLARE @CourierId INT;
    DECLARE @OrderId INT;

    SELECT TOP 1 @CustomerId = Id FROM Customers WHERE Email = 'customer.demo@deliveryapp.com' ORDER BY Id DESC;
    SELECT TOP 1 @StoreId = Id FROM Stores WHERE Name = 'Burger Street' ORDER BY Id DESC;
    SELECT TOP 1 @AddressId = Id FROM Addresses WHERE Street = '25 Rue de Rivoli' AND City = 'Paris' ORDER BY Id DESC;
    SELECT TOP 1 @CourierId = Id FROM Couriers WHERE PhoneNumber = '+33620000000' ORDER BY Id DESC;

    IF @CustomerId IS NULL OR @StoreId IS NULL OR @AddressId IS NULL OR @CourierId IS NULL
    BEGIN
        THROW 50000, 'Missing reference data. Run 01_seed_reference_data.sql first.', 1;
    END;

    INSERT INTO Orders (OrderDate, TotalPrice, Status, CustomerId, StoreId, DeliveryAddressId, CourierId)
    VALUES (GETUTCDATE(), 0, 0, @CustomerId, @StoreId, @AddressId, @CourierId);

    SET @OrderId = SCOPE_IDENTITY();

    INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
    SELECT @OrderId, p.Id, 2, p.Price
    FROM Products p
    WHERE p.StoreId = @StoreId AND p.Name = 'Smash Burger';

    INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
    SELECT @OrderId, p.Id, 1, p.Price
    FROM Products p
    WHERE p.StoreId = @StoreId AND p.Name = 'Crispy Fries';

    UPDATE o
    SET o.TotalPrice = x.TotalPrice
    FROM Orders o
    CROSS APPLY (
        SELECT SUM(oi.Quantity * oi.UnitPrice) AS TotalPrice
        FROM OrderItems oi
        WHERE oi.OrderId = o.Id
    ) x
    WHERE o.Id = @OrderId;

    UPDATE Orders SET Status = 1 WHERE Id = @OrderId; -- Accepted
    UPDATE Orders SET Status = 2 WHERE Id = @OrderId; -- Preparing
    UPDATE Orders SET Status = 3 WHERE Id = @OrderId; -- ReadyForPickup
    UPDATE Orders SET Status = 4 WHERE Id = @OrderId; -- PickedUp
    UPDATE Orders SET Status = 5 WHERE Id = @OrderId; -- OutForDelivery
    UPDATE Orders SET Status = 6 WHERE Id = @OrderId; -- Delivered

    INSERT INTO Payments (OrderId, Amount, PaymentDate, Method, Status)
    SELECT o.Id, o.TotalPrice, GETUTCDATE(), 0, 2
    FROM Orders o
    WHERE o.Id = @OrderId;

    INSERT INTO Reviews (OrderId, Rating, Comment, CreatedAt)
    VALUES (@OrderId, 5, 'Avis client: livraison rapide et commande chaude.', GETUTCDATE());

    SELECT o.Id, o.Status, o.TotalPrice, o.CustomerId, o.StoreId, o.CourierId
    FROM Orders o
    WHERE o.Id = @OrderId;

    SELECT * FROM OrderItems WHERE OrderId = @OrderId;
    SELECT * FROM Payments WHERE OrderId = @OrderId;
    SELECT * FROM Reviews WHERE OrderId = @OrderId;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;
