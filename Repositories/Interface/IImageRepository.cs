using DotNetAPI2.Models;

namespace DotNetAPI2.Repositories.Interface
{
  public interface IImageRepository
  {
    public Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

    public Task<IEnumerable<BlogImage>> GetAll();
  }
}
