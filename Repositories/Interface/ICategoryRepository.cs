using DotNetAPI2.Models;

namespace DotNetAPI2.Repositories.Interface
{
  public interface ICategoryRepository
  {
    public Task<Category> CreateAsync(Category category);

    public Task<IEnumerable<Category>>GetAllAsync(string? query = null, string? sortBy = null, string? sortDirection = null);

    public Task<Category?> GetById(Guid id);

    public Task<Category?> UpdateAsync(Category category);

    public Task<Category?> DeleteAsync(Guid id);
  }
}
