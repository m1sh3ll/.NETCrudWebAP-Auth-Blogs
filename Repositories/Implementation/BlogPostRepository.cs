using DotNetAPI2.Data;
using DotNetAPI2.Models;
using DotNetAPI2.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPI2.Repositories.Implementation
{
  public class BlogPostRepository : IBlogPostRepository
  {

    private readonly ApplicationDbContext _db;


    //Inject the DbContext
    public BlogPostRepository(ApplicationDbContext db)
    {
      this._db = db;
    }


    //Add A Post
    public  async Task<BlogPost> CreateAsync(BlogPost blogPost)
    {
      await _db.BlogPosts.AddAsync(blogPost);
      await _db.SaveChangesAsync();

      return blogPost; //return the domain model
    }


    public async Task<BlogPost?> DeleteAsync(Guid id)
    {
      var existingBlogPost = await _db.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

      if (existingBlogPost is null)
      {
        return null;

      }
      _db.Remove(existingBlogPost);
      await _db.SaveChangesAsync();
      return existingBlogPost;
    }



    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
      //return await _db.BlogPosts.ToListAsync();
      return await _db.BlogPosts.Include(x=>x.Categories).ToListAsync();
    }


    public async Task<BlogPost?> GetById(Guid id)
    {
      return await _db.BlogPosts.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
      var existingBlogPost = await _db.BlogPosts.FirstOrDefaultAsync(x => x.Id == blogPost.Id);

      if (existingBlogPost is not null)
      {
        _db.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
        //same as this
        //existingBlogPost.Name = blogPost.Name;
        //existingBlogPost.UrlHandle = blogPost.UrlHandle;
        await _db.SaveChangesAsync();
        return blogPost;
      }

      return null;
    }
  }
}
