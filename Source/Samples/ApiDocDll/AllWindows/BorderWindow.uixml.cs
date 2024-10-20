using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class BorderWindow : Window
    {
        public BorderWindow()
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