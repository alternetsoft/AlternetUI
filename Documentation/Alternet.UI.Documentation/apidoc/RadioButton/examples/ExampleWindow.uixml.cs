using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.RadioButton
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void RadioButtonExample1()
        {
            #region RadioButtonCSharpCreation
            var panel2 = new StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };

            panel2.Children.Add(new Alternet.UI.RadioButton() { Text = "Option 1.1", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new Alternet.UI.RadioButton() { Text = "Option 1.2", Margin = new Thickness(0, 0, 0, 5) });
            panel2.Children.Add(new Alternet.UI.RadioButton() { Text = "Option 1.3", Margin = new Thickness(0, 0, 0, 5) });
            #endregion
        }

        #region RadioButtonEventHandler
        private static void RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            var text = ((Alternet.UI.RadioButton)sender).IsChecked.ToString();
            MessageBox.Show(text, string.Empty);
        }

        #endregion    
    }
}