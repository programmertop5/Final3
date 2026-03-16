using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FINALPROJ.Data.Entity
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; } = null!;
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public int PageCount { get; set; }
        public int Year { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public int Stock { get; set; }
        public bool IsSequel { get; set; }
        public string? SequelNote { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime AddedAt { get; set; } = DateTime.Now;

        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<WriteOffBook> WriteOffs { get; set; } = new List<WriteOffBook>();
    }

}
