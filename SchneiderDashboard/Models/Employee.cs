using System;
using System.Collections.Generic;

namespace SchneiderDashboard.Models;

public partial class Employee
{
    public string SesaId { get; set; } = null!;

    public string? Name { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? GenderCode { get; set; }

    public string? DepartmentCode { get; set; }

    public virtual Department? DepartmentCodeNavigation { get; set; }

    public virtual Gender? GenderCodeNavigation { get; set; }
}
