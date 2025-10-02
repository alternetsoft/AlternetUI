using Alternet.Drawing;
using Alternet.UI;

namespace PropertyGridSample
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
            logoImage = Image.FromUrl(ObjectInit.ResPrefixImage);
            VerticalAlignment = VerticalAlignment.Stretch;
            HorizontalAlignment = HorizontalAlignment.Stretch;

            SuggestedSize = new(300, 400);
            richText.Parent = this;

            richText.ReadOnly = true;
            richText.AutoUrlOpen = true;
            richText.AutoUrlModifiers = Alternet.UI.ModifierKeys.None;
            RenderText();
        }

        private void RenderText()
        {
            var baseFontSize = (int)AbstractControl.DefaultFont.SizeInPoints;
            var homePage = @"https://www.alternet-ui.com/";
            var docsHomePage = @"https://docs.alternet-ui.com/";
            var docsUrl = $"{docsHomePage}introduction/getting-started.html";

            var r = richText;

            r.SetDefaultStyle(r.CreateTextAttr());

            r.BeginUpdate();
            r.BeginSuppressUndo();

            r.BeginParagraphSpacing(0, 20);

            var urlStyle = r.CreateUrlAttr();

            r.ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Center);

            r.WriteUrl(urlStyle, homePage, "Home page");
            r.WriteText("    ");
            r.WriteUrl(urlStyle, docsUrl, "Documentation");

            r.NewLine();

            r.NewLine();
            r.BeginBold();
            r.BeginFontSize(baseFontSize + 7);
            r.WriteText("PropertyGrid");
            r.EndFontSize();

            r.EndBold();
            r.NewLine();

            r.WriteImage(logoImage);

            r.NewLine();

            r.BeginFontSize(14);
            r.WriteText("Specialized grid for editing properties.");
            r.EndFontSize();

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
    }
}
