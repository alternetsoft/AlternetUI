using Alternet.Drawing;
using Alternet.UI;
using SampleManagement.Common;
using System;

namespace VSCodeSampleSwitcher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeSamples();

            UpdateControls();
        }

        private void InitializeSamples()
        {
            samplesListBox.BeginInit();
            foreach (var sample in SamplesProvider.AllSamples)
                samplesListBox.Items.Add(sample);
            samplesListBox.EndInit();

            samplesListBox.SelectedItem = ActiveSampleService.GetActiveSample();
        }

        private void UpdateControls()
        {
            switchButton.Enabled = samplesListBox.SelectedItem != null;
        }

        private void SwitchButton_Click(object sender, System.EventArgs e)
        {
            if (samplesListBox.SelectedItem == null)
                throw new Exception();

            ActiveSampleService.SetActiveSample((Sample)samplesListBox.SelectedItem);
        }

        private void SamplesListBox_SelectionChanged(object sender, System.EventArgs e)
        {
            UpdateControls();
        }
    }
}