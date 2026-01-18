using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements labeled control.
    /// </summary>
    /// <remarks> You can use <see cref="TextBoxAndLabel"/>,
    /// <see cref="ComboBoxAndLabel"/> or derive from <see cref="ControlAndLabel{TControl,TLabel}"/>
    /// in order to implement your own custom labeled control.</remarks>
    /// <typeparam name="TControl">Type of the inner control.</typeparam>
    /// <typeparam name="TLabel">Type of the label.</typeparam>
    [ControlCategory("Hidden")]
    public partial class ControlAndLabel<TControl, TLabel>
        : ControlAndControl, IControlAndLabel, INotifyDataErrorInfo
        where TControl : AbstractControl, new()
        where TLabel : AbstractControl, new()
    {
        /// <summary>
        /// Gets or sets function that creates default labels used in the control.
        /// </summary>
        public static Func<AbstractControl> CreateDefaultLabel = () => new TLabel();

        private readonly AbstractControl label;
        private readonly TControl mainControl;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ControlAndLabel{TControl,TLabel}"/> class with
        /// the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ControlAndLabel(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndLabel{TControl,TLabel}"/> class.
        /// </summary>
        public ControlAndLabel()
        {
            SuspendHandlerTextChange();
            ParentBackColor = true;
            ParentForeColor = true;
            Layout = LayoutStyle.Horizontal;

            label = CreateLabel();
            label.VerticalAlignment = UI.VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Margin = (0, 0, KnownMetrics.ControlLabelDistance, 0);
            label.Parent = this;

            mainControl = CreateControl();
            mainControl.Alignment = (HorizontalAlignment.Fill, VerticalAlignment.Center);
            mainControl.MinWidth = KnownMetrics.InnerControlMinWidth;
            mainControl.Parent = this;
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
        /// Gets or sets <see cref="AbstractControl.SuggestedWidth"/> property of
        /// the main child control.
        /// </summary>
        [DefaultValue(Coord.PositiveInfinity)]
        public virtual Coord LabelSuggestedWidth
        {
            get => Label.SuggestedWidth;
            set => Label.SuggestedWidth = value;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.SuggestedWidth"/> property of
        /// the main child control.
        /// </summary>
        [DefaultValue(Coord.NaN)]
        public virtual Coord InnerSuggestedWidth
        {
            get => MainControl.SuggestedWidth;
            set => MainControl.SuggestedWidth = value;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.SuggestedHeight"/> property of
        /// the main child control.
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
        /// Gets attached <see cref="Label"/> control.
        /// </summary>
        [Browsable(false)]
        public AbstractControl Label => label;

        /// <summary>
        /// Gets or sets visibility of the attached <see cref="Label"/> control.
        /// </summary>
        public virtual bool LabelVisible
        {
            get => Label.Visible;

            set
            {
                if (LabelVisible == value)
                    return;
                Label.Visible = value;
                UpdateErrorPictureLayout();
            }
        }

        /// <summary>
        /// Gets or sets text of the attached <see cref="Label"/> control.
        /// </summary>
        public override object? TitleAsObject
        {
            get => Label.Text;
            set => Label.Text = value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Gets main child control.
        /// </summary>
        [Browsable(false)]
        public TControl MainControl => mainControl;

        /// <inheritdoc/>
        public override bool HasErrors
        {
            get => (MainControl as INotifyDataErrorInfo)?.HasErrors ?? false;
        }

        AbstractControl IControlAndLabel.Label => Label;

        AbstractControl IControlAndLabel.MainControl => MainControl;

        /// <summary>
        /// Gets or sets a value which specifies label and control alignment.
        /// </summary>
        public virtual StackPanelOrientation LabelToControl
        {
            get
            {
                if (Layout == LayoutStyle.Horizontal)
                    return StackPanelOrientation.Horizontal;
                else
                    return StackPanelOrientation.Vertical;
            }

            set
            {
                if (value == LabelToControl)
                    return;
                PerformLayoutAndInvalidate(() =>
                {
                    if (value == StackPanelOrientation.Horizontal)
                    {
                        label.Margin = (0, 0, KnownMetrics.ControlLabelDistance, 0);
                        Layout = LayoutStyle.Horizontal;
                    }
                    else
                    {
                        label.Margin = (0, 0, 0, KnownMetrics.ControlLabelDistance);
                        Layout = LayoutStyle.Vertical;
                    }

                    UpdateErrorPictureLayout();
                });
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

        /// <inheritdoc/>
        public override bool SetFocus()
        {
            return MainControl.SetFocus();
        }

        /// <summary>
        /// Creates main child control.
        /// </summary>
        /// <remarks>
        /// For example, main control for the <see cref="TextBoxAndLabel"/>
        /// is <see cref="TextBox"/>.
        /// </remarks>
        protected virtual TControl CreateControl() => new();

        /// <summary>
        /// Creates label control.
        /// </summary>
        /// <remarks>
        /// By default <see cref="Label"/> is created.
        /// </remarks>
        protected virtual AbstractControl CreateLabel() => CreateDefaultLabel();

        /// <inheritdoc/>
        protected override void UpdateErrorPictureLayout()
        {
            if (!IsErrorPictureCreated)
                return;

            ErrorPicture.IsImageCentered = !LabelVisible
                || LabelToControl == StackPanelOrientation.Horizontal;
        }
    }
}
