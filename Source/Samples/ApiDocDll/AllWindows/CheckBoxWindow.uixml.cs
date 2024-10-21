using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class CheckBoxWindow : Window
    {
       public CheckBoxWindow()
        {
            InitializeComponent();
            comboBox.Add("One");
            comboBox.Add("Two");
            comboBox.Add("Three");
            comboBox.SelectFirstItem();
        }

        public static CheckBox CheckBoxExample1()
        {
            #region CheckBoxCSharpCreation
            var checkBox = new Alternet.UI.CheckBox()
            {
                Text = "Option 1.1",
                Margin = new Thickness(0, 0, 0, 5),
            };
            return checkBox;
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