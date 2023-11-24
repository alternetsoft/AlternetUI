using System;
using System.ComponentModel;
using System.Collections;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;

namespace EmployeeFormSample
{
    public partial class MainWindow : Window
    {
/*
        private readonly CardPanelHeader panelHeader = new();
*/
        public MainWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:EmployeeFormSample.Sample.ico");

            InitializeComponent();

            employeeFoto.Image = Image.FromUrl(
                "embres:EmployeeFormSample.Resources.EmployeePhoto.jpg");

            PopulateComboBoxes();

            DataContext = new Employee
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

            LayoutUpdated += MainWindow_LayoutUpdated;

            // On Linux height of the ComboBox is greater than height of the TextBox.
            // We need to increase height of all window's TextBoxes.
            LayoutFactory.AdjustTextBoxesHeight(this);
/*
            panelHeader.Add("Information", infoPanel);
            panelHeader.Add("Contacts", contactsPanel);
            panelHeader.Add("Evaluations", evalPanel);
            tabControlPanel.Children.Insert(0, panelHeader);
            panelHeader.SelectedTab = panelHeader.Tabs[0];
*/
            this.MinimumSize = new(900, 700);

            firstNameTextBox.BindText(nameof(Employee.FirstName));
            lastNameTextBox.BindText(nameof(Employee.LastName));
            statusTextBox.BindText(nameof(Employee.Status));
            emailTextBox.BindText(nameof(Employee.Email));
            skypeTextBox.BindText(nameof(Employee.Skype));            
            titleTextBox.BindText(nameof(Employee.Title));
            addressTextBox.BindText(nameof(Employee.Address));
            cityTextBox.BindText(nameof(Employee.City));
            zipCodeTextBox.BindText(nameof(Employee.ZipCode));
            homePhoneTextBox.BindText(nameof(Employee.HomePhone));
            mobilePhone.BindText(nameof(Employee.MobilePhone));
            prefixComboBox.BindSelectedItem(nameof(Employee.Prefix));
            stateComboBox.BindSelectedItem(nameof(Employee.State));
            departmentComboBox.BindSelectedItem(nameof(Employee.Department));
            birthDatePicker.BindValue(nameof(Employee.BirthDate));
            hireDatePicker.BindValue(nameof(Employee.HireDate));          

            this.SetSizeToContent();
        }

        private void MainWindow_LayoutUpdated(object? sender, EventArgs e)
        {
        }        

        private void PopulateComboBoxes()
        {
            static void FillComboBoxWithEnumValues(ComboBox cb, Type enumType)
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