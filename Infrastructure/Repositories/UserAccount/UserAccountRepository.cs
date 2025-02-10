using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.UserAccount
{
    public class UserAccountRepository : GenericRepository<User>, IUserAccountRepository
    {
        public UserAccountRepository(ExpenseTrackerDbContext context) : base(context)
        {
            
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
