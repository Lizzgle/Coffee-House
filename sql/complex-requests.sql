SELECT *
FROM Orders
WHERE UserId = 1
AND Date > '2023-01-01';

SELECT Users.Name AS UserName, COUNT(Orders.Id) AS OrderCount
FROM Users
LEFT JOIN Orders ON Users.Id = Orders.UserId
GROUP BY Users.Name
ORDER BY OrderCount DESC;

SELECT Users.Name AS UserName, Orders.Date AS OrderDate, Drinks.Name AS DrinkName, Desserts.Name AS DessertName
FROM Users
JOIN Orders ON Users.Id = Orders.UserId
LEFT JOIN OrdersDrinks ON Orders.Id = OrdersDrinks.OrderId
LEFT JOIN Drinks ON OrdersDrinks.DrinkId = Drinks.Id
LEFT JOIN OrdersDesserts ON Orders.Id = OrdersDesserts.OrderId
LEFT JOIN Desserts ON OrdersDesserts.DessertId = Desserts.Id
WHERE Users.Id = 1;

SELECT Users.Name, Orders.Id AS OrderId
FROM Users
LEFT JOIN Orders ON Users.Id = Orders.UserId;

SELECT Categories.Name, COUNT(Drinks.Id) AS TotalDrinks
FROM Categories
LEFT JOIN Drinks ON Categories.Id = Drinks.CategoryId
GROUP BY Categories.Id;

SELECT Categories.Name, AVG(Drinks.Price) AS AvgPrice
FROM Categories
LEFT JOIN Drinks ON Categories.Id = Drinks.CategoryId
GROUP BY Categories.Id;

SELECT Orders.Id, Drinks.Name AS DrinkName, Drinks.Size
FROM Orders
INNER JOIN OrdersDrinks ON Orders.Id = OrdersDrinks.OrderId
INNER JOIN Drinks ON OrdersDrinks.DrinkId = Drinks.Id
WHERE Drinks.Size = 'M';

SELECT Users.Name, Drinks.Name
FROM Users
CROSS JOIN Drinks;

SELECT CategoryId, COUNT(*) AS DrinkCount
FROM Drinks
GROUP BY CategoryId;

SELECT Name, 'Drink' AS Type
FROM Drinks
UNION
SELECT Name, 'Dessert' AS Type
FROM Desserts;

SELECT Name, CategoryId, Price,
       RANK() OVER(PARTITION BY CategoryId ORDER BY Price) AS CategoryRank
FROM Drinks;

SELECT CategoryId, AVG(Price) AS AvgPrice
FROM Drinks
GROUP BY CategoryId
HAVING AVG(Price) > 3.0;

SELECT CategoryId, COUNT(*) AS DrinkCount
FROM Drinks
GROUP BY CategoryId;

SELECT Name, 'Drink' AS Type
FROM Drinks
UNION
SELECT Name, 'Dessert' AS Type
FROM Desserts;

SELECT Name, CategoryId, Price,
       RANK() OVER(PARTITION BY CategoryId ORDER BY Price) AS CategoryRank
FROM Drinks;

SELECT CategoryId, AVG(Price) AS AvgPrice
FROM Drinks
GROUP BY CategoryId
HAVING AVG(Price) > 3.0;

SELECT Name,
    CASE 
        WHEN Price > 3 THEN 'Expensive'
        ELSE 'Affordable'
    END AS PriceCategory
FROM Drinks;

SELECT Name
FROM Desserts d
WHERE EXISTS (
    SELECT 1
    FROM OrdersDesserts od
    WHERE od.DessertId = d.Id
);


INSERT INTO Feedbacks (Date, Rating, UserId)
SELECT CURRENT_DATE, 5, Id
FROM Users
WHERE Name = 'John';

EXPLAIN SELECT *
FROM Users u
JOIN Orders o ON u.Id = o.UserId;

