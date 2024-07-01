using Microsoft.AspNetCore.Identity;

namespace DotNetAPI2.Models
{
  public class ApplicationUser: IdentityUser
  {
  //were extending the default Identity user and adding a name field
  //because the default identity user doesnt have a name field by default
    public string Name { get; set; }
  }
}
