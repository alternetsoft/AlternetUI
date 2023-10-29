using System;
using System.Collections.Generic;
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

        private readonly IValueValidatorText ValidatorNumberSigned =
            ValueValidatorFactory.Default.CreateValueValidatorNum(ValueValidatorNumStyle.Signed);
        private readonly IValueValidatorText ValidatorNumberUnsigned =
            ValueValidatorFactory.Default.CreateValueValidatorNum(ValueValidatorNumStyle.Unsigned);
        private readonly IValueValidatorText ValidatorNumberFloat =
            ValueValidatorFactory.Default.CreateValueValidatorNum(ValueValidatorNumStyle.Float);
        private readonly IValueValidatorText ValidatorNumberHex =
            ValueValidatorFactory.Default.CreateValueValidatorNum(ValueValidatorNumStyle.Unsigned, 16);
        private readonly CardPanelHeader panelHeader = new();
        private IPageSite? site;

        public TextInputPage()
        {
            InitializeComponent();
            textBox.EmptyTextHint = "Sample Hint";
            textBox.Text = "sample text";
            multiLineTextBox.Text = LoremIpsum;
            multiLineTextBox.TextUrl += MultiLineTextBox_TextUrl;

            panelHeader.Add("TextBox", tab1);
            panelHeader.Add("Memo", tab2);
            panelHeader.Add("RichEdit", tab3);
            panelHeader.Add("Numbers", tab4);
            tabControl.Children.Prepend(panelHeader);
            panelHeader.SelectedTab = panelHeader.Tabs[0];

            wordWrapComboBox.BindEnumProp(multiLineTextBox, nameof(TextBox.TextWrap));
            textAlignComboBox.BindEnumProp(textBox, nameof(TextBox.TextAlign));
            RichEditButton_Click(null, EventArgs.Empty);

            readOnlyCheckBox.BindBoolProp(textBox, nameof(TextBox.ReadOnly));
            passwordCheckBox.BindBoolProp(textBox, nameof(TextBox.IsPassword));
            hasBorderCheckBox.BindBoolProp(textBox, nameof(TextBox.HasBorder));

            numberSignedTextBox.Validator = ValidatorNumberSigned;
            numberUnsignedTextBox.Validator = ValidatorNumberUnsigned;
            numberFloatTextBox.Validator = ValidatorNumberFloat;
            numberHexTextBox.Validator = ValidatorNumberHex;

            numberSignedTextBox.TextChanged += TextBox_ValueChanged;
            numberUnsignedTextBox.TextChanged += TextBox_ValueChanged;
            numberFloatTextBox.TextChanged += TextBox_ValueChanged;
            numberHexTextBox.TextChanged += TextBox_ValueChanged;

            numberSignedTextBox.TextChanged += NumberSignedTextBox_ValueChanged;
            numberUnsignedTextBox.TextChanged += NumberUnsignedTextBox_ValueChanged;
            numberFloatTextBox.TextChanged += NumberFloatTextBox_ValueChanged;
            numberHexTextBox.TextChanged += NumberHexTextBox_ValueChanged;

            numberSignedTextBox.DataType = typeof(short);
            numberUnsignedTextBox.DataType = typeof(byte);
            numberFloatTextBox.DataType = typeof(double);
            numberHexTextBox.DataType = typeof(int);

            numberSignedTextBox.ValidatorReporter = numberSignedImage;
            numberUnsignedTextBox.ValidatorReporter = numberUnsignedImage;
            numberFloatTextBox.ValidatorReporter = numberFloatImage;
            numberHexTextBox.ValidatorReporter = numberHexImage;

            var rangeFormat = " Range is [{0}..{1}].";

            numberSignedTextBox.ValidatorErrorText =
                $"A number is expected.{numberSignedTextBox.GetMinMaxRangeStr(rangeFormat)}";
            numberUnsignedTextBox.ValidatorErrorText =
                $"A number is expected.{numberUnsignedTextBox.GetMinMaxRangeStr(rangeFormat)}";
            numberFloatTextBox.ValidatorErrorText = "A floating point number is expected.";
            numberHexTextBox.ValidatorErrorText = "A hexadecimal number is expected.";

            noBellOnErrorCheckBox.BindBoolProp(ValueValidatorFactory.Default, nameof(ValueValidatorFactory.IsSilent));

            ControlSet numberLabels = new(
                numberSignedLabel,
                numberUnsignedLabel,
                numberFloatLabel,
                numberHexLabel);
            numberLabels.SuggestedWidthToMax().VerticalAlignment(VerticalAlignment.Center);

            ControlSet numberImages = new(
                numberSignedImage,
                numberUnsignedImage,
                numberFloatImage,
                numberHexImage);

            var imageSize = Toolbar.GetDefaultImageSize(Application.FirstWindow()!);

            var imageSet = KnownSvgImages.GetForSize(imageSize).ImgMessageBoxWarning;
            var image = imageSet.AsImage(imageSize);

            numberImages.Visible(true).Action<PictureBox>(InitPictureBox)
                .VerticalAlignment(VerticalAlignment.Center);

            void InitPictureBox(PictureBox picture)
            {
                picture.Image = image;
                picture.ImageVisible = false;
                picture.ImageStretch = false;
            }
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

            if (string.IsNullOrEmpty(editor.Text))
            {
                ReportValidatorError(editor.EmptyTextAllow);
                return;
            }

            var typeCode = editor.GetDataTypeCode();
            if (typeCode == TypeCode.String)
                return;

            var isOk = StringUtils.TryParseNumber(
                typeCode,
                editor.Text,
                editor.NumberStyles,
                editor.FormatProvider,
                out _);

            ReportValidatorError(isOk);

            void ReportValidatorError(bool ok, string? errorText = null)
            {

                var hint = string.Empty;
                if (ok)
                {
                    editor.ResetBackgroundColor();
                    editor.ResetForegroundColor();
                }
                else
                {
                    editor.BackgroundColor = TextBox.DefaultErrorBackgroundColor;
                    editor.ForegroundColor = TextBox.DefaultErrorForegroundColor;
                    hint = errorText ?? editor.ValidatorErrorText;
                    hint ??= TextBox.DefaultValidatorErrorText; 
                }

                var reporter = editor.ValidatorReporter as PictureBox; // IValidatorReporter

                if (reporter is not null)
                {
                    reporter.ToolTip = hint;
                    reporter.ImageVisible = !ok;
                }
            }
        }

        private void NumberSignedTextBox_ValueChanged(object? sender, TextChangedEventArgs e)
        {
            ValidateFormat(sender, e);
        }

        private void NumberUnsignedTextBox_ValueChanged(object? sender, TextChangedEventArgs e)
        {
            ValidateFormat(sender, e);
        }

        private void NumberFloatTextBox_ValueChanged(object? sender, TextChangedEventArgs e)
        {
            ValidateFormat(sender, e);
        }

        private void NumberHexTextBox_ValueChanged(object? sender, TextChangedEventArgs e)
        {
            ValidateFormat(sender, e);
        }

        private void TextBox_ValueChanged(object? sender, TextChangedEventArgs e)
        {
            var name = (sender as Control)?.Name;
            var value = ((TextBox)sender!).Text;
            if (name is null)
                site?.LogEvent($"TextBox: {value}");
            else
                site?.LogEvent($"{name}: {value}");
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

        private void RichEditButton_Click(object? sender, EventArgs e)
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