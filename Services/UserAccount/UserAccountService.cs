using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO.UserAccount;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Repositories.UserAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Security;

namespace Services.UserAccount
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserAccountService(IUserAccountRepository userAccountRepository, IMapper mapper, IConfiguration configuration)
        {
            _userAccountRepository = userAccountRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<string> AuthenticateUserAsync(LoginDto loginDto)
        {
            var user = await _userAccountRepository.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            bool passwordValid = PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash, user.PasswordSalt);

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var jwtToken = GenerateJwtToken(user);

            return jwtToken;
        }

        public async Task<User> RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            var newUser = _mapper.Map<User>(userRegisterDto);

            byte[] salt;
            var hashedPassword = PasswordHasher.HashPassword(userRegisterDto.Password, out salt);
            newUser.PasswordHash = hashedPassword;
            newUser.PasswordSalt = salt;

            var result = await _userAccountRepository.CreateAsync(newUser);

            return result;
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSecretKey = _configuration["Jwt:SecretKey"];
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
