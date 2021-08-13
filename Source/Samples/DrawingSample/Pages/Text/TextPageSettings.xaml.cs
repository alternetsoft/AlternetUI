using Alternet.UI;
using System;

namespace DrawingSample
{
    internal class TextPageSettings : Control
    {
        private readonly TextPage page;

        public TextPageSettings(TextPage page)
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("DrawingSample.Pages.Text.TextPageSettings.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);
            this.page = page;

            ((Slider)FindControl("fontSizeSlider")).ValueChanged += FontSizeSlider_ValueChanged;

            ((CheckBox)FindControl("boldCheckBox")).CheckedChanged += (o, e) => ApplyFontStyle(FontStyle.Bold, ((CheckBox)o!).IsChecked);
            ((CheckBox)FindControl("italicCheckBox")).CheckedChanged += (o, e) => ApplyFontStyle(FontStyle.Italic, ((CheckBox)o!).IsChecked);
            ((CheckBox)FindControl("underlinedCheckBox")).CheckedChanged += (o, e) => ApplyFontStyle(FontStyle.Underlined, ((CheckBox)o!).IsChecked);
            ((CheckBox)FindControl("strikethroughCheckBox")).CheckedChanged += (o, e) => ApplyFontStyle(FontStyle.Strikethrough, ((CheckBox)o!).IsChecked);
        }

        private void FontSizeSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.FontSize = ((Slider)sender!).Value;
        }

        private void ApplyFontStyle(FontStyle style, bool value)
        {
            if (value)
                page.FontStyle |= style;
            else
                page.FontStyle &= ~style;
        }
    }
}