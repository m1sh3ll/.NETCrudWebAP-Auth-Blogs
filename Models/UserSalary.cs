using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetAPI2.Models
{
  public partial class UserSalary
  {
  [Key]
    public int UserId { get; set; }

    public double Salary { get; set; }

    public double AvgSalary { get; set; }  

    public UserSalary()
    {
      
    }
  }
}
