using FINALPROJ.Data;
using FINALPROJ.Data.Entity;

namespace FINALPROJ.Services
{
    public class Entrance
    {
        private readonly DataContext _db;

        public Entrance(DataContext db)
        {
            _db = db;
        }

        public User Login()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ВХІД В СИСТЕМУ ===\n");

                Console.Write("Логін: ");
                string login = Console.ReadLine();

                if (login == null)
                {
                    Console.WriteLine("Логін не може бути порожнім!");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Пароль: ");
                string password = Console.ReadLine();

                if (password == null)
                {
                    Console.WriteLine("Пароль не може бути порожнім!");
                    Console.ReadKey();
                    continue;
                }

                // Запасний вхід якщо бд не має користувача
                if (login == "admin" && password == "admin123")
                {
                    User adminUser = _db.Users.FirstOrDefault(u => u.Login == "admin");

                    if (adminUser == null)
                    {
                        adminUser = new User();
                        adminUser.Login = "admin";
                        adminUser.PasswordHash = "admin123";
                        adminUser.Role = "Admin";
                        _db.Users.Add(adminUser);
                        _db.SaveChanges();
                    }

                    Console.WriteLine("\nВітаємо, admin! Роль: Admin");
                    Console.ReadKey();
                    return adminUser;
                }

                User user = _db.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == password);

                if (user == null)
                {
                    Console.WriteLine("\nНевірний логін або пароль!");
                    Console.WriteLine("Підказка: логін = admin, пароль = admin123");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("\nВітаємо, " + user.Login + "! Роль: " + user.Role);
                Console.ReadKey();
                return user;
            }
        }
    }
}