using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="TextBox"/> with attached label.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class TextBoxAndLabel : ControlAndLabel<TextBox, Label>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxAndLabel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public TextBoxAndLabel(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxAndLabel"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="text">Default value of the <see cref="Text"/> property.</param>
        public TextBoxAndLabel(string title, string? text = default)
            : this()
        {
            Title = title;
            if (text is not null)
                TextBox.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxAndLabel"/> class.
        /// </summary>
        public TextBoxAndLabel()
        {
            Init();
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
        /// Gets main inner <see cref="TextBox"/> control.
        /// </summary>
        [Browsable(false)]
        public TextBox TextBox => MainControl;

        /// <summary>
        /// <see cref="CustomTextBox.IsRequired"/>
        /// </summary>
        public virtual bool IsRequired
        {
            get => TextBox.IsRequired;

            set => TextBox.IsRequired = value;
        }

        /// <summary>
        /// Gets whether editor contains a valid e-mail address.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsValidMail => ValidationUtils.IsValidMailAddress(Text);

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
            MainControl.AutoShowError = true;
            MainControl.TextChanged += (s, e) =>
            {
                RaiseTextChanged(EventArgs.Empty);
            };

            MainControl.DelayedTextChanged += (s, e) =>
            {
                MainControlTextChanged();
            };
        }

        /// <summary>
        /// Called when text is changed in the main child control.
        /// </summary>
        /// <remarks>
        /// This method is called after some delay and is suitable for the text validation.
        /// </remarks>
        protected virtual void MainControlTextChanged()
        {
        }
    }
}
