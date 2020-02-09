CREATE FUNCTION GetRate (@date VARCHAR(50), @currency VARCHAR(50))
    RETURNS MONEY
    BEGIN
        DECLARE @rate MONEY
        SELECT @rate = Rates.Rate FROM Rates
	WHERE Rates.Currency = @currency AND Rates.Date = @date
        RETURN @rate
    END;
