﻿using DotNetAPI2.Dtos;
using DotNetAPI2.Models;
using DotNetAPI2.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI2.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BlogPostsController : ControllerBase
  {
    private readonly IBlogPostRepository _blogPostRepository;


    public BlogPostsController(IBlogPostRepository blogPostRepository)
    {
      this._blogPostRepository = blogPostRepository;

    }

    //api/categories
    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts()
    {
      var categories = await _blogPostRepository.GetAllAsync();

      var response = new List<BlogPostDto>();
      // map domain to dto
      foreach (var blogPost in categories)
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
          IsVisible = blogPost.IsVisible
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

    //POST: {apibasurl}/api/blogposts
    [HttpPost]  
    public async Task<IActionResult> CreateBlogPost([FromBody] BlogPostCreateDto blogPostCreateDto)
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
        IsVisible = blogPostCreateDto.IsVisible
      };

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
    public async Task<IActionResult> EditBlogPost([FromRoute] Guid id, BlogPostUpdateDto blogPostUpdateDto)
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
