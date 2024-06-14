using System;
using System.Collections.Generic;

namespace HTQLTV.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleID { get; set; }

    //public int AssociatedId { get; set; }
}
