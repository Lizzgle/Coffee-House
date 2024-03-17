using Coffee.Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Coffee
{
    internal class Requests
    {
        private readonly NpgsqlDataSource _source;
        public Requests()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=3851;Database=Coffee-house";
            try
            {
                _source = NpgsqlDataSource.Create(connectionString);
                _source.OpenConnection();
                Console.WriteLine("Connection to the database established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Дополнительная обработка ошибок или логирование здесь
            }
        }

        #region User CRUD

        public async Task<List<User>> GetAllUsers(string entityType, string actionType)
        {
            List<User> result = new List<User>();
            try
            {
                _source.OpenConnection();
                await using var command = _source.CreateCommand("SELECT * FROM Users;");
                //await LogEntityActionToDatabase(entityType, actionType);

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var id = (long)reader["Id"];
                    var loginN = reader["Login"].ToString();
                    var name = reader["Name"].ToString();
                    var role = reader["Role"].ToString();
                    Role roleEnum = (Role)Enum.Parse(typeof(Role), role);
                    var date = DateTime.Parse(reader["DateOfBirth"].ToString()!);

                    result.Add(new User() { Id = id, Name = name, Login = loginN, Role = roleEnum, DateOfBirth = date });
                }

                return result;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
            }
            return result;

        }

        public async Task<bool> CreateUser(string userLogin, string userName, string roleCode, DateTime dateofbirth, string Password)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL CreateUser('{userLogin}', '{userName}', '{dateofbirth}', '{roleCode}', '{Password}');");

            try
            {
                await command.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateUser(string entityType, string actionType, long userId, string userName, DateTime dateofbirth)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL UpdateUser({userId}, '{userName}', '{dateofbirth}');");
            //await LogEntityActionToDatabaseWithId(entityType, (int)userId, actionType);
            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }
        // Обновление роли пользователя
        public async Task<bool> UpdateUserRole(string entityType, string actionType, int userId, string newRole)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL UpdateUserRole({userId}, '{newRole}');");
            //await LogEntityActionToDatabaseWithId(entityType, (int)userId, actionType);

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }

        }

        public async Task<User> GetUserByLoginAndPassword(string login, string password)
        {
            _source.OpenConnection();

            try
            {
                await using var command = _source.CreateCommand($"SELECT * FROM Users WHERE Login = '{login}' AND Password = '{password}'");
                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var id = (long)reader["Id"];
                    var loginN = reader["Login"].ToString();
                    var name = reader["Name"].ToString();
                    var role = reader["Role"].ToString();
                    Role roleEnum = (Role)Enum.Parse(typeof(Role), role);
                    var date = DateTime.Parse(reader["DateOfBirth"].ToString()!);
                    var passwordN = reader["Password"].ToString();
                    //var cartid = (int)reader["CartId"];

                    return new User
                    {
                        Id = id,
                        Login = loginN,
                        Name = name,
                        Role = roleEnum,
                        DateOfBirth = date,
                        Password = passwordN,
                        //CartId = cartid
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return new User();
        }

        // Удаление пользователя по ID
        public async Task<bool> DeleteUserById(string entityType, string actionType, int userId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL DeleteUserById({userId});");
            //await LogEntityActionToDatabaseWithId(entityType, userId, actionType);

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        #endregion

        #region Order CR
        public async Task<bool> CreateOrder(int userId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL CreateOrderFromCart('{userId}', '{userId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                Console.WriteLine("Заказ оформлен");
                return true;
            }
            catch (Exception ex)
            {
                //await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Order>> GetOrders(long userId)
        {
            List<Order> orders = new();
            try
            {
                _source.OpenConnection();
                await using var command = _source.CreateCommand($"SELECT * FROM Orders WHERE Userid = '{userId}'");

                var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    var id = (long)reader["Id"];
                    var date = DateTime.Parse(reader["Date"].ToString());

                    orders.Add(new Order() { Id = id, Date = date });

                }

                return orders;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return orders;
            }
           
        }

        #endregion

        #region Feedback CR

        public async Task<bool> CreateFeedback(int rating, string discription, int Userid)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL CreateFeedback('{Userid}'," + $"'{discription}', " + $"{rating});");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Feedback>> GetAllFeedbacks(string v1, string v2)
        {
            List<Feedback> res = new();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM Feedbacks");
            //await LogEntityActionToDatabase(entityType, actionType);

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var id = (int)reader["Id"];
                var description = reader["Description"].ToString();
                var date = DateTime.Parse(reader["Date"].ToString()!);
                // var userId = (int)reader["UserId"];
                var rating = (int)reader["Rating"];


                res.Add(new Feedback()
                {
                    Id = id,
                    Rating = rating,
                    Description = description,
                    Date = date
                });
            }

            return res;
        }
        #endregion

        #region Drink CRUD

        public async Task<List<Drink>> GetAllDrink(string entityType, string actionType)
        {
            List<Drink> res = new();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM Drinks;");
            //await LogEntityActionToDatabase(entityType, actionType);

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var id = (long)reader["Id"];
                var name = reader["Name"].ToString();
                var price = (float)reader["Price"];
                var size = reader["Size"].ToString();
                Size sizeEnum = (Size)Enum.Parse(typeof(Size), size);

                res.Add(new Drink() { Id = id, Name = name, Size = sizeEnum, Price = price });
            }

            return res;
        }

        public async Task<List<Drink>> GetAllDrinkInCart(string entityType, string actionType, int cartId)
        {
            List<Drink> res = new();
            try
            {
                _source.OpenConnection();
                await using var command = _source.CreateCommand($"SELECT * FROM Drinks WHERE Id IN (SELECT DrinkId FROM CartsDrinks WHERE CartId = {cartId})");
                //await LogEntityActionToDatabase(entityType, actionType);

                var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    var id = (long)reader["Id"];
                    var name = reader["Name"].ToString();
                    var price = (float)reader["Price"];
                    var size = reader["Size"].ToString();
                    Size sizeEnum = (Size)Enum.Parse(typeof(Size), size);

                    res.Add(new Drink() { Id = id, Name = name, Size = sizeEnum, Price = price });
                }

                return res;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return res;
            }

            
        }

        public async Task<List<Drink>> PartialSearchDrink(string entityType, string actionType, string partialName)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM PartialSearchDrink('{partialName}')");
            // await LogEntityActionToDatabase(entityType.ToString(), id, actionType, actionDetails);
            //await LogEntityActionToDatabase(entityType, actionType);

            var result = new List<Drink>();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = (long)reader["Id"];
                var name = reader["Name"].ToString();
                var size = reader["Size"].ToString();
                Size sizeEnum = (Size)Enum.Parse(typeof(Size), size);
                var price = (float)reader["Price"];

                result.Add(new Drink() { Id = id, Name = name, Size = sizeEnum, Price = price });
            }

            return result;
        }

        public async Task<bool> CreateDrink(string entityType, string actionType,  Size size, float price, string name)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL CreateDrink('{size}', " + $"'{name}', " +
                                                $"'{price.ToString(CultureInfo.GetCultureInfo("en-US"))}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                //await LogEntityActionToDatabase(entityType, actionType);
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateDrink(string entityType, string actionType, int id, Size size, float price, string name)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL UpdateDrink('{id}', '{size}', " + $"'{name}', " +
                                                $"'{price.ToString(CultureInfo.GetCultureInfo("en-US"))}');");
            //await LogEntityActionToDatabaseWithId(entityType, id, actionType);

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteDrinkById(string entityType, string actionType, long id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL DeleteDrinkById('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                //await LogEntityActionToDatabaseWithId(entityType, (int)id, actionType);
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task AddDrinksToCart(long cartId, long[] drinkIds)
        {
            _source.OpenConnection();
            try
            {
                foreach (var drinkId in drinkIds)
                {
                    await using var insertCommand = _source
                        .CreateCommand($"INSERT INTO CartsDrinks (CartId, DrinkId) VALUES ({cartId}, {drinkId})");

                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        #endregion

        #region Dessert CRUD

        public async Task<List<Dessert>> GetAllDesserts(string entityType, string actionType)
        {
            List<Dessert> res = new();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM Desserts;");
            //await LogEntityActionToDatabase(entityType, actionType);

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var id = (long)reader["Id"];
                var name = reader["Name"].ToString();
                var price = (float)reader["Price"];
                var calories = (int)reader["Calories"];

                res.Add(new Dessert() { Id = id, Name = name, Calories = calories, Price = price });
            }

            return res;
        }

        public async Task<List<Dessert>> GetAllDessertInCart(string entityType, string actionType, int cartId)
        {
            List<Dessert> res = new();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM Desserts WHERE Id IN (SELECT DessertId FROM CartsDesserts WHERE CartId = {cartId})");
            //await LogEntityActionToDatabase(entityType, actionType);

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var id = (long)reader["Id"];
                var name = reader["Name"].ToString();
                var price = (float)reader["Price"];
                var calories = (int)reader["Calories"];

                res.Add(new Dessert() { Id = id, Name = name, Calories = calories, Price = price });
            }

            return res;
        }

        public async Task<List<Dessert>> PartialSearchDessert(string entityType, string actionType, string partialName)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM PartialSearchDessert('{partialName}')");
            //await LogEntityActionToDatabase(entityType, actionType);

            var result = new List<Dessert>();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = (long)reader["Id"];
                var name = reader["Name"].ToString();
                var price = (float)reader["Price"];
                var calories = (int)reader["Calories"];

                result.Add(new Dessert() { Id = id, Name = name, Calories = calories, Price = price });
            }

            return result;
        }

        public async Task<bool> CreateDessert(string entityType, string actionType, int calories, float price, string name)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL CreateDessert('{calories}', " + $"'{name}', " +
                                                $"'{price.ToString(CultureInfo.GetCultureInfo("en-US"))}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                //await LogEntityActionToDatabase(entityType, actionType);
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateDessert(string entityType, string actionType, int id, int calories, float price, string name)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL UpdateDessert('{id}', '{calories}', " + $"'{name}', " +
                                                $"'{price.ToString(CultureInfo.GetCultureInfo("en-US"))}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                //await LogEntityActionToDatabaseWithId(entityType, id, actionType);
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteDessertById(string entityType, string actionType, long id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL DeleteDessertById('{id}');");
            //await LogEntityActionToDatabaseWithId(entityType, (int)id, actionType);

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task AddDessertsToCart(long cartId, long[] dessertIds)
        {
            _source.OpenConnection();
            try
            {
                foreach (var dessertId in dessertIds)
                {
                    await using var insertCommand = _source
                        .CreateCommand($"INSERT INTO CartsDesserts (CartId, DessertId) VALUES ({cartId}, {dessertId})");

                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        #endregion

        #region Cart CRU

        public async Task<bool> CreateCart(long id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL create_cart_for_new_user('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<Cart> GetCart(long Uid)
        {
            _source.OpenConnection();

            try
            {
                await using var command = _source.CreateCommand($"SELECT * FROM Carts WHERE UserId = '{Uid}'");
                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var id = (long)reader["Id"];
                    var userId = (long)reader["UserId"];

                    return new Cart (userId)
                    {
                        Id = id
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
               
            }
            return new Cart(0);
        }


        public async Task<bool> DeleteAllInCart(long id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"DELETE FROM CartsDrinks WHERE CartId = {id};" +
                $"DELETE FROM CartsDesserts WHERE CartId = {id};");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        #endregion
       
        #region Autorization

        public async Task<bool> IsExist(string login, string password)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM IsExistUser('{login}', '{password}');");
            var res = command.ExecuteScalar();
            return (bool)res;
        }

        

        #endregion

        //private async Task LogEntityActionToDatabase(string entityType, string actionType)
        //{
        //    try
        //    {
        //        await using var command = _source.CreateCommand($@"
        //    INSERT INTO EntityActions (EntityType, ActionType) 
        //    VALUES ('{entityType}', '{actionType}')
        //");
        //        await command.ExecuteNonQueryAsync();
        //    }
        //    catch (Exception logEx)
        //    {
        //        Console.WriteLine($"Error logging entity action to database: {logEx.Message}");
        //    }
        //}

        //private async Task LogEntityActionToDatabaseWithId(string entityType, int entityId, string actionType)
        //{
        //    try
        //    {

        //        await using var command = _source.CreateCommand($@"
        //    INSERT INTO EntityActions (EntityType, EntityId, ActionType) 
        //    VALUES ('{entityType}', {entityId}, '{actionType}')
        //");
        //        await command.ExecuteNonQueryAsync();
        //    }
        //    catch (Exception logEx)
        //    {
        //        Console.WriteLine($"Error logging entity action to database: {logEx.Message}");
        //    }
        //}

    }
}
