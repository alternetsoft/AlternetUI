using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class NumericUpDownWindow : Window
    {
        public NumericUpDownWindow()
        {
            InitializeComponent();
        }

        public void NumericUpDownExample1()
        {
            #region NumericUpDownCSharpCreation
            var progressBarControlNumericUpDown = new Alternet.UI.NumericUpDown() { Maximum = 10, Margin = new Thickness(0, 0, 0, 5) };
            var progressBar = new Alternet.UI.ProgressBar() { Maximum = 10, Margin = new Thickness(0, 0, 0, 5) };

            progressBarControlNumericUpDown.ValueChanged += (o, e) => progressBar.Value = (int)progressBarControlNumericUpDown.Value;
            progressBarControlNumericUpDown.Value = 1;
            #endregion
        }

        #region NumericUpDownEventHandler
        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            string text = numericUpDown.Value.ToString();
            MessageBox.Show(text, string.Empty);
        }
        #endregion    
    }
}