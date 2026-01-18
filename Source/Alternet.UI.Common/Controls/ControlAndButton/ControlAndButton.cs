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
        /// Represents the default margin applied to <see cref="SubstituteControl"/>.
        /// </summary>
        public static Thickness DefaultSubstituteControlMargin = (3, 0, 3, 0);

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

        private bool autoBackColor = false;
        private ObjectUniqueId? idButtonCombo;
        private ObjectUniqueId? idButtonEllipsis;
        private ObjectUniqueId? idButtonPlus;
        private ObjectUniqueId? idButtonMinus;
        private KnownButton? comboBoxKnownImage;
        private SvgImage? comboBoxSvg;
        private AbstractControl? substituteControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndButton"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ControlAndButton(AbstractControl parent)
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

                mainControl.TextChanged += (s, e) =>
                {
                    UpdateSubstituteControlText();
                };

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
        /// Occurs when button is clicked.
        /// </summary>
        public event EventHandler<ControlAndButtonClickEventArgs>? ButtonClick;

        /// <summary>
        /// Gets whether substitute control is created.
        /// </summary>
        [Browsable(false)]
        public bool IsSubstituteControlCreated => substituteControl != null;

        /// <summary>
        /// Gets or sets whether substitute control is used instead of the main child control.
        /// </summary>
        public virtual bool UseSubstituteControl
        {
            get => !mainControl.Visible;
            set
            {
                if (value)
                {
                    DoInsideLayout(() =>
                    {
                        MainControl.Visible = false;
                        SubstituteControl.Visible = true;
                    });
                }
                else
                {
                    DoInsideLayout(() =>
                    {
                        substituteControl?.SetVisible(false);
                        MainControl.Visible = true;
                    });
                }
            }
        }

        /// <summary>
        /// Gets or sets substitute control which can be used instead of the main child control.
        /// </summary>
        [Browsable(false)]
        public virtual AbstractControl SubstituteControl
        {
            get
            {
                if (substituteControl != null)
                    return substituteControl;
                SubstituteControl = CreateSubstituteControl();
                return substituteControl!;
            }

            set
            {
                if (substituteControl == value)
                    return;

                if (substituteControl is not null)
                {
                    substituteControl.MouseLeftButtonDown -= OnSubstituteControlMouseLeftButtonDown;
                    substituteControl.Parent = null;
                }

                substituteControl = value;

                if (substituteControl != null)
                {
                    substituteControl.Visible = false;
                    substituteControl.HasBorder = MainControl.HasBorder;
                    substituteControl.Alignment = MainControl.Alignment;
                    substituteControl.Margin = DefaultSubstituteControlMargin;
                    substituteControl.Parent = this;
                    substituteControl.MouseLeftButtonDown -= OnSubstituteControlMouseLeftButtonDown;
                    substituteControl.MouseLeftButtonDown += OnSubstituteControlMouseLeftButtonDown;
                    UpdateSubstituteControlText();
                }
            }
        }

        /// <summary>
        /// Gets or sets 'Ellipsis' button image as <see cref="UI.KnownButton"/>.
        /// Default is null, which means that <see cref="DefaultBtnEllipsisImage"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual KnownButton? BtnEllipsisImage { get; set; }

        /// <summary>
        /// Gets or sets 'Plus' button image as <see cref="UI.KnownButton"/>.
        /// Default is null, which means that <see cref="DefaultBtnPlusImage"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual KnownButton? BtnPlusImage { get; set; }

        /// <summary>
        /// Gets or sets 'Plus' button image as <see cref="SvgImage"/>.
        /// Default is null, which means that <see cref="BtnPlusImage"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual SvgImage? BtnPlusImageSvg { get; set; }

        /// <summary>
        /// Gets or sets 'Ellipsis' button image as <see cref="SvgImage"/>.
        /// Default is null, which means that <see cref="BtnEllipsisImage"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual SvgImage? BtnEllipsisImageSvg { get; set; }

        /// <summary>
        /// Gets or sets 'Minus' button image as <see cref="SvgImage"/>.
        /// Default is null, which means that <see cref="BtnMinusImage"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual SvgImage? BtnMinusImageSvg { get; set; }

        /// <summary>
        /// Gets or sets 'Minus' button image as <see cref="UI.KnownButton"/>.
        /// Default is null, which means that <see cref="DefaultBtnMinusImage"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual KnownButton? BtnMinusImage { get; set; }

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
                Buttons.OverrideButtonType(GetBtnComboType(), () =>
                {
                    SetHasButton(
                        comboBoxKnownImage ?? DefaultBtnComboBoxImage,
                        ref idButtonCombo,
                        value,
                        comboBoxSvg ?? DefaultBtnComboBoxSvg);
                });
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
                Buttons.OverrideButtonType(GetBtnEllipsisType(), () =>
                {
                    SetHasButton(
                        BtnEllipsisImage ?? DefaultBtnEllipsisImage,
                        ref idButtonEllipsis,
                        value,
                        BtnEllipsisImageSvg);
                });
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
                    Buttons.OverrideButtonType(GetBtnPlusMinusType(), () =>
                    {
                        SetHasButton(
                            BtnPlusImage ?? DefaultBtnPlusImage,
                            ref idButtonPlus,
                            value,
                            BtnPlusImageSvg);
                    });
                }

                void ToggleMinusButton()
                {
                    Buttons.OverrideButtonType(GetBtnPlusMinusType(), () =>
                    {
                        SetHasButton(
                            BtnMinusImage ?? DefaultBtnMinusImage,
                            ref idButtonMinus,
                            value,
                            BtnMinusImageSvg);
                    });
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

        /// <summary>
        /// Gets or sets whether main inner control has border.
        /// </summary>
        public virtual bool HasInnerBorder
        {
            get
            {
                return MainControl.HasBorder;
            }

            set
            {
                MainControl.HasBorder = value;
                if(substituteControl is not null)
                    substituteControl.HasBorder = value;
            }
        }

        /// <summary>
        /// Gets combo button if it is available.
        /// </summary>
        [Browsable(false)]
        public SpeedButton? ButtonCombo => buttons.FindTool(IdButtonCombo);

        /// <summary>
        /// Gets ellipsis button if it is available.
        /// </summary>
        [Browsable(false)]
        public SpeedButton? ButtonEllipsis => buttons.FindTool(IdButtonEllipsis);

        /// <summary>
        /// Gets 'Plus' button if it is available.
        /// </summary>
        [Browsable(false)]
        public SpeedButton? ButtonPlus => buttons.FindTool(IdButtonPlus);

        /// <summary>
        /// Gets 'Minus' button if it is available.
        /// </summary>
        [Browsable(false)]
        public SpeedButton? ButtonMinus => buttons.FindTool(IdButtonMinus);

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
                if (changed)
                    RaiseAutoBackColorChanged();
            }
        }

        /// <inheritdoc/>
        public override bool HasErrors
        {
            get => (MainControl as INotifyDataErrorInfo)?.HasErrors ?? false;
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
                if (value)
                    RaiseAutoBackColorChanged();
            }
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

        /// <summary>
        /// Sets the SVG image and/or known image for the 'Minus' button.
        /// </summary>
        /// <param name="svg">The SVG image for the 'Minus' button.</param>
        /// <param name="knownImage">The known image for the 'Minus' button.</param>
        public virtual void SetMinusImage(SvgImage? svg, KnownButton? knownImage)
        {
            BtnMinusImage = knownImage;
            BtnMinusImageSvg = svg;

            if (IdButtonMinus is not null)
            {
                if (svg is not null)
                {
                    Buttons.SetToolSvg(IdButtonMinus.Value, BtnMinusImageSvg);
                }
                else
                {
                    Buttons.SetToolImage(
                    IdButtonMinus.Value,
                    BtnMinusImage ?? DefaultBtnMinusImage);
                }
            }
        }

        /// <summary>
        /// Sets the SVG image and/or known image for the 'Plus' button.
        /// </summary>
        /// <param name="svg">The SVG image for the 'Plus' button.</param>
        /// <param name="knownImage">The known image for the 'Plus' button.</param>
        public virtual void SetPlusImage(SvgImage? svg, KnownButton? knownImage)
        {
            BtnPlusImage = knownImage;
            BtnPlusImageSvg = svg;

            if (IdButtonPlus is not null)
            {
                if (svg is not null)
                {
                    Buttons.SetToolSvg(IdButtonPlus.Value, BtnPlusImageSvg);
                }
                else
                {
                    Buttons.SetToolImage(
                    IdButtonPlus.Value,
                    BtnPlusImage ?? DefaultBtnPlusImage);
                }
            }
        }

        /// <summary>
        /// Sets images for the plus and minus buttons.
        /// </summary>
        /// <param name="plusImage">The image for the plus button.</param>
        /// <param name="minusImage">The image for the minus button.</param>
        public virtual void SetPlusMinusImages(
            SvgImage? plusImage,
            SvgImage? minusImage)
        {
            SetMinusImage(minusImage, null);
            SetPlusImage(plusImage, null);
        }

        /// <summary>
        /// Sets images for the plus and minus buttons.
        /// </summary>
        /// <param name="plusImage">The image for the plus button.</param>
        /// <param name="minusImage">The image for the minus button.</param>
        public virtual void SetPlusMinusImages(
            KnownButton? plusImage,
            KnownButton? minusImage)
        {
            SetMinusImage(null, minusImage);
            SetPlusImage(null, plusImage);
        }

        /// <inheritdoc/>
        public override bool SetFocus()
        {
            if(MainControl.CanFocus)
                return MainControl.SetFocus();
            return base.SetFocus();
        }

        /// <summary>
        /// Initializes buttons so only combo button with the specified image remains visible.
        /// </summary>
        /// <param name="button">Known button identifier.</param>
        public virtual void SetSingleButton(UI.KnownButton? button)
        {
            Buttons.SetChildrenVisible(false);
            HasBtnComboBox = true;
            BtnComboBoxSvg = null;
            ButtonOverride = button;
        }

        /// <summary>
        /// Reports an error information.
        /// </summary>
        /// <param name="showError">Indicates whether to show the error.</param>
        /// <param name="errorText">The error message to display.</param>
        public virtual void ReportError(
            bool showError,
            string? errorText = null)
        {
            ErrorPicture.Visible = showError;
            (ErrorPicture as IValidatorReporter)?.SetErrorStatus(this, showError, errorText);
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
        /// Called when mouse left button is pressed on the substitute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnSubstituteControlMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
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

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            if (AutoUpdateColors)
                RaiseAutoBackColorChanged();

            base.OnSystemColorsChanged(e);
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
        /// Creates substitute control which can be optionally used instead of the main child control.
        /// By default, it is <see cref="Label"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractControl CreateSubstituteControl()
        {
            return new Label();
        }

        /// <summary>
        /// Called when combo box image needs to be updated. Assigns combo box image
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
        /// Gets type of the <see cref="SpeedButton"/> ancestor class used
        /// as combo box button.
        /// </summary>
        /// <returns></returns>
        protected virtual Type GetBtnComboType()
        {
            return typeof(SpeedButton);
        }

        /// <summary>
        /// Gets type of the <see cref="SpeedButton"/> ancestor class used
        /// as 'Plus' and 'Minus' buttons.
        /// </summary>
        /// <returns></returns>
        protected virtual Type GetBtnPlusMinusType()
        {
            return typeof(SpeedButton);
        }

        /// <summary>
        /// Updates the text of the substitute control to match the text of the main control.
        /// </summary>
        /// <remarks>This method synchronizes the text of the substitute control with
        /// the text of the main control. It is intended to be overridden in derived
        /// classes to customize the behavior.</remarks>
        protected virtual void UpdateSubstituteControlText()
        {
            if (!IsSubstituteControlCreated)
                return;

            SubstituteControl.Text = mainControl.Text;
            SubstituteControl.Refresh();
        }

        /// <summary>
        /// Gets type of the <see cref="SpeedButton"/> ancestor class used
        /// as 'Ellipsis' button.
        /// </summary>
        /// <returns></returns>
        protected virtual Type GetBtnEllipsisType()
        {
            return typeof(SpeedButton);
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
            {
                return;
            }

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

                    id = Buttons.AddSpeedBtn(btn, svg, ClickAction);

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
