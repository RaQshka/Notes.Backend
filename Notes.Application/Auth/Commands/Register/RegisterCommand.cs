using MediatR;
using Notes.Application.Auth.Dtos;
using Notes.Application.Common.Models;

namespace Notes.Application.Auth.Commands.Register;

public class RegisterCommand : IRequest<Result<AuthResponseDto>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
