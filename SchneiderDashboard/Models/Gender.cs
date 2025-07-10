using System;
using System.Collections.Generic;

namespace SchneiderDashboard.Models;

public partial class Gender
{
    public string GenderCode { get; set; } = null!;

    public string? GenderDesc { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
