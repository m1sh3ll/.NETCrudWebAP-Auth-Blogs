using DotNetAPI2.Models;

namespace DotNetAPI2.Dtos
{
  public class CategoryDto
  {
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string UrlHandle { get; set; }
    public List<BlogPost> BlogPosts { get; set; }
  }
}
