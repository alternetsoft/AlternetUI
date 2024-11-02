using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Base abstract class for control with side buttons.
    /// </summary>
    public abstract partial class ControlAndButton : HiddenBorder, INotifyDataErrorInfo
    {
        private readonly ToolBar buttons;
        private readonly AbstractControl mainControl;

        private readonly PictureBox errorPicture = new()
        {
            Margin = new Thickness(ControlAndLabel.DefaultControlLabelDistance, 0, 0, 0),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndButton"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ControlAndButton(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndButton"/> class.
        /// </summary>
        public ControlAndButton()
        {
            Layout = LayoutStyle.Horizontal;

            SuspendLayout();
            try
            {
                mainControl = CreateControl();
                mainControl.VerticalAlignment = UI.VerticalAlignment.Center;
                mainControl.Parent = this;

                buttons = new();
                buttons.VerticalAlignment = UI.VerticalAlignment.Center;
                buttons.ParentBackColor = true;
                buttons.ParentForeColor = true;
                buttons.Parent = this;
                IdButton = buttons.AddSpeedBtn(KnownButton.TextBoxCombo, (_, _) => RaiseButtonClick());

                CustomTextBox.InitErrorPicture(errorPicture);
                errorPicture.Parent = this;
                errorPicture.ParentBackColor = true;
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Occcurs when button is clicked.
        /// </summary>
        public event EventHandler? ButtonClick;

        /// <summary>
        /// Gets id of the first button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId IdButton { get; internal set; }

        /// <summary>
        /// Gets attached <see cref="PictureBox"/> control which
        /// displays validation error information.
        /// </summary>
        [Browsable(false)]
        public PictureBox ErrorPicture => errorPicture;

        /// <summary>
        /// Gets or sets visibility of the attached <see cref="PictureBox"/> control which
        /// displays validation error information.
        /// </summary>
        public virtual bool ErrorPictureVisible
        {
            get => ErrorPicture.Visible;
            set => ErrorPicture.Visible = value;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.SuggestedWidth"/> property of the main child control.
        /// </summary>
        public virtual Coord InnerSuggestedWidth
        {
            get => MainControl.SuggestedWidth;
            set => MainControl.SuggestedWidth = value;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.SuggestedHeight"/> property of the main child control.
        /// </summary>
        public virtual Coord InnerSuggestedHeight
        {
            get => MainControl.SuggestedHeight;
            set => MainControl.SuggestedHeight = value;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.SuggestedSize"/> property of the main child control.
        /// </summary>
        public virtual SizeD InnerSuggestedSize
        {
            get => MainControl.SuggestedSize;
            set => MainControl.SuggestedSize = value;
        }

        /// <summary>
        /// Gets attached <see cref="ToolBar"/> control.
        /// </summary>
        [Browsable(false)]
        public AbstractControl Buttons => buttons;

        /// <summary>
        /// Gets or sets visibility of the attached <see cref="ToolBar"/> control.
        /// </summary>
        public virtual bool ButtonsVisible
        {
            get => Buttons.Visible;
            set => Buttons.Visible = value;
        }

        /// <summary>
        /// Gets main child control.
        /// </summary>
        [Browsable(false)]
        public AbstractControl MainControl => mainControl;

        /// <inheritdoc/>
        public override bool HasErrors
        {
            get => (MainControl as INotifyDataErrorInfo)?.HasErrors ?? false;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        /// <inheritdoc/>
        public override IEnumerable GetErrors(string? propertyName)
        {
            return (MainControl as INotifyDataErrorInfo)?.GetErrors(propertyName) ?? Array.Empty<string>();
        }

        /// <summary>
        /// Raises <see cref="ButtonClick"/> event and calls <see cref="OnButtonClick"/> method.
        /// </summary>
        public void RaiseButtonClick()
        {
            ButtonClick?.Invoke(this, EventArgs.Empty);
            OnButtonClick();
        }

        /// <summary>
        /// Called when first button is clicked.
        /// </summary>
        public virtual void OnButtonClick()
        {
        }

        /// <summary>
        /// Creates main child control.
        /// </summary>
        /// <remarks>
        /// For example, main control for the <see cref="TextBoxAndButton"/> is <see cref="TextBox"/>.
        /// </remarks>
        protected abstract AbstractControl CreateControl();
    }
}
