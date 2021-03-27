using MadPay724.Common.Helper.Interface;
using MadPay724.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MadPay724.Common.Helper
{
    public class Utilities : IUtilities
    {
        private readonly IConfiguration _config;

        public Utilities(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(User user, bool isRemember)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName)
            };
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDes = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = isRemember ? DateTime.Now.AddDays(1) : DateTime.Now.AddHours(2),
                SigningCredentials = creds
            };
            var tokenHandeler = new JwtSecurityTokenHandler();
            var token = tokenHandeler.CreateToken(tokenDes);
            return tokenHandeler.WriteToken(token);
        }
        public  void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public  bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i]!=passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
