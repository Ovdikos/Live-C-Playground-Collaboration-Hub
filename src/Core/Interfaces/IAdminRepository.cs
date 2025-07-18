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
}