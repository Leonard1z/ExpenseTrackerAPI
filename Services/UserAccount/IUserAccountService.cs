using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.UserAccount;
using Domain.Entities;

namespace Services.UserAccount
{
    public interface IUserAccountService
    {
        Task<User> RegisterUserAsync(UserRegisterDto userRegisterDto);
        Task<string> AuthenticateUserAsync(LoginDto loginDto);
    }
}
