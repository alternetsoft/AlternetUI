using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

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
            Padding = new(10,0,0,0);
            var homePage = @"https://www.alternet-ui.com/";
            var docsHomePage = @"https://docs.alternet-ui.com/";
            var docsUrl = $"{docsHomePage}introduction/getting-started.html";

            richText.Parent = this;
            var r = richText;

            r.SetDefaultStyle(TextBox.CreateTextAttr());

            r.BeginUpdate();
            r.BeginSuppressUndo();

            r.BeginParagraphSpacing(0, 20);

            var urlStyle = r.CreateUrlAttr();

            r.WriteUrl(urlStyle, homePage, "Home page");
            r.WriteText("    ");
            r.WriteUrl(urlStyle, docsUrl, "Documentation");
            r.ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Right);

            r.NewLine();

            r.ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Center);

            r.BeginBold();
            r.BeginFontSize(24);
            r.WriteText("Alternet UI");
            r.EndFontSize();

            r.NewLine();
            r.BeginFontSize(14);
            r.WriteText("Cross-platform .NET UI Framework");
            r.EndFontSize();

            r.EndBold();
            r.NewLine();

            var logoImage = Image.FromUrl("embres:ControlsSample.Resources.logo-128x128.png");
            r.WriteImage(logoImage);

            r.NewLine();

            r.WriteText(
                "Use established .NET standards and productivity tools for your cross-platform"+
                " desktop application. Keep up good engineering practices. Deliver your application"+
                " quickly. Be native on the desktop, whether it is Windows, macOS, or Linux.");

            r.EndSuppressUndo();
            r.EndUpdate();
            r.ReadOnly = true;
            r.AutoUrlOpen = true;
            r.AutoUrlModifiers = ModifierKeys.None;
        }
    }
}
