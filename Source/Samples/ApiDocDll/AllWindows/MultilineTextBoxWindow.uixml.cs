using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class MultilineTextBoxWindow : Window
    {
        private const string LoremIpsum =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. " +
            "Suspendisse tincidunt orci vitae arcu congue commodo. " +
            "Proin fermentum rhoncus dictum.\n";

        public MultilineTextBoxWindow()
        {
            InitializeComponent();
            textBox1.Text = LoremIpsum;
            CreateTextBox();
        }

        #region EventHandler
        internal void TextBox_TextChanged(object? sender, EventArgs e)
        {
            App.Log("Text changed");
        }
        #endregion

        public MultilineTextBox CreateTextBox()
        {
            #region CSharpCreation
            MultilineTextBox result = new()
            {
                HasBorder = false,
                MinimumSize = (200, 50),
                Dock = DockStyle.Bottom,
                Parent = mainPanel,
            };
            result.Text = "Bottom";
            #endregion

            return result;
        }
    }
}