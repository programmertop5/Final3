using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINALPROJ.Data.Entity
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime SoldAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    }
}
