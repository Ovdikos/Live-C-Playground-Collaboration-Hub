using Core.Entities;

namespace Core.Interfaces;

public interface IAdminRepository
{
    Task<List<User>> GetUsersAsync(
        string? search = null,
        DateTime? createdFrom = null,
        DateTime? createdTo = null,
        bool? isAdmin = null,
        string? orderBy = null,
        bool desc = false
    );

    public Task<User?> GetUserDetailsByUsernameAsync(string username);

    public Task<bool> SetUserBlockedAsync(Guid userId, bool blocked, string? blockedByEmail);

    public Task<bool> DeleteUserAsync(Guid userId);


}