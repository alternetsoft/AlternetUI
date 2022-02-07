using Alternet.UI;
using System;

namespace SRAdder
{
    public partial class MainWindow : Window
    {
        private Engine engine;

        public MainWindow()
        {
            InitializeComponent();

            engine = new Engine(
                new[] { @"C:\Temp\OpenSources\wpf\src\Microsoft.DotNet.Wpf\src\PresentationCore\Resources\Strings.resx" },
                @"C:\Work\UI\Source\Alternet.UI\Resources\Strings.resx",
                @"C:\Work\UI\Source\Alternet.UI\Resources\SRID.cs");

            UpdateControls();
        }

        private void SrIdTextBox_TextChanged(object sender, System.EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            var status = GetStatus();
            statusLabel.Text = status.ToString();
            applyButton.Enabled = status == Status.ReadyToAdd;
        }

        private Status GetStatus() => engine.GetStatus(GetId());

        private string GetId() => srIdTextBox.Text;

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (GetStatus() != Status.ReadyToAdd)
                throw new InvalidOperationException();

            engine.Apply(GetId());
            UpdateControls();
        }
    }
}