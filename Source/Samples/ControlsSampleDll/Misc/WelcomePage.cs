using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal class WelcomePage : Control
    {
        private readonly RichTextBox richText = new()
        {
            HasBorder = false,
        };

        public WelcomePage()
        {
            VerticalAlignment = VerticalAlignment.Stretch;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            var homePage = @"https://www.alternet-ui.com/";
            var docsHomePage = @"https://docs.alternet-ui.com/";
            var docsUrl = $"{docsHomePage}introduction/getting-started.html";

            var baseFontSize = (int)Control.DefaultFont.SizeInPoints;

            richText.Parent = this;
            var r = richText;

            r.TextUrl += RichTextBox_TextUrl;

            r.SetDefaultStyle(r.CreateTextAttr());

            r.BeginUpdate();
            r.BeginSuppressUndo();

            r.BeginParagraphSpacing(0, 20);

            r.ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Center);

            r.BeginBold();
            r.BeginFontSize(baseFontSize + 15);


            r.WriteText("Alternet UI");
            r.EndFontSize();

            r.NewLine();
            r.BeginFontSize(baseFontSize + 3);
            r.WriteText("Cross-platform .NET UI Framework");
            r.EndFontSize();

            r.EndBold();
            r.NewLine();

            var logoImage = Image.FromUrl("embres:ControlsSampleDll.Resources.logo128x128.png");
            r.WriteImage(logoImage);

            r.NewLine();

            r.WriteText(
                "Use established .NET standards and productivity tools for your cross-platform" +
                " desktop application. Keep up good engineering practices. Deliver your application" +
                " quickly. Be native on the desktop, whether it is Windows, macOS, or Linux.");

            r.NewLine(2);

            var urlStyle = r.CreateUrlAttr();
            r.WriteUrl(urlStyle, homePage, "Home page");
            r.WriteText("    ");
            r.WriteUrl(urlStyle, docsUrl, "Documentation");

            r.NewLine();
            r.WriteUrl(urlStyle, "PropertyGrid", "Run Property Grid Demo");

            r.EndSuppressUndo();
            r.EndUpdate();
            r.ReadOnly = true;
            r.AutoUrlOpen = true;
            r.AutoUrlModifiers = Alternet.UI.ModifierKeys.None;
        }

        private void RichTextBox_TextUrl(object? sender, UrlEventArgs e)
        {
            if(e.Url == "PropertyGrid")
            {
                e.Cancel = true;
                App.AddIdleTask(() =>
                {
                    var form = new PropertyGridSample.MainWindow();
                    form.Show();
                });
            }
        }
    }
}
