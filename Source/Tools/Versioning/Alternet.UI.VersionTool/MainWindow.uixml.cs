using Alternet.UI;
using Alternet.UI.Versioning;
using System;

namespace Alternet.UI.VersionTool
{
    public partial class MainWindow : Window
    {
        private string versionFilePath;

        public MainWindow()
        {
            InitializeComponent();

            versionFilePath = VersionFileLocator.LocateVersionFile(RepoLocator.GetRepoRootPath());
            var version = VersionReader.GetVersion(versionFilePath);
            versionTextBox.Text = version.GetSimpleVersion();

            SetType(version);
        }

        private void ApplyButton_Click(object sender, System.EventArgs e)
        {
            var version = TryGetNewVersion();
            if (version == null)
                return;

            try
            {
                VersionSetter.SetVersion(versionFilePath, version);
                MessageBox.Show("New version was set successfully.", "Version Tool");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error Setting Version");
            }
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

        private void VersionTextBox_TextChanged(object sender, System.EventArgs e)
        {
            UpdateNewVersion();
        }

        private void UpdateNewVersion()
        {
            var version = TryGetNewVersion();
            versionTextLabel.Text = version == null ? "<Invalid input>" : version.GetInformationalVersion();
            versionTextLabel.PerformLayout(); // todo
        }

        private VersionType? TryGetVersionType()
        {
            if (releaseRadioButton.IsChecked)
                return VersionType.Release;
            if (betaRadioButton.IsChecked)
                return VersionType.Beta;
            
            return null;
        }

        private ProductVersion? TryGetNewVersion()
        {
            if (!ProductVersion.TryParseSimpleVersion(versionTextBox.Text, out var version))
                return null;

            var type = TryGetVersionType();
            if (type == null)
                return null;

            return version.WithType(type.Value);
        }

        private void TypeRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateNewVersion();
        }
    }
}