using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Calculator control with buttons and display.
    /// </summary>
    [ControlCategory(KnownControlCategory.Other)]
    public partial class Calculator : HiddenGenericBorder
    {
        /// <summary>
        /// Gets or sets whether the toggle sign button is visible in the calculator.
        /// </summary>
        public static bool DefaultShowToggleSignButton = false;

        /// <summary>
        /// Gets or sets whether the 'erase to the left' button is visible in the calculator.
        /// </summary>
        public static bool DefaultShowEraseLeftButton = true;

        /// <summary>
        /// Gets or sets whether clicking on a button will trigger repeated click events when the button is held down.
        /// </summary>
        public static bool DefaultIsClickRepeated = true;

        /// <summary>
        /// Gets or sets whether a double-click on a button should be treated as a single click.
        /// </summary>
        public static bool DefaultDoubleClickAsClick = false;

        /// <summary>
        /// Gets the text displayed on the "plus/minus" button.
        /// </summary>
        public static readonly string ButtonTextPlusMinus = " \u00B1";
        
        /// <summary>
        /// Gets the text displayed on the "divide" button.
        /// </summary>
        public static readonly string ButtonTextDivide = "/";
        
        /// <summary>
        /// Gets the text displayed on the "multiply" button.
        /// </summary>
        public static readonly string ButtonTextMultiply = "*";
        
        /// <summary>
        /// Gets the text displayed on the "clear" button.
        /// </summary>
        public static readonly string ButtonTextClear = "AC";
        
        /// <summary>
        /// Gets the text displayed on the "erase to the left" button.
        /// </summary>
        public static readonly string ButtonTextEraseLeft = "CE";

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

        private static readonly ButtonKind[] operatorButtons = new ButtonKind[]
        {
            ButtonKind.Plus,
            ButtonKind.Minus,
            ButtonKind.Multiply,
            ButtonKind.Divide,
            ButtonKind.Equals,
        };

        private readonly TextPicker displayTextBox;
        private readonly List<GenericControl> buttons = new();
        private readonly List<GenericControl> rowPanels = new();
        private readonly ControlSet buttonSet;
        private readonly GenericControl? clearLastButton;

        static Calculator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Calculator"/> class.
        /// </summary>
        public Calculator()
        {
            FormulaEngine.Init();

            this.RoundCorners();

            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Padding = 10;

            Layout = LayoutStyle.Vertical;

            displayTextBox = new()
            {
                Margin = (0, 0, 0, DefaultDistanceToDisplay),
                Parent = this,
            };

            displayTextBox.TabStop = false;
            displayTextBox.CanSelect = false;

            displayTextBox.EnterPressed += (s, e) =>
            {
                if (WantReturn)
                {
                    DoActionCalcFormula();
                }
            };

            displayTextBox.DelayedTextChanged += (s, e) =>
            {
                ReportError(false);
            };

            ButtonKind?[] buttonKinds =
            {
            ButtonKind.Clear, ButtonKind.LeftParenthesis, ButtonKind.RightParenthesis, ButtonKind.Divide, null,
            ButtonKind.Digit7, ButtonKind.Digit8, ButtonKind.Digit9, ButtonKind.Multiply, null,
            ButtonKind.Digit4, ButtonKind.Digit5, ButtonKind.Digit6, ButtonKind.Minus, null,
            ButtonKind.Digit1, ButtonKind.Digit2, ButtonKind.Digit3, ButtonKind.Plus, null,
            ButtonKind.ToggleSign, ButtonKind.Digit0, ButtonKind.DecimalPoint, ButtonKind.Equals, ButtonKind.EraseLeft,
            };

            bool[] buttonVisibility =
            {
            true, true, true, true, false,
            true, true, true, true, false,
            true, true, true, true, false,
            true, true, true, true, false,
            DefaultShowToggleSignButton, true, true, true, DefaultShowEraseLeftButton,
            };

            TwoDimensionalBuffer<GenericControl> buttons2d = new(width: 5, height: 5);

            for (int i = 0; i < buttonKinds.Length; i++)
            {
                var bk = buttonKinds[i];

                if (bk is null)
                    continue;

                var kind = bk.Value;

                int row = i / 5;
                int col = i % 5;

                var button = CreateButton();
                button.CustomAttr["ButtonKind"] = kind;
                button.Text = GetButtonText(kind);
                button.Visible = buttonVisibility[i];

                buttons.Add(button);

                buttons2d[i] = button;

                button.Click += OnButtonClick;
                button.DoubleClick += OnButtonDoubleClick;
            }

            for (int i = 0; i < buttons2d.Height; i++)
            {
                var rowItems = buttons2d.GetRowItems(i).Where(b => b != null).ToArray();
                var panel = new TransparentPanel().WithChildren(rowItems);
                rowPanels.Add(panel);
                panel.Layout = LayoutStyle.Horizontal;
                panel.Parent = this;
            }

            buttonSet = new(buttons);

            clearLastButton = GetButton(ButtonKind.EraseLeft);
            UpdateEraseToTheLeftChar();
        }

        /// <summary>
        /// Defines the kinds of buttons available in the calculator.
        /// </summary>
        public enum ButtonKind
        {
            /// <summary>
            /// Represents the "clear" button, which clears the entire input.
            /// </summary>
            Clear,

            /// <summary>
            /// Represents the "(" button, which inserts a left parenthesis.
            /// </summary>
            LeftParenthesis,

            /// <summary>
            /// Represents the ")" button, which inserts a right parenthesis.
            /// </summary>
            RightParenthesis,

            /// <summary>
            /// Represents the "divide" button, which performs division.
            /// </summary>
            Divide,

            /// <summary>
            /// Represents the "7" button, which inputs the digit 7.
            /// </summary>
            Digit7,

            /// <summary>
            /// Represents the "8" button, which inputs the digit 8.
            /// </summary>
            Digit8,

            /// <summary>
            /// Represents the "9" button, which inputs the digit 9.
            /// </summary>
            Digit9,

            /// <summary>
            /// Represents the "multiply" button, which performs multiplication.
            /// </summary>
            Multiply,

            /// <summary>
            /// Represents the "4" button, which inputs the digit 4.
            /// </summary>
            Digit4,

            /// <summary>
            /// Represents the "5" button, which inputs the digit 5.
            /// </summary>
            Digit5,

            /// <summary>
            /// Represents the "6" button, which inputs the digit 6.
            /// </summary>
            Digit6,

            /// <summary>
            /// Represents the "minus" button, which performs subtraction.
            /// </summary>
            Minus,

            /// <summary>
            /// Represents the "1" button, which inputs the digit 1.
            /// </summary>
            Digit1,

            /// <summary>
            /// Represents the "2" button, which inputs the digit 2.
            /// </summary>
            Digit2,

            /// <summary>
            /// Represents the "3" button, which inputs the digit 3.
            /// </summary>
            Digit3,

            /// <summary>
            /// Represents the "plus" button, which performs addition.
            /// </summary>
            Plus,

            /// <summary>
            /// Represents the "toggle sign" button, which toggles the sign of the current input.
            /// </summary>
            ToggleSign,

            /// <summary>
            /// Represents the "0" button, which inputs the digit 0.
            /// </summary>
            Digit0,

            /// <summary>
            /// Represents the "." button, which inputs a decimal point.
            /// </summary>
            DecimalPoint,

            /// <summary>
            /// Represents the "=" button, which evaluates the current expression.
            /// </summary>
            Equals,

            /// <summary>
            /// Represents the "clear last" button, which removes the last character from the input.
            /// </summary>
            EraseLeft,
        }

        /// <summary>
        /// Gets or sets a value indicating whether operator buttons are visible in the calculator.
        /// </summary>
        public virtual bool ShowOperatorButtons
        {

            get
            {
                return AllButtonsVisible(operatorButtons, true);
            }

            set
            {
                SetButtonsVisible(operatorButtons, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the clear button is visible in the calculator.
        /// </summary>
        public virtual bool ShowClearButton
        {

            get
            {
                return AllButtonsVisible([ButtonKind.Clear], true);
            }

            set
            {
                SetButtonsVisible([ButtonKind.Clear], value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the clear last button is visible in the calculator.
        /// </summary>
        public virtual bool ShowClearLastButton
        {

            get
            {
                return AllButtonsVisible([ButtonKind.EraseLeft], true);
            }

            set
            {
                SetButtonsVisible([ButtonKind.EraseLeft], value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the toggle sign button is visible in the calculator.
        /// </summary>
        public virtual bool ShowToggleSignButton
        {

            get
            {
                return AllButtonsVisible([ButtonKind.ToggleSign], true);
            }

            set
            {
                SetButtonsVisible([ButtonKind.ToggleSign], value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the decimal point button is visible in the calculator.
        /// </summary>
        public virtual bool ShowDecimalPointButton
        {

            get
            {
                return AllButtonsVisible([ButtonKind.DecimalPoint], true);
            }

            set
            {
                SetButtonsVisible([ButtonKind.DecimalPoint], value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether parenthesis buttons are visible in the calculator.
        /// </summary>
        public virtual bool ShowParenthesisButtons
        {

            get
            {
                return AllButtonsVisible([ButtonKind.LeftParenthesis, ButtonKind.RightParenthesis], true);
            }

            set
            {
                SetButtonsVisible([ButtonKind.LeftParenthesis, ButtonKind.RightParenthesis], value);
            }
        }      

        /// <summary>
        /// Gets or sets a value indicating whether the display text box is visible.
        /// </summary>
        public virtual bool IsDisplayVisible
        {
            get => displayTextBox.Visible;
            set => displayTextBox.Visible = value;
        }

        /// <summary>
        /// Gets or sets script options used in the formula evaluation. Default is Null.
        /// </summary>
        [Browsable(false)]
        public virtual object? FormulaOptions { get; set; }

        /// <summary>
        /// Gets or sets display value format.
        /// </summary>
        public virtual string? Format { get; set; }

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
        public GenericControl DisplayTextBox => displayTextBox;

        /// <summary>
        /// Gets collection of calculator buttons.
        /// </summary>
        [Browsable(false)]
        public IReadOnlyList<GenericControl> Buttons => buttons;

        /// <summary>
        /// Gets collection of row controls which contain the calculator buttons.
        /// </summary>
        [Browsable(false)]
        public IReadOnlyList<GenericControl> RowControls => rowPanels;

        /// <inheritdoc/>
        public override string Text
        {
            get => displayTextBox.Text;
            set
            {
                displayTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current formula has an error.
        /// </summary>
        public virtual bool HasError
        {
            get
            {
                return AsDouble == null;
            }
        }

        /// <summary>
        /// Gets or sets value format provider. If not set, <see cref="CultureInfo.CurrentCulture"/> is used.
        /// </summary>
        public virtual IFormatProvider? FormatProvider { get; set; }

        /// <summary>
        /// Evaluates the formula shown in the display and returns the result.
        /// If the formula is invalid, returns null.
        /// </summary>
        public virtual object? Value
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(displayTextBox.Text))
                        return 0;

                    object? result = Evaluate(displayTextBox.Text);
                    return result;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Evaluates the formula shown in the display and returns the result as a double.
        /// If the formula is invalid or cannot be converted to a double, returns null.
        /// </summary>
        public virtual double? AsDouble
        {
            get
            {
                try
                {
                    var doubleResult = Convert.ToDouble(Value, FormatProvider ?? CultureInfo.InvariantCulture);
                    return doubleResult;
                }
                catch
                {
                    return null;
                }                
            }
        }

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
        /// Gets the button control corresponding to the specified <see cref="ButtonKind"/>.
        /// </summary>
        /// <param name="kind">The kind of button to retrieve.</param>
        /// <returns>The button control if found; otherwise, null.</returns>
        public virtual GenericControl? GetButton(ButtonKind kind)
        {
            foreach (var button in buttons)
            {
                var buttonKind = GetButtonKind(button);

                if (buttonKind == kind)
                {
                    return button;
                }
            }

            return null;
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
        /// <returns>The created button control.</returns>
        public virtual GenericControl CreateButton()
        {
            var result = new SpeedTextButton();
            result.UseTheme = SpeedButton.KnownTheme.StaticBorder;
            result.Padding = DefaultButtonPadding;
            result.MinimumSize = DefaultMinButtonSize;
            result.IsClickRepeated = DefaultIsClickRepeated;
            result.Margin = DefaultButtonDistance;
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

                if (result is Exception exception)
                {
                    ReportError(exception);
                    return;
                }

                var formatProvider = FormatProvider ?? CultureInfo.InvariantCulture;

                try
                {
                    var asDouble = Convert.ToDouble(result, formatProvider);
                    displayTextBox.Text = asDouble.ToString(Format, formatProvider) ?? string.Empty;
                }
                catch
                {
                    displayTextBox.Text = result.ToString() ?? string.Empty;
                }

                ReportError(false);
            }
            catch (Exception e)
            {
                ReportError(e);
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
        /// Gets a value indicating whether all specified buttons have the specified visibility.
        /// </summary>
        /// <param name="buttonKinds">An array of button kinds to check visibility for.</param>
        /// <param name="value">A boolean value indicating the visibility to check for.</param>
        /// <returns><c>true</c> if all specified buttons have the specified visibility; otherwise, <c>false</c>.</returns>
        public virtual bool AllButtonsVisible(ButtonKind[] buttonKinds, bool value)
        {
            foreach (var button in buttons)
            {
                var buttonKind = GetButtonKind(button);
                if (buttonKind is not null && buttonKinds.Contains(buttonKind.Value))
                {
                    if (button.Visible != value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the kind of button associated with the specified <see cref="GenericControl"/>.
        /// </summary>
        /// <param name="button">The button for which to get the kind.</param>
        /// <returns>The kind of the button, or <c>null</c> if the kind is not set.</returns>
        public virtual ButtonKind? GetButtonKind(GenericControl? button)
        {
            return button?.CustomAttr.GetAttribute("ButtonKind") as ButtonKind?;
        }

        /// <summary>
        /// Sets the visibility of buttons in the calculator based on their kinds.
        /// </summary>
        /// <param name="buttonKinds">An array of button kinds to set visibility for.</param>
        /// <param name="value">A boolean value indicating whether the buttons should be visible.</param>
        public virtual void SetButtonsVisible(ButtonKind[] buttonKinds, bool value)
        {
            foreach (var buttonKind in buttonKinds)
            {
                var button = GetButton(buttonKind);
                if (button != null)
                {
                    button.Visible = value;
                }
            }
        }

        /// <summary>
        /// Reports an error message to the user interface.
        /// </summary>
        /// <param name="e">The exception that occurred.</param>
        protected virtual void ReportError(Exception e)
        {
            while (e.InnerException != null)
                e = e.InnerException;
            ReportError(true, $"Error: {e.Message}");
        }

        /// <summary>
        /// Gets the text to display on a button based on its kind.
        /// </summary>
        /// <param name="buttonKind">The kind of the button.</param>
        /// <returns>The text to display on the button.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual string GetButtonText(ButtonKind buttonKind)
        {
            return buttonKind switch
            {
                ButtonKind.Clear => ButtonTextClear,
                ButtonKind.LeftParenthesis => "(",
                ButtonKind.RightParenthesis => ")",
                ButtonKind.Divide => ButtonTextDivide,
                ButtonKind.Digit7 => "7",
                ButtonKind.Digit8 => "8",
                ButtonKind.Digit9 => "9",
                ButtonKind.Multiply => ButtonTextMultiply,
                ButtonKind.Digit4 => "4",
                ButtonKind.Digit5 => "5",
                ButtonKind.Digit6 => "6",
                ButtonKind.Minus => "-",
                ButtonKind.Digit1 => "1",
                ButtonKind.Digit2 => "2",
                ButtonKind.Digit3 => "3",
                ButtonKind.Plus => "+",
                ButtonKind.ToggleSign => ButtonTextPlusMinus,
                ButtonKind.Digit0 => "0",
                ButtonKind.DecimalPoint => ".",
                ButtonKind.Equals => "=",
                ButtonKind.EraseLeft => ButtonTextEraseLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(buttonKind), buttonKind, null),
            };
        }

        /// <summary>
        /// Reports a validation error with the specified message.
        /// </summary>
        /// <param name="isError">Indicates whether an error occurred.</param>
        /// <param name="message">The error message to display.</param>
        protected virtual void ReportError(bool isError, string? message = null)
        {
            displayTextBox.ShowErrorBorder = isError;

            if (isError)
            {
                displayTextBox.ToolTip = message ?? "An error occurred during formula evaluation.";
            }
            else
            {
                displayTextBox.ToolTip = null;
            }
        }

        /// <summary>
        /// Called when a button is clicked in the calculator.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnButtonClick(object? sender, EventArgs e)
        {
            var kind = GetButtonKind(sender as GenericControl);
            ButtonClickHandler(kind);
        }
        
        /// <summary>
        /// Called when a button is double-clicked in the calculator.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnButtonDoubleClick(object? sender, EventArgs e)   
        {
            if (!DefaultDoubleClickAsClick)
                return;

            var kind = GetButtonKind(sender as GenericControl);
            ButtonClickHandler(kind);
        }

        /// <inheritdoc/>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateEraseToTheLeftChar();
        }

        /// <summary>
        /// Updates the display character for the "erase to the left" button based on the current font.
        /// If the font supports the specific glyph for the erase character, it will be used;
        /// otherwise, a default text is displayed.
        /// </summary>
        protected virtual void UpdateEraseToTheLeftChar()
        {
            var glyphFont = FontFactory.DefaultSymbolFont.WithSize(RealFont.Size);

            var hasGlyph = glyphFont.HasGlyph(CharUtils.EraseToTheLeftDisplayChar);

            if (clearLastButton != null)
            {
                clearLastButton.Font = hasGlyph ? glyphFont : RealFont;
                clearLastButton.Text = hasGlyph ? CharUtils.EraseToTheLeftDisplayChar.ToString() : ButtonTextEraseLeft;
            }
        }

        /// <summary>
        /// Handles button click events by performing actions based on the button's text.
        /// </summary>
        /// <remarks>The method supports various button actions, including clearing text,
        /// removing the last character, evaluating expressions,  and appending mathematical
        /// operators or other characters. Specific
        /// actions are determined by the value of <paramref name="buttonKind"/>.</remarks>
        /// <param name="buttonKind">The kind of button that was clicked.
        /// This determines the action to perform.</param>
        protected virtual void ButtonClickHandler(ButtonKind? buttonKind)
        {
            if (buttonKind is null)
                return;

            void AddText(string text)
            {
                displayTextBox.Text += text;
            }   

            displayTextBox.CancelEdit();

            switch (buttonKind)
            {
                case ButtonKind.Clear:
                    DoActionClearAll();
                    break;
                case ButtonKind.EraseLeft:
                    DoActionClearLast();
                    break;
                case ButtonKind.Equals:
                    DoActionCalcFormula();
                    break;
                case ButtonKind.Multiply:
                    displayTextBox.Text += "*";
                    break;
                case ButtonKind.Divide:
                    displayTextBox.Text += "/";
                    break;
                case ButtonKind.ToggleSign:
                    DoActionToggleSign();
                    break;
                case ButtonKind.LeftParenthesis:
                    AddText("(");
                    break;
                case ButtonKind.RightParenthesis:
                    AddText(")");
                    break;
                case ButtonKind.Digit0:
                    AddText("0");
                    break;
                case ButtonKind.Digit1:
                    AddText("1");
                    break;
                case ButtonKind.Digit2:
                    AddText("2");
                    break;
                case ButtonKind.Digit3:
                    AddText("3");
                    break;
                case ButtonKind.Digit4:
                    AddText("4");
                    break;
                case ButtonKind.Digit5:
                    AddText("5");
                    break;
                case ButtonKind.Digit6:
                    AddText("6");
                    break;
                case ButtonKind.Digit7:
                    AddText("7");
                    break;
                case ButtonKind.Digit8:
                    AddText("8");
                    break;
                case ButtonKind.Digit9:
                    AddText("9");
                    break;
                case ButtonKind.Minus:
                    AddText("-");
                    break;
                case ButtonKind.Plus:
                    AddText("+");
                    break;
                case ButtonKind.DecimalPoint:
                    AddText(".");
                    break;
            }
        }
    }

    /// <summary>
    /// Represents a specialized calculator control designed for PIN code entry.
    /// This control inherits from <see cref="Calculator"/> but hides most 
    /// calculator-specific UI elements, leaving only the numeric keypad.
    /// </summary>
    [ControlCategory(KnownControlCategory.Other)]
    public partial class PinCodePicker : Calculator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PinCodePicker"/> class.
        /// </summary>
        public PinCodePicker()
        {
            IsDisplayVisible = false;
            ShowOperatorButtons = false;
            ShowParenthesisButtons = false;
            ShowClearButton = false;
            ShowDecimalPointButton = false;
            ShowToggleSignButton = false;
        }
    }
}