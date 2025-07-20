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
    
    Task<List<CodeSnippet>> GetAllSnippetsAsync(bool? isPublic = null);

    public Task<List<CollabSession>> GetAllSessionsAsync(
        string? search = null,
        bool? isActive = null,
        DateTime? createdFrom = null,
        DateTime? createdTo = null,
        int? minParticipants = null);
    
    Task<CodeSnippet?> GetSnippetByTitleAsync(string title);

    Task<bool> UpdateSnippetAsync(CodeSnippet snippet);
    
    Task<bool> DeleteSnippetAsync(Guid snippetId);
    
    
    Task<CollabSession?> GetByIdAsync(Guid id);
    Task<CollabSession?> GetByNameAsync(string name);
    Task<bool> UpdateAsync(CollabSession session);
    Task<bool> DeleteSessionByNameAsync(string name);

}