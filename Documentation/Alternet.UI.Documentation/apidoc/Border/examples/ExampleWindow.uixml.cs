using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.Border
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            border.BorderColor = Alternet.Drawing.Color.DarkGray;
            border.Background = Alternet.Drawing.Brushes.Khaki;
        }

        public void BorderExample1()
        {
            #region BorderCSharpCreation
            var Border = new Alternet.UI.Border();
            Border.BorderColor = Alternet.Drawing.Color.DarkGray;
            Border.Background = Alternet.Drawing.Brushes.Khaki;
            #endregion
        }
    }
}