#pragma warning disable
using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class TextBoxWindow : Window
    {
        public TextBoxWindow()
        {
            InitializeComponent();
            TextBoxExample1();
        }

        public void TextBoxExample1()
        {
            #region TextBoxCSharpCreation
            var textBox = new Alternet.UI.TextBox
            {
                Text = "Text 1.2",
                Margin = 10,
                Parent = mainPanel,
            };
            
            #endregion
        }

        #region TextBoxEventHandler
        private void TextBox_TextChanged(object? sender, EventArgs e)
        {
            var text = textBox.Text == "" ? "\"\"" : textBox.Text;
            MessageBox.Show(text, string.Empty);
        }
        #endregion    
    }
}