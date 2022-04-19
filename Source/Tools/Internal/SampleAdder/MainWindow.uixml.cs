using Alternet.Drawing;
using Alternet.UI;
using System;

namespace SampleAdder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateControls();
        }

        private void UpdateControls()
        {
            var validationResult = SampleValidator.ValidateSampleName(sampleNameTextBox.Text);

            addSampleButton.Enabled = validationResult.Ok;
            validationLabel.Visible = !validationResult.Ok;
            validationLabel.Text = validationResult.Message;
            validationLabel.Foreground = Brushes.Red;
        }

        private void AddSampleButton_Click(object sender, EventArgs e)
        {
            try
            {
                SampleAdder.AddSample(sampleNameTextBox.Text);
                MessageBox.Show("Sample was added.", "Success");
                sampleNameTextBox.Text = "";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void SampleNameTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
    }
}