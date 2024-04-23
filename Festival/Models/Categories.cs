using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festival.Models;

public partial class Categories
{
    [Key]
    public int Category_Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategoryDescription { get; set; } = null!;

    public string Image { get; set; } = null!;

    public virtual ICollection<Events> Events { get; } = new List<Events>();

    public virtual ICollection<News> News { get; } = new List<News>();
}
