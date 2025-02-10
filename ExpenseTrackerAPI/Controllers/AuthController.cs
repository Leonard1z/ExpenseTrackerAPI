using Domain.DTO.UserAccount;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.UserAccount;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IValidator<LoginDto> _validator;

        public AuthController(IUserAccountService userAccountService, IValidator<LoginDto> validator)
        {
            _userAccountService = userAccountService;
            _validator = validator;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var validatorResult = _validator.Validate(loginDto);

                if (!validatorResult.IsValid)
                    return BadRequest(validatorResult.Errors);

                var token = await _userAccountService.AuthenticateUserAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Message = "Invalid credentials." });
            }
        }
    }
}
