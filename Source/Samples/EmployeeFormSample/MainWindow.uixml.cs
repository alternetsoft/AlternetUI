using System;
using Alternet.Drawing;
using Alternet.UI;

namespace EmployeeFormSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new Employee
            {
                Image = new Image(GetType().Assembly.GetManifestResourceStream("EmployeeFormSample.Resources.EmployeePhoto.jpg") ?? throw new Exception())
            };
        }


    }
}