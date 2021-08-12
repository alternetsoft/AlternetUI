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
        }

        private void FontSizeSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.FontSize = ((Slider)sender!).Value;
        }
    }
}