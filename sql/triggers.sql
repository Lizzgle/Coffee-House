---дата для оформления заказа не больше ли сегодняшней---
CREATE OR REPLACE FUNCTION CheckDateOrderThanToday()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.Date <= CURRENT_DATE THEN
        RAISE EXCEPTION 'Дата должна быть больше текущей';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER InsertDateOrderThanTodayTrigger
BEFORE INSERT ON Orders
FOR EACH ROW EXECUTE FUNCTION CheckDateOrderThanToday();

INSERT INTO Orders (id, Date) VALUES
(5, '2023-11-29');



-- удаляет запись о десерте из заказа, если этот десерт был удален из таблицы 
CREATE OR REPLACE FUNCTION remove_deleted_dessert()
RETURNS TRIGGER AS $$
BEGIN
    DELETE FROM OrdersDesserts WHERE DessertId = OLD.Id;
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER delete_order_dessert
AFTER DELETE ON Desserts
FOR EACH ROW
EXECUTE FUNCTION remove_deleted_dessert();


-- проверка ограничения на количество элементов в корзине:
CREATE OR REPLACE FUNCTION checkCartLimitTrigger()
RETURNS TRIGGER AS
$$
BEGIN
    IF TG_OP = 'INSERT' THEN
        IF (SELECT COUNT(*) FROM Carts WHERE UserId = NEW.UserId) >= 2 THEN
            RAISE EXCEPTION 'Cart limit exceeded (max 2 items per user)';
        END IF;
    END IF;
    RETURN NEW;
END;
$$
LANGUAGE plpgsql;

CREATE TRIGGER checkCartLimit
    BEFORE INSERT ON Carts
    FOR EACH ROW
    EXECUTE FUNCTION checkCartLimitTrigger();

-- запрещает добавление ингредиента к рецепту, 
-- если этот ингредиент уже использован в этом рецепте
CREATE OR REPLACE FUNCTION check_recipe_ingredient()
RETURNS TRIGGER AS $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM RecipesIngredients
        WHERE RecipeId = NEW.RecipeId AND IngredientId = NEW.IngredientId
    ) THEN
        RAISE EXCEPTION 'Ingredient already exists in recipe';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER restrict_recipe_ingredient
BEFORE INSERT ON RecipesIngredients
FOR EACH ROW
EXECUTE FUNCTION check_recipe_ingredient();