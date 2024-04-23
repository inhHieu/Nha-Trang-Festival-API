using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festival.Models;

public partial class EventsDetail
{
    [Key]
    public int EventId { get; set; }

    public int CategoryId { get; set; }

    public DateTime DateStart { get; set; }

    public string TakePlace { get; set; } = null!;

    public string EventDescription { get; set; } = null!;

    public string EventName { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int TotalSub { get; set; }

    public virtual Categories Category { get; set; } = null!;

    public virtual ICollection<Subscribed> Subscribeds { get; } = new List<Subscribed>();
}
