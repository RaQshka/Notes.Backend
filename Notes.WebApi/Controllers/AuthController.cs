using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Auth.Commands.ChangePassword;
using Notes.Application.Auth.Commands.Login;
using Notes.Application.Auth.Commands.RefreshToken;
using Notes.Application.Auth.Commands.Register;
using Notes.Application.Auth.Commands.RevokeToken;
using Notes.Application.Auth.Dtos;
using Notes.Domain;

namespace Notes.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand request)
    {
        var result = await _mediator.Send(request);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }   
        return Ok(result.Data);
    }
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand request)
    {
        var result = await _mediator.Send(request);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }   
        return Ok(result.Data);
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ChangePasswordCommand request)
    {
        var result = await _mediator.Send(request);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }   
        return Ok(result.Data);
    }
    
    [HttpPost("refresh-token")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken(RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
    
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Data);
    }

    [Authorize]
    [HttpPost("revoke-token")]
    public async Task<ActionResult> RevokeToken(RevokeTokenCommand command)
    {
        var result = await _mediator.Send(command);
    
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors });

        return Ok(new { message = "Token revoked successfully" });
    }

}