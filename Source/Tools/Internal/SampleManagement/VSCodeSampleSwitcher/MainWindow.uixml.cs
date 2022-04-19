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
            InitializeConfigurations();

            UpdateControls();
        }

        private void InitializeSamples()
        {
            samplesListBox.BeginInit();
            foreach (var sample in SamplesProvider.AllSamples)
                samplesListBox.Items.Add(sample);
            samplesListBox.EndInit();
        }

        private void InitializeConfigurations()
        {
            configurationsListBox.BeginInit();
            foreach (var sample in ProjectConfiguration.All)
                configurationsListBox.Items.Add(sample);
            configurationsListBox.EndInit();
        }

        private void UpdateControls()
        {
        }

        private void SwitchButton_Click(object sender, System.EventArgs e)
        {
            
        }
    }
}