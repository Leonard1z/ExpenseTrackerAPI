using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Repositories.UserAccount
{
    public interface IUserAccountRepository : IGenericRepository<User>
    {
         Task<User> GetByEmailAsync(string email);
    }
}
