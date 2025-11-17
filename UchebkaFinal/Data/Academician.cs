using System;
using System.Collections.Generic;

namespace UchebkaFinal.Data;

public partial class Academician
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public DateOnly? Birthdate { get; set; }

    public string? Specialization { get; set; }

    public int? Titleyear { get; set; }
}
