using System;
using System.Collections.Generic;

namespace UchebkaFinal.Data;

public partial class UserAccount
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual UserRole Role { get; set; } = null!;
}
