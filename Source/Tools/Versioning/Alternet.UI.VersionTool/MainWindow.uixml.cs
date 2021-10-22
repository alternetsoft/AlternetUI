using Alternet.UI;
using Alternet.UI.Versioning;
using System;

namespace Alternet.UI.VersionTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var version = VersionReader.GetVersion(VersionFileLocator.LocateVersionFile(RepoLocator.GetRepoRootPath()));
            versionTextBox.Text = version.GetSimpleVersion();

            SetType(version);
        }

        private void ApplyButton_Click(object sender, System.EventArgs e)
        {
        }

        private void SetType(ProductVersion version)
        {
            switch (version.Type)
            {
                case VersionType.Release:
                    releaseRadioButton.IsChecked = true;
                    break;
                case VersionType.Beta:
                    betaRadioButton.IsChecked = true;
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}