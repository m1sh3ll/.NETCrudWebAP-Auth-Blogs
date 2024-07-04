using DotNetAPI2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPI2.Data
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }   

    public DbSet<Category> Categories { get; set; }

    public DbSet<BlogPost> BlogPosts { get; set; }  
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserSalary> UserSalary { get; set; }
    public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
     

    }


  }
}
