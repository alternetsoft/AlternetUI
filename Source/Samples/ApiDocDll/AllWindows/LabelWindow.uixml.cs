using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class LabelWindow : Window
    {
        public LabelWindow()
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