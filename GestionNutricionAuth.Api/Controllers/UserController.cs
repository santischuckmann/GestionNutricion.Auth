using GestionNutricionAuth.Infraestructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GestionNutricionAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;
        public UserController(IUserService userService) 
        { 
            _userService = userService;
        }
        [HttpPost("Login", Name = nameof(Login))]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(UserTokenDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var response = await _userService.Login(userLoginDto);

            return Ok(response);
        }

        [HttpPost("Register", Name = nameof(Register))]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(UserDtoCreation userDtoCreation)
        {
            var response = await _userService.Register(userDtoCreation);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("Check", Name = "Check")]
        public IActionResult Check()
        {
            return Ok("Autorizado");
        }
    }
}
