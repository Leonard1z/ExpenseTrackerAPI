using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ExpenseTrackerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options)
        {
        }
    }
}
