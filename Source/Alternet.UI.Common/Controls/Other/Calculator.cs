using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.CodeAnalysis.CSharp.Scripting;

/*

Thanks to https://github.com/neoxeo for the calculator sample
https://github.com/alternetsoft/AlternetUI/issues/157.
This control is based on his idea, but for the evaluation we use
CSharpScript.EvaluateAsync as it gives us more power.

*/

namespace Alternet.UI
{
    /// <summary>
    /// Calculator control with buttons and display.
    /// </summary>
    public partial class Calculator : HiddenBorder
    {
        private const string ButtonTextPlusMinus = " \u00B1";
        private const string ButtonTextDivide = "/";
        private const string ButtonTextMultiply = "*";
        private const string ButtonTextClear = "AC";
        private const string ButtonTextClearLast = "CE";

        private static bool engineInitialized;

        private readonly TextBoxAndButton displayTextBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="Calculator"/> class.
        /// </summary>
        public Calculator()
        {
            if (!engineInitialized)
            {
                CSharpScript.EvaluateAsync("2");
                engineInitialized = true;
            }

            Layout = LayoutStyle.Vertical;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Padding = 10;
            MinChildMargin = 10;

            displayTextBox = new TextBoxAndButton
            {
                ButtonsVisible = false,
                SuggestedWidth = 300,
                Parent = this,
            };

            displayTextBox.DelayedTextChanged += (s, e) =>
            {
                displayTextBox.MainControl.ReportValidatorError(false);
            };

            var buttonGrid = new Grid
            {
                RowColumnCount = (5, 4),
            };

            string[] buttonLabels =
            {
            ButtonTextClear, "(", ")", ButtonTextDivide,
            "7", "8", "9", ButtonTextMultiply,
            "4", "5", "6", "-",
            "1", "2", "3", "+",
            ButtonTextPlusMinus, "0", ".", "=",
            };

            for (int i = 0; i < buttonLabels.Length; i++)
            {
                var button = new Button
                {
                    Text = buttonLabels[i],
                    Margin = 2,
                };

                int row = i / 4;
                int col = i % 4;

                Grid.SetRowColumn(button, row, col);
                button.Parent = buttonGrid;

                button.Click += (sender, e) =>
                {
                    ButtonClickHandler(button.Text, displayTextBox);
                };
            }

            buttonGrid.Parent = this;
        }

        /// <summary>
        /// Gets display control.
        /// </summary>
        public TextBoxAndButton DisplayTextBox => displayTextBox;

        private static void ButtonClickHandler(string buttonText, TextBoxAndButton displayTextBox)
        {
            switch (buttonText)
            {
                case ButtonTextClear:
                    displayTextBox.TextBox.Clear();
                    break;
                case ButtonTextClearLast:
                    var length = displayTextBox.Text.Length;
                    if (length > 0)
                        displayTextBox.Text = displayTextBox.Text.Remove(length - 1, 1);
                    break;

                case "=":
                    try
                    {
                        object? result = CSharpScript.EvaluateAsync(displayTextBox.Text).Result;
                        displayTextBox.Text = result?.ToString() ?? string.Empty;
                        displayTextBox.MainControl.ReportValidatorError(false);
                    }
                    catch (Exception e)
                    {
                        displayTextBox.MainControl
                            .ReportValidatorError(true, $"Evaluation error: {e.Message}");
                    }

                    break;

                case ButtonTextMultiply:
                    displayTextBox.Text += "*";
                    break;
                case ButtonTextDivide:
                    displayTextBox.Text += "/";
                    break;
                case ButtonTextPlusMinus:
                    if (displayTextBox.Text.StartsWith("-"))
                    {
                        displayTextBox.Text = displayTextBox.Text.Remove(0, 1);
                    }
                    else
                    {
                        displayTextBox.Text = "-" + displayTextBox.Text;
                    }

                    break;
                default:
                    displayTextBox.Text += buttonText;
                    break;
            }
        }
    }
}
