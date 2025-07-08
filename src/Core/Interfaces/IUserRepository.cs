using Core.Entities;

namespace Core.Interfaces;

public interface IUserRepository
{
    
    Task<bool> ExistsByUsernameOrEmail(string username, string email);
    Task AddAsync(User user);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(Guid id);
    
    
    // For admin
    Task<List<User>> GetAllAsync();
    
}