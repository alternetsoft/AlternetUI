using System;
using System.Collections.Generic;
using System.IO;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal class TextRichPage : Control
    {
        const string HtmlFilesFilter = "HTML Files (*.html; *.htm)|*.html;*.htm";
        const string TextFilesFilter = "TXT Files (*.txt)|*.txt";
        const string XmlFilesFilter = "XML Files (*.xml)|*.xml";

        const string SaveFileDialogFilter = $"{TextFilesFilter}|{HtmlFilesFilter}|{XmlFilesFilter}";

        const string AllFilesFilter = "All Files (*.*)|*.*";
        const string SupportedFilesFilter = "Supported Files (*.txt; *.xml)|*.txt;*.xml;";
        const string OpenFileDialogFilter =
            $"{SupportedFilesFilter}|{AllFilesFilter}";

        private readonly PanelRichTextBox richPanel = new();

        public TextRichPage()
        {
            Margin = 10;
            richPanel.ActionsControl.Required();
            richPanel.SuggestedSize = new(500, 400); // how without it?
            richPanel.Parent = this;
            // richEdit.CurrentPositionChanged += TextBox_CurrentPositionChanged;
            richPanel.TextBox.KeyDown += RichEdit_KeyDown;
            richPanel.TextBox.AutoUrlOpen = true;
            richPanel.TextBox.TextUrl += TextMemoPage.MultiLineTextBox_TextUrl;
            richPanel.TextBox.EnterPressed += RichTextBox_EnterPressed;
            // richEdit.TextUrl += MultiLineTextBox_TextUrl;
            richPanel.FileNewClick += RichPanel_FileNewClick;
            richPanel.FileOpenClick += RichPanel_FileOpenClick;
            richPanel.FileSaveClick += RichPanel_FileSaveClick;
            InitRichEdit2();
            richPanel.TextBox.SetCaretPosition(0, true);
            richPanel.TextBox.TextChanged += RichTextBox_TextChanged;

            // ==== Add test actions

            richPanel.AddAction("Bell", SoundUtils.Bell);
            richPanel.AddAction("Go To Line", richPanel.TextBox.ShowDialogGoToLine);

            PerformLayout();
        }

        internal void InitRichEdit()
        {
            var baseFontSize = (int)Control.DefaultFont.SizeInPoints;

            var taTextColorRed = TextBox.CreateTextAttr();
            taTextColorRed.SetTextColor(Color.Red);

            var taBackColorYellow = TextBox.CreateTextAttr();
            taBackColorYellow.SetBackgroundColor(Color.Yellow);
            taBackColorYellow.SetTextColor(Color.Black);

            var taUnderlined = TextBox.CreateTextAttr();
            taUnderlined.SetFontUnderlined();

            var taItalic = TextBox.CreateTextAttr();
            taItalic.SetFontItalic();

            var taBold = TextBox.CreateTextAttr();
            taBold.SetFontWeight(FontWeight.Bold);

            var taStrikeOut = TextBox.CreateTextAttr();
            taStrikeOut.SetFontStrikethrough();

            var homePage = @"https://www.alternet-ui.com/";

            var taUrl = TextBox.CreateTextAttr();
            taUrl.SetURL(homePage);

            var taDefault = TextBox.CreateTextAttr();

            var taUnorderedList = TextBox.CreateTextAttr();
            taUnorderedList.SetBulletStyle(TextBoxTextAttrBulletStyle.Standard);
            taUnorderedList.SetBulletName("standard/circle");

            var taOrderedList = TextBox.CreateTextAttr();
            taOrderedList.SetBulletStyle(TextBoxTextAttrBulletStyle.Arabic);
            taOrderedList.SetBulletNumber(1);

            var taBig = TextBox.CreateTextAttr();
            taBig.SetFontPointSize(baseFontSize + 15);

            var taUnderlined2 = TextBox.CreateTextAttr();
            taUnderlined2.SetFontUnderlinedEx(
                TextBoxTextAttrUnderlineType.Special,
                Color.Red);

            object[] list =
            {
                "Text color is ",
                taTextColorRed,
                "red",
                taDefault,
                ".\n",
                "Background color is ",
                taBackColorYellow,
                "yellow",
                taDefault,
                ".\n",
                "Font is ",
                "<u>",
                "underlined",
                "</u>",
                ".\n",
                "Font is ",
                "<b>",
                "bold",
                "</b>",
                ".\n",
                "Font is ",
                "<i>",
                "italic",
                "</i>",
                ".\n",
                "Font is ",
                taStrikeOut,
                "strikeout",
                taDefault,
                ".\n",
                "Font is ",
                taBig,
                "big",
                taDefault,
                ".\n",
                "Font is ",
                taUnderlined2,
                "special underlined",
                taDefault,
                ".\n",
                "This is url: ",
                taUrl,
                homePage,
                taDefault,
                ".\n",
                "\n",
            };

            // richEdit.AutoUrl = true;

            var richEdit = richPanel.TextBox;

            richEdit.DoInsideUpdate(() =>
            {
                richEdit.AppendTextAndStyles(list);
                richEdit.Refresh();
            });

            const string sUnorderedListItem = "Unordered List Item";
            const string sOrderedListItem = "Ordered List Item";

            richEdit.SetDefaultStyle(taUnorderedList);
            for (int i = 1; i < 4; i++)
            {
                richEdit.WriteText(sUnorderedListItem + i + "\n");
            }
            richEdit.SetDefaultStyle(taDefault);
            richEdit.NewLine();

            richEdit.SetDefaultStyle(taOrderedList);
            for (int i = 1; i < 4; i++)
            {
                richEdit.WriteText(sOrderedListItem + i + "\n");
            }
            richEdit.SetDefaultStyle(taDefault);
        }

        public void WriteKnownImages(RichTextBox r)
        {
            var images = KnownSvgImages.GetForSize(r.GetSvgColor(KnownSvgColor.Normal), 24).GetAllImages();
            foreach (var image in images)
            {
                r.WriteImage(image.AsImage(24));
                r.WriteText(" | ");
            }
            r.NewLine();
        }

        public void InitRichEdit2()
        {
            var baseFontSize = (int)Control.DefaultFont.SizeInPoints;

            const string itWasInJanuary =
                "It was in January, the most down-trodden month of an Edinburgh winter." +
                " An attractive woman came into the cafe, which is nothing remarkable.";

            var r = richPanel.TextBox;

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

            var logoImage = Image.FromUrl("embres:ControlsSample.Resources.icon-48x48.png");

            r.WriteImage(logoImage);

            r.NewLine();

            r.EndAlignment();

            r.NewLine();

            r.WriteText("What can you do with this thing? ");

            r.WriteImage(KnownSvgImages.GetForSize(
                r.GetSvgColor(KnownSvgColor.Normal),
                24).ImgMessageBoxInformation.AsImage(24));

            r.WriteText(" Well, you can change text ");

            r.BeginTextColor(Color.Red);
            r.WriteText("color, like this red bit. ");
            r.EndTextColor();

            var backgroundColourAttr = RichTextBox.CreateRichAttr();
            backgroundColourAttr.SetBackgroundColor(Color.Green);
            backgroundColourAttr.SetTextColor(Color.Yellow);
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

            /*int[] tabs = { 400, 600, 800, 1000 };
            var attr = TextBox.CreateTextAttr();
            attr.SetFlags(TextBoxTextAttrFlags.Tabs);
            attr.SetTabs(tabs);
            r.SetDefaultStyle(attr);
            r.WriteText("This line contains tabs:\tFirst tab\tSecond tab\tThird tab");
            r.NewLine();*/

            /*
            const string zebraLeftText =
                "This is a simple test for a floating left image test." +
                " The image should be placed at the left side of the current buffer " +
                "and all the text should flow around it at the right side. ";
            const string zebraLeftTripleText = zebraLeftText + zebraLeftText + zebraLeftText;

            const string zebraRightText =
            "This is a simple test for a floating right image test. " +
            "The image should be placed at the right side of the current buffer and" +
            " all the text should flow around it at the left side. ";
            const string zebraRightTripleText = zebraRightText + zebraRightText + zebraRightText;

            r.BeginAlignment(TextBoxTextAttrAlignment.Left);
            var imageAttr = TextBox.CreateTextAttr();
            imageAttr.GetTextBoxAttr().SetFloatMode(wxTEXT_BOX_ATTR_FLOAT_LEFT);
            r.WriteText(zebraLeftTripleText);

            r.WriteImage(logoImage, BitmapType.Png, imageAttr);

            imageAttr.GetTextBoxAttr().GetTop().SetValue(200);
            imageAttr.GetTextBoxAttr().GetTop().SetUnits(wxTEXT_ATTR_UNITS_PIXELS);
            imageAttr.GetTextBoxAttr().SetFloatMode(wxTEXT_BOX_ATTR_FLOAT_RIGHT);
            r.WriteImage(logoImage, BitmapType.Png, imageAttr);
            r.WriteText(zebraRightTripleText);
            r.EndAlignment();
            */

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

            // Move key help to KeyInfo
            // Move this to control
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

        private void RichEdit_KeyDown(object? sender, KeyEventArgs e)
        {
            static void Test()
            {
            }

            if (KnownKeys.RunTest.Run(e, Test))
                return;
        }

        private void RichTextBox_TextChanged(object? sender, EventArgs e)
        {
            var s = "RichTextBox Text Changed.";
            Application.LogReplace(s, s);
        }

        private void RichTextBox_EnterPressed(object? sender, EventArgs e)
        {
            var s = "RichTextBox Enter pressed.";
            Application.LogReplace(s, s);
        }

        private RichTextFileType GetFileTypeFromName(string filename)
        {
            var ext = Path.GetExtension(filename).ToLower();
            if (ext == ".txt")
                return RichTextFileType.Text;
            if (ext == ".xml")
                return RichTextFileType.Xml;
            if (ext == ".html" || ext == ".htm")
                return RichTextFileType.Html;
            return RichTextFileType.Any;
        }

        private void RichPanel_FileSaveClick(object? sender, EventArgs e)
        {
            RichPanelFileSave();
        }

        private void RichPanelFileSave()
        {
            bool useFile = true;

            bool SaveFile(string filename)
            {
                if (File.Exists(filename))
                    File.Delete(filename);
                if (useFile)
                    return richPanel.TextBox.SaveToFile(filename, RichTextFileType.Any);
                try
                {
                    using var stream = File.Create(filename);
                    var fileType = GetFileTypeFromName(filename);
                    if (fileType == RichTextFileType.Any)
                        return false;
                    richPanel.TextBox.SaveToStream(stream, fileType);
                    return true;
                }
                catch (Exception e)
                {
                    LogUtils.LogException(e);
                    return false;
                }
            }

            using SaveFileDialog dialog = new()
            {
                Filter = SaveFileDialogFilter,
                OverwritePrompt = true,
                FileName = richPanel.TextBox.FileName,
            };

            if (dialog.ShowModal(this) != ModalResult.Accepted)
                return;

            if (SaveFile(dialog.FileName!))
            {
                richPanel.TextBox.FileName = dialog.FileName!;
                Application.Log($"Saved to file: {dialog.FileName}");
            }
            else
                Application.Log($"Error saving to file: {dialog.FileName}");
        }

        private void RichPanel_FileOpenClick(object? sender, EventArgs e)
        {
            bool useFile = true;

            bool LoadFile(string filename)
            {
                if (useFile)
                    return richPanel.TextBox.LoadFromFile(filename, RichTextFileType.Any);
                try
                {
                    return false;
                }
                catch (Exception e)
                {
                    LogUtils.LogException(e);
                    return false;
                }
            }

            using OpenFileDialog dialog = new()
            {
                Filter = OpenFileDialogFilter,
                FileMustExist = true,
            };

            if (dialog.ShowModal(this) != ModalResult.Accepted)
                return;

            if (LoadFile(dialog.FileName!))
            {
                richPanel.TextBox.FileName = dialog.FileName!;
                Application.Log($"Loaded from file: {dialog.FileName}");
            }
            else
                Application.Log($"Error loading from file: {dialog.FileName}");
        }

        private void RichPanel_FileNewClick(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Clear all text?",
                "Prompt",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.None);
            if (result == DialogResult.Yes)
                richPanel.TextBox.Clear();
        }
    }
}
