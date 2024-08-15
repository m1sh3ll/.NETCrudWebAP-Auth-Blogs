using DotNetAPI2.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI2.Repositories.Interface
{
  public interface ICategoryRepository
  {
    public Task<Category> CreateAsync(Category category);

    public Task<IEnumerable<Category>>GetAllAsync(
    string? query = null, 
    string? sortBy = null, 
    string? sortDirection = null,
    int? pageNumber = 1,
    int? pageSize=15);

    public Task<Category?> GetById(Guid id);

    public Task<Category?> UpdateAsync(Category category);

    public Task<Category?> DeleteAsync(Guid id);

    public Task<int> GetCount();

  }
}
