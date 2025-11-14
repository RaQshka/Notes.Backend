using MediatR;
using Microsoft.AspNetCore.Identity;
using Notes.Application.Common.Models;
using Notes.Domain;

namespace Notes.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    private readonly UserManager<User> _userManager;

    public ChangePasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result<bool>.Failure("User not found");

        var result = await _userManager.ChangePasswordAsync(
            user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Result<bool>.Failure(errors);
        }

        return Result<bool>.Success(true);
    }
}