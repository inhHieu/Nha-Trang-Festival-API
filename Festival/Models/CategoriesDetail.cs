using System.ComponentModel.DataAnnotations;

namespace Festival.Models;

public partial class CategoriesDetail
{
    [Key]
    public int Category_Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategoryDescription { get; set; } = null!;
    public string Image { get; set; } = null!;

    public int TotalNews { get; set; }
    public int TotalEvents { get; set; }
    public int TotalSubscribers { get; set; }



}
