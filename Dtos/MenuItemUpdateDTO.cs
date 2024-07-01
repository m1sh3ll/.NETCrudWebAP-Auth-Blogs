using System.ComponentModel.DataAnnotations;

namespace DotNetAPI2.Dtos
{
  public class MenuItemUpdateDTO
  {
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public string SpecialTag { get; set; }
    public string Category { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid price")]
    public double Price { get; set; }
    public string Image { get; set; }
  }
}
