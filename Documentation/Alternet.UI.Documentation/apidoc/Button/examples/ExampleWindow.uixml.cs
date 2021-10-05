using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples
{
    public partial class ExampleWindow : Window
    {
        public ExampleWindow()
        {
            InitializeComponent();
        }

        public void ButtonExample1()
        {
            #region ButtonCSharpCreation
            var button = new Button();
            button.Text = "Hello, world!";
            button.Click += ExecuteButton_Click;
            #endregion
        }

        #region ButtonEventHandler
        private void ExecuteButton_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Hello", "Message");
        }
        #endregion
    }
}
