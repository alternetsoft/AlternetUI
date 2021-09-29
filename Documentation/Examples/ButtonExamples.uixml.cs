using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.CodeExamples
{
    public partial class ButtonExamples : Window
    {
        public ButtonExamples()
        {
            InitializeComponent();
        }

        public static void ButtonExample1()
        {
            #region ButtonExample1
            var button = new Button();
            button.Text = "Hello, world!";
            button.Click += Button_Click;
            #endregion
        }

        private static void Button_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
