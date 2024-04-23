using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festival.Models;

public partial class NewsPut
{
    [Key]
    public int NewsId { get; set; }

    public int CategoryId { get; set; }

    public string NewsTitle { get; set; } = null!;

    public string NewsContent { get; set; } = null!;

    public DateTime PostedDate { get; set; }

    public int Views { get; set; }

    public string Summary { get; set; } = null!;

    public string TitleImg { get; set; } = null!;

    //public virtual Categories Category { get; set; } = null!;
}
