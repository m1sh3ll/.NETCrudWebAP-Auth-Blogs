using Azure.Core;
using DotNetAPI2.Dtos;
using DotNetAPI2.Models;
using DotNetAPI2.Repositories.Implementation;
using DotNetAPI2.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DotNetAPI2.Controllers
{


  [Route("api/[controller]")]
  [ApiController]
  public class BlogPostsController : ControllerBase
  {
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ICategoryRepository _categoryRepository;

    public BlogPostsController(IBlogPostRepository blogPostRepository,
    ICategoryRepository categoryRepository)
    {
      this._blogPostRepository = blogPostRepository;
      this._categoryRepository = categoryRepository;

    }



    //POST: {apibasurl}/api/blogposts
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto blogPostCreateDto)
    {
      // Convert DTO to Domain Model
      var blogPost = new BlogPost
      {
        Title = blogPostCreateDto.Title,
        ShortDescription = blogPostCreateDto.ShortDescription,
        Content = blogPostCreateDto.Content,
        FeaturedImageUrl = blogPostCreateDto.FeaturedImageUrl,
        UrlHandle = blogPostCreateDto.UrlHandle,
        PublishedDate = blogPostCreateDto.PublishedDate,
        Author = blogPostCreateDto.Author,
        IsVisible = blogPostCreateDto.IsVisible,
        Categories = new List<Category>()
      };



      foreach (var categoryGuid in blogPostCreateDto.Categories)
      {
        var existingCategory = await _categoryRepository.GetById(categoryGuid);
        if (existingCategory is not null)
        {
          blogPost.Categories.Add(existingCategory);
        }
      }

      blogPost = await _blogPostRepository.CreateAsync(blogPost);

      //Domain model to Dto
      var response = new BlogPostDto
      {
        Id = blogPost.Id,
        Title = blogPost.Title,
        ShortDescription = blogPost.ShortDescription,
        Content = blogPost.Content,
        FeaturedImageUrl = blogPost.FeaturedImageUrl,
        UrlHandle = blogPost.UrlHandle,
        PublishedDate = blogPost.PublishedDate,
        Author = blogPost.Author,
        IsVisible = blogPost.IsVisible,
        Categories = blogPost.Categories.Select(x => new CategoryDto
        {
          Id = x.Id,
          Name = x.Name,
          UrlHandle = x.UrlHandle
        }).ToList()
      };
      return Ok(response);
    }


    //GET: {apibaseurl}/api/blogposts
    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts()
    {
      var blogPosts = await _blogPostRepository.GetAllAsync();

      //Convert Domain Model to DTO
      var response = new List<BlogPostDto>();

      foreach (var blogPost in blogPosts)
      {
        response.Add(new BlogPostDto
        {
          Id = blogPost.Id,
          Title = blogPost.Title,
          ShortDescription = blogPost.ShortDescription,
          Content = blogPost.Content,
          FeaturedImageUrl = blogPost.FeaturedImageUrl,
          UrlHandle = blogPost.UrlHandle,
          PublishedDate = blogPost.PublishedDate,
          Author = blogPost.Author,
          IsVisible = blogPost.IsVisible,
          Categories = blogPost.Categories.Select(x => new CategoryDto
          {
            Id = x.Id,
            Name = x.Name,
            UrlHandle = x.UrlHandle
          }).ToList()
        });
      }
      return Ok(response);
    }



    //api/blogposts/{guid}}
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
    {
      var blogPost = await _blogPostRepository.GetById(id);

      if (blogPost is null)
      {
        return NotFound();
      }

      var response = new BlogPostDto
      {
        Id = blogPost.Id,
        Title = blogPost.Title,
        ShortDescription = blogPost.ShortDescription,
        Content = blogPost.Content,
        FeaturedImageUrl = blogPost.FeaturedImageUrl,
        UrlHandle = blogPost.UrlHandle,
        PublishedDate = blogPost.PublishedDate,
        Author = blogPost.Author,
        IsVisible = blogPost.IsVisible
      };
      return Ok(response);
    }

    //api/blogposts/{id}
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
    {
      var blogPost = await _blogPostRepository.DeleteAsync(id);
      if (blogPost is null)
      {
        return NotFound();
      }
      //convert domain to dto
      var response = new BlogPostDto
      {
        Id = blogPost.Id,
        Title = blogPost.Title,
        ShortDescription = blogPost.ShortDescription,
        Content = blogPost.Content,
        FeaturedImageUrl = blogPost.FeaturedImageUrl,
        UrlHandle = blogPost.UrlHandle,
        PublishedDate = blogPost.PublishedDate,
        Author = blogPost.Author,
        IsVisible = blogPost.IsVisible

      };

      return Ok(response);

    }


    //api/blogposts/{id}
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> EditBlogPost([FromRoute] Guid id, UpdateBlogPostRequestDto blogPostUpdateDto)
    {
      //convert dto to Domain model
      var blogPost = new BlogPost
      {
        Id = id,
        Title = blogPostUpdateDto.Title,
        ShortDescription = blogPostUpdateDto.ShortDescription,
        Content = blogPostUpdateDto.Content,
        FeaturedImageUrl = blogPostUpdateDto.FeaturedImageUrl,
        UrlHandle = blogPostUpdateDto.UrlHandle,
        PublishedDate = blogPostUpdateDto.PublishedDate,
        Author = blogPostUpdateDto.Author,
        IsVisible = blogPostUpdateDto.IsVisible
      };

      blogPost = await _blogPostRepository.UpdateAsync(blogPost);

      if (blogPost is null)
      {
        return NotFound();
      }

      //convert domain model to DTO
      var response = new BlogPostDto
      {
        Id = id,
        Title = blogPostUpdateDto.Title,
        ShortDescription = blogPostUpdateDto.ShortDescription,
        Content = blogPostUpdateDto.Content,
        FeaturedImageUrl = blogPostUpdateDto.FeaturedImageUrl,
        UrlHandle = blogPostUpdateDto.UrlHandle,
        PublishedDate = blogPostUpdateDto.PublishedDate,
        Author = blogPostUpdateDto.Author,
        IsVisible = blogPostUpdateDto.IsVisible
      };


      return Ok(response);


    }

  }
}
