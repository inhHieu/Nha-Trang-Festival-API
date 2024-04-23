using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festival.Models;

public partial class Users
{
    [Key]
    public int User_ID { get; set; }

    public int RoleId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Sex { get; set; }

    public DateTime Age { get; set; }

    public string? Avatar { get; set; }

    //public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Subscribed> Subscribeds { get; } = new List<Subscribed>();
}
