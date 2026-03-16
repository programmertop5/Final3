using FINALPROJ.Data;
using FINALPROJ.Data.Entity;
using FINALPROJ.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

DataContext db = new DataContext();
db.Database.EnsureCreated();
db.Seed();

if (!db.Users.Any(u => u.Login == "admin"))
{
    var admin = new User();
    admin.Login = "admin";
    admin.PasswordHash = "admin";
    admin.Role = "Admin";
    db.Users.Add(admin);
    db.SaveChanges();
}

Entrance auth = new Entrance(db);
var currentUser = auth.Login();

BookMenu menu = new BookMenu(db);
Statistic stats = new Statistic(db);

while (true)
{
    Console.Clear();
    Console.WriteLine("----------- КНИЖНИЙ МАГАЗИН -------");
    Console.WriteLine("1) Каталог книг");
    Console.WriteLine("2) Додати книгу");
    Console.WriteLine("3) Редагувати книгу");
    Console.WriteLine("4) Видалити книгу");
    Console.WriteLine("5) Списати книгу");
    Console.WriteLine("6) Акції");
    Console.WriteLine("7) Відкласти для покупця");
    Console.WriteLine("8) Пошук книг");
    Console.WriteLine("9) Статистика");
    Console.WriteLine("0) Вихід");
    Console.Write("\nВибір: ");

    string choice = Console.ReadLine();

    if (choice == "1") menu.ShowAllBooks();
    else if (choice == "2") menu.AddBook();
    else if (choice == "3") menu.EditBook();
    else if (choice == "4") menu.DeleteBook();
    else if (choice == "5") menu.WriteOffBook(currentUser.Id);
    else if (choice == "6") menu.ManagePromotions();
    else if (choice == "7") menu.ReserveBook();
    else if (choice == "8") menu.SearchBooks();
    else if (choice == "9") stats.Show();
    else if (choice == "0") break;
}