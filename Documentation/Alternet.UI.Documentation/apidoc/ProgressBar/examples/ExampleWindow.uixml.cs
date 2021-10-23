using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.ProgressBar
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ProgressBarExample1()
        {
            #region ProgressBarCSharpCreation
            var progressBarControlNumericUpDown = new Alternet.UI.NumericUpDown() { Maximum = 10, Margin = new Thickness(0, 0, 0, 5) };
            var progressBar = new Alternet.UI.ProgressBar() { Maximum = 10, Margin = new Thickness(0, 0, 0, 5) };

            progressBarControlNumericUpDown.ValueChanged += (o, e) => progressBar.Value = (int)progressBarControlNumericUpDown.Value;
            progressBarControlNumericUpDown.Value = 1;
            #endregion
        }

        #region ProgressBarEventHandler
        private void ProgressBar_ValueChanged(object sender, EventArgs e)
        {
            string text = progressBar.Value.ToString();
            MessageBox.Show(text, string.Empty);
        }
        #endregion    
    }
}