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
    public abstract partial class ControlAndButton : ControlAndControl, INotifyDataErrorInfo
    {
        /// <summary>
        /// Gets or sets default image for the 'ComboBox' button.
        /// </summary>
        public static KnownButton DefaultBtnComboBoxImage = UI.KnownButton.TextBoxCombo;

        /// <summary>
        /// Gets or sets default image for the 'Ellipsis' button.
        /// </summary>
        public static KnownButton DefaultBtnEllipsisImage = UI.KnownButton.TextBoxEllipsis;

        /// <summary>
        /// Gets or sets default image for the 'Plus' button.
        /// </summary>
        public static KnownButton DefaultBtnPlusImage = UI.KnownButton.TextBoxPlus;

        /// <summary>
        /// Gets or sets default image for the 'Minus' button.
        /// </summary>
        public static KnownButton DefaultBtnMinusImage = UI.KnownButton.TextBoxMinus;

        /// <summary>
        /// Gets or sets default svg image for 'ComboBox' button.
        /// </summary>
        /// <remarks>
        /// If not specified, <see cref="DefaultBtnComboBoxImage"/> is used.
        /// </remarks>
        public static SvgImage? DefaultBtnComboBoxSvg;

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

        private ObjectUniqueId? idButtonCombo;
        private ObjectUniqueId? idButtonEllipsis;
        private ObjectUniqueId? idButtonPlus;
        private ObjectUniqueId? idButtonMinus;
        private KnownButton? comboBoxKnownImage;
        private SvgImage? comboBoxSvg;

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
            SuspendHandlerTextChange();
            ParentBackColor = true;
            ParentForeColor = true;
            Layout = LayoutStyle.Horizontal;

            SuspendLayout();
            try
            {
                mainControl = CreateControl();
                mainControl.Alignment = (HorizontalAlignment.Fill, VerticalAlignment.Center);
                mainControl.Parent = this;

                buttons = new();
                buttons.Alignment = (HorizontalAlignment.Right, VerticalAlignment.Center);
                buttons.ParentBackColor = true;
                buttons.ParentForeColor = true;

                if(NeedDefaultButton())
                    HasBtnComboBox = true;
                buttons.Parent = this;
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Occurs when <see cref="AbstractControl.TextChanged"/> event of the
        /// inner control is changed.
        /// </summary>
        public new event EventHandler? TextChanged
        {
            add => MainControl.TextChanged += value;
            remove => MainControl.TextChanged -= value;
        }

        /// <summary>
        /// Occurs when <see cref="AbstractControl.DelayedTextChanged"/> event of the
        /// inner control is changed.
        /// </summary>
        public new event EventHandler<EventArgs>? DelayedTextChanged
        {
            add => MainControl.DelayedTextChanged += value;
            remove => MainControl.DelayedTextChanged -= value;
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
                SetHasButton(
                    comboBoxKnownImage ?? DefaultBtnComboBoxImage,
                    ref idButtonCombo,
                    value,
                    comboBoxSvg ?? DefaultBtnComboBoxSvg);
            }
        }

        /// <summary>
        /// Gets or sets whether buttons are aligned to the left or to the right.
        /// </summary>
        public virtual bool IsButtonLeft
        {
            get
            {
                return Buttons.HorizontalAlignment == HorizontalAlignment.Left;
            }

            set
            {
                if (IsButtonLeft == value)
                    return;
                if (value)
                {
                    Buttons.HorizontalAlignment = HorizontalAlignment.Left;
                }
                else
                {
                    Buttons.HorizontalAlignment = HorizontalAlignment.Right;
                }
            }
        }

        /// <summary>
        /// Gets or sets 'ComboBox' button image as <see cref="UI.KnownButton"/>.
        /// </summary>
        public virtual KnownButton? ButtonOverride
        {
            get
            {
                return comboBoxKnownImage;
            }

            set
            {
                if (comboBoxKnownImage == value)
                    return;
                comboBoxKnownImage = value;
                UpdateComboBoxImage();
            }
        }

        /// <summary>
        /// Gets or sets 'ComboBox' button image as <see cref="SvgImage"/>.
        /// </summary>
        [Browsable(false)]
        public virtual SvgImage? BtnComboBoxSvg
        {
            get
            {
                return comboBoxSvg;
            }

            set
            {
                if (comboBoxSvg == value)
                    return;
                comboBoxSvg = value;
                UpdateComboBoxImage();
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
                SetHasButton(DefaultBtnEllipsisImage, ref idButtonEllipsis, value);
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
                    SetHasButton(DefaultBtnPlusImage, ref idButtonPlus, value);
                }

                void ToggleMinusButton()
                {
                    SetHasButton(DefaultBtnMinusImage, ref idButtonMinus, value);
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
                return Buttons.GetToolCount() > 0 && Buttons.IsToolClickRepeated;
            }

            set
            {
                Buttons.IsToolClickRepeated = value;
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
        /// Gets or sets <see cref="AbstractControl.SuggestedWidth"/> property
        /// of the main child control.
        /// </summary>
        [DefaultValue(Coord.NaN)]
        public virtual Coord InnerSuggestedWidth
        {
            get => MainControl.SuggestedWidth;
            set => MainControl.SuggestedWidth = value;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.SuggestedHeight"/> property
        /// of the main child control.
        /// </summary>
        [DefaultValue(Coord.NaN)]
        public virtual Coord InnerSuggestedHeight
        {
            get => MainControl.SuggestedHeight;
            set => MainControl.SuggestedHeight = value;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.SuggestedSize"/> property
        /// of the main child control.
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
        public ToolBar Buttons => buttons;

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
            return (MainControl as INotifyDataErrorInfo)?.GetErrors(propertyName)
                ?? Array.Empty<string>();
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
            var result = Buttons.GetToolVisible(id.Value);
            return result;
        }

        /// <inheritdoc/>
        public override bool SetFocus()
        {
            return MainControl.SetFocus();
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
        /// Gets whether default button need to be created in the constructor.
        /// </summary>
        /// <returns></returns>
        protected virtual bool NeedDefaultButton() => true;

        /// <summary>
        /// Creates main child control.
        /// </summary>
        /// <remarks>
        /// For example, main control for the <see cref="TextBoxAndButton"/> is <see cref="TextBox"/>.
        /// </remarks>
        protected abstract AbstractControl CreateControl();

        /// <summary>
        /// Called when combobox image needs to be updated. Assigns combobox image
        /// using different properties of the control.
        /// </summary>
        protected virtual void UpdateComboBoxImage()
        {
            if (idButtonCombo is null)
                return;

            if (comboBoxSvg is null)
            {
                Buttons.SetToolImage(
                    idButtonCombo.Value,
                    comboBoxKnownImage ?? DefaultBtnComboBoxImage);
            }
            else
                Buttons.SetToolSvg(idButtonCombo.Value, comboBoxSvg ?? DefaultBtnComboBoxSvg);
        }

        /// <summary>
        /// Sets whether button with the specified id is visible.
        /// </summary>
        /// <param name="btn">Known button.</param>
        /// <param name="svg">Known button.</param>
        /// <param name="id">Id of the created button.</param>
        /// <param name="value">Whether button is visible.</param>
        private void SetHasButton(
            KnownButton btn,
            ref ObjectUniqueId? id,
            bool value,
            SvgImage? svg = null)
        {
            if (HasButton(id) == value)
                return;
            if (value)
            {
                if (id is null)
                {
                    var isClickRepeat = IsBtnClickRepeated;

                    void ClickAction(object? s, EventArgs e)
                    {
                        ControlAndButtonClickEventArgs args = new();
                        args.ButtonId = (s as AbstractControl)?.UniqueId;
                        RaiseButtonClick(args);
                    }

                    if(svg is null)
                    {
                        id = Buttons.AddSpeedBtn(btn, ClickAction);
                    }
                    else
                    {
                        id = Buttons.AddSpeedBtn(null, svg, ClickAction);
                    }

                    if (isClickRepeat)
                    {
                        Buttons.SetToolIsClickRepeated(id.Value, true);
                    }
                }
                else
                {
                    Buttons.SetToolVisible(id.Value, true);
                }
            }
            else
            {
                if (id is null)
                    return;
                Buttons.SetToolVisible(id.Value, false);
            }
        }
    }
}
