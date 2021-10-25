using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.TextBox
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void TextBoxExample1()
        {
            #region TextBoxCSharpCreation
            var TextBox = new Alternet.UI.TextBox{ Text = "Text 1.1", Margin = new Thickness(0, 0, 0, 5) };
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