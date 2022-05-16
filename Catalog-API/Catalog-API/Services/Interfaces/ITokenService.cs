using Catalog_API.Models;

namespace Catalog_API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string key, string issuer, string audience, User user);
    }
}
