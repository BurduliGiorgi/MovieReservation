using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieReservation.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservation.Infrastructure.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(TokenService config)
        {
            _config = config._config;
        }
        public string CreateAccsesToken(int userId, string email, string role)
        {
            //Claims = data stored in token
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
             };

            //2.Signing Key  - must match what's in appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //3. Create Token
            var token = new JwtSecurityToken(
                issuer: _config["Token:Issuer"],
                audience: _config["Token:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"]!)),
                signingCredentials: credentials
                );

            //4. Serialize To String
            return new JwtSecurityTokenHandler().WriteToken(token);


        }

        public string CreateRefreshToken()
        {
            //Refresh Token is a random string, not a JWT
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
