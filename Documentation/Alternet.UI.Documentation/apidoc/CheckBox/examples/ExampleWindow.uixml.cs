using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.CheckBox
{
    public partial class MainWindow : Window
    {
       public MainWindow()
        {
            InitializeComponent();
        }

        public void CheckBoxExample1()
        {
            #region CheckBoxCSharpCreation
            var CheckBox = new Alternet.UI.CheckBox() { Text = "Option 1.1", Margin = new Thickness(0, 0, 0, 5) };
            #endregion
        }

        #region CheckBoxEventHandler
        private void AllowTextEditingCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            comboBox.IsEditable = allowTextEditingCheckBox.IsChecked;
        }

        #endregion    
    }
}