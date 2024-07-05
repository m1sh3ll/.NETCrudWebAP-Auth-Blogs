using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace DotNetAPI2.Models
{
  public class BlogPost
  {

   [Key]
    public Guid Id { get; set; }
    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string Content { get; set; }

    public string FeaturedImageUrl { get; set; }

    public string UrlHandle { get; set; }

    public DateTime PublishedDate { get; set; }

    public string Author { get; set; }

    public bool IsVisible { get; set; }

    public ICollection<Category> Categories { get; set; } 

  }
}
