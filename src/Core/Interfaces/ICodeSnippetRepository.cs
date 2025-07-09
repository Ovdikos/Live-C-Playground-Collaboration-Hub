using Core.Entities;

namespace Core.Interfaces;

public interface ICodeSnippetRepository
{
    
    Task<IEnumerable<CodeSnippet>> GetAllAsync(Guid ownerId);
    
    Task<CodeSnippet?> GetByIdAsync(Guid id);
    
    Task AddAsync(CodeSnippet snippet);
    
    Task UpdateAsync(CodeSnippet snippet);
    
    Task DeleteAsync(Guid id);
    
}