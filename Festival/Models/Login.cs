using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festival.Models;

public partial class Login
{
    [Key, Required]
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
