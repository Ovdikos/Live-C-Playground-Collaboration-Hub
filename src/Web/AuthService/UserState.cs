using Application.DTOs.UserDtos;

namespace Web.AuthService;

public class UserState
{
    private const string TokenKey = "token";
    private const string UserKey = "user";
    private readonly LocalStorageService _localStorage;

    public string? Token { get; private set; }
    public UserDto? User { get; private set; }
    public bool IsAuthenticated => Token != null && User != null;

    public UserState(LocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        Token = await _localStorage.GetItemAsync<string>(TokenKey);
        User = await _localStorage.GetItemAsync<UserDto>(UserKey);
    }

    public async Task SetAuthAsync(string token, UserDto user)
    {
        Token = token;
        User = user;
        await _localStorage.SetItemAsync(TokenKey, token);
        await _localStorage.SetItemAsync(UserKey, user);
    }

    public async Task LogoutAsync()
    {
        Token = null;
        User = null;
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(UserKey);
    }
}