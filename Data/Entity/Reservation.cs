using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINALPROJ.Data.Entity
{
    public class Reservation
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public string CustomerName { get; set; } = "";
        public string? CustomerPhone { get; set; }
        public DateTime ReservedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
