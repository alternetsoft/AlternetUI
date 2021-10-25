using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.Label
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            label.Background = Alternet.Drawing.Brushes.DarkGray;
        }

        public void LabelExample1()
        {
            #region LabelCSharpCreation
            var Label = new Alternet.UI.Label();
            #endregion
        }

        #region LabelEventHandler
        private void Label_TextChanged(object? sender, EventArgs e)
        {
            var text = label.Text == "" ? "\"\"" : label.Text;
            MessageBox.Show(text, string.Empty);
        }
        #endregion
    }
}