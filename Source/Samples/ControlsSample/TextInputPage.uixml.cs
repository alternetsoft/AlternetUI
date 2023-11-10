using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.UI.Localization;

namespace ControlsSample
{
    internal partial class TextInputPage : Control
    {
        private const string ErrorMinValueTextDouble = "-2.7976931348623157E+308";
        private const string ErrorMaxValueTextDouble = "2.7976931348623157E+308";
        private const string MinValueTextDouble = "-1.7976931348623157E+308";
        private const string MaxValueTextDouble = "1.7976931348623157E+308";
        private const string LoremIpsum =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. "+
            "Suspendisse tincidunt orci vitae arcu congue commodo. "+
            "Proin fermentum rhoncus dictum.\n\n"+
            "Sample url: https://www.alternet-ui.com/\n";

        private readonly CardPanelHeader panelHeader = new();
        private readonly PanelRichTextBox richPanel = new();
        private readonly ValueEditorInt16 shortEdit = new("Int16", -25);
        private readonly ValueEditorByte byteEdit = new("Byte", 230);
        private readonly ValueEditorDouble doubleEdit = new("Double", -15.3);
        private readonly ValueEditorUDouble udoubleEdit = new("UDouble", 1002);
        private readonly HexEditorUInt32 uint32HexEdit = new("UInt32 Hex", 0x25E6);

        private readonly ValueEditorUInt32 minLengthEdit = new("Min Length")
        {
            Margin = new(0, 0, 0, 5),
            Text = "0",
        };
        private readonly ValueEditorUInt32 maxLengthEdit = new("Max Length")
        {
            Margin = new(0, 0, 0, 5),
            Text = "0",
        };
        private readonly ValueEditorEMail emailEdit = new("E-mail")
        {
            Margin = new(0, 0, 0, 5),
            InnerSuggestedWidth = 200,
        };

        private readonly MultilineTextBox multiLineTextBox = new()
        {
            SuggestedWidth = 350,
            SuggestedHeight = 130,
            Margin = 5,
        };         

        private IPageSite? site;

        public TextInputPage()
        {
            InitializeComponent();

            void BindTextChanged(ValueEditorCustom control)
            {
                control.TextChanged += ReportValueChanged;
            }

            ControlSet.New(
                shortEdit,
                byteEdit,
                doubleEdit,
                udoubleEdit,
                uint32HexEdit).Margin(0,5,5,5).Parent(numbersPanel).InnerSuggestedWidth(200)
                .Action<ValueEditorCustom>(BindTextChanged).LabelColumnIndex(0);

            numbersPanel.GetColumnGroup(0, true).SuggestedWidthToMax();

            panelHeader.Add("Text", tab1);
            panelHeader.Add("Multiline", tab2);
            panelHeader.Add("Rich Text", tab3);
            panelHeader.Add("Numbers", tab4);
            panelHeader.Add("Other", tab5);
            tabControl.Children.Prepend(panelHeader);
            panelHeader.SelectFirstTab();

            // ==== textBox

            textBox.EmptyTextHint = "Sample Hint";
            textBox.Text = "sample text";
            textBox.ValidatorReporter = textImage;
            textBox.TextMaxLength += TextBox_TextMaxLength;
            textBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;
            textBox.Options |= TextBoxOptions.DefaultValidation;
            textBox.TextChanged += ReportValueChanged;
            TextBox.InitErrorPicture(textImage);

            // ==== Email editor

            emailEdit.Parent = otherParent;

            // ==== multiLineTextBox

            multiLineTextBox.AutoUrl = true;
            if(!Application.IsMacOs)
                multiLineTextBox.AutoUrlOpen = true;
            multilineParent.Children.Prepend(multiLineTextBox);
            multiLineTextBox.Text = LoremIpsum;
            multiLineTextBox.TextUrl += MultiLineTextBox_TextUrl;
            multiLineTextBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;

            // ==== Other initializations

            wordWrapComboBox.BindEnumProp(multiLineTextBox, nameof(TextBox.TextWrap));
            textAlignComboBox.BindEnumProp(textBox, nameof(TextBox.TextAlign));

            readOnlyCheckBox.BindBoolProp(textBox, nameof(TextBox.ReadOnly));
            passwordCheckBox.BindBoolProp(textBox, nameof(TextBox.IsPassword));
            hasBorderCheckBox.BindBoolProp(textBox, nameof(TextBox.HasBorder));
            logPositionCheckBox.BindBoolProp(this, nameof(LogPosition));

            bellOnErrorCheckBox.BindBoolProp(
                ValueValidatorFactory.Default,
                nameof(ValueValidatorFactory.BellOnError));

            ControlSet.New(
                textAlignLabel,
                minLengthEdit.Label,
                maxLengthEdit.Label).SuggestedWidthToMax();
            ControlSet.New(
                textAlignComboBox,
                minLengthEdit.TextBox,
                maxLengthEdit.TextBox).SuggestedWidthToMax();

            Application.Current.Idle += Application_Idle;

            // ==== Min and Max length editors

            minLengthEdit.TextBox.TextChanged += MinLengthBox_TextChanged;
            minLengthEdit.TextBox.IsRequired = true;
            minLengthEdit.Parent = textBoxOptionsPanel;

            maxLengthEdit.TextBox.TextChanged += MaxLengthBox_TextChanged;
            maxLengthEdit.TextBox.IsRequired = true;
            maxLengthEdit.Parent = textBoxOptionsPanel;

            // ==== richEdit

            richPanel.ActionsControl.Required();
            richPanel.SuggestedSize = new Size(500, 400); // how without it?
            richPanel.Parent = tab3;
            // richEdit.CurrentPositionChanged += TextBox_CurrentPositionChanged;
            richPanel.TextBox.KeyDown += RichEdit_KeyDown;
            if (!Application.IsMacOs)
                richPanel.TextBox.AutoUrlOpen = true;
            richPanel.TextBox.TextUrl += MultiLineTextBox_TextUrl;
            richPanel.TextBox.EnterPressed += RichTextBox_EnterPressed;
            // richEdit.TextUrl += MultiLineTextBox_TextUrl;
            richPanel.FileNewClick += RichPanel_FileNewClick;
            richPanel.FileOpenClick += RichPanel_FileOpenClick;
            richPanel.FileSaveClick += RichPanel_FileSaveClick;
            InitRichEdit2();
            richPanel.TextBox.TextChanged += RichTextBox_TextChanged;
        }

        private void RichTextBox_TextChanged(object? sender, EventArgs e)
        {
            var s = "RichTextBox Text Changed.";
            Application.LogReplace(s,s);
        }

        private void RichTextBox_EnterPressed(object? sender, EventArgs e)
        {
            var s = "RichTextBox Enter pressed.";
            Application.LogReplace(s, s);
        }

        private void RichPanel_FileSaveClick(object? sender, EventArgs e)
        {
            const string FileDialogFilter =
                "HTML Files (*.html;*.htm)|*.html;*.htm|"+
                "TXT Files (*.txt)|*.txt|" +
                "XML Files (*.xml)|*.xml";

            using SaveFileDialog dialog = new()
            {
                Filter = FileDialogFilter,
            };

            if (dialog.ShowModal(this) != ModalResult.Accepted)
                return;

            if (richPanel.TextBox.SaveFile(dialog.FileName!, RichTextFileType.Any))
                Application.Log($"Saved to file: {dialog.FileName}");
            else
                Application.Log($"Error saving to file: {dialog.FileName}");
        }

        private void RichPanel_FileOpenClick(object? sender, EventArgs e)
        {
            Application.Log("File.Open");
        }

        private void RichPanel_FileNewClick(object? sender, EventArgs e)
        {
            Application.Log("File.New");
        }

        internal string GetFontStatus()
        {
            //var position = richEdit.GetInsertionPoint();
            //var fs = richEdit.GetStyle(position);
            //var fontInfo = fs.GetFontInfo();
            //return fontInfo.ToString();
            return string.Empty;
        }

        private void RichEdit_KeyDown(object? sender, KeyEventArgs e)
        {
            static void Test()
            {
            }

            if (KnownKeys.RunTest.Run(e, Test))
                return;
            richPanel.TextBox.HandleAdditionalKeys(e);
        }

        private void SetDoubleMinMMButton_Click(object? sender, EventArgs e)
        {
            doubleEdit.TextBox.Text = ErrorMinValueTextDouble;
        }

        private void SetDoubleMaxPPButton_Click(object? sender, EventArgs e)
        {
            doubleEdit.TextBox.Text = ErrorMaxValueTextDouble;
        }

        private void SetDoubleMinButton_Click(object? sender, EventArgs e)
        {
            doubleEdit.TextBox.Text = MinValueTextDouble;
        }

        private void SetDoubleMaxButton_Click(object? sender, EventArgs e)
        {
            doubleEdit.TextBox.Text = MaxValueTextDouble;
        }

        public bool LogPosition { get; set; }

        private void TextBox_CurrentPositionChanged(object? sender, EventArgs e)
        {
            if (LogPosition && sender is TextBox control)
                LogTextBoxPosition(control);
        }

        private void LogTextBoxPosition(TextBox control)
        {
            var currentPos = control.CurrentPosition;
            if (currentPos is null)
                return;
            var name = control.Name ?? control.GetType().Name;
            var prefix = $"{name}.CurrentPos:";

            // var fontStatus = string.Empty;
            // if (control == richEdit)
            //    fontStatus = GetFontStatus();

            site?.LogEventSmart($"{prefix} {currentPos.Value+1}", prefix);
        }

        private void Application_Idle(object? sender, EventArgs e)
        {
            if (tab1.Visible)
            {
                textBox.IdleAction();
            }

            if (tab2.Visible)
            {
                multiLineTextBox.IdleAction();
            }

            if (tab3.Visible)
            {
                // richEdit.IdleAction();
            }
        }

        private void TextBox_TextMaxLength(object? sender, EventArgs e)
        {
            site?.LogEvent("TextBox: Text max length reached");
        }

        private void MaxLengthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = maxLengthEdit.TextBox.TextAsNumberOrDefault<uint>(0);
            textBox.MaxLength = (int)value;
            textBox.RunDefaultValidation();
        }

        private void MinLengthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = minLengthEdit.TextBox.TextAsNumberOrDefault<uint>(0);
            textBox.MinLength = (int)value;
            textBox.RunDefaultValidation();
        }

        private void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            var name = (sender as Control)?.Name ?? sender.GetType().Name;
            site?.LogEvent($"Focused: {name}" );
        }

        private void MultiLineTextBox_TextUrl(object? sender, UrlEventArgs e)
        {
            site?.LogEvent("TextBox: Url clicked =>" + e.Url);
            if(e.Modifiers != ModifierKeys.Control)
                site?.LogEvent("Use Ctrl+Click to open in the default browser" + e.Url);
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void ReportValueChanged(object? sender, EventArgs e)
        {
            var textBox = (sender as ValueEditorCustom)?.TextBox;
            textBox ??= sender as TextBox;
            if (textBox is null)
                return;
            var name = (sender as Control)?.Name;
            var value = textBox.Text;
            string prefix;
            if (name is null)
                prefix = "TextBox: ";
            else
                prefix = $"{name}: ";

            var asNumber = textBox.TextAsNumber;

            if (asNumber is not null)
                asNumber = $" => {asNumber} | {asNumber.GetType().Name}";

            site?.LogEventSmart($"{prefix}{value}{asNumber}", prefix);
        }

        private void AddLetterAButton_Click(object? sender, EventArgs e)
        {
            textBox.Text += "A";
        }

        internal static void GetWordIndex(
            string s,
            string word,
            out int startIndex,
            out int endIndex)
        {
            startIndex = s.IndexOf(word);
            if (startIndex < 0)
            {
                endIndex = -1;
                return;
            }
            endIndex = startIndex + word.Length;
        }

        internal void InitRichEdit()
        {
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
            taBig.SetFontPointSize((int)Font.Default.SizeInPoints + 15);

            var taUnderlined2 = TextBox.CreateTextAttr();
            taUnderlined2.SetFontUnderlinedEx(
                TextBoxTextAttrUnderlineType.Special,
                Color.Red);

            List<object> list = new()
            {
                "Text color is ", taTextColorRed, "red", taDefault, ".\n",
                "Background color is ", taBackColorYellow, "yellow", taDefault, ".\n",
                "Font is ", "<u>", "underlined", "</u>", ".\n",
                "Font is ", "<b>", "bold", "</b>", ".\n",
                "Font is ", "<i>", "italic", "</i>", ".\n",
                "Font is ", taStrikeOut, "strikeout", taDefault, ".\n",
                "Font is ", taBig, "big", taDefault, ".\n",
                "Font is ", taUnderlined2, "special underlined", taDefault, ".\n",
                "This is url: ",taUrl, homePage, taDefault, ".\n",
                "\n",
//                "Keys:\n",
//                "Ctrl+B - Toggle Bold style.\n",
//                "Ctrl+I - Toggle Italic style.\n",
//                "Ctrl+U - Toggle Underline style.\n",
//                "Ctrl+Shift+L - Left Align\n",
//                "Ctrl+Shift+E - Center Align.\n",
//                "Ctrl+Shift+R - Right Align.\n",
//                "Ctrl+Shift+J - Justify.\n",
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
                richEdit.WriteText(sUnorderedListItem+ i + "\n");
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

        public void InitRichEdit2()
        {
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

            r.BeginFontSize(14);

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

            r.WriteImage(KnownSvgImages.GetForSize(24).ImgMessageBoxInformation.AsImage(24)!);
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

            r.BeginFontSize(14);
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
            r.WriteText("This is my first item. Note that RichTextBox can apply"+
                " numbering and bullets automatically based on list styles, but this"+
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
            r.WriteText("Style sheets with named character and paragraph styles,"+
                " and control for applying named styles.");
            r.NewLine();
            r.EndSymbolBullet();

            r.BeginSymbolBullet("*", 100, 60);
            r.WriteText("A design that can easily be extended to other content types,"+
                " ultimately with text boxes, tables, controls, and so on.");
            r.NewLine();
            r.EndSymbolBullet();

            // Make a style suitable for showing a URL
            var urlStyle = RichTextBox.CreateRichAttr(); 
            urlStyle.SetTextColor(Color.Blue);
            urlStyle.SetFontUnderlined(true);

            r.WriteText("RichTextBox can also display URLs, such as this one: ");
            r.BeginStyle(urlStyle);
            r.BeginURL("http://www.alternet-ui.com");
            r.WriteText("Alternet UI Web Site");
            r.EndURL();
            r.EndStyle();
            r.WriteText(". Click on the URL to generate an event.");

            r.NewLine();

            r.WriteText("Note: this sample content was generated programmatically"+
                " from within the demo. The images were loaded from resources. Enjoy RichTextBox!\n");

            r.EndParagraphSpacing();

            r.EndSuppressUndo();

            r.EndUpdate();
        }
    }

    public static class Extensions
    {
    }
}