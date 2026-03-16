using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINALPROJ.Data.Entity
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
