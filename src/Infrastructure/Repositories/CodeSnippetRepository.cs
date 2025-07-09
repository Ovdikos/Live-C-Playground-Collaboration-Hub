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

    public async Task AddAsync(CodeSnippet snippet)
    {
        await _context.CodeSnippets.AddAsync(snippet);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CodeSnippet snippet)
    {
        _context.CodeSnippets.Update(snippet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
     
        var snippet = await _context.CodeSnippets.FindAsync(id);
        if (snippet != null)
        {
            _context.CodeSnippets.Remove(snippet);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException("Snippet not found");
        }
        
    }
}