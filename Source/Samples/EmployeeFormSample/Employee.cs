using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;

namespace EmployeeFormSample
{
    internal class Employee
    {
        public Image? Image { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? BirthDate { get; set; }
        public string? Title { get; set; }
        public EmployeePrefix Prefix { get; set; }
    }

    enum EmployeePrefix
    {
        Mr,
        Ms,
        Mrs,
        Miss,
        Dr,
        Prof
    }
}