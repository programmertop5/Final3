using Microsoft.EntityFrameworkCore;
using FINALPROJ.Data;
using FINALPROJ.Data.Entity;

namespace FINALPROJ.Services
{
    public class Statistic
    {
        private readonly DataContext _db;

        public Statistic(DataContext db)
        {
            _db = db;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== СТАТИСТИКА ===\n");
                Console.WriteLine("1. Новинки");
                Console.WriteLine("2. Найпопулярніші книги");
                Console.WriteLine("3. Найпопулярніші жанри");
                Console.WriteLine("0. Назад");
                Console.Write("\nВибір: ");
                string choice = Console.ReadLine();

                if (choice == "1") ShowNewBooks();
                else if (choice == "2") ShowPopularBooks();
                else if (choice == "3") ShowPopularGenres();
                else if (choice == "0") break;
            }
        }

        // ─── НОВИНКИ (книги за останні 30 днів) ─────────────────────
        private void ShowNewBooks()
        {
            Console.Clear();
            Console.WriteLine("=== НОВИНКИ ===\n");
            Console.WriteLine("1. За останній місяць");
            Console.WriteLine("2. За останній рік");
            Console.Write("\nВибір: ");
            string choice = Console.ReadLine();

            DateTime from = DateTime.Today;

            if (choice == "1") from = DateTime.Today.AddDays(-30);
            else if (choice == "2") from = DateTime.Today.AddYears(-1);
            else
            {
                Console.WriteLine("Невірний вибір.");
                Console.ReadKey();
                return;
            }

            var books = _db.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Where(b => b.IsActive && b.Year >= from.Year)
                .OrderByDescending(b => b.Year)
                .ToList();

            if (books.Count == 0)
            {
                Console.WriteLine("Новинок немає.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nID    Назва                     Автор                  Рік    Ціна");
            Console.WriteLine("----------------------------------------------------------------------");

            foreach (var b in books)
            {
                Console.WriteLine(b.Id + "     " + b.Title + "     " + b.Author.FullName + "     " + b.Year + "     " + b.SalePrice);
            }

            Console.WriteLine("\nВсього: " + books.Count);
            Console.ReadKey();
        }

        // ─── НАЙПОПУЛЯРНІШІ КНИГИ ────────────────────────────────────
        private void ShowPopularBooks()
        {
            Console.Clear();
            Console.WriteLine("=== НАЙПОПУЛЯРНІШІ КНИГИ ===\n");
            Console.WriteLine("1. За день");
            Console.WriteLine("2. За тиждень");
            Console.WriteLine("3. За рік");
            Console.Write("\nВибір: ");
            string choice = Console.ReadLine();

            DateTime from = DateTime.Today;

            if (choice == "1") from = DateTime.Today;
            else if (choice == "2") from = DateTime.Today.AddDays(-7);
            else if (choice == "3") from = DateTime.Today.AddYears(-1);
            else
            {
                Console.WriteLine("Невірний вибір.");
                Console.ReadKey();
                return;
            }

            var popular = _db.SaleItems
                .Include(si => si.Book)
                .Where(si => si.Sale.SoldAt >= from)
                .GroupBy(si => si.Book)
                .Select(g => new
                {
                    Book = g.Key,
                    TotalQty = g.Sum(si => si.Quantity)
                })
                .OrderByDescending(x => x.TotalQty)
                .Take(10)
                .ToList();

            if (popular.Count == 0)
            {
                Console.WriteLine("Продажів за цей період немає.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n#     Назва                          Продано");
            Console.WriteLine("-----------------------------------------------");

            int rank = 1;
            foreach (var item in popular)
            {
                Console.WriteLine(rank + ".    " + item.Book.Title + "     " + item.TotalQty + " шт.");
                rank++;
            }

            Console.ReadKey();
        }

        // ─── НАЙПОПУЛЯРНІШІ ЖАНРИ ────────────────────────────────────
        private void ShowPopularGenres()
        {
            Console.Clear();
            Console.WriteLine("=== НАЙПОПУЛЯРНІШІ ЖАНРИ ===\n");
            Console.WriteLine("1. За день");
            Console.WriteLine("2. За тиждень");
            Console.WriteLine("3. За рік");
            Console.Write("\nВибір: ");
            string choice = Console.ReadLine();

            DateTime from = DateTime.Today;

            if (choice == "1") from = DateTime.Today;
            else if (choice == "2") from = DateTime.Today.AddDays(-7);
            else if (choice == "3") from = DateTime.Today.AddYears(-1);
            else
            {
                Console.WriteLine("Невірний вибір.");
                Console.ReadKey();
                return;
            }

            var popular = _db.SaleItems
                .Include(si => si.Book)
                .ThenInclude(b => b.Genre)
                .Where(si => si.Sale.SoldAt >= from)
                .GroupBy(si => si.Book.Genre)
                .Select(g => new
                {
                    Genre = g.Key,
                    TotalQty = g.Sum(si => si.Quantity)
                })
                .OrderByDescending(x => x.TotalQty)
                .ToList();

            if (popular.Count == 0)
            {
                Console.WriteLine("Продажів за цей період немає.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n#     Жанр                    Продано");
            Console.WriteLine("---------------------------------------");

            int rank = 1;
            foreach (var item in popular)
            {
                string genreName = item.Genre == null ? "Невідомо" : item.Genre.Name;
                Console.WriteLine(rank + ".    " + genreName + "     " + item.TotalQty + " шт.");
                rank++;
            }

            Console.ReadKey();
        }
    }
}