using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.SplitterPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SplitterPanelExample1()
        {
            #region SplitterPanelCSharpCreation
            var splitterPanel = new Alternet.UI.SplitterPanel();
            ListBox listBox3 = new();
            ListBox listBox4 = new();

            listBox3.Items.Add("listBox3");
            listBox4.Items.Add("listBox4");

            splitterPanel.Children.Add(listBox3);
            splitterPanel.Children.Add(listBox4);

            splitterPanel.SplitHorizontal(listBox3, listBox4, 350);

            Children.Add(splitterPanel);
            #endregion
        }
    }
}