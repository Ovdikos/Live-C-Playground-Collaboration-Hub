using Core.Entities;

namespace Core.Interfaces;

public interface ICodeSnippetRepository
{
    
    Task<IEnumerable<CodeSnippet>> GetAllAsync(Guid ownerId);
    
    
}