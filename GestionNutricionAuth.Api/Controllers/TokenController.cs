using GestionNutricionAuth.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionNutricionAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService) 
        { 
            _tokenService = tokenService;
        }
        [HttpPost]
        public IActionResult Authentication(UserDtoCreation loginInfo)
        {
        }
    }
}
