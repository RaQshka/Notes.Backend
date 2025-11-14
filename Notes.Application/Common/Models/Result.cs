namespace Notes.Application.Common.Models;

public class Result<T>
{
    public bool Succeeded { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; }
    
    public static Result<T> Success(T data) => new() { Succeeded = true, Data = data };
    public static Result<T> Failure(List<string> errors) => new() { Succeeded = false, Errors = errors };
    public static Result<T> Failure(string error) => new() { Succeeded = false, Errors = new List<string> { error } };
}