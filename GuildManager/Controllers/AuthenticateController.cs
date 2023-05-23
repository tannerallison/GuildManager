using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuildManager.Data;
using GuildManager.Models;
using GuildManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuildManager.Controllers;

[Route("")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticateController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }


    // POST: login
    [HttpPost("login")]
    public Task<ActionResult<AuthenticationResponse>> Login([FromBody] AuthenticationRequest request)
    {
        var response = _authenticationService.Authenticate(request);
        return Task.FromResult<ActionResult<AuthenticationResponse>>(response);
    }

    // POST: register
    [HttpPost("register")]
    public Task<ActionResult<AuthenticationResponse>> Register([FromBody] AuthenticationRequest request)
    {
        var response = _authenticationService.Register(request);
        return Task.FromResult<ActionResult<AuthenticationResponse>>(response);
    }
}
