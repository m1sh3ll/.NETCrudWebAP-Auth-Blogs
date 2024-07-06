using DotNetAPI2.Data;
using DotNetAPI2.Models;
using DotNetAPI2.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPI2.Repositories.Implementation
{
  public class ImageRepository : IImageRepository
  {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _db;

    public ImageRepository(
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext db)
    {
      this._webHostEnvironment = webHostEnvironment;
      this._httpContextAccessor = httpContextAccessor;
      this._db = db;
    }

    public async Task<IEnumerable<BlogImage>> GetAll()
    {
      return await _db.BlogImages.ToListAsync();
    }

    public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
    {
      // 1- Upload the Image to API/Images
      var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
      using var stream = new FileStream(localPath, FileMode.Create);
      await file.CopyToAsync(stream);

      // 2-Update the database
      // https://michellenesbitt.com/images/somefilename.jpg
      var httpRequest = _httpContextAccessor.HttpContext.Request;
      var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

      blogImage.Url = urlPath;

      await _db.BlogImages.AddAsync(blogImage);
      await _db.SaveChangesAsync();

      return blogImage;
    }
  }
}

