using AutoMapper;
using Notes.Application.Auth.Commands.Login;
using Notes.Application.Common.Mappings;

namespace Notes.Application.Auth.Dtos;

public class LoginDto 
{
    public string Username { get; set; }
    public string Password { get; set; }
    
}