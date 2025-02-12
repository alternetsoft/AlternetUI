using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class StackPanelWindow : Window
    {
        public StackPanelWindow()
        {
            InitializeComponent();
            stackPanel.HasBorder = true;
        }

        public void StackPanelExample1()
        {
            #region StackPanelCSharpCreation
            var StackPanel = new Alternet.UI.StackPanel { Orientation = StackPanelOrientation.Vertical, Padding = new Thickness(10) };
            StackPanel.Children.Add(new Button() { Text = "Add Item", Margin = new Thickness(8, 8, 8, 8) });
            StackPanel.Children.Add(new Button() { Text = "remove Item", Margin = new Thickness(8, 8, 8, 8) });
            #endregion
        }
    }
}