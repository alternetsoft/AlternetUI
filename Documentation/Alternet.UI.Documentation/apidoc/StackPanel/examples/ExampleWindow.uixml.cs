using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.StackPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            stackPanel.Background = Alternet.Drawing.Brushes.LightGray;
        }

        public void StackPanelExample1()
        {
            #region StackPanelCSharpCreation
            var StackPanel = new Alternet.UI.StackPanel { Orientation = StackPanelOrientation.Vertical, Padding = new Thickness(10) };
            StackPanel.Children.Add(new Button() { Text = "Add Item", Margin = new Thickness(8, 8, 8, 8) });
            StackPanel.Children.Add(new Button() { Text = "remove Item", Margin = new Thickness(8, 8, 8, 8) });
            #endregion
        }

        #region StackPanelEventHandler
        private void StackPanel_OrientationChanged(object? sender, EventArgs e)
        {
            var text = stackPanel.Orientation.ToString();
            MessageBox.Show(text, string.Empty);
        }
        #endregion    
    }
}