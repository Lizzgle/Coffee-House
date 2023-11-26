CREATE OR REPLACE PROCEDURE AddNewUser(
    IN login_text TEXT,
    IN password_text TEXT,
    IN name_text TEXT,
    IN dob DATE
)
AS $$
BEGIN
    INSERT INTO Users (Login, Password, Name, DateOfBirth)
    VALUES (login_text, password_text, name_text, dob);
END;
$$
LANGUAGE plpgsql;


CREATE OR REPLACE PROCEDURE DeleteOrderAndRelatedItems(
    IN order_id INTEGER
)
AS $$
BEGIN
    DELETE FROM Orders WHERE Id = order_id;
    DELETE FROM OrdersDrinks WHERE OrderId = order_id;
    DELETE FROM OrdersDesserts WHERE OrderId = order_id;
END;
$$
LANGUAGE plpgsql;


CREATE OR REPLACE PROCEDURE AddItemToCart(
    IN cart_id INTEGER,
    IN drink_id INTEGER,
    IN dessert_id INTEGER
)
AS $$
BEGIN
    IF drink_id IS NOT NULL THEN
        INSERT INTO CartsDrinks (CartId, DrinkId)
        VALUES (cart_id, drink_id);
    END IF;

    IF dessert_id IS NOT NULL THEN
        INSERT INTO CartsDesserts (CartId, DessertId)
        VALUES (cart_id, dessert_id);
    END IF;
END;
$$
LANGUAGE plpgsql;