using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitRichEdit();
            CreateRichTextBox();
        }

        #region InitControl
        public void InitRichEdit()
        {
            var baseFontSize = (int)Control.DefaultFont.SizeInPoints;

            const string itWasInJanuary =
                "It was in January, the most down-trodden month of an Edinburgh winter." +
                " An attractive woman came into the cafe, which is nothing remarkable.";

            var r = richTextBox1;

            r.SetDefaultStyle(TextBox.CreateTextAttr());

            r.BeginUpdate();
            r.BeginSuppressUndo();

            r.BeginParagraphSpacing(0, 20);
            r.BeginAlignment(TextBoxTextAttrAlignment.Center);
            r.BeginBold();

            r.BeginFontSize(baseFontSize + 5);

            r.WriteText("Welcome to RichTextBox, a control");
            r.LineBreak();
            r.WriteText("for editing and presenting styled text and images\n");
            r.EndFontSize();

            r.EndBold();
            r.NewLine();

            var logoImage = Alternet.Drawing.Image.FromUrl("embres:Examples.RichTextBox.icon-48x48.png");

            r.WriteImage(logoImage);

            r.NewLine();

            r.EndAlignment();

            r.NewLine();

            r.WriteText("What can you do with this thing? ");

            r.WriteImage(KnownSvgImages.GetForSize(
                r.GetSvgColor(KnownSvgColor.Normal),
                24).ImgMessageBoxInformation.AsImage(24));

            r.WriteText(" Well, you can change text ");

            r.BeginTextColor(Alternet.Drawing.Color.Red);
            r.WriteText("color, like this red bit. ");
            r.EndTextColor();

            var backgroundColourAttr = RichTextBox.CreateRichAttr();
            backgroundColourAttr.SetBackgroundColor(Alternet.Drawing.Color.Green);
            backgroundColourAttr.SetTextColor(Alternet.Drawing.Color.Yellow);
            r.BeginStyle(backgroundColourAttr);
            r.WriteText("And this yellow on green bit");
            r.EndStyle();

            r.WriteText(". Naturally you can make things ");
            r.BeginBold();
            r.WriteText("bold ");
            r.EndBold();
            r.BeginItalic();
            r.WriteText("or italic ");
            r.EndItalic();
            r.BeginUnderline();
            r.WriteText("or underlined.");
            r.EndUnderline();

            r.BeginFontSize(baseFontSize + 3);
            r.WriteText(" Different font sizes on the same line is allowed, too.");
            r.EndFontSize();

            r.WriteText(" Next we'll show an indented paragraph.");

            r.NewLine();

            r.BeginLeftIndent(60);
            r.WriteText(itWasInJanuary);
            r.NewLine();

            r.EndLeftIndent();

            r.WriteText(
                "Next, we'll show a first-line indent, achieved using BeginLeftIndent(100, -40).");

            r.NewLine();

            r.BeginLeftIndent(100, -40);

            r.WriteText(itWasInJanuary);
            r.NewLine();

            r.EndLeftIndent();

            r.WriteText("Numbered bullets are possible, again using subindents:");
            r.NewLine();

            r.BeginNumberedBullet(1, 100, 60);
            r.WriteText("This is my first item. Note that RichTextBox can apply" +
                " numbering and bullets automatically based on list styles, but this" +
                " list is formatted explicitly by setting indents.");
            r.NewLine();
            r.EndNumberedBullet();

            r.BeginNumberedBullet(2, 100, 60);
            r.WriteText("This is my second item.");
            r.NewLine();
            r.EndNumberedBullet();

            r.WriteText("The following paragraph is right-indented:");
            r.NewLine();

            r.BeginRightIndent(200);

            r.WriteText(itWasInJanuary);
            r.NewLine();

            r.EndRightIndent();

            r.WriteText("The following paragraph is right-aligned with 1.5 line spacing:");
            r.NewLine();

            r.BeginAlignment(TextBoxTextAttrAlignment.Right);
            r.BeginLineSpacing((int)TextBoxTextAttrLineSpacing.Half);
            r.WriteText(itWasInJanuary);
            r.NewLine();
            r.EndLineSpacing();
            r.EndAlignment();

            r.WriteText("Other notable features of RichTextBox include:");
            r.NewLine();

            r.BeginSymbolBullet("*", 100, 60);
            r.WriteText("Compatibility with TextBox API.");
            r.NewLine();
            r.EndSymbolBullet();

            r.BeginSymbolBullet("*", 100, 60);
            r.WriteText(
                "Easy stack-based BeginXXX()...EndXXX() style setting in addition to SetStyle().");
            r.NewLine();
            r.EndSymbolBullet();

            r.BeginSymbolBullet("*", 100, 60);
            r.WriteText("XML and other formats loading and saving.");
            r.NewLine();
            r.EndSymbolBullet();

            r.BeginSymbolBullet("*", 100, 60);
            r.WriteText("Undo/Redo, with batching option and Undo suppressing.");
            r.NewLine();
            r.EndSymbolBullet();

            r.BeginSymbolBullet("*", 100, 60);
            r.WriteText("Clipboard copy and paste.");
            r.NewLine();
            r.EndSymbolBullet();

            r.BeginSymbolBullet("*", 100, 60);
            r.WriteText("Style sheets with named character and paragraph styles," +
                " and control for applying named styles.");
            r.NewLine();
            r.EndSymbolBullet();

            r.BeginSymbolBullet("*", 100, 60);
            r.WriteText("A design that can easily be extended to other content types," +
                " ultimately with text boxes, tables, controls, and so on.");
            r.NewLine();
            r.EndSymbolBullet();

            // Make a style suitable for showing a URL
            var urlStyle = r.CreateUrlAttr();

            r.WriteText("RichTextBox can also display URLs, such as this one: ");
            r.BeginStyle(urlStyle);
            r.BeginURL("http://www.alternet-ui.com");
            r.WriteText("Alternet UI Web Site");
            r.EndURL();
            r.EndStyle();
            r.WriteText(". Click on the URL to generate an event.");

            r.NewLine();

            r.WriteText("Note: this sample content was generated programmatically" +
                " from within the demo. The images were loaded from resources. Enjoy RichTextBox!\n");

            r.NewLine();

            static string St(KeyInfo[] keys)
            {
                var filteredKeys = KeyInfo.FilterBackendOs(keys);

                return StringUtils.ToString(
                                filteredKeys,
                                string.Empty,
                                string.Empty,
                                " or ");
            }

            r.WriteText("Keys:\n");

            r.WriteText($"{St(KnownKeys.RichEditKeys.ToggleBold)} - Toggle Bold style.\n");
            r.WriteText($"{St(KnownKeys.RichEditKeys.ToggleItalic)} - Toggle Italic style.\n");
            r.WriteText($"{St(KnownKeys.RichEditKeys.ToggleUnderline)} - Toggle Underline style.\n");
            r.WriteText($"{St(KnownKeys.RichEditKeys.ToggleStrikethrough)} - Toggle Strikethrough style.\n");
            r.WriteText($"{St(KnownKeys.RichEditKeys.LeftAlign)} - Left Align\n");
            r.WriteText($"{St(KnownKeys.RichEditKeys.CenterAlign)} - Center Align.\n");
            r.WriteText($"{St(KnownKeys.RichEditKeys.RightAlign)} - Right Align.\n");
            r.WriteText($"{St(KnownKeys.RichEditKeys.Justify)} - Justify.\n");
            r.WriteText($"{St(KnownKeys.RichEditKeys.ClearTextFormatting)} - Clear formatting.\n");

            r.EndParagraphSpacing();
            r.EndSuppressUndo();
            r.EndUpdate();
        }
        #endregion

        public RichTextBox CreateRichTextBox()
        {
            #region CSharpCreation
            RichTextBox result = new()
            {
                HasBorder = false,
                MinimumSize = (200, 50),
                Dock = DockStyle.Bottom,
                Parent = mainPanel,
            };
            result.Text = "Bottom";
            #endregion

            return result;
        }
}
}