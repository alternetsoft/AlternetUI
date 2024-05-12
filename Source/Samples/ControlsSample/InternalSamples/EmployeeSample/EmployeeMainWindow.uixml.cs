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
            Icon = new("embres:ControlsSample.Sample.ico");

            InitializeComponent();

            employeeFoto.Image = Image.FromUrl(
                "embres:ControlsSample.Resources.EmployeePhoto.jpg");

            prefixComboBox.AddEnumValues<EmployeePrefix>();
            stateComboBox.AddEnumValues<State>();
            departmentComboBox.AddEnumValues<Department>();

            var employee = new Employee
            {
                FirstName = "Alice",
                LastName = "Jameson",
                BirthDate = new System.DateTime(1993, 10, 2),
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
                HireDate = new System.DateTime(2018, 3, 5),
                Status = Status.Salaried
            };

            DataContext = employee;

            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2018,12,4).ToShortDateString(),
                "2018 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2019,12,10).ToShortDateString(),
                "2019 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2020,12,1).ToShortDateString(),
                "2020 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2021,12,20).ToShortDateString(),
                "2021 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2022,12,5).ToShortDateString(),
                "2022 Employee Review", "James Smith" }));
            evaluationsListView.Columns[0].WidthMode = ListViewColumnWidthMode.AutoSize;
            evaluationsListView.Columns[1].WidthMode = ListViewColumnWidthMode.AutoSize;
            evaluationsListView.Columns[2].WidthMode = ListViewColumnWidthMode.AutoSize;

            // On Linux height of the ComboBox is greater than height of the TextBox.
            // We need to increase height of all window's TextBoxes.
            TextBoxUtils.AdjustTextBoxesHeight(this);
            /*
                        panelHeader.Add("Information", infoPanel);
                        panelHeader.Add("Contacts", contactsPanel);
                        panelHeader.Add("Evaluations", evalPanel);
                        tabControlPanel.Children.Insert(0, panelHeader);
                        panelHeader.SelectedTab = panelHeader.Tabs[0];
            */
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
                employee.Status = (Status)Enum.Parse(typeof(Status), statusTextBox.Text);
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

            prefixComboBox.SelectedItem = employee.Prefix;
            prefixComboBox.SelectedItemChanged += (s, e) =>
            {
                employee.Prefix = prefixComboBox.SelectedItemAs<EmployeePrefix>();
            };

            stateComboBox.SelectedItem = employee.State;
            stateComboBox.SelectedItemChanged += (s, e) =>
            {
                employee.State = stateComboBox.SelectedItemAs<State>();
            };

            departmentComboBox.SelectedItem = employee.Department;
            departmentComboBox.SelectedItemChanged += (s, e) =>
            {
                employee.Department = departmentComboBox.SelectedItemAs<Department>();
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
    }
}