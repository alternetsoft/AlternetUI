using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace PropertyGridSample
{
    internal class WelcomeControl : Control
    {
        private readonly VerticalStackPanel stackPanel = new()
        {
            Padding = new(10),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };
        private const string descText = "Specialized grid for editing properties.";
        private readonly Label header = new()
        {
            Text = "PropertyGrid",
            Font = Font.Default.AsBold,
        };
        private readonly Label desc = new()
        {
            Text = descText,
            Margin = new(0,5,0,15),
        };

        /*private WebBrowser webBrowser;*/

        private static string GetPandaFileName()
        {
            var s = PathUtils.GetAppFolder() +
                "Resources/welcome.html";
            return s;
        }

        private static string GetPandaUrl()
        {
            var s = WebBrowser.PrepareFileUrl(GetPandaFileName());
            return s;
        }

        public WelcomeControl()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            SuggestedHeight = 500;
            SuggestedWidth = 500;

            /*webBrowser = new(GetPandaUrl())
            {
                HasBorder = false,
                Visible = false,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Parent = this,
                SuggestedHeight = 500,
                SuggestedWidth = 500,
            };
            webBrowser.Loaded += WebBrowser_Loaded;*/

            DoInsideLayout(() =>
            {
                Children.Add(stackPanel);
                stackPanel.Children.Add(header);
                stackPanel.Children.Add(desc);
            });
        }

        /*private void WebBrowser_Loaded(object? sender, WebBrowserEventArgs e)
        {
            webBrowser.Visible = true;
        } */
    }
}
