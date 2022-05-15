using Catalog_API.Models;
using Catalog_API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Catalog_API.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(string key, string issuer, string audience, User user)
        {
            //Claims are statements about the user of the token that will make up the JWT Payload
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };
            //Generates a secure key by encoding the key received in the key parameter
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            //Apply the HmacSha256 algorithm on top of the encrypted key, obtaining a symmetric key
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),//defines the expiration of this token, in this case it will expire in 5 minutes
                signingCredentials: credentials);
            //Deserializes the token into a string and returns it
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}
