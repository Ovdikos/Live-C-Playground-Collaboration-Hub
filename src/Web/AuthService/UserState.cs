using Application.DTOs;


public class UserState
{
    public string? Token { get; private set; }
    public UserDto? User { get; private set; }
    public bool IsAuthenticated => Token != null && User != null;

    public void SetAuth(string token, UserDto user)
    {
        Token = token;
        User = user;
    }

    public void Logout()
    {
        Token = null;
        User = null;
    }
}