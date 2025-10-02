using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal class WelcomePage : HiddenBorder
    {
        private readonly Image logoImage;

        private readonly RichTextBox richText = new()
        {
            HasBorder = false,
        };

        public WelcomePage()
        {
            VerticalAlignment = VerticalAlignment.Stretch;
            HorizontalAlignment = HorizontalAlignment.Stretch;

            richText.Parent = this;
            richText.TextUrl += RichTextBox_TextUrl;
            logoImage = Image.FromUrl("embres:ControlsSampleDll.Resources.logo128x128.png");

            richText.ReadOnly = true;
            richText.AutoUrlOpen = true;
            richText.AutoUrlModifiers = Alternet.UI.ModifierKeys.None;
            RenderText();
        }

        private void RenderText()
        {
            var homePage = @"https://www.alternet-ui.com/";
            var docsHomePage = @"https://docs.alternet-ui.com/";
            var docsUrl = $"{docsHomePage}introduction/getting-started.html";

            var baseFontSize = (int)AbstractControl.DefaultFont.SizeInPoints;

            var r = richText;

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

            r.WriteImage(logoImage);

            r.NewLine(2);

            r.BeginFontSize(baseFontSize + 1);
            r.WriteLineText(
                "Use established .NET standards and productivity tools for your cross-platform" +
                " desktop application. ");
            r.WriteLineText(
                "Keep up good engineering practices. Deliver your application quickly.");
            r.WriteLineText("Be native on the desktop, whether it is Windows, macOS, or Linux.");
            r.EndFontSize();

            r.NewLine();

            var urlStyle = r.CreateUrlAttr();
            r.WriteUrl(urlStyle, homePage, "Home");
            r.WriteText("    ");
            r.WriteUrl(urlStyle, docsUrl, "Help");
            r.WriteText("    ");
            r.WriteUrl(urlStyle, "PropertyGrid", "Property Grid");

            r.EndSuppressUndo();
            r.EndUpdate();
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            richText.DoInsideUpdate(() =>
            {
                richText.Clear();
                RenderText();
            });
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
