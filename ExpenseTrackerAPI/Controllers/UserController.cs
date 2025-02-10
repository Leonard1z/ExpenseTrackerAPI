using Domain.DTO.UserAccount;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.UserAccount;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/userAccount")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IValidator<UserRegisterDto> _validator;

        public UserController(IUserAccountService userAccountService, IValidator<UserRegisterDto> validator)
        {
            _userAccountService = userAccountService;
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {

            var validatorResult = _validator.Validate(userRegisterDto);

            if (!validatorResult.IsValid)
            {
                var problemDetails = new HttpValidationProblemDetails(validatorResult.ToDictionary())
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed",
                    Detail = "One or more validation errors occurred",
                    Instance = "api/register"
                };

                return BadRequest(problemDetails);
            }

            var result = await _userAccountService.RegisterUserAsync(userRegisterDto);

            if (result != null)
            {
                return Ok("User registered successfully");
            }

            return BadRequest("Failed to create user");
        }
    }
}
