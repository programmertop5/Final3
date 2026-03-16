using Microsoft.EntityFrameworkCore;
using FINALPROJ.Data.Entity;
using System.Collections.Generic;
using System.Reflection.Emit;
using static System.Reflection.Metadata.BlobBuilder;

namespace FINALPROJ.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Publisher> Publishers => Set<Publisher>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<SaleItem> SaleItems => Set<SaleItem>();
        public DbSet<Promotion> Promotions => Set<Promotion>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=BookstoreDB;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Promotions)
                .WithMany(p => p.Books)
                .UsingEntity(j => j.ToTable("BookPromotions"));

            modelBuilder.Entity<SaleItem>()
                .Property(s => s.DiscountPercent).HasPrecision(5, 2);
            modelBuilder.Entity<SaleItem>()
                .Property(s => s.PriceAtSale).HasPrecision(18, 2);
            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountPercent).HasPrecision(5, 2);
        }

        public void Seed()
        {
            if (Users.Any()) return;

            var user = new User();
            user.Login = "admin";
            user.PasswordHash = "admin";
            user.Role = "Admin";
            Users.Add(user);
            SaveChanges();

            var author1 = new Author { FullName = "Ліна Костенко" };
            var author2 = new Author { FullName = "Іван Франко" };
            var author3 = new Author { FullName = "Стівен Кінг" };
            var author4 = new Author { FullName = "Василь Шкляр" };
            var author5 = new Author { FullName = "Сергій Жадан" };
            var author6 = new Author { FullName = "Джордж Орвелл" };
            var author7 = new Author { FullName = "Агата Крісті" };
            var author8 = new Author { FullName = "Френк Герберт" };
            Authors.AddRange(author1, author2, author3, author4, author5, author6, author7, author8);

            var pub1 = new Publisher { Name = "Видавництво Старого Лева" };
            var pub2 = new Publisher { Name = "А-БА-БА-ГА-ЛА-МА-ГА" };
            Publishers.AddRange(pub1, pub2);

            var genre1 = new Genre { Name = "Поезія" };
            var genre2 = new Genre { Name = "Роман" };
            var genre3 = new Genre { Name = "Жахи" };
            var genre4 = new Genre { Name = "Детектив" };
            var genre5 = new Genre { Name = "Фантастика" };
            Genres.AddRange(genre1, genre2, genre3, genre4, genre5);

            SaveChanges();

            var books = new List<Book>
    {
        new Book { Title = "Маруся Чурай", AuthorId = author1.Id, PublisherId = pub1.Id, GenreId = genre1.Id, PageCount = 200, Year = 2015, CostPrice = 80, SalePrice = 150, Stock = 10, IsActive = true },
        new Book { Title = "Берестечко", AuthorId = author1.Id, PublisherId = pub1.Id, GenreId = genre1.Id, PageCount = 156, Year = 2012, CostPrice = 70, SalePrice = 130, Stock = 8, IsActive = true },
        new Book { Title = "Над берегами вічної ріки", AuthorId = author1.Id, PublisherId = pub2.Id, GenreId = genre1.Id, PageCount = 180, Year = 2011, CostPrice = 65, SalePrice = 120, Stock = 6, IsActive = true },
        new Book { Title = "Сад нетанучих скульптур", AuthorId = author1.Id, PublisherId = pub1.Id, GenreId = genre1.Id, PageCount = 140, Year = 2014, CostPrice = 60, SalePrice = 110, Stock = 4, IsActive = true },
        new Book { Title = "Річка Геракліта", AuthorId = author1.Id, PublisherId = pub2.Id, GenreId = genre1.Id, PageCount = 160, Year = 2016, CostPrice = 72, SalePrice = 135, Stock = 7, IsActive = true },

        new Book { Title = "Захар Беркут", AuthorId = author2.Id, PublisherId = pub2.Id, GenreId = genre2.Id, PageCount = 180, Year = 2010, CostPrice = 60, SalePrice = 120, Stock = 5, IsActive = true },
        new Book { Title = "Борислав сміється", AuthorId = author2.Id, PublisherId = pub1.Id, GenreId = genre2.Id, PageCount = 220, Year = 2009, CostPrice = 75, SalePrice = 140, Stock = 6, IsActive = true },
        new Book { Title = "Перехресні стежки", AuthorId = author2.Id, PublisherId = pub2.Id, GenreId = genre2.Id, PageCount = 310, Year = 2013, CostPrice = 90, SalePrice = 160, Stock = 4, IsActive = true },
        new Book { Title = "Лис Микита", AuthorId = author2.Id, PublisherId = pub1.Id, GenreId = genre2.Id, PageCount = 130, Year = 2008, CostPrice = 50, SalePrice = 95, Stock = 9, IsActive = true },
        new Book { Title = "Украдене щастя", AuthorId = author2.Id, PublisherId = pub2.Id, GenreId = genre1.Id, PageCount = 110, Year = 2011, CostPrice = 45, SalePrice = 90, Stock = 7, IsActive = true },

        new Book { Title = "Залишенець. Чорний ворон", AuthorId = author4.Id, PublisherId = pub1.Id, GenreId = genre2.Id, PageCount = 320, Year = 2016, CostPrice = 100, SalePrice = 185, Stock = 8, IsActive = true },
        new Book { Title = "Елементал", AuthorId = author4.Id, PublisherId = pub2.Id, GenreId = genre2.Id, PageCount = 290, Year = 2014, CostPrice = 90, SalePrice = 165, Stock = 6, IsActive = true },
        new Book { Title = "Ключ", AuthorId = author4.Id, PublisherId = pub1.Id, GenreId = genre2.Id, PageCount = 270, Year = 2012, CostPrice = 85, SalePrice = 155, Stock = 5, IsActive = true },
        new Book { Title = "Тамплієр", AuthorId = author4.Id, PublisherId = pub2.Id, GenreId = genre2.Id, PageCount = 340, Year = 2018, CostPrice = 110, SalePrice = 195, Stock = 4, IsActive = true },

        new Book { Title = "Інтернат", AuthorId = author5.Id, PublisherId = pub1.Id, GenreId = genre2.Id, PageCount = 260, Year = 2017, CostPrice = 88, SalePrice = 160, Stock = 9, IsActive = true },
        new Book { Title = "Ворошиловград", AuthorId = author5.Id, PublisherId = pub2.Id, GenreId = genre2.Id, PageCount = 340, Year = 2015, CostPrice = 105, SalePrice = 190, Stock = 6, IsActive = true },
        new Book { Title = "Месопотамія", AuthorId = author5.Id, PublisherId = pub1.Id, GenreId = genre2.Id, PageCount = 230, Year = 2014, CostPrice = 80, SalePrice = 148, Stock = 5, IsActive = true },
        new Book { Title = "Антена", AuthorId = author5.Id, PublisherId = pub1.Id, GenreId = genre1.Id, PageCount = 180, Year = 2019, CostPrice = 70, SalePrice = 128, Stock = 8, IsActive = true },

        new Book { Title = "Дюна", AuthorId = author8.Id, PublisherId = pub1.Id, GenreId = genre5.Id, PageCount = 680, Year = 2021, CostPrice = 180, SalePrice = 320, Stock = 7, IsActive = true },
        new Book { Title = "Месія Дюни", AuthorId = author8.Id, PublisherId = pub1.Id, GenreId = genre5.Id, PageCount = 320, Year = 2022, CostPrice = 110, SalePrice = 200, Stock = 5, IsActive = true },
    };

            Books.AddRange(books);
            SaveChanges();
        }
    }
}