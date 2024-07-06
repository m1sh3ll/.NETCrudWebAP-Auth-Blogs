﻿using DotNetAPI2.Dtos;
using DotNetAPI2.Models;
using DotNetAPI2.Repositories.Implementation;
using DotNetAPI2.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI2.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ImagesController : ControllerBase
  {
    private readonly IImageRepository _imageRepository;

    public ImagesController(IImageRepository imageRepository)
    {
      this._imageRepository = imageRepository;
    }
    // GET: {apibaseURL}/api/Images
    [HttpGet]
    public async Task<IActionResult> GetAllImages()
    {
      // call image repository to get all images
      var images = await _imageRepository.GetAll();

      // Convert Domain model to DTO
      var response = new List<BlogImageDto>();
      foreach (var image in images)
      {
        response.Add(new BlogImageDto
        {
          Id = image.Id,
          Title = image.Title,
          DateCreated = image.DateCreated,
          FileExtension = image.FileExtension,
          FileName = image.FileName,
          Url = image.Url
        });
      }

      return Ok(response);
    }


    //POST {apibaseurl}/api/images
    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, 
    [FromForm] string fileName, [FromForm] string title) 
    {
      ValidateFileUpload(file);

      if (ModelState.IsValid)
      {
        // File upload
        var blogImage = new BlogImage
        {
          FileExtension = Path.GetExtension(file.FileName).ToLower(),
          FileName = fileName,
          Title = title,
          DateCreated = DateTime.Now
        };

        blogImage = await _imageRepository.Upload(file, blogImage);

        // Convert Domain Model to DTO
        var response = new BlogImageDto
        {
          Id = blogImage.Id,
          Title = blogImage.Title,
          DateCreated = blogImage.DateCreated,
          FileExtension = blogImage.FileExtension,
          FileName = blogImage.FileName,
          Url = blogImage.Url
        };

        return Ok(response);
      }

      return BadRequest(ModelState);






    }

    private void ValidateFileUpload(IFormFile file)
    {
      var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

      if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
      {
        ModelState.AddModelError("file", "Unsupported file format");
      }

      if (file.Length > 104857600)
      {
        ModelState.AddModelError("file", "File size cannot be more than 100MB");
      }
    }

  }
}
