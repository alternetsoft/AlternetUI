﻿using System;
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
        private bool autoBackColor = true;

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
        /// Gets or sets whether background color is updated when <see cref="InnerOuterBorder"/>
        /// property is changed.
        /// </summary>
        public virtual bool AutoBackColor
        {
            get => autoBackColor;
            set
            {
                if (autoBackColor == value)
                    return;
                autoBackColor = value;
                if(value)
                    RaiseAutoBackColorChanged();
            }
        }

        /// <summary>
        /// Gets or sets where border is painted (around the child or around the parent).
        /// When this property is set, background color is also updated
        /// if <see cref="AutoBackColor"/> is true.
        /// </summary>
        public virtual InnerOuterSelector InnerOuterBorder
        {
            get
            {
                return ConversionUtils.ToInnerOuterSelector(HasBorder, HasInnerBorder);
            }

            set
            {
                if (InnerOuterBorder == value)
                    return;
                var (outer, inner) = ConversionUtils.FromInnerOuterSelector(value);
                var changed = (HasBorder != outer) || (HasInnerBorder != inner);
                HasBorder = outer;
                HasInnerBorder = inner;
                if(changed)
                    RaiseAutoBackColorChanged();
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

        /// <inheritdoc/>
        public override void BindHandlerEvents()
        {
            base.BindHandlerEvents();
            Handler.TextChanged = null;
        }

        /// <summary>
        /// Shows common border and validation error place.
        /// </summary>
        public virtual void InitErrorAndBorder()
        {
            ErrorPictureVisible = true;
            MainControl.HasBorder = false;
            HasBorder = true;
            BackColor = MainControl.BackColor;
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
            this.ButtonOverride = UI.KnownButton.TextBoxShowPassword;

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
        /// Raised when <see cref="AutoBackColor"/> property is changed.
        /// </summary>
        protected virtual void RaiseAutoBackColorChanged()
        {
            if (!AutoBackColor)
                return;
            switch (InnerOuterBorder)
            {
                case InnerOuterSelector.None:
                    ParentBackColor = true;
                    break;
                case InnerOuterSelector.Inner:
                    ParentBackColor = true;
                    break;
                case InnerOuterSelector.Outer:
                    ParentBackColor = false;
                    BackColor = MainControl.BackColor;
                    break;
                case InnerOuterSelector.Both:
                    ParentBackColor = false;
                    BackColor = MainControl.BackColor;
                    break;
            }
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
            MainControl.AutoShowError = true;
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
