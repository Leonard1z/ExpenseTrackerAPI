using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ExpenseDto
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
