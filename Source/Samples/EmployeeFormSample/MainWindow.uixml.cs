using System;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;

namespace EmployeeFormSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            prefixComboBox.Items.AddRange(Enum.GetValues(typeof(EmployeePrefix)).Cast<object>());
            stateComboBox.Items.AddRange(Enum.GetValues(typeof(State)).Cast<object>());
            departmentComboBox.Items.AddRange(Enum.GetValues(typeof(Department)).Cast<object>());

            DataContext = new Employee
            {
                Image = new Image(GetType().Assembly.GetManifestResourceStream("EmployeeFormSample.Resources.EmployeePhoto.jpg") ?? throw new Exception()),
                FirstName = "Alice",
                LastName = "Jameson",
                BirthDate = new DateTime(1993, 10, 2).ToShortDateString(),
                Title = "Customer Success Manager",
                Prefix = EmployeePrefix.Mrs,
                Address = "143 Coolidge St.",
                City = "Phoenix",
                State = State.AZ,
                ZipCode = "85001",
                HomePhone = "(341) 433-4377",
                MobilePhone = "(341) 232-6535",
                Email = "AliceJ@mycompany.com",
                Skype = "AliceJ12",
                Department = Department.Sales,
                HireDate = new DateTime(2018, 3, 5).ToShortDateString(),
                Status = Status.Salaried
            };
        }


    }
}