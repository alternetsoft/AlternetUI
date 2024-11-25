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
        /// <summary>
        /// Gets or sets whether minus button is shown before plus button. Default value is True.
        /// </summary>
        /// <remarks>
        /// This property affects all <see cref="ControlAndButton"/> descendants created after
        /// it was changed.
        /// </remarks>
        public static bool IsMinusButtonFirst = true;

        private readonly ToolBar buttons;
        private readonly AbstractControl mainControl;

        private readonly PictureBox errorPicture = new()
        {
            Margin = new Thickness(ControlAndLabel.DefaultControlLabelDistance, 0, 0, 0),
        };

        private ObjectUniqueId? idButtonCombo;
        private ObjectUniqueId? idButtonEllipsis;
        private ObjectUniqueId? idButtonPlus;
        private ObjectUniqueId? idButtonMinus;

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
                HasBtnComboBox = true;
                buttons.Parent = this;

                CustomTextBox.InitErrorPicture(errorPicture);
                errorPicture.Visible = false;
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
        public event EventHandler<ControlAndButtonClickEventArgs>? ButtonClick;

        /// <summary>
        /// Gets or sets whether 'ComboBox' button is visible.
        /// </summary>
        public virtual bool HasBtnComboBox
        {
            get
            {
                return HasButton(idButtonCombo);
            }

            set
            {
                SetHasButton(KnownButton.TextBoxCombo, ref idButtonCombo, value);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Ellipsis' button is visible.
        /// </summary>
        public virtual bool HasBtnEllipsis
        {
            get
            {
                return HasButton(IdButtonEllipsis);
            }

            set
            {
                SetHasButton(KnownButton.TextBoxEllipsis, ref idButtonEllipsis, value);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Plus' and 'Minus' buttons are visible.
        /// </summary>
        public virtual bool HasBtnPlusMinus
        {
            get
            {
                return HasButton(IdButtonPlus) && HasButton(IdButtonMinus);
            }

            set
            {
                if (IsMinusButtonFirst)
                {
                    ToggleMinusButton();
                    TogglePlusButton();
                }
                else
                {
                    TogglePlusButton();
                    ToggleMinusButton();
                }

                void TogglePlusButton()
                {
                    SetHasButton(KnownButton.TextBoxPlus, ref idButtonPlus, value);
                }

                void ToggleMinusButton()
                {
                    SetHasButton(KnownButton.TextBoxMinus, ref idButtonMinus, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets 'IsClickRepeated' property of the buttons.
        /// </summary>
        public virtual bool IsBtnClickRepeated
        {
            get
            {
                return buttons.GetToolCount() > 0 && buttons.IsToolClickRepeated;
            }

            set
            {
                buttons.IsToolClickRepeated = value;
            }
        }

        /// <summary>
        /// Gets id of the combo button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId? IdButtonCombo => idButtonCombo;

        /// <summary>
        /// Gets id of the ellipsis button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId? IdButtonEllipsis => idButtonEllipsis;

        /// <summary>
        /// Gets id of the plus button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId? IdButtonPlus => idButtonPlus;

        /// <summary>
        /// Gets id of the minus button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId? IdButtonMinus => idButtonMinus;

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
        [Browsable(false)]
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
        public void RaiseButtonClick(ControlAndButtonClickEventArgs e)
        {
            ButtonClick?.Invoke(this, e);
            if (e.Handled)
                return;
            OnButtonClick(e);
        }

        /// <summary>
        /// Called when button is clicked.
        /// </summary>
        public virtual void OnButtonClick(ControlAndButtonClickEventArgs e)
        {
        }

        /// <summary>
        /// Gets whether button with the specified id is visible.
        /// </summary>
        /// <param name="id">Button id.</param>
        /// <returns></returns>
        public virtual bool HasButton(ObjectUniqueId? id)
        {
            if (id is null)
                return false;
            var result = buttons.GetToolVisible(id.Value);
            return result;
        }

        /// <summary>
        /// Gets button name for the debug purposes for the specified button id.
        /// </summary>
        /// <param name="id">Button id.</param>
        /// <returns></returns>
        public virtual string? GetBtnName(ObjectUniqueId? id)
        {
            if (id is null)
                return null;
            if (id == idButtonCombo)
                return "combo";
            if (id == idButtonEllipsis)
                return "ellipsis";
            if (id == idButtonPlus)
                return "plus";
            if (id == idButtonMinus)
                return "minus";
            return "other";
        }

        /// <summary>
        /// Sets whether button with the specified id is visible.
        /// </summary>
        /// <param name="btn">Known button.</param>
        /// <param name="id">Id of the created button.</param>
        /// <param name="value">Whether button is visible.</param>
        public virtual void SetHasButton(KnownButton btn, ref ObjectUniqueId? id, bool value)
        {
            if (HasButton(id) == value)
                return;
            if (value)
            {
                if (id is null)
                {
                    id
                        = buttons.AddSpeedBtn(btn, (s, e) =>
                        {
                            ControlAndButtonClickEventArgs args = new();
                            args.ButtonId = (s as AbstractControl)?.UniqueId;
                            RaiseButtonClick(args);
                        });
                }
                else
                {
                    buttons.SetToolVisible(id.Value, true);
                }
            }
            else
            {
                if (id is null)
                    return;
                buttons.SetToolVisible(id.Value, false);
            }
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
