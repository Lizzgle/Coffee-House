-- Создание функции для создания корзины
CREATE OR REPLACE PROCEDURE create_cart_for_new_user(userId INTEGER)
AS $$
BEGIN
    INSERT INTO Carts (userid) 
	VALUES (userId);
END;
$$ LANGUAGE plpgsql;

-- -- Создание триггера, который будет вызывать функцию при вставке новой записи в таблицу пользователей (замените "users" на ваше имя таблицы)
-- CREATE TRIGGER create_cart_trigger
-- AFTER INSERT ON users
-- FOR EACH ROW
-- EXECUTE FUNCTION create_cart_for_new_user();

---Проверяет наличие пользователя---
CREATE OR REPLACE FUNCTION IsExistUser(
    loginm TEXT,
    passwordm TEXT
) RETURNS BOOLEAN AS $$
DECLARE isExists BOOLEAN;
BEGIN
    SELECT EXISTS(
        SELECT 1 FROM Users AS u 
        WHERE u.Login = loginm AND u.Password = passwordm)
    INTO isExists;

    return isExists;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE CreateFeedback(idUser INTEGER, discriptionUser TEXT,
                                        ratingUser INTEGER)
AS $$
BEGIN
	IF LENGTH(discriptionUser) = 0 THEN
            RAISE EXCEPTION 'Имя не может быть пустым';
    END IF;
    IF ratingUser <= 0 THEN
            RAISE EXCEPTION 'Стоимость должна быть больше 0';
    END IF;

    INSERT INTO Feedbacks (UserId, Rating, Description) 
    VALUES (idUser, ratingUser, discriptionUser);
END;
$$ LANGUAGE plpgsql;

---CRUD для Drinks(Size, Price)---
CREATE OR REPLACE PROCEDURE CreateDrink(newsize TEXT, newname TEXT,
                                        newprice REAL)
AS $$
BEGIN
	IF LENGTH(newname) = 0 THEN
            RAISE EXCEPTION 'Имя не может быть пустым';
    END IF;
    IF newsize !~ '^(S|M|L)$' THEN
            RAISE EXCEPTION 'Размер должен быть S/M/L';
    END IF;
    IF newprice <= 0 THEN
            RAISE EXCEPTION 'Стоимость должна быть больше 0';
    END IF;

    INSERT INTO Drinks (size, Name, Price) 
    VALUES (newsize, newname, newprice);
END;
$$ LANGUAGE plpgsql;

-- CALL CreateHall(200, 260);

CREATE OR REPLACE PROCEDURE UpdateDrink(drinkId BIGINT,
                                        newsize TEXT,
										newname TEXT,
                                        newPrice REAL)
AS $$
BEGIN
    IF LENGTH(newname) = 0 THEN
            RAISE EXCEPTION 'Имя не может быть пустым';
    END IF;
    IF newsize !~ '^(S|M|L)$' THEN
            RAISE EXCEPTION 'Размер должен быть S/M/L';
    END IF;
    IF newprice <= 0 THEN
            RAISE EXCEPTION 'Стоимость должна быть больше 0';
    END IF;

    UPDATE Drinks
    SET 
        Price = CASE WHEN Price <> newPrice THEN newPrice ELSE Price END,
        Size = CASE WHEN Size <> newsize THEN newsize ELSE Size END,
        Name = CASE WHEN Name <> newname THEN newname ELSE Name END
    WHERE Id = drinkId;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE PROCEDURE DeleteDrinkById(drinkId BIGINT)
AS $$
BEGIN
    DELETE FROM Drinks 
    WHERE Id = drinkId; 
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION PartialSearchDrink(partialName TEXT)
RETURNS TABLE (Id BIGINT, Name TEXT, Size TEXT, Price REAL)
AS $$
BEGIN
    RETURN QUERY
    SELECT d.Id, d.Name, d.Size, d.Price
    FROM Drinks AS d
    WHERE d.Name ILIKE '%' || partialName || '%';
END;
$$ LANGUAGE plpgsql;

---CRUD для Dessert(Size, Price)---
CREATE OR REPLACE PROCEDURE CreateDessert(newCalories INTEGER, newname TEXT,
                                        newprice REAL)
AS $$
BEGIN
	IF LENGTH(newname) = 0 THEN
            RAISE EXCEPTION 'Имя не может быть пустым';
    END IF;
    IF newsize !~ '^(S|M|L)$' THEN
            RAISE EXCEPTION 'Размер должен быть S/M/L';
    END IF;
    IF newCalories <= 0 THEN
            RAISE EXCEPTION 'Стоимость должна быть больше 0';
    END IF;

    INSERT INTO Desserts (Calories, Name, Price) 
    VALUES (newCalories, newname, newprice);
END;
$$ LANGUAGE plpgsql;

-- CALL CreateHall(200, 260);

CREATE OR REPLACE PROCEDURE UpdateDessert(dessertId BIGINT,
                                        newCalories INTEGER,
										newname TEXT,
                                        newPrice REAL)
AS $$
BEGIN
    IF LENGTH(newname) = 0 THEN
            RAISE EXCEPTION 'Имя не может быть пустым';
    END IF;
    IF newCalories <= 0 THEN
            RAISE EXCEPTION 'Разме';
    END IF;
    IF newprice <= 0 THEN
            RAISE EXCEPTION 'Стоимость должна быть больше 0';
    END IF;

    UPDATE Desserts
    SET 
        Price = CASE WHEN Price <> newPrice THEN newPrice ELSE Price END,
        Calories = CASE WHEN Calories <> newCalories THEN newCalories ELSE Calories END,
        Name = CASE WHEN Name <> newname THEN newname ELSE Name END
    WHERE Id = dessertId;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE PROCEDURE DeleteDrinkById(drinkId BIGINT)
AS $$
BEGIN
    DELETE FROM Drinks 
    WHERE Id = drinkId; 
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION PartialSearchDrink(partialName TEXT)
RETURNS TABLE (Id BIGINT, Name TEXT, Size TEXT, Price REAL)
AS $$
BEGIN
    RETURN QUERY
    SELECT d.Id, d.Name, d.Size, d.Price
    FROM Drinks AS d
    WHERE d.Name ILIKE '%' || partialName || '%';
END;
$$ LANGUAGE plpgsql;

-- CREATE OR REPLACE FUNCTION GetOrders(userId INTEGER)
-- RETURNS TABLE (
--     OrderDate DATE,
    
-- ) AS $$
-- BEGIN
--     RETURN QUERY 
--     SELECT O.Date AS OrderDate, D.Name AS DrinkName, D.Size AS DrinkSize, DE.Name AS DessertName
--     FROM Orders O
--     LEFT JOIN OrdersDrinks OD ON O.Id = OD.OrderId
--     LEFT JOIN OrdersDesserts ODess ON O.Id = ODess.OrderId
--     LEFT JOIN Drinks D ON OD.DrinkId = D.Id
--     LEFT JOIN Desserts DE ON ODess.DessertId = DE.Id
--     WHERE O.UserId = userId;
-- END;
-- $$ LANGUAGE plpgsql;

-- Создание пользователя
CREATE OR REPLACE PROCEDURE CreateUser(
    UserLogin TEXT,
    UserName TEXT,
    UserDateOfBirth DATE,
    roleCode TEXT,
	Userpassword TEXT
)
AS $$
BEGIN
    IF LENGTH(UserLogin) = 0 THEN
        RAISE EXCEPTION 'Логин не может быть пустой';
    END IF;
    IF LENGTH(UserName) = 0 THEN
        RAISE EXCEPTION 'Имя пользователя не может быть пустым';
    END IF;
    IF UserDateOfBirth IS NULL THEN
        RAISE EXCEPTION 'Дата рождения не может быть пустой';
    END IF;
    IF roleCode !~ '^(Admin|User|)$' THEN
	IF roleCode !~ '^(Admin)$'  THEN
        RAISE EXCEPTION 'Роль с указанным кодом не существует';
		END IF;
    END IF;
	    IF LENGTH(Userpassword) = 0 THEN
        RAISE EXCEPTION 'Пароль не может быть пустой';
    END IF;

    INSERT INTO Users (Login, Name, DateOfBirth, role, Password) 
    VALUES (UserLogin, UserName, UserDateOfBirth, roleCode, Userpassword);
END;
$$ LANGUAGE plpgsql;

-- Обновление пользователя
CREATE OR REPLACE PROCEDURE UpdateUser(
    userId BIGINT,
    newUserLogin TEXT,
    newUserName TEXT,
    newDateOfBirth TEXT,
    newPassword TEXT
)
AS $$
BEGIN
    IF LENGTH(newUserLogin) = 0 THEN
        RAISE EXCEPTION 'Логин не может быть пустой';
    END IF;
    IF LENGTH(newUserName) = 0 THEN
        RAISE EXCEPTION 'Имя пользователя не может быть пустым';
    END IF;
    IF LENGTH(newDateOfBirth) = 0 THEN
        RAISE EXCEPTION 'Дата рождения не может быть пустой';
    END IF;
    IF LENGTH(newPassword) = 0 THEN
        RAISE EXCEPTION 'Пароль не может быть пустой';
    END IF;

    UPDATE Users
    SET 
		Login = newUserLogin,
        Name = newUserName,
        DateOfBirds = newDateOfBirth,
		Password = newPassword
    WHERE Id = userId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE UpdateUserRole(
    userId BIGINT,
    newRoleCode TEXT
)
AS $$
BEGIN
    IF newRoleCode !~ '^(Admin|User)$' THEN
        RAISE EXCEPTION 'Роль с указанным кодом не существует';
    END IF;

    UPDATE Users
    SET 
        Role = newRoleCode
    WHERE Id = userId;
END;
$$ LANGUAGE plpgsql;
