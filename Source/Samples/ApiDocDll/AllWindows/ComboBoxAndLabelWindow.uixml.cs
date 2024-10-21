using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class ComboBoxAndLabelWindow : Window
    {
        public ComboBoxAndLabelWindow()
        {
            InitializeComponent();
            var control2 = CreateComboBox1();
            Group(control1, control2).InnerSuggestedWidthToMax().LabelSuggestedWidthToMax();
        }

        public ComboBoxAndLabel CreateComboBox1()
        {
            #region CSharpCreation
            ComboBoxAndLabel control = new();
            control.Margin = 5;
            control.ComboBox.IsEditable = false;
            control.ComboBox.Items.Add("Value 1");
            control.ComboBox.Items.Add("Value 2");
            control.ComboBox.Items.Add("Value 3");
            control.Title = "This is very long label";
            control.ComboBox.SelectedItem = "Value 2";
            control.Parent = mainPanel;
            #endregion

            return control;
        }
    }
}