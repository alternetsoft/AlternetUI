using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class TextNumbersPage : VerticalStackPanel
    {
        private const string ErrorMinValueTextDouble = "-2.7976931348623157E+308";
        private const string ErrorMaxValueTextDouble = "2.7976931348623157E+308";
        private const string MinValueTextDouble = "-1.7976931348623157E+308";
        private const string MaxValueTextDouble = "1.7976931348623157E+308";

        [IsTextLocalized(true)]
        private readonly ValueEditorInt16 shortEdit = new("Int16", -25);

        [IsTextLocalized(true)]
        private readonly ValueEditorByte byteEdit = new("Byte", 230);

        [IsTextLocalized(true)]
        private readonly ValueEditorDouble doubleEdit = new("Double", -15.3);

        [IsTextLocalized(true)]
        private readonly ValueEditorUDouble unsignedDoubleEdit = new("UDouble", 1002);

        [IsTextLocalized(true)]
        private readonly HexEditorUInt32 uint32HexEdit = new("UInt32 Hex", 0x25E6);

        [IsTextLocalized(true)]
        private readonly Label label = new("Try to enter invalid numbers");

        private readonly ValueEditorByte twoDigitsEdit = new("Two digits", 15);

        public TextNumbersPage()
        {
            Padding = 10;

            static void BindTextChanged(ValueEditorCustom control)
            {
                control.TextChanged += TextInputPage.ReportValueChanged;
            }

            twoDigitsEdit.TextBox.MaxLength = 2;
            twoDigitsEdit.TextBox.ErrorsChanged += TextInputPage.TextBox_ErrorsChanged;

            Group(shortEdit, byteEdit, doubleEdit, unsignedDoubleEdit, uint32HexEdit, twoDigitsEdit)
                .Margin(0, 5, 5, 5).Parent(this).InnerSuggestedWidth(200)
                .ParentForeColor(true).ParentBackColor(true)
                .Action<ValueEditorCustom>(BindTextChanged).LabelSuggestedWidthToMax();

            var horzLine = new HorizontalLine();
            horzLine.Parent = this;
            horzLine.Margin = (0,10,0,10);

            label.Parent = this;
            
            RichToolTip toolTip = new();
            toolTip.Margin = (0,10,0,10);
            toolTip.VerticalAlignment = VerticalAlignment.Fill;
            toolTip.Parent = this;
            toolTip.ParentBackColor = true;
            toolTip.ParentForeColor = true;
            ToolTipProvider = toolTip;

            BackgroundColor = Color.Gainsboro;
            ForegroundColor = Color.Black;
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
    }
}