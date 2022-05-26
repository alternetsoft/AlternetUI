using System;
using Alternet.UI;

namespace MenuSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Menu = new MainMenu();

            var fileItem = new MenuItem("&File");
            fileItem.Items.Add(new MenuItem("E&xit", (o, e) => Close()));
            Menu.Items.Add(fileItem);
        }
    }
}