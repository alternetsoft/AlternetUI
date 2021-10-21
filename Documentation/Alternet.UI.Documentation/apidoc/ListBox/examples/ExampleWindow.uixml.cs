using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.ListBox
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            listBox.Items.Add("One");
            listBox.Items.Add("Two");
            listBox.Items.Add("Three");
            listBox.SelectedIndex = 1;
        }

        public void ListBoxExample1()
        {
            #region ListBoxCSharpCreation
            var ListBox = new Alternet.UI.ListBox();
            ListBox.Items.Add("One");
            ListBox.Items.Add("Two");
            ListBox.Items.Add("Three");
            ListBox.SelectedIndex = 1;
            #endregion
        }

        #region ListBoxEventHandler
        private void ListBox_SelectionChanged(object? sender, EventArgs e)
        {
            string selectedIndicesString = listBox.SelectedIndices.Count > 100 ? "too many indices to display" : string.Join(",", listBox.SelectedIndices);
            MessageBox.Show("ListBox: SelectionChanged. SelectedIndices: ({selectedIndicesString})", string.Empty);
        }

        #endregion    
    }
}