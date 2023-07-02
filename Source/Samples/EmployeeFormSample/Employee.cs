using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;

namespace EmployeeFormSample
{
    internal class Employee
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? BirthDate { get; set; }
        public string? Title { get; set; }
        public EmployeePrefix Prefix { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public State State { get; set; }
        public string? ZipCode { get; set; }
        public string? HomePhone { get; set; }
        public string? MobilePhone { get; set; }
        public string? Email { get; set; }
        public string? Skype { get; set; }
        public Department Department { get; set; }
        public string? HireDate { get; set; }
        public Status Status { get; set; }
    }

    enum Status
    {
        Salaried,
        Terminated,
        OnLeave
    }

    enum Department
    {
        Sales,
        HR,
        Engineering,
        IT
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

    enum State
    {
        AL, AK, AZ, AR, CA, CO, CT, DE, DC, FL, GA, HI, ID, IL, IN, IA, KS, KY, LA, ME, MD, MA, MI, MN,
        MS, MO, MT, NE, NV, NH, NJ, NM, NY, NC, ND, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VT, VA, WA, WV, WI, WY
    }
}