using Coffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Coffee
{
    internal class Menu
    {
        public static User CurUser { get; set; } = new();
        private Requests requests = new Requests();

        public void GetMainMenu()
        {
            int select;
            do
            {
                Console.Clear();
                int max = 4;
                Console.WriteLine("1. Завершить программу");
                Console.WriteLine("2. Меню напитков");
                Console.WriteLine("3. Меню десертов");
                Console.WriteLine("4. Отзывы");


                if (CurUser.Role == Role.NoName)
                {
                    Console.WriteLine("5. Авторизация");
                    Console.WriteLine("6. Регистрация");
                    max = 6;
                }

                if (CurUser.Role >= Role.User || CurUser.Role == Role.Admin || CurUser.Role == Role.Moderator)
                {
                    Console.WriteLine("7. Профиль");
                    Console.WriteLine("8. Добавить отзыв");
                    max = 8;
                }


                if (CurUser.Role == Role.Admin || CurUser.Role == Role.Moderator)
                {
                    Console.WriteLine("9. Меню пользователей");
                    max = 9;
                }

                Console.WriteLine();
                select = CheckInput(max);
                Console.WriteLine();


                switch (select)
                {
                    case 2:
                        GetDrinkMenu();
                        break;
                    case 3:
                        GetDessertMenu();
                        break;
                    case 4:
                        GetFeedbackMenu();
                        break;
                    case 5:
                        Authorize();
                        break;
                    case 6:
                        Registration();
                        break;
                    case 7:
                        GetProfile();
                        break;
                    case 8:
                        AddFeedbacks();
                        break;
                    case 9:
                        GetUserChangeMenu();
                        break;
                }
            } while (select != 1);
        }

        #region Drink
        public void GetDrinkMenu()
        {
            Console.Clear();
            int select;
            do
            {
                int max = 2;
                Console.WriteLine($"1. Вернуться");
                Console.WriteLine($"2. Увидеть все напитки");
                if (CurUser.Role == Role.Admin)
                {
                    Console.WriteLine($"3. Добавить напиток");
                    Console.WriteLine($"4. Удалить напиток по Id");
                    Console.WriteLine($"5. Обновить напиток");
                    Console.WriteLine($"6. Частичный поиск");
                    max = 6;
                }
                Console.WriteLine();
                select = CheckInput(max);

                switch (select)
                {
                    case 2:
                        foreach (var drink in requests.GetAllDrink("Drink", "read").Result)
                        {
                            Console.WriteLine($"Номер напитка(id): {drink.Id} , название {drink.Name}, размер {drink.Size} , цена {drink.Price} руб.");
                        }
                        break;
                    case 3:
                        SetDrinkParam(out Size size, out float price, out string name);
                        requests.CreateDrink("Drink", "create", size, price, name).Wait();
                        break;
                    case 4:
                        int id = GetId();
                        requests.DeleteDrinkById("Drink", "delete", id).Wait();
                        break;
                    case 5:
                        id = GetId();
                        SetDrinkParam(out size, out price, out name);
                        requests.UpdateDrink("Drink", "update", id, size, price, name).Wait();
                        break;
                    case 6:
                        Console.WriteLine("Введите часть названия:");
                        string part = CheckString();
                        foreach (var drink in requests.PartialSearchDrink("Drink", "search", part).Result)
                        {
                            Console.WriteLine($"Номер напитка(id): {drink.Id} , название {drink.Name}, размер {drink.Size} , цена {drink.Price} руб.");
                        }
                        break;
                }
                Console.WriteLine("==========================================================================\n");
            } while (select != 1);

            Console.Clear();
        }

        private string CheckString()
        {
            string res = Console.ReadLine();
            while (string.IsNullOrEmpty(res))
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку: ");
                res = Console.ReadLine();
            }

            return res;
        }

        private void SetDrinkParam(out Size size, out float price, out string name)
        {
            Console.WriteLine("Введите название напитка: ");
            while (string.IsNullOrWhiteSpace(name = Console.ReadLine()))
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку: ");
            }

            Console.WriteLine("Введите размер напитка (S, M, L): ");
            while (true)
            {
                var inputSize = Console.ReadLine();
                if (Enum.TryParse(inputSize, true, out Size parsedSize))
                {
                    size = parsedSize;
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Повторите попытку: ");
                }
            }

            Console.WriteLine("Введите стоимость напитка: ");
            while (!float.TryParse(Console.ReadLine(), out price))
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку: ");
            }
        }
        #endregion

        #region Dessert
        public void GetDessertMenu()
        {
            Console.Clear();
            int select;
            do
            {
                int max = 2;
                Console.WriteLine($"1. Вернуться");
                Console.WriteLine($"2. Увидеть все десерты");
                if (CurUser.Role == Role.Admin)
                {
                    Console.WriteLine($"3. Добавить десерт");
                    Console.WriteLine($"4. Удалить десерт по Id");
                    Console.WriteLine($"5. Обновить десерт");
                    Console.WriteLine($"6. Частичный поиск");
                    max = 6;
                }
                Console.WriteLine();
                select = CheckInput(max);

                switch (select)
                {
                    case 2:
                        foreach (var dessert in requests.GetAllDesserts("Dessert", "read").Result)
                        {
                            Console.WriteLine($"Номер десерта(id): {dessert.Id} , название {dessert.Name}, калории {dessert.Calories} , цена {dessert.Price} руб.");
                        }
                        break;
                    case 3:
                        SetDessertParam(out int calories, out float price, out string name);
                        requests.CreateDessert("Dessert", "create", calories, price, name).Wait();
                        break;
                    case 4:
                        int id = GetId();
                        requests.DeleteDessertById("Dessert", "delete", id).Wait();
                        break;
                    case 5:
                        id = GetId();
                        SetDessertParam(out calories, out price, out name);
                        requests.UpdateDessert("Dessert", "update", id, calories, price, name).Wait();
                        break;
                    case 6:
                        Console.WriteLine("Введите часть названия:");
                        string part = CheckString();
                        foreach (var dessert in requests.PartialSearchDessert("Dessert", "search",part).Result)
                        {
                            Console.WriteLine($"Номер десерта(id): {dessert.Id} , название {dessert.Name}, калории {dessert.Calories} , цена {dessert.Price} руб.");
                        }
                        break;
                }
                Console.WriteLine("==========================================================================\n");
            } while (select != 1);

            Console.Clear();
        }

        private void SetDessertParam(out int calories, out float price, out string name)
        {
            Console.WriteLine("Введите название десерта: ");
            while (string.IsNullOrWhiteSpace(name = Console.ReadLine()))
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку: ");
            }

            Console.WriteLine("Введите калорийность десерта: ");
            while (!int.TryParse(Console.ReadLine(), out calories))
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку: ");
            }

            Console.WriteLine("Введите стоимость десерта: ");
            while (!float.TryParse(Console.ReadLine(), out price))
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку: ");
            }
        }
        #endregion

        #region Feedback
        private void AddFeedbacks()
        {
            SetFeedbackParam(out int rating, out string discription);
            requests.CreateFeedback(rating, discription, (int)CurUser.Id).Wait();
        }

        private void SetFeedbackParam(out int rating, out string discription)
        {
            Console.WriteLine("Введите оценку: ");
            while (!int.TryParse(Console.ReadLine(), out rating))
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку: ");
            }

            Console.WriteLine("Напишите описание: ");
            while (string.IsNullOrWhiteSpace(discription = Console.ReadLine()))
            {
                Console.WriteLine("Некорректный ввод. Повторите попытку: ");
            }

        }
        #endregion

        #region Profile

        public void GetProfile()
        {

            Console.Clear();
            Console.WriteLine($"Логин: {CurUser.Login}");
            Console.WriteLine($"Имя: {CurUser.Name}");
            Console.WriteLine($"Дата рождения: {CurUser.DateOfBirth}");
            Console.WriteLine($"Роль: {CurUser.Role}");
            Console.WriteLine($"Роль: {CurUser.Id}");
            //Console.WriteLine($"Роль: {CurUser.cart}");
            Console.WriteLine("========================================");
            //int id = GetId();
            int id = (int)CurUser.Id;

            int select;
            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Назад");
                Console.WriteLine("2. Изменить пользователя");
                Console.WriteLine("3. Просмотреть заказы");
                Console.WriteLine("4. Оформить заказ");
                Console.WriteLine("5. Очистить корзину");
                Console.WriteLine("6. Добавить в корзину");
                Console.WriteLine("7. Посмотреть корзину");
                Console.WriteLine();

                select = CheckInput(7);

                switch (select)
                {
                    case 2:
                        SetUserParametr(out string name, out DateTime dateOfBirth);
                        requests.UpdateUser("User", "update", CurUser.Id, name, dateOfBirth).Wait();
                        CurUser = requests.GetUserByLoginAndPassword(CurUser.Login, CurUser.Password).Result;
                        Console.Clear();
                        Console.WriteLine($"Логин: {CurUser.Login}");
                        Console.WriteLine($"Имя: {CurUser.Name}");
                        Console.WriteLine($"Дата рождения: {CurUser.DateOfBirth}");
                        Console.WriteLine($"Роль: {CurUser.Role}");
                        break;
                    case 3:
                        foreach (Order order in requests.GetOrders(CurUser.Id).Result)
                        {
                            Console.WriteLine($"Заказ: {order.Id}, Дата: {order.Date}");
                        }
                        break;
                    case 4:
                        int num = 0;
                        foreach (var drink in requests.GetAllDrinkInCart("Drink", "read", id).Result)
                        {
                            num++;
                        }
                        foreach (var dessert in requests.GetAllDessertInCart("Dessert", "read", id).Result)
                        {
                            num++;
                        }
                        if ( num == 0)
                        {
                            Console.WriteLine($"Корзина пуста");
                            break;
                        }
                        requests.CreateOrder((int)CurUser.Id).Wait();
                        break;
                    case 5:
                        requests.DeleteAllInCart(id).Wait();
                        break;
                    case 6:
                        Console.WriteLine("...........................................................\n");
                        Console.WriteLine("|                     Меню                                  |");
                        Console.WriteLine("...........................................................\n");
                        Console.WriteLine("...........................................................\n");
                        Console.WriteLine("|                     Напитки                               |");
                        Console.WriteLine("...........................................................\n");
                        foreach (var drink in requests.GetAllDrink("Drink", "read").Result)
                        {
                            Console.WriteLine($"Номер напитка(id): {drink.Id}, название: {drink.Name}, размер: {drink.Size}, цена: {drink.Price}");
                        }
                        Console.WriteLine("\n");
                        Console.WriteLine("...........................................................\n");
                        Console.WriteLine("|                     Десерты                               |");
                        Console.WriteLine("...........................................................\n");
                        foreach (var dessert in requests.GetAllDesserts("Dessert", "read").Result)
                        {
                            Console.WriteLine($"Номер напитка(id): {dessert.Id}, название: {dessert.Name}, размер: {dessert.Calories}");
                        }
                        Console.WriteLine("\n");
                        Console.WriteLine("Введите 1 - назад, 2 - если хотите выбрать напитки, 3 - если десерты");
                        int selectOrder = CheckInput(3);
                        while (selectOrder != 1)
                        {
                            if (selectOrder == 2)
                            {
                                Console.WriteLine("Введите через пробел номера напитков");
                                requests.AddDrinksToCart(id, Console.ReadLine().Split().Select(s => long.Parse(s)).ToArray());
                                break;
                            }
                            if (selectOrder == 3)
                            {
                                Console.WriteLine("Введите через пробел номера десертов");
                                requests.AddDessertsToCart(id, Console.ReadLine().Split().Select(s => long.Parse(s)).ToArray());
                                break;
                            }
                        }
                        break;

                    case 7:
                        foreach (var drink in requests.GetAllDrinkInCart("Drink", "read", id).Result)
                        {
                            Console.WriteLine($"Номер напитка(id): {drink.Id} , название {drink.Name}, размер {drink.Size} , цена {drink.Price} руб.");
                        }
                        foreach (var dessert in requests.GetAllDessertInCart("Dessert", "read", id).Result)
                        {
                            Console.WriteLine($"Номер напитка(id): {dessert.Id} , название {dessert.Name}, размер {dessert.Calories} , цена {dessert.Price} руб.");
                        }
                        break;
                
                }
                Console.WriteLine("==========================================================================\n");

            } while (select != 1);

        }

        private void SetUserParametr(out string name, out DateTime dateOfBirth)
        {
           
            Console.WriteLine("Введите имя: ");
            name = CheckString();

            Console.WriteLine("Введите дату рождения: ");
            string dateTime = Console.ReadLine();
            DateTime.TryParseExact(dateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth);

        }

        #endregion

        #region Authorize

        public void Authorize()
        {
            bool tryThis = true;

            while (tryThis)
            {
                string login;
                string password;

                Console.Clear();
                Console.WriteLine("Введите логин: ");
                login = CheckString();
                Console.WriteLine("Введите пароль: ");
                password = CheckString();

                if (requests.IsExist(login, password).Result)
                {
                    CurUser = requests.GetUserByLoginAndPassword(login, password).Result;
                    Console.Clear();
                    return;
                }
                else
                {
                    Console.WriteLine("Такого пользователя нет. Хотите повторить попытку (y/n)?");
                    tryThis = Console.ReadLine() == "y";
                }
            }

        }
        public void Registration()
        {
            bool tryThis = true;

            while (tryThis)
            {
                Console.Clear();

                Console.WriteLine("Введите логин: ");
                string login = CheckString();
 
                Console.WriteLine("Введите имя: ");
                string name = CheckString();

                Console.WriteLine("Введите дату рождения: ");
                string dateTime = Console.ReadLine();
                DateTime.TryParseExact(dateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth);

                Console.WriteLine("Введите пароль: ");
                string password = CheckString();

                string roleEnum = Enum.GetName(typeof(Role), Role.User);

                if (requests.CreateUser(login, name, roleEnum, dateOfBirth, password).Result)
                {
                    CurUser = requests.GetUserByLoginAndPassword(login, password).Result;
                    requests.CreateCart(CurUser.Id);

                    return;
                }
                else
                {
                    
                    Console.WriteLine("Были введены некорректные данные. Хотите повторить попытку (y/n)?");
                    tryThis = Console.ReadLine() == "y";
                }
            }
        }
        #endregion

        #region UserMenu
        public void GetUserChangeMenu()
        {
            int select;

            do
            {
                Console.WriteLine("1. Вернуться");
                Console.WriteLine("2. Увидеть всех пользователей");
                Console.WriteLine("3. Забанить пользователя");
                Console.WriteLine("4. Изменить роль пользователя");
                Console.WriteLine();
                select = CheckInput(4);

                switch (select)
                {
                    case 2:
                        ViewAllUsers();
                        break;
                    case 3:
                        UpdateUserRole();
                        break;
                    case 4:
                        UpdateUserRole();
                        break;
                }

                Console.WriteLine("==========================================================================\n");
            } while (select != 1);

            Console.Clear();
        }

        private void ViewAllUsers()
        {
            var users = requests.GetAllUsers("User", "read").Result;
            Console.WriteLine("===== Все пользователи =====");
            foreach (var user in users)
            {
                Console.WriteLine($"ID пользователя: {user.Id}, Имя: {user.Name}, Логин: {user.Login}, Дата рождения: {user.DateOfBirth}, Роль: {user.Role}");
            }
        }

        
        private void UpdateUserRole()
        {
            int userId = GetId();

            bool tryThis = true;

            while (tryThis)
            {
                Console.Write("Введите новый код роли: ");
                var newRoleCode = CheckString();

                if (requests.UpdateUserRole("User", "update", userId, newRoleCode).Result)
                {
                    return;
                }
                else
                {
                    //Console.Clear();
                    Console.WriteLine("Были введены некорректные данные. Хотите повторить попытку (y/n)?");
                    tryThis = Console.ReadLine() == "y";
                }
            }
            
        }

        private void DeleteUser()
        {
            int userId = GetId();
            requests.DeleteUserById("User", "delete", userId).Wait();
        }
        #endregion

        private void GetFeedbackMenu()
        {
            Console.Clear();
            int select;
            do
            {
                int max = 2;
                Console.WriteLine($"1. Вернуться");
                Console.WriteLine($"2. Увидеть все отзывы");
                Console.WriteLine();
                select = CheckInput(max);

                switch (select)
                {
                    case 2:
                        foreach (var feedbacks in requests.GetAllFeedbacks("Feedback", "read").Result)
                        {
                            // Console.WriteLine($"Номер пользователя(id): {feedbacks.UserId}, оценка: {feedbacks.Rating}");
                            Console.WriteLine($"Oценка: {feedbacks.Rating}");
                            Console.WriteLine($"описание: {feedbacks.Description}");
                            Console.WriteLine($"дата: {feedbacks.Date}");
                            Console.WriteLine("................................................................\n");
                        }
                        Console.WriteLine("\n");
                        break;
                    
                }
                Console.WriteLine("==========================================================================\n");
            } while (select != 1);

            Console.Clear();

            
        }

        private int CheckInput(int max)
        {
            int change;
            Console.WriteLine("Сделайте выбор: ");
            while (!int.TryParse(Console.ReadLine(), out change) || (change < 1 || change > max))
            {
                Console.WriteLine("Некорректный ввод. Сделайте выбор: ");
            }

            return change;
        }

        private int GetId()
        {
            int change;
            Console.WriteLine("Введите id объекта: ");
            while (!int.TryParse(Console.ReadLine(), out change))
            {
                Console.WriteLine("Некорректный ввод. Сделайте выбор: ");
            }

            return change;
        }
    }


}
