using DotNetAPI2.Data;
using DotNetAPI2.Models;
using DotNetAPI2.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPI2.Repositories.Implementation
{
  public class CategoryRepository : ICategoryRepository
  {

    private readonly ApplicationDbContext _db;
    private const int _defaultPageSize = 15;

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



    public async Task<IEnumerable<Category>> GetAllAsync(
    string? query = null, 
    string? sortBy = null, 
    string? sortDirection = null,
    int? pageNumber = 1,
    int? pageSize= _defaultPageSize)
    {

      //Query
      var categories = _db.Categories.AsQueryable();

      // Filtering
      if (string.IsNullOrWhiteSpace(query) == false)
      {
        categories = categories.Where(x => x.Name.Contains(query));
      }

      // Sorting
      if (string.IsNullOrWhiteSpace(sortBy) == false)
      {
        if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
        {
          var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
              ? true : false;


          categories = isAsc ? categories.OrderBy(x => x.Name) : categories.OrderByDescending(x => x.Name);
        }

        if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
        {
          var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
              ? true : false;

          categories = isAsc ? categories.OrderBy(x => x.UrlHandle) : categories.OrderByDescending(x => x.UrlHandle);
        }
      }

      //Pagination 
      //Pagesize 5
      //Pagenumber 1 - skip 0, take 5
      //Pagenumber 2 - skip 5 take 5
      //Pagenumber 3 - skip 10 take 5
      var skipResults = (pageNumber - 1) * pageSize;

      categories = categories.Skip(skipResults ?? 0).Take(pageSize ?? _defaultPageSize); //skipResults defaults to 0, pageSize defaults to default

      return await categories.ToListAsync();

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
