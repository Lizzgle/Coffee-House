INSERT INTO Users (Login, Password, Name, DateOfBirth) VALUES
    ('user1', 'password1', 'John', '1990-05-15'),
    ('user2', 'password2', 'Svan', '2001-12-20');


INSERT INTO Coupons (Discount, DateOfEnd) VALUES
    (10, '2023-12-31'),
    (20, '2023-10-31');


INSERT INTO Feedbacks (Date, Rating, Description, UserId) VALUES
    (CURRENT_DATE, 5, 'Great service!', 1),
    (CURRENT_DATE, 4, 'Good food.', 2);


INSERT INTO Orders (Date, UserId) VALUES
    (CURRENT_DATE, 1),
    (CURRENT_DATE, 2);
    
INSERT INTO Categories (Name) VALUES
    ('Hot'),
    ('Cold');
    
INSERT INTO Recipes (Description) VALUES
    ('Brew coffee.'),
    ('Steep tea bag in hot water, add ice.');

INSERT INTO Drinks (Name, Size, Price, RecipeId, CategoryId) VALUES
    ('Coffee', 'M', 3.99, 1, 1),
    ('Ice-Tea', 'S', 2.49, 2, 2);

INSERT INTO Ingredients (Name) VALUES
    ('Coffee beans'),
    ('Tea bag');

INSERT INTO Desserts (Name, Calories) VALUES
    ('Chocolate Cake', 300),
    ('Ice Cream', 150);

INSERT INTO Carts (UserId) VALUES
    (1),
    (2);

INSERT INTO UsersCoupons (CouponId, UserId) VALUES
    (1, 1),
    (2, 2);

INSERT INTO OrdersDrinks (OrderId, DrinkId) VALUES
    (1, 1),
    (2, 2);

INSERT INTO OrdersDesserts (OrderId, DessertId) VALUES
    (1, 2),
    (2, 1);

INSERT INTO CartsDrinks (CartId, DrinkId) VALUES
    (1, 1),
    (2, 2);


INSERT INTO CartsDesserts (CartId, DessertId) VALUES
    (1, 1),
    (2, 2);

INSERT INTO RecipesIngredients (RecipeId, IngredientId) VALUES
    (1, 1),
    (2, 2);
