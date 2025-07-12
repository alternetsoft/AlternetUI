using System;
using System.ComponentModel;
using System.Collections;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;

namespace EmployeeFormSample
{
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow()
        {
            Icon = App.DefaultIcon;

            InitializeComponent();

            employeeFoto.Image = Image.FromAssemblyUrl(
                GetType().Assembly,
                "Resources.EmployeePhoto.jpg");

            prefixComboBox.EnumType = typeof(Employee.EmployeePrefix);
            stateComboBox.EnumType = typeof(Employee.States);
            departmentComboBox.EnumType = typeof(Employee.EmployeeDepartment);

            var employee = new Employee
            {
                FirstName = "Alice",
                LastName = "Jameson",
                BirthDate = new System.DateTime(1993, 10, 2),
                Title = "Customer Success Manager",
                Prefix = Employee.EmployeePrefix.Mrs,
                Address = "143 Coolidge St.",
                City = "Phoenix",
                State = Employee.States.AZ,
                ZipCode = "85001",
                HomePhone = "(341) 433-4377",
                MobilePhone = "(341) 232-6535",
                Email = "AliceJ@company.com",
                Skype = "AliceJ12",
                Department = Employee.EmployeeDepartment.Sales,
                HireDate = new System.DateTime(2018, 3, 5),
                Status = Employee.EmployeeStatus.Salaried,
            };

            DataContext = employee;

            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2018,12,21).ToShortDateString(),
                "2018 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2019,12,10).ToShortDateString(),
                "2019 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2020,12,13).ToShortDateString(),
                "2020 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2021,12,20).ToShortDateString(),
                "2021 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2022,12,15).ToShortDateString(),
                "2022 Employee Review", "James Smith" }));
            evaluationsListView.Columns[0].WidthMode = ListViewColumnWidthMode.AutoSize;
            evaluationsListView.Columns[1].WidthMode = ListViewColumnWidthMode.AutoSize;
            evaluationsListView.Columns[2].WidthMode = ListViewColumnWidthMode.AutoSize;

            stateComboBox.PopupKind = PickerPopupKind.ListBox;

            // On Linux height of the ComboBox is greater than height of the TextBox.
            // We need to increase height of all window's TextBoxes.
            TextBoxUtils.AdjustTextBoxesHeight(this);

            this.MinimumSize = new(900, 700);

            firstNameTextBox.Text = employee.FirstName;
            firstNameTextBox.TextChanged += (s, e) =>
            {
                employee.FirstName = firstNameTextBox.Text;
            };

            lastNameTextBox.Text = employee.LastName;
            lastNameTextBox.TextChanged += (s, e) =>
            {
                employee.LastName = lastNameTextBox.Text;
            };

            statusTextBox.Text = employee.Status.ToString();
            statusTextBox.TextChanged += (s, e) =>
            {
                employee.Status
                = (Employee.EmployeeStatus)Enum.Parse(typeof(Employee.EmployeeStatus), statusTextBox.Text);
            };

            emailTextBox.Text = employee.Email;
            emailTextBox.TextChanged += (s, e) =>
            {
                employee.Email = emailTextBox.Text;
            };

            skypeTextBox.Text = employee.Skype;
            skypeTextBox.TextChanged += (s, e) =>
            {
                employee.Skype = skypeTextBox.Text;
            };

            titleTextBox.Text = employee.Title;
            titleTextBox.TextChanged += (s, e) =>
            {
                employee.Title = titleTextBox.Text;
            };

            addressTextBox.Text = employee.Address;
            addressTextBox.TextChanged += (s, e) =>
            {
                employee.Address = addressTextBox.Text;
            };

            cityTextBox.Text = employee.City;
            cityTextBox.TextChanged += (s, e) =>
            {
                employee.City = cityTextBox.Text;
            };

            zipCodeTextBox.Text = employee.ZipCode;
            zipCodeTextBox.TextChanged += (s, e) =>
            {
                employee.ZipCode = zipCodeTextBox.Text;
            };

            homePhoneTextBox.Text = employee.HomePhone;
            homePhoneTextBox.TextChanged += (s, e) =>
            {
                employee.HomePhone = homePhoneTextBox.Text;
            };

            mobilePhone.Text = employee.MobilePhone;
            mobilePhone.TextChanged += (s, e) =>
            {
                employee.MobilePhone = mobilePhone.Text;
            };

            prefixComboBox.Value = employee.Prefix;
            prefixComboBox.ValueChanged += (s, e) =>
            {
                employee.Prefix = prefixComboBox.ValueAs<Employee.EmployeePrefix>();
            };

            stateComboBox.Value = employee.State;
            stateComboBox.ValueChanged += (s, e) =>
            {
                employee.State = stateComboBox.ValueAs<Employee.States>();
            };

            departmentComboBox.Value = employee.Department;
            departmentComboBox.ValueChanged += (s, e) =>
            {
                employee.Department = departmentComboBox.ValueAs<Employee.EmployeeDepartment>();
            };

            birthDatePicker.Value = employee.BirthDate;
            birthDatePicker.ValueChanged += (s, e) =>
            {
                employee.BirthDate = birthDatePicker.Value;
            };

            hireDatePicker.Value = employee.HireDate;
            hireDatePicker.ValueChanged += (s, e) =>
            {
                employee.HireDate = hireDatePicker.Value;
            };

            this.SetSizeToContent();
        }

       internal class Employee
        {
            private string? firstName;
            private string? lastName;
            private DateTime? birthDate;
            private string? title;
            private EmployeePrefix prefix;
            private string? address;
            private string? city;
            private States state;
            private string? zipCode;
            private string? homePhone;
            private string? mobilePhone;
            private string? email;
            private string? skype;
            private EmployeeDepartment department;
            private DateTime? hireDate;
            private EmployeeStatus status;

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

            public DateTime? BirthDate
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

            public States State
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

            public EmployeeDepartment Department
            {
                get => department;
                set => department = value;
            }

            public DateTime? HireDate
            {
                get => hireDate;
                set => hireDate = value;
            }

            public EmployeeStatus Status
            {
                get => status;
                set => status = value;
            }

            public enum EmployeeStatus
            {
                Salaried,
                Terminated,
                OnLeave
            }

            public enum EmployeeDepartment
            {
                Sales,
                HR,
                Engineering,
                IT
            }

            public enum EmployeePrefix
            {
                Mr,
                Ms,
                Mrs,
                Miss,
                Dr,
                Prof
            }

            public enum States
            {
                AL, AK, AZ, AR, CA, CO, CT, DE, DC, FL, GA, HI, ID, IL, IN, IA, KS, KY, LA, ME, MD, MA, MI, MN,
                MS, MO, MT, NE, NV, NH, NJ, NM, NY, NC, ND, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VT, VA, WA, WV, WI, WY
            }
        }
    }
}