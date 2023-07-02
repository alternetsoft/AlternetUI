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
        public MainWindow()
        {
            InitializeComponent();

            PopuplateComboBoxes();

            DataContext = new Employee
            {
                FirstName = "Alice",
                LastName = "Jameson",
                BirthDate = new System.DateTime(1993, 10, 2).ToShortDateString(),
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
                HireDate = new System.DateTime(2018, 3, 5).ToShortDateString(),
                Status = Status.Salaried
            };

            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2018,12,4).ToShortDateString(), "2018 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2019,12,10).ToShortDateString(), "2019 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2020,12,1).ToShortDateString(), "2020 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2021,12,20).ToShortDateString(), "2021 Employee Review", "James Smith" }));
            evaluationsListView.Items.Add(new ListViewItem(new[] {
                new DateTime(2022,12,5).ToShortDateString(), "2022 Employee Review", "James Smith" }));
            evaluationsListView.Columns[0].WidthMode = ListViewColumnWidthMode.AutoSize;
            evaluationsListView.Columns[1].WidthMode = ListViewColumnWidthMode.AutoSize;
            evaluationsListView.Columns[2].WidthMode = ListViewColumnWidthMode.AutoSize;

            LayoutUpdated += MainWindow_LayoutUpdated;

            // On Linux height of the ComboBox is greater than height of the TextBox.
            // We need to increase height of all window's TextBoxes.
            AdjustTextBoxesHeight(this, prefixComboBox, firstNameTextBox);
        }

        private void MainWindow_LayoutUpdated(object sender, EventArgs e)
        {
        }

        private void AdjustTextBoxesHeight(Control container, Control comboBox, Control? textBox)
        {
            var comboBoxHeight = comboBox.Bounds.Height;
            double textBoxHeight = 0;
            if (textBox != null)
            {
                textBoxHeight = textBox.Bounds.Height;
                if (comboBoxHeight == textBoxHeight)
                    return;
            }
            var maxHeight = Math.Max(comboBoxHeight, textBoxHeight);

            container.SuspendLayout();

            try
            {
                Collection<Control> editors = new Collection<Control>();
                addTextEditors(container);

                foreach (Control control in editors)
                    control.Height = maxHeight;

                void addTextEditors(Control container)
                {
                    foreach (Control control in container.Children)
                    {
                        if (control is TextBox || control is ComboBox)
                        {
                            if (control.Bounds.Height < maxHeight)
                                editors.Add(control);
                        }
                        else
                            addTextEditors(control);
                    }
                }
            }
            finally
            {
                container.ResumeLayout(true);
            }
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