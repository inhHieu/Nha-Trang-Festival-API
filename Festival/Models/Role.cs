using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festival.Models;

public partial class Role
{
    [Key]
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<Users> Users { get; } = new List<Users>();
}
