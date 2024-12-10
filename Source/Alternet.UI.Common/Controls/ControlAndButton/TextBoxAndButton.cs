using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
            Init();
        }

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
        /// Gets or sets whether main inner control has border.
        /// </summary>
        public virtual bool HasInnerBorder
        {
            get
            {
                return TextBox.HasBorder;
            }

            set
            {
                TextBox.HasBorder = value;
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

        /// <inheritdoc/>
        public override void BindHandlerEvents()
        {
            base.BindHandlerEvents();
            Handler.TextChanged = null;
        }

        /// <summary>
        /// Initializes this control for the password editing.
        /// </summary>
        public virtual void InitPasswordEdit()
        {
            TextBox.IsPassword = true;
            Buttons.SetChildrenVisible(false);
            this.HasBtnComboBox = true;
            this.BtnComboBoxSvg = null;
            this.BtnComboBoxKnownImage = KnownButton.TextBoxShowPassword;

            void TogglePasswordButtonClick(object? s, EventArgs e)
            {
                TextBox.IsPassword = !TextBox.IsPassword;
                if (TextBox.IsPassword)
                {
                    this.BtnComboBoxKnownImage = KnownButton.TextBoxShowPassword;
                }
                else
                {
                    this.BtnComboBoxKnownImage = KnownButton.TextBoxHidePassword;
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
        /// Initializes main child control.
        /// </summary>
        /// <remarks>
        /// Override this method to do custom initialization. Do not call this method
        /// directly, it is called automatically from constructor. When overriding you
        /// must call base method before any other initializations.
        /// </remarks>
        protected virtual void Init()
        {
            MainControl.ValidatorReporter = ErrorPicture;
            MainControl.TextChanged += MainControl_TextChanged;
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
