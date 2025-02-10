using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.UserAccount
{
    public class UserExpenseDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public double TotalExpenses { get; set; }
    }
}
