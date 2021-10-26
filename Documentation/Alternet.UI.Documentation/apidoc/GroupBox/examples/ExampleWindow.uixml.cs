using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.GroupBox
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void GroupBoxExample1()
        {
            #region GroupBoxCSharpCreation
            var groupBox1 = new Alternet.UI.GroupBox { Title = "Horizontal Sliders" };
            var panel2 = new Alternet.UI.StackPanel { Orientation = StackPanelOrientation.Vertical, Margin = new Thickness(5) };
            groupBox1.Children.Add(panel2);
            #endregion
        }

        #region GroupBoxEventHandler
        #endregion    
    }
}