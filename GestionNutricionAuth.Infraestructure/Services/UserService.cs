using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GestionNutricionAuth.Core.Entities;
using GestionNutricionAuth.Core.Handlers;
using Microsoft.Extensions.Configuration;

namespace GestionNutricionAuth.Infraestructure.Services
{
    public interface IUserService 
    {
        Task<UserTokenDto> Login(UserLoginDto userLoginDto);
        Task<bool> Register(UserDtoCreation userDtoCreation);
    }

    public class BaseUserDto
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserDto : BaseUserDto
    {
        public int Id { get; set; }
    }

    public class UserTokenDto
    {
        public DateTime ExpirationDate { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class UserDtoCreation : BaseUserDto { }

    public class UserService: IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserHandler _userHandler;
        public UserService(IConfiguration configuration, IUserHandler userHandler)
        {
            _configuration = configuration;
            _userHandler = userHandler;
        }

        public async Task<UserTokenDto> Login(UserLoginDto userLoginDto)
        {
            if (userLoginDto.Username == null || userLoginDto.Password == null) {
                throw new Exception("Ingrese las credenciales completas");
            }

            User user = await _userHandler.GetUserByUsername(userLoginDto.Username);
            
            if (user == null) throw new Exception("El usuario no se encuentra en el sistema");

            Check(user.Password, userLoginDto.Password);

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Password = user.Password,
                Email = user.Email,
                FullName = user.Username,
                Username = user.Username
            };

            var usuarioTokenDto = GenerateToken(userDto);

            return usuarioTokenDto;
        }
        public async Task<bool> Register(UserDtoCreation userDto)
        {
            if (!IsValidUser(userDto))
            {
                return false;
            }

            User user = new User()
            {
                Password = Hash(userDto.Password),
                Email = userDto.Email,
                FullName = userDto.FullName,
                Username = userDto.Username
            };

            await _userHandler.CreateUser(user);

            return true;
        }

        private bool IsValidUser(UserDtoCreation userDto)
        {
            return !userDto.Password.IsNullOrEmpty() && !userDto.Email.IsNullOrEmpty() && !userDto.Username.IsNullOrEmpty();
        }
        private UserTokenDto GenerateToken(UserDto userDto)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtIssuerOptions:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            var claims = new List<Claim>
            {
                new Claim("user_id", userDto.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userDto.Username),
                new Claim(JwtRegisteredClaimNames.GivenName, userDto.FullName),
                new Claim(JwtRegisteredClaimNames.Email, userDto.Email),
                new Claim(ClaimTypes.Role, "Administrador")
            };

            var expiration = DateTime.UtcNow.AddDays(3);

            var payload = new JwtPayload(
                _configuration["JwtIssuerOptions:Issuer"],
                _configuration["JwtIssuerOptions:Audience"],
                claims,
                DateTime.Now,
                expiration
            );

            return new UserTokenDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(header, payload)),
                ExpirationDate = expiration,
                Email = userDto.Email,
                Username = userDto.Username,
                FullName = userDto.FullName,
            };
        }

        private bool Check(string hash, string password)
        {
            string hashed = Hash(password);

            if (hash != hashed)
                throw new Exception("Usuario y / o contraseña incorrecto / s");
            
            return true;
        }

        private string Hash(string password)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                SHA256 sha256 = SHA256.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha256.ComputeHash(encoding.GetBytes(password));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                var hashed = sb.ToString();
                return hashed;
            }
        }
    }
}
