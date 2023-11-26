CREATE TABLE Users (
    Id             SERIAL          PRIMARY KEY,
    Login          TEXT            NOT NULL UNIQUE,
    Password       TEXT            NOT NULL,
    Name           TEXT            NOT NULL UNIQUE,
    DateOfBirth    DATE            NOT NULL
);

CREATE TABLE Coupons (
    Id             SERIAL          PRIMARY KEY,
    Discount       INTEGER         NOT NULL,
    DateOfEnd      DATE            NOT NULL
);

CREATE TABLE Feedbacks (
    Id             SERIAL          PRIMARY KEY,
    Date           DATE            NOT NULL DEFAULT CURRENT_DATE,
    Rating         INTEGER         NOT NULL,
    Description    TEXT,
    UserId         INTEGER         REFERENCES Users (Id) ON DELETE SET NULL
);

CREATE TABLE Orders (
    Id             SERIAL         PRIMARY KEY,
    Date           DATE           NOT NULL DEFAULT CURRENT_DATE,
    UserId         INTEGER        REFERENCES Users(Id) ON DELETE SET NULL
);

CREATE TABLE Categories (
    Id             SERIAL         PRIMARY KEY,
    Name           TEXT           NOT NULL UNIQUE
);

CREATE TABLE Recipes (
    Id             SERIAL          PRIMARY KEY,
    Description    TEXT            NOT NULL
);

CREATE TABLE Drinks (
    Id             SERIAL          PRIMARY KEY,
    Name           TEXT            NOT NULL,
    Size           TEXT            NOT NULL CHECK (Size IN ('S', 'M', 'L')),
    Price          REAL            NOT NULL CHECK(Price > 0),
    RecipeId       INTEGER         REFERENCES Recipes(Id) ON DELETE SET NULL,
    CategoryId     INTEGER         REFERENCES Categories(Id) ON DELETE SET NULL
);

CREATE TABLE Ingredients (
    Id             SERIAL          PRIMARY KEY,
    Name           TEXT            NOT NULL UNIQUE
);

CREATE TABLE Desserts (
    Id             SERIAL          PRIMARY KEY,
    Name           TEXT            NOT NULL UNIQUE,
    Calories       INTEGER         NOT NULL CHECK(Calories > 0) 
);

CREATE TABLE Carts (
    Id             SERIAL          PRIMARY KEY,
    UserId         INTEGER         NOT NULL UNIQUE REFERENCES Users (Id) ON DELETE CASCADE
);

CREATE TABLE UsersCoupons (
    CouponId       INTEGER         REFERENCES Coupons(Id) ON DELETE CASCADE,
    UserId         INTEGER         REFERENCES Users(Id) ON DELETE CASCADE,
    
    UNIQUE (CouponId, UserId)
);

CREATE TABLE OrdersDrinks (
    OrderId        INTEGER         REFERENCES Orders(Id) ON DELETE CASCADE,
    DrinkId        INTEGER         REFERENCES Drinks(Id) ON DELETE CASCADE,
    
    UNIQUE (OrderId, DrinkId)
);

CREATE TABLE OrdersDesserts (
    OrderId        INTEGER         REFERENCES Orders(Id) ON DELETE CASCADE,
    DessertId      INTEGER         REFERENCES Desserts(Id) ON DELETE CASCADE,
    
    UNIQUE (OrderId, DessertId)
);

CREATE TABLE CartsDrinks (
    CartId         INTEGER         REFERENCES Carts(Id) ON DELETE CASCADE,
    DrinkId        INTEGER         REFERENCES Drinks(Id) ON DELETE CASCADE,
    
    UNIQUE (CartId, DrinkId)
);

CREATE TABLE CartsDesserts (
    CartId         INTEGER         REFERENCES Carts(Id) ON DELETE CASCADE,
    DessertId      INTEGER         REFERENCES Desserts(Id) ON DELETE CASCADE,
    
    UNIQUE (CartId, DessertId)
);

CREATE TABLE RecipesIngredients (
    RecipeId       INTEGER         REFERENCES Recipes(Id) ON DELETE CASCADE,
    IngredientId   INTEGER         REFERENCES Ingredients(Id) ON DELETE CASCADE,
    
    UNIQUE (RecipeId, IngredientId)
);
