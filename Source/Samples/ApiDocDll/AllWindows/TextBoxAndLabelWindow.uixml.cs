using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class TextBoxAndLabelWindow : Window
    {
        public TextBoxAndLabelWindow()
        {
            InitializeComponent();
            logListBox.BindApplicationLog();
            var control2 = CreateControl();
            Group(control1, control2).InnerSuggestedWidthToMax().LabelSuggestedWidthToMax();
        }

        #region EventHandler
        public void TextBoxTextChanged(object? sender, EventArgs e)
        {
            App.Log($"TextBox {(sender as Control)?.Name} Text changed");
        }
        #endregion

        public TextBoxAndLabel CreateControl()
        {
            #region CSharpCreation
            TextBoxAndLabel control = new();
            control.Margin = 10;
            control.Text = "some value 2";
            control.Title = "This is very long label";
            control.Parent = mainPanel;
            control.Name = "control2";
            control.TextChanged += TextBoxTextChanged;
            control.InnerSuggestedWidth = 200;
            #endregion

            return control;
        }
    }
}