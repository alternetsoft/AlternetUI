using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class ButtonWindow : Window
    {
        public ButtonWindow()
        {
            InitializeComponent();
        }

        public void ButtonExample1()
        {
            #region ButtonCSharpCreation
            var button = new Button
            {
                Text = "Hello, world!",
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            button.Click += ExecuteButton_Click;
            #endregion
        }

        #region ButtonEventHandler
        private void ExecuteButton_Click(object? sender, System.EventArgs e)
        {
            MessageBox.Show("Hello", "Message");
        }
        #endregion
    }
}
