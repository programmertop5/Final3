using Microsoft.EntityFrameworkCore;
using FINALPROJ.Data;
using FINALPROJ.Data.Entity;

namespace FINALPROJ.Services
{
    public class BookMenu
    {
        private readonly DataContext _db;

        public BookMenu(DataContext db)
        {
            _db = db;
        }

        /*1*/
        public void ShowAllBooks()
        {
            Console.Clear();
            Console.WriteLine("------ КАТАЛОГ КНИГ --------\n");

            var books = _db.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Where(b => b.IsActive)
                .ToList();

            if (books.Count == 0)
            {
                Console.WriteLine("Книг немає.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("ID:    Назва:               Автор:             Жанр:        Рік:   Ціна:     Склад:");

            foreach (var b in books)
            {
                Console.WriteLine($"{b.Id} | {b.Title} | {b.Author.FullName} | {b.Genre.Name} | {b.Year} | {b.SalePrice} грн | {b.Stock} шт.");
            }

            Console.WriteLine($"\nВсього книг: {books.Count}");
            Console.ReadKey();
        }

        /*2*/
        public void AddBook()
        {
            Console.Clear();
            Console.WriteLine("--------ДОДАТИ КНИГУ ---------\n");

            Console.Write("Назва книги: ");
            string title = Console.ReadLine();

            if (title == null)
            {
                Console.WriteLine("Назва не може бути порожньою!");
                Console.ReadKey();
                return;
            }

            Console.Write("Автор (ПІБ): ");
            string authorName = Console.ReadLine();

            if (authorName == null)
            {
                Console.WriteLine("Ім'я автора не може бути порожнім!");
                Console.ReadKey();
                return;
            }

            Author author = _db.Authors.FirstOrDefault(a => a.FullName == authorName);

            if (author == null)
            {
                author = new Author();
                author.FullName = authorName;
                _db.Authors.Add(author);
                _db.SaveChanges();
                Console.WriteLine($"Новий автор додано до бази: {authorName}");
            }
            else
            {
                Console.WriteLine($"Автор знайдений: {author.FullName}");
            }

            Console.Write("Видавництво: ");
            string pubName = Console.ReadLine()!;

            if (pubName == null)
            {
                Console.WriteLine("Назва видавництва не може бути порожньою!");
                Console.ReadKey();
                return;
            }

            Publisher publisher = _db.Publishers.FirstOrDefault(p => p.Name == pubName)!;

            if (publisher == null)
            {
                publisher = new Publisher();
                publisher.Name = pubName;
                _db.Publishers.Add(publisher);
                _db.SaveChanges();
            }

            var genres = _db.Genres.ToList();

            if (genres.Count == 0)
            {
                Console.WriteLine("Жанрів немає в базі даних!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nДоступні жанри:");
            for (int i = 0; i < genres.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + genres[i].Name);
            }

            Console.Write("Введіть номер жанру: ");
            int genreNumber = int.Parse(Console.ReadLine());
            Genre genre = genres[genreNumber - 1];

            Console.Write("Кількість сторінок: ");
            int pages = int.Parse(Console.ReadLine());

            Console.Write("Рік видання: ");
            int year = int.Parse(Console.ReadLine());

            Console.Write("Собівартість (грн): ");
            double cost = double.Parse(Console.ReadLine());

            Console.Write("Ціна продажу (грн): ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Кількість на складі: ");
            int stock = int.Parse(Console.ReadLine());

            Console.Write("Є продовженням? (т/н): ");
            string sequelAnswer = Console.ReadLine();
            bool isSequel = false;
            string sequelNote = null;

            if (sequelAnswer == "т" || sequelAnswer == "Т")
            {
                isSequel = true;
                Console.Write("Примітка (напр. Частина 2): ");
                sequelNote = Console.ReadLine();
            }

            Book book = new Book();
            book.Title = title;
            book.AuthorId = author.Id;
            book.PublisherId = publisher.Id;
            book.GenreId = genre.Id;
            book.PageCount = pages;
            book.Year = year;
            book.CostPrice = cost;
            book.SalePrice = price;
            book.Stock = stock;
            book.IsSequel = isSequel;
            book.SequelNote = sequelNote;

            _db.Books.Add(book);
            _db.SaveChanges();

            Console.WriteLine("\nКнигу " + title + " успішно додано! ID: " + book.Id);
            Console.ReadKey();
        }

        /*3*/
        public void DeleteBook()
        {
            Console.Clear();
            Console.WriteLine("------- ВИДАЛИТИ КНИГУ -------\n");
            ShowBooksShort();

            Console.Write("\nВведіть ID книги для видалення: ");
            int id = int.Parse(Console.ReadLine()!);

            Book book = _db.Books.Find(id)!;

            if (book == null)
            {
                Console.WriteLine("Книгу не знайдено.");
                Console.ReadKey();
                return;
            }

            if (book.IsActive == false)
            {
                Console.WriteLine("Книгу не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.Write($"Видалити {book.Title} (т/н): ");
            string answer = Console.ReadLine();

            if (answer != "т" && answer != "Т")
            {
                return;
            }

            book.IsActive = false;
            _db.SaveChanges();

            Console.WriteLine("Книгу видалено.");
            Console.ReadKey();
        }

        /*4*/
        public void EditBook()
        {
            Console.Clear();
            Console.WriteLine("-------- РЕДАГУВАТИ КНИГУ ---------\n");
            ShowBooksShort();

            Console.Write("\nВведіть ID книги: ");
            int id = int.Parse(Console.ReadLine());

            Book book = _db.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Genre)
                .FirstOrDefault(b => b.Id == id && b.IsActive == true)!;

            if (book == null)
            {
                Console.WriteLine("Книгу не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nРедагуємо: " + book.Title);

            Console.Write($"Назва: {book.Title}: ");
            string inputTitle = Console.ReadLine()!;
            if (inputTitle != "")
            {
                book.Title = inputTitle;
            }

            Console.Write($"Ціна продажу {book.SalePrice}: ");
            string inputPrice = Console.ReadLine()!;
            if (inputPrice != "")
            {
                book.SalePrice = double.Parse(inputPrice);
            }

            Console.Write($"Кількість на складі {book.Stock}: ");
            string inputStock = Console.ReadLine()!;
            if (inputStock != "")
            {
                book.Stock = int.Parse(inputStock);
            }

            Console.Write($"Рік {book.Year}: ");
            string inputYear = Console.ReadLine()!;
            if (inputYear != "")
            {
                book.Year = int.Parse(inputYear);
            }

            _db.SaveChanges();
            Console.WriteLine("\nЗміни збережено.");
            Console.ReadKey();
        }
       
        /*5*/
        public void WriteOffBook(int currentUserId)
        {
            Console.Clear();
            Console.WriteLine("----------- СПИСАТИ КНИГУ ----------\n");
            ShowBooksShort();

            Console.Write("\nВведіть ID книги: ");
            int id = int.Parse(Console.ReadLine()!);

            Book book = _db.Books.Find(id)!;

            if (book == null)
            {
                Console.WriteLine("Книгу не знайдено.");
                Console.ReadKey();
                return;
            }

            if (book.IsActive == false)
            {
                Console.WriteLine("Книгу не знайдено.");
                Console.ReadKey();
                return;
            }

            if (book.Stock <= 0)
            {
                Console.WriteLine("Книги немає на складі!");
                Console.ReadKey();
                return;
            }

            Console.Write($"Скільки списати, максимум {book.Stock}: ");
            int quantity = int.Parse(Console.ReadLine())!;

            if (quantity <= 0 || quantity > book.Stock)
            {
                Console.WriteLine("Невірна кількість.");
                Console.ReadKey();
                return;
            }

            Console.Write($"Списати {quantity} шт. : {book.Title} (т/н): ");
            string answer = Console.ReadLine()!;

            if (answer != "т" && answer != "Т")
            {
                return;
            }

            book.Stock = book.Stock - quantity;
            _db.SaveChanges();

            Console.WriteLine($"\nГотово! {book.Title}, списано {quantity} шт., залишок: {book.Stock}");
            Console.ReadKey();
        }

        /*6*/
        public void ManagePromotions()
        {
            Console.Clear();
            Console.WriteLine("--------- АКЦІЇ -------\n");
            Console.WriteLine("1. Створити нову акцію");
            Console.WriteLine("2. Додати книгу до акції");
            Console.WriteLine("3. Переглянути активні акції");
            Console.WriteLine("0. Назад");
            Console.Write("\nВибір: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                CreatePromotion();
            }
            else if (choice == "2")
            {
                AddBookToPromotion();
            }
            else if (choice == "3")
            {
                ShowPromotions();
            }
        }

        private void CreatePromotion()
        {
            Console.Clear();
            Console.WriteLine("-------- НОВА АКЦІЯ ------\n");

            Console.Write("Назва акції: ");
            string name = Console.ReadLine()!;

            if (name == null)
            {
                Console.WriteLine("Назва не може бути порожньою!");
                Console.ReadKey();
                return;
            }

            Console.Write("Знижка (%): ");
            double discount = double.Parse(Console.ReadLine());

            Console.Write("Дата початку (дд.мм.рррр): ");
            DateTime start = DateTime.Parse(Console.ReadLine());

            Console.Write("Дата завершення (дд.мм.рррр): ");
            DateTime end = DateTime.Parse(Console.ReadLine());

            Promotion promo = new Promotion();
            promo.Name = name;
            promo.DiscountPercent = (decimal)discount;
            promo.StartDate = start;
            promo.EndDate = end;

            _db.Promotions.Add(promo);
            _db.SaveChanges();

            Console.WriteLine($"\nАкція \"{name}\" {discount}%  : збережено!");
            Console.ReadKey();
        }

        private void AddBookToPromotion()
        {
            Console.Clear();

            var promos = _db.Promotions
                .Where(p => p.EndDate >= DateTime.Today)
                .ToList();

            if (promos.Count == 0)
            {
                Console.WriteLine("Немає активних акцій.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Активні акції:");
            foreach (var p in promos)
            {
                Console.WriteLine($"{p.Id}. {p.Name}, {p.DiscountPercent}% до {p.EndDate:dd.MM.yyyy}");
            }

            Console.Write("\nID акції: ");
            int promoId = int.Parse(Console.ReadLine());

            Promotion promo = _db.Promotions
                .Include(p => p.Books)
                .FirstOrDefault(p => p.Id == promoId)!;

            if (promo == null)
            {
                Console.WriteLine("Акцію не знайдено.");
                Console.ReadKey();
                return;
            }

            if (promo.Books == null)
            {
                Console.WriteLine("Помилка: список книг акції порожній.");
                Console.ReadKey();
                return;
            }

            ShowBooksShort();
            Console.Write("\nID книги: ");
            int bookId = int.Parse(Console.ReadLine()!);

            Book book = _db.Books.Find(bookId)!;

            if (book == null)
            {
                Console.WriteLine("Книгу не знайдено.");
                Console.ReadKey();
                return;
            }

            bool alreadyAdded = false;
            foreach (var b in promo.Books)
            {
                if (b.Id == bookId)
                {
                    alreadyAdded = true;
                    break;
                }
            }

            if (alreadyAdded == false)
            {
                promo.Books.Add(book);
                _db.SaveChanges();
                Console.WriteLine($"{book.Title} додано до акції {promo.Name}");
            }
            else
            {
                Console.WriteLine("Книга вже є в цій акції.");
            }

            Console.ReadKey();
        }

        private void ShowPromotions()
        {
            Console.Clear();
            Console.WriteLine("----- АКТИВНІ АКЦІЇ ------\n");

            var promos = _db.Promotions
                .Include(p => p.Books)
                .Where(p => p.EndDate >= DateTime.Today)
                .ToList();

            if (promos.Count == 0)
            {
                Console.WriteLine("Акцій немає.");
                Console.ReadKey();
                return;
            }

            foreach (var p in promos)
            {
                Console.WriteLine($"{p.Name},{p.DiscountPercent}%, {p.StartDate:dd.MM} до {p.EndDate:dd.MM.yyyy}");

                if (p.Books == null)
                {
                    Console.WriteLine("(книг немає)");
                }
                else
                {
                    foreach (var b in p.Books)
                    {
                        Console.WriteLine("" + b.Title);
                    }
                }

                Console.WriteLine();
            }

            Console.ReadKey();
        }
        /*6*/
        public void ReserveBook()
        {
            Console.Clear();
            Console.WriteLine("--------- ВІДКЛАСТИ ДЛЯ ПОКУПЦЯ ----------\n");
            ShowBooksShort();

            Console.Write("\nID книги: ");
            int id = int.Parse(Console.ReadLine());

            Book book = _db.Books.Find(id)!;

            if (book == null)
            {
                Console.WriteLine("Книгу не знайдено.");
                Console.ReadKey();
                return;
            }

            if (book.IsActive == false)
            {
                Console.WriteLine("Книгу не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.Write("Ім'я покупця: ");
            string name = Console.ReadLine()!;

            if (name == null)
            {
                Console.WriteLine("Ім'я не може бути порожнім!");
                Console.ReadKey();
                return;
            }

            Console.Write("Телефон (або Enter щоб пропустити): ");
            string phone = Console.ReadLine()!;

            Reservation reservation = new Reservation();
            reservation.BookId = book.Id;
            reservation.CustomerName = name;
            reservation.ReservedAt = DateTime.Now;
            reservation.IsActive = true;

            if (phone != "")
            {
                reservation.CustomerPhone = phone;
            }

            _db.Reservations.Add(reservation);
            _db.SaveChanges();

            Console.WriteLine($"\n{book.Title} відкладено для {name}");
            Console.ReadKey();
        }

        /*7*/
        public void SearchBooks()
        {
            Console.Clear();
            Console.WriteLine("----- ПОШУК КНИГ -----\n");
            Console.WriteLine("1. За назвою");
            Console.WriteLine("2. За автором");
            Console.WriteLine("3. За жанром");
            Console.Write("\nВибір: ");
            string choice = Console.ReadLine()!;

            Console.Write("Пошуковий запит: ");
            string query = Console.ReadLine()!;

            if (query == null)
            {
                Console.WriteLine("Запит не може бути порожнім.");
                Console.ReadKey();
                return;
            }

            var allBooks = _db.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Where(b => b.IsActive == true)
                .ToList();

            var result = new List<Book>();

            foreach (var b in allBooks)
            {
                if (choice == "1" && b.Title != null && b.Title.Contains(query))
                {
                    result.Add(b);
                }
                else if (choice == "2" && b.Author != null && b.Author.FullName != null && b.Author.FullName.Contains(query))
                {
                    result.Add(b);
                }
                else if (choice == "3" && b.Genre != null && b.Genre.Name != null && b.Genre.Name.Contains(query))
                {
                    result.Add(b);
                }
            }

            Console.Clear();
            Console.WriteLine($"\nЗнайдено: {result.Count}\n");

            if (result.Count == 0)
            {
                Console.WriteLine("Нічого не знайдено.");
                Console.ReadKey();
                return;
            }

            foreach (var b in result)
            {
                Console.WriteLine($"{b.Id}, {b.Title}, {b.Author.FullName}, {b.Genre.Name}, {b.SalePrice} грн | {b.Stock} шт.");
            }

            Console.ReadKey();
        }

        /*8*/
        private void ShowBooksShort()
        {
            var books = _db.Books
                .Include(b => b.Author)
                .Where(b => b.IsActive == true)
                .ToList();

            Console.WriteLine("---------------ID:    Назва:    Автор:          Склад:----------------");

            foreach (var b in books)
            {
                Console.WriteLine($"{b.Id}, {b.Title},{b.Author.FullName},{b.Stock} шт.");
            }
        }
    }
}