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
        }

        private void InitializeSamples()
        {
            samplesListBox.BeginInit();
            foreach (var sample in SamplesProvider.AllSamples)
                samplesListBox.Items.Add(sample);
            samplesListBox.EndInit();

            var activeSample = ActiveSampleService.GetActiveSample();
            if (activeSample != null)
                samplesListBox.SelectedItem = activeSample;
        }

        private void SwitchButton_Click(object sender, System.EventArgs e)
        {
            if (samplesListBox.SelectedItem == null)
                return;

            ActiveSampleService.SetActiveSample((Sample)samplesListBox.SelectedItem);
        }
    }
}