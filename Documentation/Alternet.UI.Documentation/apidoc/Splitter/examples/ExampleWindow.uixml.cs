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
        }

        public void Example1()
        {
            #region CSharpCreation
            LayoutPanel panel = new();

            ListBox listBox1 = new();
            listBox1.Dock = DockStyle.Fill;
            listBox1.Parent = panel;

            Splitter splitter = new();
            splitter.Dock = DockStyle.Right;
            splitter.Parent = panel;

            ListBox listBox2 = new();
            listBox2.Dock = DockStyle.Right;
            listBox2.Parent = panel;
            listBox2.MinWidth = 250;

            panel.Parent = fillPanel;
            #endregion
        }
    }
}