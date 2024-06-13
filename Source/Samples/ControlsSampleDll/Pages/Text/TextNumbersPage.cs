﻿using System;
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

        private readonly ValueEditorInt16 shortEdit = new("Int16", -25);
        private readonly ValueEditorByte byteEdit = new("Byte", 230);
        private readonly ValueEditorDouble doubleEdit = new("Double", -15.3);
        private readonly ValueEditorUDouble udoubleEdit = new("UDouble", 1002);
        private readonly HexEditorUInt32 uint32HexEdit = new("UInt32 Hex", 0x25E6);
        private readonly CheckBox bellOnErrorCheckBox = new("Bell On Error");
        private readonly Label label = new("Try to enter invalid numbers");

        private readonly ValueEditorByte twoDigitsEdit = new("Two digits", 15);

        public TextNumbersPage()
        {
            Margin = 10;

            static void BindTextChanged(ValueEditorCustom control)
            {
                control.TextChanged += TextInputPage.ReportValueChanged;
            }

            twoDigitsEdit.TextBox.MaxLength = 2;
            twoDigitsEdit.TextBox.ErrorsChanged += TextInputPage.TextBox_ErrorsChanged;

            Group(shortEdit, byteEdit, doubleEdit, udoubleEdit, uint32HexEdit, twoDigitsEdit)
                .Margin(0, 5, 5, 5).Parent(this).InnerSuggestedWidth(200)
                .Action<ValueEditorCustom>(BindTextChanged).LabelSuggestedWidthToMax();

            bellOnErrorCheckBox.BindBoolProp(
                ValueValidatorFactory.Default,
                nameof(ValueValidatorFactory.BellOnError));

            label.Parent = this;
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