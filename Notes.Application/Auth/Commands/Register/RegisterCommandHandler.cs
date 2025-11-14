using MediatR;
using Microsoft.AspNetCore.Identity;
using Notes.Application.Auth.Dtos;
using Notes.Application.Common.Models;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public RegisterCommandHandler(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken ct)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return Result<AuthResponseDto>.Failure("User with this email already exists");

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Result<AuthResponseDto>.Failure(errors);
        }

        await _userManager.AddToRoleAsync(user, "User");

        var token = _tokenService.GenerateToken(user);

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(12),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName!,
                LastName = user.LastName!
            }
        });
    }
}