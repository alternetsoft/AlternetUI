using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Example1();
            topListBox.Add("Top");
        }

        public void Example1()
        {
            #region CSharpCreation
            LayoutPanel panel = new();

            ListBox listBox1 = new()
            {
                Dock = DockStyle.Fill,
                Parent = panel,
            };
            listBox1.Add("Fill");

            Splitter splitter = new()
            {
                Dock = DockStyle.Right,
                Parent = panel,
            };

            ListBox listBox2 = new()
            {
                Dock = DockStyle.Right,
                Parent = panel,
                MinWidth = 250,
            };
            listBox2.Add("Right");

            panel.Parent = fillPanel;
            #endregion
        }
    }
}