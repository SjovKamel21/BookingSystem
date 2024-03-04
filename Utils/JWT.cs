using Booking_System.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Booking_System.Utils
{
    public static class JWT
    {
        public static string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("etellerandetvirkeliglangtgerne256bit"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: "tyrkiskkamel.biz",
              claims: new Claim[]
            { 
                new Claim("user", JsonSerializer.Serialize(userInfo)),
            },
              expires: DateTime.Now.AddDays(7),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
