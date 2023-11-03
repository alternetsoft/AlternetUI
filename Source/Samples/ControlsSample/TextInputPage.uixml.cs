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
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. Suspendisse tincidunt orci vitae arcu congue commodo. Proin fermentum rhoncus dictum.";

        private readonly CardPanelHeader panelHeader = new();
        private readonly RichTextBox richEdit = new()
        {
            Name = "richEdit",
            SuggestedSize = new Size(350, 250),
            Margin = new Thickness(0, 0, 0, 5),
        };
        private readonly TextBoxAndLabel numberSignedEdit = new("1. Signed (short)");
        private readonly TextBoxAndLabel numberUnsignedEdit = new("2. Unsigned (byte)");
        private readonly TextBoxAndLabel numberFloatEdit = new("3. Signed (double)");
        private readonly TextBoxAndLabel unsignedFloatEdit = new("4. Unsigned (double)");
        private readonly TextBoxAndLabel numberHexEdit = new("5. Unsigned Hex (uint)");

        private IPageSite? site;

        public TextInputPage()
        {
            InitializeComponent();

            ControlSet.New(
                numberSignedEdit,
                numberUnsignedEdit,
                numberFloatEdit,
                unsignedFloatEdit,
                numberHexEdit).Margin(0,5,5,5).Parent(numbersPanel).InnerSuggestedWidth(200);

            panelHeader.Add("TextBox", tab1);
            panelHeader.Add("Memo", tab2);
            panelHeader.Add("RichEdit", tab3);
            panelHeader.Add("Numbers", tab4);
            tabControl.Children.Prepend(panelHeader);
            panelHeader.SelectFirstTab();

            // ==== textBox

            textBox.EmptyTextHint = "Sample Hint";
            textBox.Text = "sample text";
            textBox.ValidatorReporter = textImage;
            textBox.TextMaxLength += TextBox_TextMaxLength;
            textBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;

            // ==== multiLineTextBox

            multiLineTextBox.Text = LoremIpsum;
            multiLineTextBox.TextUrl += MultiLineTextBox_TextUrl;
            multiLineTextBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;

            // ==== numberSignedTextBox

            numberSignedEdit.TextBox.UseValidator<short>();
            numberSignedEdit.TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);

            // ==== numberUnsignedTextBox

            // We need to apply min and max values before ValidatorErrorText
            // is assigned as they are used in error text.
            numberUnsignedEdit.TextBox.MinValue = 2;
            // 2000 is greater than Byte can hold. It is assigned here for testing purposes.
            // Actual max is a minimal of Byte.MinValue and TextBox.MaxValue.
            numberUnsignedEdit.TextBox.MaxValue = 2000;
            numberUnsignedEdit.TextBox.UseValidator<byte>();
            numberUnsignedEdit.TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);

            // ==== numberFloatTextBox

            numberFloatEdit.TextBox.UseValidator<double>();
            numberFloatEdit.TextBox.SetErrorText(ValueValidatorKnownError.FloatIsExpected);

            // ==== unsignedFloatTextBox

            unsignedFloatEdit.TextBox.MinValue = 0d;
            unsignedFloatEdit.TextBox.UseValidator<double>();
            unsignedFloatEdit.TextBox.SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);

            // ==== numberHexTextBox

            numberHexEdit.TextBox.NumberStyles = NumberStyles.HexNumber;
            numberHexEdit.TextBox.Validator = TextBox.CreateValidator(ValueValidatorKind.UnsignedHex);
            numberHexEdit.TextBox.DataType = typeof(uint);
            numberHexEdit.TextBox.SetErrorText(ValueValidatorKnownError.HexNumberIsExpected);

            // ==== Other initializations

            void BindTextChanged(TextBox control)
            {
                control.TextChanged += ReportValueChanged;
                control.TextChanged += ValidateFormat;
            }

            ControlSet.New(
                numberHexEdit.TextBox,
                numberFloatEdit.TextBox,
                unsignedFloatEdit.TextBox,
                numberUnsignedEdit.TextBox,
                numberSignedEdit.TextBox,
                textBox).Action<TextBox>(BindTextChanged);          

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
                numberSignedEdit.Label,
                numberUnsignedEdit.Label,
                numberFloatEdit.Label,
                unsignedFloatEdit.Label,
                numberHexEdit.Label)
                .SuggestedWidthToMax();

            ControlSet.New(textAlignLabel, minLengthLabel, maxLengthLabel).SuggestedWidthToMax();

            ControlSet.New(textAlignComboBox, minLengthBox, maxLengthBox).SuggestedWidthToMax();

            // !!! update UseValidator to support unsigned int
            // !!! but need to set MinValue = 0
            minLengthBox.DataType = typeof(int);
            minLengthBox.Validator = TextBox.CreateValidator(ValueValidatorKind.UnsignedInt);
            minLengthBox.TextChanged += MinLengthBox_TextChanged;

            maxLengthBox.DataType = typeof(int);
            maxLengthBox.Validator = TextBox.CreateValidator(ValueValidatorKind.UnsignedInt);
            maxLengthBox.TextChanged += MaxLengthBox_TextChanged;

            Application.Current.Idle += Application_Idle;

            setDoubleMaxButton.Click += SetDoubleMaxButton_Click;
            setDoubleMinButton.Click += SetDoubleMinButton_Click;
            setDoubleMaxPPButton.Click += SetDoubleMaxPPButton_Click;
            setDoubleMinMMButton.Click += SetDoubleMinMMButton_Click;

            // ==== richEdit

            richEdit.Parent = richEditParent;
            richEdit.CurrentPositionChanged += TextBox_CurrentPositionChanged;
            richEdit.KeyDown += RichEdit_KeyDown;
            InitRichEdit();
        }

        string GetFontStatus()
        {
            var position = richEdit.GetInsertionPoint();
            var fs = richEdit.GetStyle(position);
            var style = fs.GetFontStyle();

            var underlined = style.HasFlag(FontStyle.Underlined) ? "underlined" : string.Empty;
            var strikeout = style.HasFlag(FontStyle.Strikethrough) ? "strikethrough" : string.Empty;
            var italic = style.HasFlag(FontStyle.Italic) ? "italic" : string.Empty;
            var bold = style.HasFlag(FontStyle.Bold) ? "bold" : string.Empty;

            var result = $"{fs.GetFontFaceName()} {fs.GetFontSize()} {bold} {underlined} {strikeout} {italic}";

            return result;
        }

        private void RichEdit_KeyDown(object? sender, KeyEventArgs e)
        {
            richEdit.HandleRichEditKeys(e);

            void Test()
            {
                richEdit.SelectionSetColor(Color.White, Color.Red);
            }

            if (KnownKeys.RunTest.Run(e, Test))
                return;
        }

        private void SetDoubleMinMMButton_Click(object? sender, EventArgs e)
        {
            numberFloatEdit.TextBox.Text = ErrorMinValueTextDouble;
        }

        private void SetDoubleMaxPPButton_Click(object? sender, EventArgs e)
        {
            numberFloatEdit.TextBox.Text = ErrorMaxValueTextDouble;
        }

        private void SetDoubleMinButton_Click(object? sender, EventArgs e)
        {
            numberFloatEdit.TextBox.Text = MinValueTextDouble;
        }

        private void SetDoubleMaxButton_Click(object? sender, EventArgs e)
        {
            numberFloatEdit.TextBox.Text = MaxValueTextDouble;
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

            var fontStatus = string.Empty;

            if (control == richEdit)
                fontStatus = GetFontStatus();

            site?.LogEventSmart($"{prefix} {currentPos.Value+1} {fontStatus}", prefix);
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
                richEdit.IdleAction();
            }
        }

        private void TextBox_TextMaxLength(object? sender, EventArgs e)
        {
            site?.LogEvent("TextBox: Text max length reached");
        }

        private void MaxLengthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Improve TextBox binding to use DataType

            var value = maxLengthBox.TextAsNumber;
            if (value is null)
                textBox.MaxLength = 0;
            else
                textBox.MaxLength = (int)value;
            ValidateFormat(textBox);
        }

        private void MinLengthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = minLengthBox.TextAsNumber;
            if (value is null)
                textBox.MinLength = 0;
            else
                textBox.MinLength = (int)value;
            ValidateFormat(textBox);
        }

        private void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            var name = (sender as Control)?.Name ?? sender.GetType().Name;
            site?.LogEvent($"Focused: {name}" );
        }

        private void MultiLineTextBox_TextUrl(object? sender, EventArgs e)
        {
            string? url = multiLineTextBox.DoCommand("GetReportedUrl")?.ToString();
            site?.LogEvent("TextBox: Url clicked =>" + url);
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void ValidateFormat(object? sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox editor)
                return;
            ValidateFormat(editor);
        }

        private void ValidateFormat(TextBox editor)
        {
            if (editor.ReportErrorEmptyText())
                return;

            if (editor.ReportErrorBadNumber())
                return;

            if (editor.ReportErrorMinMaxLength())
                return;
            
            editor.ReportValidatorError(false);
        }

        private void ReportValueChanged(object? sender, TextChangedEventArgs e)
        {
            var textBox = (sender as TextBox)!;
            var name = (sender as Control)?.Name;
            var value = textBox.Text;
            string prefix;
            if (name is null)
                prefix = "TextBox: ";
            else
                prefix = $"{name}: ";

            var asNumber = textBox.TextAsNumber;

            if (asNumber is not null)
            {
                asNumber += $"| {asNumber.GetType().Name}";
            }
            else
                asNumber = "null";

            site?.LogEventSmart($"{prefix}{value} => {asNumber}", prefix);
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

        private void InitRichEdit()
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
                "Font is ", taUnderlined, "underlined", taDefault, ".\n",
                "Font is ", taBold, "bold", taDefault, ".\n",
                "Font is ", taItalic, "italic", taDefault, ".\n",
                "Font is ", taStrikeOut, "strikeout", taDefault, ".\n",
                "Font is ", taBig, "big", taDefault, ".\n",
                "Font is ", taUnderlined2, "special underlined", taDefault, ".\n",
                "This is url: ", taUrl, homePage, taDefault, ".\n",
                "\n",
                "Keys:\n",
                "Ctrl+B - Toggle Bold style.\n",
                "Ctrl+I - Toggle Italic style.\n",
                "Ctrl+U - Toggle Underline style.\n",
                "Ctrl+Shift+L - Left Align\n",
                "Ctrl+Shift+E - Center Align.\n",
                "Ctrl+Shift+R - Right Align.\n",
                "Ctrl+Shift+J - Justify.\n",
            };

            richEdit.AutoUrl = true;

            richEdit.DoInsideUpdate(() =>
            {
                richEdit.AppendTextAndStyles(list);
                richEdit.AppendNewLine();
                richEdit.Refresh();
            });

            /*
            const string sUnorderedListItem = "Unordered List Item";
            const string sOrderedListItem = "Ordered List Item";
            
            for (int i = 1; i < 4; i++)
            {
                multiLineTextBox.SetDefaultStyle(taUnorderedList);
                multiLineTextBox.AppendText(sUnorderedListItem+i);
                multiLineTextBox.AppendNewLine();
            }
            multiLineTextBox.SetDefaultStyle(taDefault);
            multiLineTextBox.AppendNewLine();

            for (int i = 1; i < 4; i++)
            {
                multiLineTextBox.SetDefaultStyle(taOrderedList);
                multiLineTextBox.AppendText(sOrderedListItem + i + "\n");
                multiLineTextBox.SetDefaultStyle(taDefault);
                multiLineTextBox.AppendNewLine();
            }
            multiLineTextBox.SetDefaultStyle(taDefault);*/
        }
    }

    public static class Extensions
    {
        public static ControlSet InnerSuggestedWidth(this ControlSet controlSet, double value)
        {
            foreach (var item in controlSet.Items)
            {
                if (item is ControlAndLabel control)
                    control.InnerSuggestedWidth = value;
            }
            return controlSet;
        }
    }
}