using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;

namespace EmployeeFormSample
{
    internal class Employee
    {
        private string? firstName;
        private string? lastName;
        private DateTime birthDate;
        private string? title;
        private EmployeePrefix prefix;
        private string? address;
        private string? city;
        private State state;
        private string? zipCode;
        private string? homePhone;
        private string? mobilePhone;
        private string? email;
        private string? skype;
        private Department department;
        private DateTime hireDate;
        private Status status;

        public string? FirstName
        {
            get => firstName;
            set => firstName = value;
        }
        
        public string? LastName
        {
            get => lastName;
            set => lastName = value;
        }

        public DateTime BirthDate
        {
            get => birthDate;
            set => birthDate = value;
        }

        public string? Title
        {
            get => title;
            set => title = value;
        }

        public EmployeePrefix Prefix
        {
            get => prefix;
            set => prefix = value;
        }

        public string? Address
        {
            get => address;
            set => address = value;
        }

        public string? City
        {
            get => city;
            set => city = value;
        }

        public State State
        {
            get => state;
            set => state = value;
        }

        public string? ZipCode
        {
            get => zipCode;
            set => zipCode = value;
        }

        public string? HomePhone
        {
            get => homePhone;
            set => homePhone = value;
        }

        public string? MobilePhone
        {
            get => mobilePhone;
            set => mobilePhone = value;
        }

        public string? Email
        {
            get => email;
            set => email = value;
        }

        public string? Skype
        {
            get => skype;
            set => skype = value;
        }

        public Department Department
        {
            get => department;
            set => department = value;
        }

        public DateTime HireDate
        {
            get => hireDate;
            set => hireDate = value;
        }

        public Status Status
        {
            get => status;
            set => status = value;
        }
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