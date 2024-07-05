using DotNetAPI2.Models;

namespace DotNetAPI2.Repositories.Interface
{
  public interface IBlogPostRepository
  {
    public Task<BlogPost> CreateAsync(BlogPost blogPost);

    public Task<IEnumerable<BlogPost>> GetAllAsync();

    public Task<BlogPost?> GetByIdAsync(Guid id);

    public Task<BlogPost?> UpdateAsync(BlogPost blogPost);  

    public Task<BlogPost?> DeleteAsync(Guid id);
  }
}
