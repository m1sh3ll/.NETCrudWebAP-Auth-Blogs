using DotNetAPI2.Data;
using DotNetAPI2.Models;
using DotNetAPI2.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPI2.Repositories.Implementation
{
  public class CategoryRepository : ICategoryRepository
  {

    private readonly ApplicationDbContext _db;

    public CategoryRepository(ApplicationDbContext db)
    {
      this._db = db;
    }


    public async Task<Category> CreateAsync(Category category)
    {
      await _db.Categories.AddAsync(category);
      await _db.SaveChangesAsync();

      return category; //return the domain model
    }



    public async Task<IEnumerable<Category>> GetAllAsync()
    {
      return await _db.Categories.ToListAsync();

    }


    public async Task<Category?> GetById(Guid id)
    {
      return await _db.Categories.FirstOrDefaultAsync(u => u.Id == id);
    }


    public async Task<Category?> UpdateAsync(Category category)
    {

      var existingCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

      if (existingCategory is not null)
      {
        _db.Entry(existingCategory).CurrentValues.SetValues(category);
        //same as this
        //existingCategory.Name = category.Name;
        //existingCategory.UrlHandle = category.UrlHandle;
        await _db.SaveChangesAsync();
        return category;
      }

      return null;

    }


    public async Task<Category?> DeleteAsync(Guid id)
    {

      var existingCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

      if (existingCategory is null)
      {
        return null;

      }
      _db.Categories.Remove(existingCategory);
      await _db.SaveChangesAsync();
      return existingCategory;

    }

  }
}
