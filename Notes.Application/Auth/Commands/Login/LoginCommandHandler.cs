using System.Windows.Input;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Notes.Application.Auth.Dtos;
using Notes.Application.Common.Models;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public LoginCommandHandler(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        IRefreshTokenService refreshTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
    }


    public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result<AuthResponseDto>.Failure("Invalid email or password");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                return Result<AuthResponseDto>.Failure("Account locked out");
            if (result.IsNotAllowed)
                return Result<AuthResponseDto>.Failure("Account not allowed to login");
            
            return Result<AuthResponseDto>.Failure("Invalid email or password");
        }

        var token = _tokenService.GenerateToken(user);
        
        var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id, request.IpAddress);

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(15), // Короткое время жизни
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName!,
                LastName = user.LastName!
            },
            RefreshToken = refreshToken.Token
        });
    }
}