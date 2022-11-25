using Alternet.UI;

namespace ControlsSample
{
    partial class ButtonPage : Control
    {
        private IPageSite? site;

        public ButtonPage()
        {
            InitializeComponent();

            ApplyText();
            ApplyDisabled();

            button.StateImages = ResourceLoader.ButtonImages;
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void TextTextBox_TextChanged(object sender, Alternet.UI.TextChangedEventArgs e)
        {
            ApplyText();
        }

        private void ApplyText()
        {
            button.Text = textTextBox.Text;
        }

        private void DisabledCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            ApplyDisabled();
        }

        private void ApplyDisabled()
        {
            button.Enabled = !disabledCheckBox.IsChecked;
        }
    }
}