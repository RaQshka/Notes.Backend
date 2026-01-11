using MediatR;
using Notes.Application.Auth.Dtos;
using Notes.Application.Common.Models;

namespace Notes.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<Result<RefreshTokenResponse>>
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

