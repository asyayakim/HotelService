using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelService.Db.Model;
using Microsoft.IdentityModel.Tokens;

namespace HotelService.Logic;

public class AuthService
{
    public object GenerateJwtToken(User user)
    {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A576E5A7234753777217A25432A462D4A614E645267556B5870327335763879"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "hotel-service",
                audience: "hotel-service-client",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        

    }
}