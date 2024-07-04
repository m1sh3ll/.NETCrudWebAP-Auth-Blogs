﻿using DotNetAPI2.Models;

namespace DotNetAPI2.Repositories.Interface
{
  public interface IBlogPostRepository
  {
    public Task<BlogPost> CreateAsync(BlogPost blogpost);

    public Task<IEnumerable<BlogPost>> GetAllAsync();

    public Task<BlogPost?> GetById(Guid id);

    public Task<BlogPost?> UpdateAsync(BlogPost blogpost);

    public Task<BlogPost?> DeleteAsync(Guid id);
  }
}
