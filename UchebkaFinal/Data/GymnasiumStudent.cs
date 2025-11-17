using System;
using System.Collections.Generic;

namespace UchebkaFinal.Data;

public partial class GymnasiumStudent
{
    public int? Id { get; set; }

    public string? Surname { get; set; }

    public string? Subject { get; set; }

    public string? School { get; set; }

    public double? Points { get; set; }
}
