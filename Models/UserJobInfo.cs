using System.ComponentModel.DataAnnotations;

namespace DotNetAPI2.Models
{
  public partial class UserJobInfo  
  {
  [Key]
    public int UserId { get; set; }

    public string JobTitle { get; set; }

    public string Department { get; set; }
   

    public UserJobInfo()
    {
      if (JobTitle == null)
      {
        JobTitle = "";
      }
      if (Department == null)
      {
        Department = "";
      }
      
    }
  }
}
