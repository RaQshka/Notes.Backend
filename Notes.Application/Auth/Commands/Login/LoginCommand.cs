using MediatR;
using Notes.Application.Auth.Dtos;
using Notes.Application.Common.Models;

namespace Notes.Application.Auth.Commands.Login;

public class LoginCommand : IRequest<Result<AuthResponseDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
}
