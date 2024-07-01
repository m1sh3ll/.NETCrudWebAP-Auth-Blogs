using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetAPI2.Models
{

  [Table("Users")]
  public partial class User
  {
    [Key]
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public bool Active { get; set; }

    public User()
    {
      if (FirstName == null)
      {
        FirstName = "";
      }
      if (LastName == null)
      {
        LastName = "";
      }
      if (Email == null)
      {
        Email = "";
      }
      if (Gender == null)
      {
        Gender = "";
      }
    }
  }
}
