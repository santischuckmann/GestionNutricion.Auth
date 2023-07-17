using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GestionNutricionAuth.Core.Entities

namespace GestionNutricionAuth.Api.Services
{
    public interface ITokenService 
    {
        string GenerateToken();
        bool IsValidUser(UserDtoCreation loginInfo);
    }

    public class BaseUserDto
    {
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
    }

    public class UserDto : BaseUserDto
    {
        public int Id { get; set; }
    }


    public class UserDtoCreation : BaseUserDto { }

    public class TokenService: ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool CreateUser(UserDtoCreation userDto)
        {
            if (!IsValidUser(userDto))
            {
                return false;
            }

            User user = new User()
            {
                Clave = Hash(userDto.Clave),
                Email = userDto.Email,
                NombreCompleto = userDto.NombreCompleto,
                NombreUsuario = userDto.NombreUsuario
            };

            if (IsValidUser(userDto))
            {
                var token = GenerateToken();
                return Ok(new { token });
            }

            return NotFound();
        }

        public Boolean IsValidUser(UserDtoCreation userDto)
        {
            return !loginInfo.Clave.IsNullOrEmpty() && !loginInfo.Email.IsNullOrEmpty() && !loginInfo.NombreUsuario.IsNullOrEmpty();
        }
        public string GenerateToken()
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtIssuerOptions:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, "Nuevo"),
                new Claim(JwtRegisteredClaimNames.Email, "schuckmannsantiago@gmail.com"),
                new Claim(ClaimTypes.Role, "Administrador"),
            };

            var expiration = DateTime.UtcNow.AddDays(3);

            var payload = new JwtPayload(
                _configuration["JwtIssuerOptions:Issuer"],
                _configuration["JwtIssuerOptions:Audience"],
                claims,
                DateTime.Now,
                expiration
            );

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(header, payload)); 
        }

        public string Hash(string password)
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
