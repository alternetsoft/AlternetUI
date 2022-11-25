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

            PopuplateComboBoxes();

            DataContext = new Employee
            {
                Image = new Bitmap(GetType().Assembly.GetManifestResourceStream("EmployeeFormSample.Resources.EmployeePhoto.jpg") ?? throw new Exception()),
                FirstName = "Alice",
                LastName = "Jameson",
                BirthDate = new DateTime(1993, 10, 2).ToShortDateString(),
                Title = "Customer Success Manager",
                Prefix = EmployeePrefix.Mrs,
                Address = "143 Coolidge St.",
                City = "Phoenix",
                State = EmployeeFormSample.State.AZ,
                ZipCode = "85001",
                HomePhone = "(341) 433-4377",
                MobilePhone = "(341) 232-6535",
                Email = "AliceJ@mycompany.com",
                Skype = "AliceJ12",
                Department = Department.Sales,
                HireDate = new DateTime(2018, 3, 5).ToShortDateString(),
                Status = Status.Salaried
            };

            evaluationsListView.Items.Add(new ListViewItem(new[] { "2018-12-4", "2018 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] { "2019-12-10", "2019 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] { "2020-12-1", "2020 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] { "2021-12-20", "2021 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] { "2022-12-5", "2022 Employee Review", "James Smith" }));
        }

        private void PopuplateComboBoxes()
        {
            void FillComboBoxWithEnumValues(ComboBox cb, Type enumType)
            {
                cb.BeginInit();
                cb.BeginUpdate();
                cb.Items.AddRange(Enum.GetValues(enumType).Cast<object>());
                cb.EndUpdate();
                cb.EndInit();
            }

            FillComboBoxWithEnumValues(prefixComboBox, typeof(EmployeePrefix));
            FillComboBoxWithEnumValues(stateComboBox, typeof(State));
            FillComboBoxWithEnumValues(departmentComboBox, typeof(Department));
        }
    }
}