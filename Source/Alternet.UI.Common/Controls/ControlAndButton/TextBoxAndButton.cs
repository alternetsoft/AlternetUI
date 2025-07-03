using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
            MainControl.ValidatorReporter = ErrorPicture;
            MainControl.TextChanged += MainControl_TextChanged;
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
        public virtual void InitFilterEdit()
        {
            TextBox.IsPassword = false;
            TextBox.EmptyTextHint = EmptyTextHints.FilterEdit;
            SetSingleButton(UI.KnownButton.Search);
            IsButtonLeft = true;
        }

        /// <summary>
        /// Initializes this control for the search text editing.
        /// </summary>
        public virtual void InitSearchEdit()
        {
            TextBox.IsPassword = false;
            TextBox.EmptyTextHint = EmptyTextHints.FindEdit;
            SetSingleButton(UI.KnownButton.Search);
            IsButtonLeft = true;
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

        private void MainControl_TextChanged(object? sender, EventArgs e)
        {
            MainControlTextChanged();
        }
    }
}
