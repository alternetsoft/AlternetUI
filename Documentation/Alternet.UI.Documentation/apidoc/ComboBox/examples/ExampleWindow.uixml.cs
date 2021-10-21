using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.ComboBox
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            comboBox.Items.Add("One");
            comboBox.Items.Add("Two");
            comboBox.Items.Add("Three");
            comboBox.SelectedIndex = 1;
        }

        public void ComboBoxExample1()
        {
            #region ComboBoxCSharpCreation
            var comboBox = new Alternet.UI.ComboBox();
            comboBox.Items.Add("One");
            comboBox.Items.Add("Two");
            comboBox.Items.Add("Three");
            comboBox.SelectedIndex = 1;
            comboBox.SelectedItemChanged += ComboBox_SelectedItemChanged;
            #endregion
        }

        #region ComboBoxEventHandler
        private void ComboBox_TextChanged(object? sender, EventArgs e)
        {
            var text = comboBox.Text == "" ? "\"\"" : comboBox.Text;
            MessageBox.Show(text, string.Empty);
        }

        private void ComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            int selectedIndex = comboBox.SelectedIndex.Value;
            object selectedItem = comboBox.SelectedItem;

            MessageBox.Show("Selected Item Text: " + selectedItem.ToString() + "\n" +
                            "Index: " + selectedIndex.ToString(), string.Empty);
        }
        #endregion    
    }
}