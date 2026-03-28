using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="TextBox"/> with side buttons.
    /// </summary>
    public partial class TextBoxAndButton : ControlAndButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxAndButton"/> class.
        /// </summary>
        public TextBoxAndButton()
        {
            MainControl.ValidatorReporter = InnerPicture;
            MainControl.TextChanged += OnMainControlTextChanged;
            MainControl.AutoShowError = true;
            MainControl.VerticalAlignment = VerticalAlignment.Center;
            AutoBackColor = true;
        }

        /// <summary>
        /// Occurs when the <see cref="IsEditable"/> property value changes.
        /// </summary>
        public event EventHandler? IsEditableChanged;

        /// <summary>
        /// Gets main child control.
        /// </summary>
        [Browsable(false)]
        public new TextBox MainControl => (TextBox)base.MainControl;

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.Text"/> property of the main child control.
        /// </summary>
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return TextBox.Text;
            }

            set
            {
                TextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets whether error reporter is automatically shown/hidden when
        /// error state is changed.
        /// </summary>
        public virtual bool AutoShowError
        {
            get => MainControl.AutoShowError;

            set
            {
                MainControl.AutoShowError = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that enables or disables editing of the text in
        /// text box area of the control.
        /// </summary>
        /// <value><c>true</c> if the text in the control can be edited;
        /// otherwise <c>false</c>.</value>
        [Category("Appearance")]
        [DefaultValue(true)]
        public virtual bool IsEditable
        {
            get
            {
                return !UseSubstituteControl;
            }

            set
            {
                if (IsEditable == value)
                    return;
                UseSubstituteControl = !value;
                IsEditableChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value specifying the style of the control.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ComboBoxStyle" /> values.
        /// </returns>
        [Category("Appearance")]
        [DefaultValue(ComboBoxStyle.DropDown)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Browsable(false)]
        public virtual ComboBoxStyle DropDownStyle
        {
            get
            {
                if (IsEditable)
                    return ComboBoxStyle.DropDown;
                else
                    return ComboBoxStyle.DropDownList;
            }

            set
            {
                if (DropDownStyle == value)
                    return;
                switch (value)
                {
                    case ComboBoxStyle.DropDown:
                        IsEditable = true;
                        break;
                    case ComboBoxStyle.DropDownList:
                        IsEditable = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets main child control, same as <see cref="MainControl"/>.
        /// </summary>
        [Browsable(false)]
        public TextBox TextBox => (TextBox)base.MainControl;

        /// <summary>
        /// Gets whether <see cref="Text"/> is null or empty.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsNullOrEmpty => string.IsNullOrEmpty(Text);

        /// <summary>
        /// Gets whether <see cref="Text"/> is null or white space.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsNullOrWhiteSpace => string.IsNullOrWhiteSpace(Text);

        /// <summary>
        /// Gets or sets init arguments which are used when <see cref="InputType"/>
        /// property is assigned.
        /// </summary>
        [Browsable(false)]
        public virtual TextBoxInitializeEventArgs? InputTypeArgs
        {
            get => TextBox.InputTypeArgs;
            set => TextBox.InputTypeArgs = value;
        }

        /// <summary>
        /// Gets or sets input type. Default is Null.
        /// </summary>
        public virtual KnownInputType? InputType
        {
            get
            {
                return TextBox.InputType;
            }

            set
            {
                TextBox.InputType = value;
            }
        }

        /// <summary>
        /// Initializes this control for the filter editing.
        /// </summary>
        public virtual void InitFilterEdit(SvgImage? image = null, string? emptyTextHint = null, UI.KnownButton? clearButton = null)
        {
            InitWithImageAndClearButton(image ?? KnownSvgImages.ImgFilter, emptyTextHint ?? EmptyTextHints.FilterEdit, clearButton);
        }

        /// <summary>
        /// Initializes the control with a specified image and configures a clear button, setting the provided hint text
        /// for empty input.
        /// </summary>
        /// <remarks>When the button is clicked, the text box is cleared and focus is set to the text box.
        /// The clear button is positioned on the right side of the control.</remarks>
        /// <param name="clearButton">The button to use for clearing the text box. If null, a default cancel button is used.</param>
        /// <param name="image">The SVG image to display on the left side of the control.</param>
        /// <param name="emptyTextHint">The hint text to display in the text box when it is empty. Provides guidance to the user about the expected
        /// input.</param>
        public virtual void InitWithImageAndClearButton(SvgImage image, string emptyTextHint, UI.KnownButton? clearButton = null)
        {
            TextBox.IsPassword = false;
            TextBox.EmptyTextHint = emptyTextHint;

            IsToolTipShownOnInnerPictureClick = false;
            IsButtonLeft = false;

            InnerPictureToolTipMessageIcon = null;
            InnerPictureSvg = image;
            InnerPicture.Dock = DockStyle.LeftAutoSize;
            InnerPicture.Margin = TextBoxUtils.GetInnerTextBoxPictureMargin(isRight: false);
            InnerPictureVisible = true;
            InnerPictureToolTip = CommonStrings.Default.ButtonClear;
            InnerPictureImageVisible = true;
            InnerPicture.CanSelect = true;

            var button = SetSingleButton(clearButton ?? UI.KnownButton.Cancel);

            if (button is not null)
            {
                ButtonClick -= OnClearButtonClick;
                ButtonClick += OnClearButtonClick;

                void OnClearButtonClick(object? s, ControlAndButtonClickEventArgs e)
                {
                    if (e.ButtonId != button.UniqueId)
                        return;

                    Text = string.Empty;
                    TextBox.SetFocusIfPossible();
                }
            }
        }

        /// <summary>
        /// Initializes this control for the search text editing.
        /// </summary>
        public virtual void InitSearchEdit(SvgImage? image = null, string? emptyTextHint = null, UI.KnownButton? clearButton = null)
        {
            InitWithImageAndClearButton(image ?? KnownSvgImages.ImgSearch, emptyTextHint ?? EmptyTextHints.FindEdit, clearButton);
        }

        /// <summary>
        /// Sets appropriate error text using the specified parameters.
        /// </summary>
        /// <param name="typeCode">The numeric type code.</param>
        /// <param name="prm">The additional parameters.</param>
        public virtual void SetErrorText(
            NumericTypeCode typeCode,
            InitAsNumericEditParams? prm = null)
        {
            if (AssemblyUtils.IsTypeCodeFloat((TypeCode)typeCode))
            {
                if (prm?.UnsignedFloat ?? false)
                {
                    TextBox.SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);
                }
                else
                {
                    TextBox.SetErrorText(ValueValidatorKnownError.FloatIsExpected);
                }
            }
            else
            {
                if (AssemblyUtils.IsTypeCodeUnsignedInt((TypeCode)typeCode))
                {
                    TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
                }
                else
                {
                    TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
                }
            }
        }

        /// <summary>
        /// Initializes this control as a numeric editor with the specified parameters.
        /// </summary>
        /// <param name="typeCode">The numeric type code.</param>
        /// <param name="prm">The additional parameters.</param>
        public virtual void InitAsNumericEdit(
            NumericTypeCode typeCode,
            InitAsNumericEditParams? prm = null)
        {
            var type = AssemblyUtils.TypeFromTypeCode((TypeCode)typeCode);

            TextBox.Options |= TextBoxOptions.DefaultValidation;
            TextBox.DataType = type;
            if (prm?.UseCharValidator ?? false)
                CharValidator = Alternet.UI.CharValidator.CreateValidator(type);

            SetErrorText(typeCode, prm);
        }

        /// <summary>
        /// Initializes this control as the <see cref="int"/> editor.
        /// </summary>
        /// <param name="useCharValidator"></param>
        public virtual void InitAsInt32Edit(bool useCharValidator = false)
        {
            TextBox.Options |= TextBoxOptions.DefaultValidation;
            TextBox.DataType = typeof(int);
            if (useCharValidator)
                TextBox.UseCharValidator<int>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }

        /// <summary>
        /// Initializes this control for the password editing.
        /// </summary>
        public virtual void InitPasswordEdit()
        {
            TextBox.IsPassword = true;
            SetSingleButton(UI.KnownButton.TextBoxShowPassword);
            TextBox.EmptyTextHint = EmptyTextHints.PasswordEdit;
            IsButtonLeft = false;

            void TogglePasswordButtonClick(object? s, EventArgs e)
            {
                TextBox.IsPassword = !TextBox.IsPassword;
                if (TextBox.IsPassword)
                {
                    this.ButtonOverride = UI.KnownButton.TextBoxShowPassword;
                }
                else
                {
                    this.ButtonOverride = UI.KnownButton.TextBoxHidePassword;
                }
            }

            this.ButtonClick -= TogglePasswordButtonClick;
            this.ButtonClick += TogglePasswordButtonClick;
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateControl()
        {
            return new TextBox();
        }

        /// <summary>
        /// Called when text is changed in the main child control.
        /// </summary>
        protected virtual void MainControlTextChanged()
        {
            OnTextChanged(EventArgs.Empty);
        }

        private void OnMainControlTextChanged(object? sender, EventArgs e)
        {
            MainControlTextChanged();
        }

        /// <summary>
        /// Defines configuration parameters for initializing a numeric editing control.
        /// Used to customize behavior such as validation logic and numeric format constraints.
        /// </summary>
        public struct InitAsNumericEditParams
        {
            /// <summary>
            /// Gets the default instance of <see cref="InitAsNumericEditParams"/>.
            /// </summary>
            public static readonly InitAsNumericEditParams Default = new();

            /// <summary>
            /// Indicates whether the numeric control should accept only unsigned
            /// floating-point values (i.e. no negative sign).
            /// </summary>
            public bool UnsignedFloat;

            /// <summary>
            /// Determines whether character-level input validation should be applied
            /// during editing.
            /// For example, restricts input to digits and a single decimal separator.
            /// </summary>
            public bool UseCharValidator;

            /// <summary>
            /// Initializes a new instance of the <see cref="InitAsNumericEditParams"/>
            /// structure with default settings.
            /// </summary>
            public InitAsNumericEditParams()
            {
            }

            /// <summary>
            /// Gets <see cref="InitAsNumericEditParams"/> with the specified value
            /// of the <see cref="UseCharValidator"/> property.
            /// </summary>
            /// <param name="useCharValidator">Value of the
            /// <see cref="UseCharValidator"/> property.</param>
            /// <returns></returns>
            public static InitAsNumericEditParams WithCharValidator(bool useCharValidator)
            {
                if (useCharValidator)
                {
                    return new()
                    {
                        UseCharValidator = true,
                    };
                }
                else
                {
                    return Default;
                }
            }
        }
    }
}
