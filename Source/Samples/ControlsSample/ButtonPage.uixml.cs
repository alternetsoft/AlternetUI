using Alternet.UI;

namespace ControlsSample
{
    internal partial class ButtonPage : Control
    {
        private IPageSite? site;

        public ButtonPage()
        {
            InitializeComponent();

            ApplyText();
            ApplyDisabled();
            ApplyImage();
            ApplyDefault();
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

        private void DisabledCheckBox_CheckedChanged(
            object? sender, 
            System.EventArgs e)
        {
            ApplyDisabled();
        }

        private void BoldCheckBox_CheckedChanged(
            object? sender,
            System.EventArgs e)
        {
            if(boldCheckBox.IsChecked)
                button.Font = Control.DefaultFont.AsBold;
            else
                button.Font = Control.DefaultFont;
        }

        private void ApplyDisabled()
        {
            button.Enabled = !disabledCheckBox.IsChecked;
        }

        private void ImageCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            ApplyImage();
        }

        private void ApplyImage()
        {
            if (button == null)
                return;
            button.StateImages = imageCheckBox.IsChecked ? ResourceLoader.ButtonImages : new ControlStateImages();
        }

        private void DefaultCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            ApplyDefault();
        }

        private void ApplyDefault()
        {
            button.IsDefault = defaultCheckBox.IsChecked;
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            site?.LogEvent("Button: Click");
        }
    }
}