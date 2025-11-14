using MediatR;
using Notes.Application.Common.Models;

namespace Notes.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Result<bool>>
{
    public Guid UserId { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

