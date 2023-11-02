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
        private const string LoremIpsum =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. Suspendisse tincidunt orci vitae arcu congue commodo. Proin fermentum rhoncus dictum.";

        private readonly CardPanelHeader panelHeader = new();
        private IPageSite? site;

        public TextInputPage()
        {
            InitializeComponent();

            panelHeader.Add("TextBox", tab1);
            panelHeader.Add("Memo", tab2);
            panelHeader.Add("RichEdit", tab3);
            panelHeader.Add("Numbers", tab4);
            tabControl.Children.Prepend(panelHeader);
            panelHeader.SelectFirstTab();

            // textBox

            textBox.EmptyTextHint = "Sample Hint";
            textBox.Text = "sample text";
            textBox.TextChanged += ReportValueChanged;
            textBox.TextChanged += ValidateFormat;
            textBox.ValidatorReporter = textImage;
            textBox.TextMaxLength += TextBox_TextMaxLength;

            // multiLineTextBox

            multiLineTextBox.Text = LoremIpsum;
            multiLineTextBox.TextUrl += MultiLineTextBox_TextUrl;

            // numberSignedTextBox

            numberSignedTextBox.Validator = TextBox.CreateValidator(ValueValidatorKind.SignedInt);
            numberSignedTextBox.DataType = typeof(short);
            numberSignedTextBox.TextChanged += ReportValueChanged;
            numberSignedTextBox.TextChanged += ValidateFormat;
            numberSignedTextBox.ValidatorReporter = numberSignedImage;
            numberSignedTextBox.ValidatorErrorText =
                numberSignedTextBox.GetKnownErrorText(ValueValidatorKnownError.NumberIsExpected);

            // numberUnsignedTextBox

            numberUnsignedTextBox.Validator = TextBox.CreateValidator(ValueValidatorKind.UnsignedInt);
            numberUnsignedTextBox.DataType = typeof(byte);
            numberUnsignedTextBox.TextChanged += ValidateFormat;
            numberUnsignedTextBox.TextChanged += ReportValueChanged;
            numberUnsignedTextBox.ValidatorReporter = numberUnsignedImage;
            // We need to apply min and max values before ValidatorErrorText
            // is assigned as they are used in error text.
            numberUnsignedTextBox.MinValue = 2;
            // 2000 is greater than Byte can hold. It is assigned here for testing purposes.
            // Actual max is a minimal of byte.MinValue and MaxValue property.
            numberUnsignedTextBox.MaxValue = 2000;
            numberUnsignedTextBox.ValidatorErrorText =
                numberUnsignedTextBox.GetKnownErrorText(ValueValidatorKnownError.NumberIsExpected);

            // numberFloatTextBox

            numberFloatTextBox.Validator = TextBox.CreateValidator(ValueValidatorKind.SignedFloat);
            numberFloatTextBox.DataType = typeof(double);
            numberFloatTextBox.TextChanged += ReportValueChanged;
            numberFloatTextBox.TextChanged += ValidateFormat;
            numberFloatTextBox.ValidatorReporter = numberFloatImage;
            numberFloatTextBox.ValidatorErrorText =
                numberFloatTextBox.GetKnownErrorText(ValueValidatorKnownError.FloatIsExpected);

            // numberHexTextBox

            numberHexTextBox.Validator = TextBox.CreateValidator(ValueValidatorKind.UnsignedHex);
            numberHexTextBox.DataType = typeof(uint);
            numberHexTextBox.NumberStyles = NumberStyles.HexNumber;
            numberHexTextBox.TextChanged += ReportValueChanged;
            numberHexTextBox.TextChanged += ValidateFormat;
            numberHexTextBox.ValidatorReporter = numberHexImage;

            // !! range output in error 
            numberHexTextBox.ValidatorErrorText =
                numberHexTextBox.GetKnownErrorText(ValueValidatorKnownError.HexNumberIsExpected);

            // Other initializations

            wordWrapComboBox.BindEnumProp(multiLineTextBox, nameof(TextBox.TextWrap));
            textAlignComboBox.BindEnumProp(textBox, nameof(TextBox.TextAlign));
            InitRichEdit();

            readOnlyCheckBox.BindBoolProp(textBox, nameof(TextBox.ReadOnly));
            passwordCheckBox.BindBoolProp(textBox, nameof(TextBox.IsPassword));
            hasBorderCheckBox.BindBoolProp(textBox, nameof(TextBox.HasBorder));
            logPositionCheckBox.BindBoolProp(this, nameof(LogPosition));

            bellOnErrorCheckBox.BindBoolProp(
                ValueValidatorFactory.Default,
                nameof(ValueValidatorFactory.BellOnError));

            ControlSet.New(numberSignedLabel, numberUnsignedLabel, numberFloatLabel, numberHexLabel)
                .SuggestedWidthToMax().VerticalAlignment(VerticalAlignment.Center);

            ControlSet.New(textAlignLabel, minLengthLabel, maxLengthLabel).SuggestedWidthToMax();

            ControlSet.New(textAlignComboBox, minLengthBox, maxLengthBox).SuggestedWidthToMax();

            var image = KnownSvgImages.GetWarningImage();

            ControlSet.New(
                textImage,
                numberSignedImage,
                numberUnsignedImage,
                numberFloatImage,
                numberHexImage).Visible(true).Action<PictureBox>(InitPictureBox)
                .VerticalAlignment(VerticalAlignment.Center);

            void InitPictureBox(PictureBox picture)
            {
                picture.Image = image;
                picture.ImageVisible = false;
                picture.ImageStretch = false;
                picture.TabStop = false;
                picture.GotFocus += Control_GotFocus;
            }

            minLengthBox.DataType = typeof(int);
            minLengthBox.Validator = TextBox.CreateValidator(ValueValidatorKind.UnsignedInt);
            minLengthBox.TextChanged += MinLengthBox_TextChanged;

            maxLengthBox.DataType = typeof(int);
            maxLengthBox.Validator = TextBox.CreateValidator(ValueValidatorKind.UnsignedInt);
            maxLengthBox.TextChanged += MaxLengthBox_TextChanged;

            Application.Current.Idle += Application_Idle;

            textBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;
            multiLineTextBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;
            richEdit.CurrentPositionChanged += TextBox_CurrentPositionChanged;
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
            if (string.IsNullOrEmpty(editor.Text))
            {
                editor.ReportValidatorError(!editor.AllowEmptyText);
                return;
            }

            var isOk = true;

            if (editor.DataTypeIsNumber())
            {
                var value = editor.TextAsNumber;
                isOk = value is not null;

                if (isOk) // min max not only for numbers
                {
                    // bool ReportErrorMinMaxValue()
                    var typeCode = editor.GetDataTypeCode();
                    var minValue = MathUtils.Max(AssemblyUtils.GetMinValue(typeCode), editor.MinValue);
                    var maxValue = MathUtils.Min(AssemblyUtils.GetMaxValue(typeCode), editor.MaxValue);
                    var valueInRange = MathUtils.ValueInRange(value, minValue, maxValue);
                    if (valueInRange is not null)
                        isOk = valueInRange.Value;
                    // report min max error text here.
                }
            }

            if (isOk && editor.ReportErrorMinMaxLength())
                return;
            
            editor.ReportValidatorError(!isOk);
        }

        private void ReportValueChanged(object? sender, TextChangedEventArgs e)
        {
            var name = (sender as Control)?.Name;
            var value = ((TextBox)sender!).Text;
            string prefix;
            if (name is null)
                prefix = "TextBox: ";
            else
                prefix = $"{name}: ";

            site?.LogEventSmart($"{prefix}{value}", prefix);
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
                "Font is ", taBig, "big", taDefault, ".\n",
                "Font is ", taUnderlined2, "special underlined", taDefault, ".\n",
                "This is url: ", taUrl, homePage, taDefault, ".\n",
            };

            richEdit.Clear();
            richEdit.AutoUrl = true;
            richEdit.IsRichEdit = true;

            richEdit.AppendTextAndStyles(list);
            richEdit.AppendNewLine();

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
    }
}