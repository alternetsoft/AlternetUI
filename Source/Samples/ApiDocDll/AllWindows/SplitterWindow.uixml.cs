#pragma warning disable
using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class SplitterWindow : Window
    {
        public SplitterWindow()
        {
            InitializeComponent();
            Example1();
            topListBox.Add("Top");
        }

        public void Example1()
        {
            #region CSharpCreation
            LayoutPanel panel = new();

            StdListBox listBox1 = new()
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

            StdListBox listBox2 = new()
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