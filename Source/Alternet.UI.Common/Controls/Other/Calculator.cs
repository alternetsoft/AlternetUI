using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

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
        /// <summary>
        /// Gets or sets default minimum button size.
        /// </summary>
        public static SizeD DefaultMinButtonSize = (50, 40);

        /// <summary>
        /// Gets or sets default distance between buttons of the calculator.
        /// </summary>
        public static Coord DefaultButtonDistance = 2;

        /// <summary>
        /// Gets or sets default distance between calculator display and buttons.
        /// </summary>
        public static Coord DefaultDistanceToDisplay = 10;

        private const string ButtonTextPlusMinus = " \u00B1";
        private const string ButtonTextDivide = "/";
        private const string ButtonTextMultiply = "*";
        private const string ButtonTextClear = "AC";
        private const string ButtonTextClearLast = "CE";

        private static bool engineInitialized;

        private readonly TextBoxAndButton displayTextBox;
        private readonly Grid buttonGrid;
        private readonly List<AbstractControl> buttons = new();
        private readonly ControlSet buttonSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Calculator"/> class.
        /// </summary>
        public Calculator()
        {
            InitFormulaEngine();

            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Padding = 10;

            buttonGrid = new Grid
            {
                RowColumnCount = (6, 4),
            };

            displayTextBox = new TextBoxAndButton
            {
                ButtonsVisible = false,
                InnerOuterBorder = InnerOuterSelector.Outer,
                RowColumn = (0, 0),
                ColumnSpan = 4,
                Parent = buttonGrid,
                Margin = (0, 0, 0, DefaultDistanceToDisplay),
            };

            displayTextBox.DelayedTextChanged += (s, e) =>
            {
                displayTextBox.MainControl.ReportValidatorError(false);
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
                var button = CreateButton();
                buttons.Add(button);
                button.Text = buttonLabels[i];

                int row = i / 4;
                int col = i % 4;

                Thickness margin = Thickness.Empty;

                if (col != 0)
                    margin.Left = DefaultButtonDistance;
                if (col != 3)
                    margin.Right = DefaultButtonDistance;
                if (row != 0)
                    margin.Top = DefaultButtonDistance;
                if (row != 4)
                    margin.Bottom = DefaultButtonDistance;

                button.Margin = margin;

                Grid.SetRowColumn(button, row + 1, col);
                button.Parent = buttonGrid;

                button.Click += (sender, e) =>
                {
                    ButtonClickHandler(button.Text, displayTextBox);
                };
            }

            buttonSet = new(buttons);

            buttonGrid.Parent = this;
        }

        /// <summary>
        /// Gets or sets script options used in <see cref="CSharpScript.EvaluateAsync"/>
        /// which is called from <see cref="Evaluate"/>. Default is Null.
        /// </summary>
        [Browsable(false)]
        public virtual ScriptOptions? FormulaOptions { get; set; } = null;

        /// <summary>
        /// Gets or sets script globals used in <see cref="CSharpScript.EvaluateAsync"/>
        /// which is called from <see cref="Evaluate"/>. Default is Null.
        /// </summary>
        [Browsable(false)]
        public virtual object? FormulaGlobals { get; set; } = null;

        /// <summary>
        /// Gets or sets script globals type used in <see cref="CSharpScript.EvaluateAsync"/>
        /// which is called from <see cref="Evaluate"/>. Default is Null.
        /// </summary>
        [Browsable(false)]
        public virtual Type? FormulaGlobalsType { get; set; } = null;

        /// <summary>
        /// Gets display control.
        /// </summary>
        [Browsable(false)]
        public TextBoxAndButton DisplayTextBox => displayTextBox;

        /// <summary>
        /// Gets panel with buttons.
        /// </summary>
        [Browsable(false)]
        public AbstractControl ButtonsPanel => buttonGrid;

        /// <summary>
        /// Gets collection of calculator buttons.
        /// </summary>
        [Browsable(false)]
        public IReadOnlyList<AbstractControl> Buttons => buttons;

        /// <summary>
        /// Gets collection of calculator buttons as <see cref="ControlSet"/>.
        /// </summary>
        [Browsable(false)]
        public ControlSet SetOfButtons => buttonSet;

        /// <summary>
        /// Initializes formula engine. Do not need to call it directly. It
        /// can be called from the application startup in order to preload formula
        /// engine libraries.
        /// </summary>
        public static void InitFormulaEngine()
        {
            if (!engineInitialized)
            {
                CSharpScript.EvaluateAsync("2");
                engineInitialized = true;
            }
        }

        /// <summary>
        /// Evaluates formula.
        /// </summary>
        /// <param name="formula">Formula to evaluate.</param>
        /// <returns></returns>
        public virtual object? Evaluate(string formula)
        {
            var result = CSharpScript.EvaluateAsync(
                formula,
                FormulaOptions,
                FormulaGlobals,
                FormulaGlobalsType).Result;
            return result;
        }

        /// <summary>
        /// Creates button used in the calculator.
        /// </summary>
        /// <returns></returns>
        public virtual AbstractControl CreateButton()
        {
            var result = new SpeedTextButton();
            result.UseTheme = SpeedButton.KnownTheme.StaticBorder;
            result.Padding = 5;
            result.MinimumSize = DefaultMinButtonSize;
            return result;
        }

        private void ButtonClickHandler(string buttonText, TextBoxAndButton displayTextBox)
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
                        object? result = Evaluate(displayTextBox.Text);
                        displayTextBox.TextBox.TextAsNumber = result;
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
