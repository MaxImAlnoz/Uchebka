using System;
using System.Collections.Generic;

namespace UchebkaFinal.Data;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Capital { get; set; }

    public int? Area { get; set; }

    public int? Population { get; set; }

    public string Continent { get; set; } = null!;
}
