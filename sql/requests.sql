SELECT * FROM Users;

SELECT * FROM Orders 
WHERE UserId = 2;

SELECT Name, Size 
FROM Drinks;

SELECT AVG(Rating) 
FROM Feedbacks;

SELECT DISTINCT Name 
FROM Users 
ORDER BY Name;

INSERT INTO Users (Login, Password, Name, DateOfBirth) 
VALUES ('newuser', 'password123', 'New User', '1995-08-30');

INSERT INTO Coupons (Discount, DateOfEnd) 
VALUES (15, '2024-01-31');

INSERT INTO Orders (UserId) 
VALUES (3);

INSERT INTO Feedbacks (Rating, Description, UserId) 
VALUES (1, '', 3);


UPDATE Users 
SET Password = 'newpassword' 
WHERE Name = 'New User';

UPDATE Coupons 
SET Discount = 25 
WHERE Id = 1;


DELETE FROM Users 
WHERE Id = 3;

DELETE FROM Feedbacks 
WHERE Rating < 3;

SELECT DISTINCT Size 
FROM Drinks;

SELECT * FROM Drinks
WHERE Price BETWEEN 1 AND 3
ORDER BY CategoryId;

SELECT * FROM Drinks
WHERE Size LIKE 'M'
ORDER BY CategoryId;


SELECT * FROM Users
WHERE Id IN (1);