using Alternet.UI;
using System;

namespace DrawingSample
{
    internal class RectanglesPageSettings : Control
    {
        private readonly RectanglesPage page;

        public RectanglesPageSettings(RectanglesPage page)
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("DrawingSample.Pages.Rectangles.RectanglesPageSettings.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);
            this.page = page;
        }
    }
}