using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal class WelcomePage : HiddenBorder
    {
        private readonly Image logoImage;
        private readonly ScrollViewer scrollViewer;
        private readonly Panel panel = new();

        public WelcomePage()
        {
            panel.Layout = LayoutStyle.Vertical;
            panel.PaddingLeft = 30;

            logoImage = Image.FromUrl("embres:ControlsSampleDll.Resources.logo128x128.png");

            RenderText(panel);

            scrollViewer = ScrollViewer.CreateWithChild(panel);
            scrollViewer.Parent = this;
            scrollViewer.UseControlColors(true);
        }

        private void RenderText(AbstractControl parent)
        {
            parent.UseControlColors(true);

            var homePage = @"https://www.alternet-ui.com/";
            var docsHomePage = @"https://docs.alternet-ui.com/";
            var docsUrl = $"{docsHomePage}introduction/getting-started.html";

            var baseFontSize = AbstractControl.DefaultFont.SizeInPoints;

            var boldFont = Font.Default.WithBold();

            var h1Font = boldFont.WithSize(baseFontSize + 15);
            var h2Font = boldFont.WithSize(baseFontSize + 3);
            var infoFont = Font.Default.WithSize(baseFontSize + 1);

            new Label("Alternet UI").WithMargin(0, 20, 0, 20).WithFont(h1Font)
                .WithAlignment(HorizontalAlignment.Center).WithParent(parent);

            new Label("Cross-platform .NET UI Framework").WithFont(h2Font).WithMargin(0, 0, 0, 20)
                .WithAlignment(HorizontalAlignment.Center).WithParent(parent);

            new PictureBox(logoImage).WithAlignment(HorizontalAlignment.Center)
                .WithMarginBottom(20).WithParent(parent);

            var s1 = "Use established .NET standards and productivity tools";
            var s2 = "for your cross-platform desktop application. ";
            var s3 = "Keep up good engineering practices. Deliver your application quickly.";
            var s4 = "Be native on the desktop, whether it is Windows, macOS, or Linux.";

            void WriteLine(string text)
            {
                new Label(text).WithFont(infoFont).WithAlignment(HorizontalAlignment.Center)
                    .WithMargin(0, 5, 0, 0).WithParent(parent);
            }

            WriteLine(s1);
            WriteLine(s2);
            WriteLine(s3);
            WriteLine(s4);

            new Label(s3).WithFont(infoFont).WithAlignment(HorizontalAlignment.Center)
                .WithMargin(0, 5, 0, 5).WithParent(parent);

            new HorizontalStackPanel()
                .WithChildren(
                    new LinkLabel("Home", homePage).WithFont(infoFont).WithMargin(0, 0, 30, 5),
                    new LinkLabel("Help", docsUrl).WithFont(infoFont).WithMarginBottom(5))
                .WithAlignment(HorizontalAlignment.Center)
                .WithMarginTop(20)
                .WithParent(parent);
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            ControlUtils.UpdateForeBackColors(panel);
            ControlUtils.UpdateForeBackColors(scrollViewer);
            base.OnSystemColorsChanged(e);
        }
    }
}
