using MediatR;
using Notes.Application.Common.Models;

namespace Notes.Application.Auth.Commands.RevokeToken;

public class RevokeTokenCommand : IRequest<Result<bool>>
{
    public string RefreshToken { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
}