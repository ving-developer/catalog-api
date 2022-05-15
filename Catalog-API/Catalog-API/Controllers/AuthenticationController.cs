using Catalog_API.Models;
using Catalog_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog_API.Controllers;

[ApiController]// Configure attribute routing requirement, ModelState validation, ModelBinding parameter inference (automatically adding [FromBody] to POST methods) and Automatic HTTP 400 responses.
[Route("[controller]")]//Sets controller route
public class AuthenticationController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public AuthenticationController(IConfiguration configuration, ITokenService tokenService)
    {
        _configuration = configuration;
        _tokenService = tokenService;
    }

    [HttpPost]
    [AllowAnonymous]// Will make this end-point usable by anonymous user
    public ActionResult Authenticate(User user)
    {
        if (user is null) return BadRequest("Invalid User");

        //Enter user credentials validation below, before generating tokenString
        var tokenString = _tokenService
                .GenerateToken(_configuration["Jwt:Key"],
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            user);
        return Ok(new { token = tokenString });
    }
}
