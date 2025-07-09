using Core.Entities;
using Core.Interfaces;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CodeSnippetRepository : ICodeSnippetRepository
{
    
    private readonly LivePlaygroundDbContext _context;

    public CodeSnippetRepository(LivePlaygroundDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<CodeSnippet>> GetAllAsync(Guid ownerId)
    {
        return await _context.CodeSnippets
            .Where(s => s.OwnerId == ownerId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<CodeSnippet?> GetByIdAsync(Guid id)
    {
        return await _context.CodeSnippets.
            Include(s => s.Owner).
            FirstOrDefaultAsync(s => s.Id == id);
    }
}