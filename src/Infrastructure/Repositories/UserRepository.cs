using Core.Entities;
using Core.Interfaces;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    
    private readonly LivePlaygroundDbContext _context;

    public UserRepository(LivePlaygroundDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByUsernameOrEmail(string username, string email) => 
        await _context.Users.AnyAsync(u => u.Username == username && u.Email == email);
    

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
}