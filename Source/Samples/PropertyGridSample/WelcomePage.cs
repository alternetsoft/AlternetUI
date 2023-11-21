using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace PropertyGridSample
{
    internal class WelcomePage : Control
    {
        private readonly RichTextBox richText = new()
        {
            HasBorder = false,
        };

        public WelcomePage()
        {
            //Padding = new(10,0,0,0);
            SuggestedSize = new(300, 400);
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

            r.ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Center);

            r.WriteUrl(urlStyle, homePage, "Home page");
            r.WriteText("    ");
            r.WriteUrl(urlStyle, docsUrl, "Documentation");

            r.NewLine();

            r.NewLine();
            r.BeginBold();
            r.BeginFontSize(20);
            r.WriteText("PropertyGrid");
            r.EndFontSize();

            r.EndBold();
            r.NewLine();

            var logoImage = Image.FromUrl("embres:PropertyGridSample.Resources.logo-128x128.png");
            r.WriteImage(logoImage);

            r.NewLine();

            r.BeginFontSize(14);
            r.WriteText("Specialized grid for editing properties.");
            r.EndFontSize();

            r.EndSuppressUndo();
            r.EndUpdate();
            r.ReadOnly = true;
            r.AutoUrlOpen = true;
            r.AutoUrlModifiers = ModifierKeys.None;
        }
    }
}
