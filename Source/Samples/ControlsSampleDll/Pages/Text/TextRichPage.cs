using System;
using System.Collections.Generic;
using System.IO;
using Alternet.Drawing;

using Alternet.UI;
using Alternet.UI.Extensions;

namespace ControlsSample
{
    internal class TextRichPage : Panel
    {
        [IsTextLocalized(true)]
        public static string itWasInJanuary =
            "It was in January, the most down-trodden month of an Edinburgh winter." +
            " An attractive woman came into the cafe, which is nothing remarkable.";

        [IsTextLocalized(true)]
        public static string sUnorderedListItem = "Unordered List Item";

        [IsTextLocalized(true)]
        public static string sOrderedListItem = "Ordered List Item";

        [IsTextLocalized(true)]
        public static string HtmlFilesFilter = "HTML Files (*.html; *.htm)|*.html;*.htm";

        [IsTextLocalized(true)]
        public static string TextFilesFilter = "TXT Files (*.txt)|*.txt";

        [IsTextLocalized(true)]
        public static string XmlFilesFilter = "XML Files (*.xml)|*.xml";

        public static string SaveFileDialogFilter = $"{TextFilesFilter}|{HtmlFilesFilter}";

        [IsTextLocalized(true)]
        public static string AllFilesFilter = "All Files (*.*)|*.*";

        [IsTextLocalized(true)]
        public static string SupportedFilesFilter = "Supported Files (*.txt)|*.txt";

        public static string OpenFileDialogFilter =
            $"{SupportedFilesFilter}|{AllFilesFilter}";

        private readonly PanelRichTextBox richPanel = new();

        public TextRichPage()
        {
            richPanel.Parent = this;
            richPanel.TextBox.KeyDown += RichEdit_KeyDown;
            richPanel.TextBox.AutoUrlOpen = true;
            richPanel.TextBox.TextUrl += TextMemoPage.MultiLineTextBox_TextUrl;
            richPanel.TextBox.EnterPressed += RichTextBox_EnterPressed;
            richPanel.FileNewClick += RichPanel_FileNewClick;
            richPanel.FileOpenClick += RichPanel_FileOpenClick;
            richPanel.FileSaveClick += RichPanel_FileSaveClick;
            InitRichEdit2();
            richPanel.TextBox.SetCaretPosition(0, true);
            richPanel.TextBox.TextChanged += RichTextBox_TextChanged;
        }

        internal void InitRichEdit()
        {
            var richEdit = richPanel.TextBox;

            var baseFontSize = (int)AbstractControl.DefaultFont.SizeInPoints;

            var taTextColorRed = richEdit.CreateTextAttr();
            taTextColorRed.SetTextColor(LightDarkColor.Red);

            var taBackColorYellow = richEdit.CreateTextAttr();
            taBackColorYellow.SetBackgroundColor(Color.Yellow);
            taBackColorYellow.SetTextColor(Color.Black);

            var taUnderlined = richEdit.CreateTextAttr();
            taUnderlined.SetFontUnderlined();

            var taItalic = richEdit.CreateTextAttr();
            taItalic.SetFontItalic();

            var taBold = richEdit.CreateTextAttr();
            taBold.SetFontWeight(FontWeight.Bold);

            var taStrikeOut = richEdit.CreateTextAttr();
            taStrikeOut.SetFontStrikethrough();

            var homePage = @"https://www.alternet-ui.com/";

            var taUrl = richEdit.CreateTextAttr();
            taUrl.SetURL(homePage);

            var taDefault = richEdit.CreateTextAttr();

            var taUnorderedList = richEdit.CreateTextAttr();
            taUnorderedList.SetBulletStyle(TextBoxTextAttrBulletStyle.Standard);
            taUnorderedList.SetBulletName("standard/circle");

            var taOrderedList = richEdit.CreateTextAttr();
            taOrderedList.SetBulletStyle(TextBoxTextAttrBulletStyle.Arabic);
            taOrderedList.SetBulletNumber(1);

            var taBig = richEdit.CreateTextAttr();
            taBig.SetFontPointSize(baseFontSize + 15);

            var taUnderlined2 = richEdit.CreateTextAttr();
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

            richEdit.DoInsideUpdate(() =>
            {
                richEdit.AppendTextAndStyles(list);
                richEdit.Refresh();
            });

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
            var images = KnownSvgImages.GetAllImages();
            foreach (var image in images)
            {
                r.WriteImage(image.AsNormalImage(24, IsDarkBackground));
                r.WriteText(" | ");
            }
            r.NewLine();
        }

        public void InitRichEdit2()
        {
            var baseFontSize = (int)AbstractControl.DefaultFont.SizeInPoints;

            var r = richPanel.TextBox;

            r.SetDefaultStyle(r.CreateTextAttr());

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

            var logoImage = Image.FromUrl("embres:ControlsSampleDll.Resources.icon-48x48.png");

            r.WriteImage(logoImage);

            r.NewLine();

            r.EndAlignment();

            r.NewLine();

            r.WriteText("What can you do with this thing? ");

            r.WriteImage(KnownSvgImages.ImgMessageBoxInformation.AsNormalImage(24,IsDarkBackground));

            r.WriteText(" Well, you can change text ");

            r.BeginTextColor(Color.Red);
            r.WriteText("color, like this red bit. ");
            r.EndTextColor();

            var backgroundColourAttr = r.CreateRichAttr();
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
            r.WriteText($"{St(KnownShortcuts.RichEditKeys.ToggleBold)} - Toggle Bold style.\n");
            r.WriteText($"{St(KnownShortcuts.RichEditKeys.ToggleItalic)} - Toggle Italic style.\n");
            r.WriteText($"{St(KnownShortcuts.RichEditKeys.ToggleUnderline)} - Toggle Underline style.\n");
            r.WriteText($"{St(KnownShortcuts.RichEditKeys.ToggleStrikethrough)} - Toggle Strikethrough style.\n");
            r.WriteText($"{St(KnownShortcuts.RichEditKeys.LeftAlign)} - Left Align\n");
            r.WriteText($"{St(KnownShortcuts.RichEditKeys.CenterAlign)} - Center Align.\n");
            r.WriteText($"{St(KnownShortcuts.RichEditKeys.RightAlign)} - Right Align.\n");
            r.WriteText($"{St(KnownShortcuts.RichEditKeys.Justify)} - Justify.\n");
            r.WriteText($"{St(KnownShortcuts.RichEditKeys.ClearTextFormatting)} - Clear formatting.\n");

            r.EndParagraphSpacing();
            r.EndSuppressUndo();
            r.EndUpdate();
        }

        private void RichEdit_KeyDown(object? sender, KeyEventArgs e)
        {
            static void Test()
            {
            }

            if (KnownShortcuts.RunTest.Run(e, Test))
                return;
        }

        private void RichTextBox_TextChanged(object? sender, EventArgs e)
        {
            var s = "RichTextBox Text Changed.";
            App.LogReplace(s, s);
        }

        private void RichTextBox_EnterPressed(object? sender, EventArgs e)
        {
            var s = "RichTextBox Enter pressed.";
            App.LogReplace(s, s);
        }

        private RichTextFileType GetFileTypeFromName(string filename)
        {
            var ext = Path.GetExtension(filename).ToLower();
            if (ext == ".txt")
                return RichTextFileType.Text;
            /*if (ext == ".xml")
                return RichTextFileType.Xml;*/
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

            var saveDialog = SaveFileDialog.Default;

            saveDialog.Filter = SaveFileDialogFilter;
            saveDialog.OverwritePrompt = true;
            saveDialog.FileName = richPanel.TextBox.FileName;

            saveDialog.ShowAsync(this.ParentWindow, () =>
            {
                if (SaveFile(saveDialog.FileName!))
                {
                    richPanel.TextBox.FileName = saveDialog.FileName!;
                    App.Log($"Saved to file: {saveDialog.FileName}");
                }
                else
                    App.Log($"Error saving to file: {saveDialog.FileName}");
            });
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

            var dialog = OpenFileDialog.Default;
            dialog.Filter = OpenFileDialogFilter;
            dialog.FileMustExist = true;

            dialog.ShowAsync(this.ParentWindow, () =>
            {
                if (LoadFile(dialog.FileName!))
                {
                    richPanel.TextBox.FileName = dialog.FileName!;
                    App.Log($"Loaded from file: {dialog.FileName}");
                }
                else
                    App.Log($"Error loading from file: {dialog.FileName}");
            });
        }

        private void RichPanel_FileNewClick(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Clear all text".AddQuestion(),
                "Prompt",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.None,
                (e) =>
                {
                    if (e.Result == DialogResult.Yes)
                        richPanel.TextBox.Clear();
                });
        }
    }
}
