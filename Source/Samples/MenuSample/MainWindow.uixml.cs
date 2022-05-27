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

            fileItem.Items.Add(new MenuItem("&Open...", (o, e) => MessageBox.Show("Open")));


            var exportItem = new MenuItem("&Export");
            exportItem.Items.Add(new MenuItem("To &PDF", (o, e) => MessageBox.Show("PDF")));
            exportItem.Items.Add(new MenuItem("To &PNG", (o, e) => MessageBox.Show("PNG")));
            fileItem.Items.Add(exportItem);


            fileItem.Items.Add(new MenuItem("E&xit", (o, e) => Close()));
            Menu.Items.Add(fileItem);
        }
    }
}