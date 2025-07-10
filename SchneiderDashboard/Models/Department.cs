using System;
using System.Collections.Generic;

namespace SchneiderDashboard.Models;

public partial class Department
{
    public string DepartmentCode { get; set; } = null!;

    public string? DepartmentDesc { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
