using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.Drawing;

/*

Thanks to https://github.com/neoxeo for the calculator sample
https://github.com/alternetsoft/AlternetUI/issues/157.
This control is based on his idea.

*/

namespace Alternet.UI
{
    /// <summary>
    /// Calculator control with buttons and display.
    /// </summary>
    [ControlCategory("Other")]
    public partial class Calculator : HiddenBorder
    {
        /// <summary>
        /// Gets or sets default minimum button size.
        /// </summary>
        public static SizeD DefaultMinButtonSize = (50, 40);

        /// <summary>
        /// Represents the default padding applied to buttons.
        /// </summary>
        public static Thickness DefaultButtonPadding = 5;

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

        private readonly TextBoxAndButton displayTextBox;
        private readonly Grid buttonGrid;
        private readonly List<AbstractControl> buttons = new();
        private readonly ControlSet buttonSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Calculator"/> class.
        /// </summary>
        public Calculator()
        {
            FormulaEngine.Init();

            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Padding = 10;

            buttonGrid = new Grid
            {
                RowColumnCount = (6, 4),
            };

            displayTextBox = new ()
            {
                ButtonsVisible = false,
                InnerOuterBorder = InnerOuterSelector.Outer,
                RowColumn = (0, 0),
                ColumnSpan = 4,
                Parent = buttonGrid,
                Margin = (0, 0, 0, DefaultDistanceToDisplay),
            };

            if(displayTextBox is TextBoxAndButton textBoxAndButton)
            {
                textBoxAndButton.MainControl.TabStop = false;
                textBoxAndButton.MainControl.CanSelect = false;
            }

            displayTextBox.MainControl.KeyDown += (s, e) =>
            {
                if(!e.HasModifiers && e.Key == Key.Return && WantReturn)
                {
                    DoActionCalcFormula();
                }
            };

            displayTextBox.DelayedTextChanged += (s, e) =>
            {
                displayTextBox.ReportError(false);
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
                button.DoubleClick += (sender, e) =>
                {
                    ButtonClickHandler(button.Text, displayTextBox);
                };
            }

            buttonSet = new(buttons);

            buttonGrid.Parent = this;
        }

        /// <summary>
        /// Gets or sets script options used in the formula evaluation. Default is Null.
        /// </summary>
        [Browsable(false)]
        public virtual object? FormulaOptions { get; set; }

        /// <summary>
        /// Gets or sets script global context used in the formula evaluation. Default is Null.
        /// </summary>
        [Browsable(false)]
        public virtual object? FormulaGlobalContext { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a "Return" key is processed.
        /// <see cref="DoActionCalcFormula"/> is called when "Return" key is pressed and
        /// <see cref="WantReturn"/> is <c>true</c> (default value).
        /// </summary>
        [DefaultValue(true)]
        public virtual bool WantReturn { get; set; } = true;

        /// <summary>
        /// Gets or sets script global type used in the formula evaluation. Default is Null.
        /// </summary>
        [Browsable(false)]
        public virtual Type? FormulaGlobalType { get; set; }

        /// <summary>
        /// Gets display control.
        /// </summary>
        [Browsable(false)]
        public AbstractControl DisplayTextBox => displayTextBox;

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
        /// Evaluates formula synchronously and returns the result.
        /// </summary>
        /// <param name="formula">Formula to evaluate.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.
        /// Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The evaluated result of the formula.</returns>
        public virtual object Evaluate(
            string formula,
            CancellationToken cancellationToken = default)
        {
            var result = EvaluateAsync(formula, cancellationToken).Result;
            return result;
        }

        /// <summary>
        /// Asynchronously evaluates the specified formula and returns the result.
        /// </summary>
        /// <remarks>The evaluation process uses the default formula options, global object,
        /// and global type configured for the current instance.</remarks>
        /// <param name="formula">The formula to evaluate.
        /// This must be a valid expression supported by the evaluation engine.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.
        /// Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the evaluated value of the formula.</returns>
        public virtual Task<object> EvaluateAsync(
            string formula,
            CancellationToken cancellationToken = default)
        {
            var result = FormulaEngine.EvaluateAsync(
                this,
                formula,
                FormulaOptions,
                FormulaGlobalContext,
                FormulaGlobalType,
                cancellationToken);
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
            result.Padding = DefaultButtonPadding;
            result.MinimumSize = DefaultMinButtonSize;
            result.IsClickRepeated = true;
            return result;
        }

        /// <summary>
        /// Evaluates the formula entered in the display text box and updates the result.
        /// </summary>
        /// <remarks>This method attempts to evaluate the formula provided in the display
        /// text box. If the evaluation is successful, the result is displayed
        /// and any validation errors are cleared. If an error
        /// occurs during evaluation, a validation error is reported with
        /// the error message.</remarks>
        public virtual void DoActionCalcFormula()
        {
            try
            {
                object? result = Evaluate(displayTextBox.Text);

                if(result is Exception exception)
                {
                    ReportError(exception);
                    return;
                }

                displayTextBox.Text = result.ToString();
                displayTextBox.ReportError(false);
            }
            catch (Exception e)
            {
                ReportError(e);
            }

            void ReportError(Exception e)
            {
                while(e.InnerException != null)
                    e = e.InnerException;
                displayTextBox.ReportError(true, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Clears all text in the formula.
        /// </summary>
        public virtual void DoActionClearAll()
        {
            displayTextBox.Text = string.Empty;
        }

        /// <summary>
        /// Clears the last character in the formula.
        /// </summary>
        public virtual void DoActionClearLast()
        {
            var length = displayTextBox.Text.Length;
            if (length > 0)
                displayTextBox.Text = displayTextBox.Text.Remove(length - 1, 1);
        }

        /// <inheritdoc/>
        public override bool SetFocus()
        {
            return false;
        }

        /// <summary>
        /// Toggles the sign of the formula.
        /// </summary>
        /// <remarks>If the formula starts with a negative sign ('-'),
        /// the sign is removed. Otherwise, a negative sign is prepended to the text.</remarks>
        public virtual void DoActionToggleSign()
        {
            if (displayTextBox.Text.StartsWith("-"))
            {
                displayTextBox.Text = displayTextBox.Text.Remove(0, 1);
            }
            else
            {
                displayTextBox.Text = "-" + displayTextBox.Text;
            }
        }

        /// <summary>
        /// Handles button click events by performing actions based on the button's text.
        /// </summary>
        /// <remarks>The method supports various button actions, including clearing text,
        /// removing the last character, evaluating expressions,  and appending mathematical
        /// operators or other characters. Specific
        /// actions are determined by the value of <paramref name="buttonText"/>.</remarks>
        /// <param name="buttonText">The text displayed on the button that was clicked.
        /// This determines the action to perform.</param>
        /// <param name="displayTextBox">The <see cref="TextBoxAndButton"/> control
        /// associated with the button, used to display or modify text.</param>
        private void ButtonClickHandler(string buttonText, AbstractControl displayTextBox)
        {
            switch (buttonText)
            {
                case ButtonTextClear:
                    DoActionClearAll();
                    break;
                case ButtonTextClearLast:
                    DoActionClearLast();
                    break;
                case "=":
                    DoActionCalcFormula();
                    break;
                case ButtonTextMultiply:
                    displayTextBox.Text += "*";
                    break;
                case ButtonTextDivide:
                    displayTextBox.Text += "/";
                    break;
                case ButtonTextPlusMinus:
                    DoActionToggleSign();
                    break;
                default:
                    displayTextBox.Text += buttonText;
                    break;
            }
        }
    }
}
