using GestionNutricionAuth.Infraestructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var response = await _userService.Login(userLoginDto);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDtoCreation userDtoCreation)
        {
            var response = await _userService.Register(userDtoCreation);

            return Ok(response);
        }
    }
}
