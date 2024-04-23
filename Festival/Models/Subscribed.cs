using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festival.Models;

public partial class Subscribed
{
    [Key]
    public int SubscribedId { get; set; }

    public int UserId { get; set; }

    public int EventId { get; set; }

    public virtual Events Event { get; set; } = null!;

    public virtual Users User { get; set; } = null!;
}
