using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINALPROJ.Data.Entity
{
    public class WriteOffBook
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public int Quantity { get; set; }
        public string Reason { get; set; } = "";
        public DateTime WrittenOffAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
    }
}
