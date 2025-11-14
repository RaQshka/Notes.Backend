namespace Notes.Application.Auth.Dtos;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; } = null!;
    public string RefreshToken { get; set; } = string.Empty;
}

