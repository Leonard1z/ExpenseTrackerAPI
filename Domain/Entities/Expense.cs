using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
