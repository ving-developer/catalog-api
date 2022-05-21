using Catalog_API.Models;
using Catalog_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Catalog_API.Controllers.V2;

/// <summary>
/// Authentication manager
/// </summary>
[EnableCors("EnableApiGet")]//Adds EnableApiGet pollicy defined in Program.cs in all methods of controller
[ApiVersion("2.0")]//maps the API version to this Controller's endpoints
[ApiController]// Configure attribute routing requirement, ModelState validation, ModelBinding parameter inference (automatically adding [FromBody] to POST methods) and Automatic HTTP 400 responses.
[Route("api/v{v:apiVersion}/[controller]")]//Sets controller route
public class AuthenticationController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public AuthenticationController(IConfiguration configuration, ITokenService tokenService)
    {
        _configuration = configuration;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Verify if API is listening
    /// </summary>
    [HttpGet]
    [AllowAnonymous]// Will make this end-point usable by anonymous user
    public ActionResult GetState()
    {
        return Ok($"Catalog Api is listening at {DateTime.Now}");
    }

    /// <summary>
    /// Receives any UserName and Password and generates a Token to be added in Authorization header
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Authentication
    ///     {
    ///        "UserName": "any string",
    ///        "Password": "any string"
    ///     }
    ///
    /// </remarks>
    /// <param name="user"></param>
    /// <returns>Bearer Token</returns>
    /// <response code="200">Returns the Json Web Token</response>
    /// <response code="400">If the user is null</response>  
    [HttpPost]
    [AllowAnonymous]// Will make this end-point usable by anonymous user
    [Produces("application/json")]
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
