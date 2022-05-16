﻿using Catalog_API.Models;
using Catalog_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog_API.Controllers;

/// <summary>
/// Authentication manager
/// </summary>
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
