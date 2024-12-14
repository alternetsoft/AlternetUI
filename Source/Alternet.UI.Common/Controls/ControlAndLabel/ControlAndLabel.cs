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
    /// <remarks> This is an abstract class. You can use <see cref="TextBoxAndLabel"/>,
    /// <see cref="ComboBoxAndLabel"/> or derive from <see cref="ControlAndLabel"/>
    /// in order to implement your own custom labeled control.</remarks>
    [ControlCategory("Hidden")]
    public abstract partial class ControlAndLabel
        : HiddenBorder, IControlAndLabel, INotifyDataErrorInfo
    {
        /// <summary>
        /// Gets or sets default distance between control and label.
        /// </summary>
        public static Coord DefaultControlLabelDistance = 5;

        /// <summary>
        /// Gets or sets function that creates default labels for the <see cref="ControlAndLabel"/>
        /// controls.
        /// </summary>
        public static Func<AbstractControl> CreateDefaultLabel = () => new Label();

        private readonly PictureBox errorPicture = new()
        {
            Margin = new Thickness(DefaultControlLabelDistance, 0, 0, 0),
        };

        private readonly AbstractControl label;
        private readonly AbstractControl mainControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndLabel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ControlAndLabel(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndLabel"/> class.
        /// </summary>
        public ControlAndLabel()
        {
            Layout = LayoutStyle.Horizontal;

            label = CreateLabel();
            label.Margin = new Thickness(0, 0, DefaultControlLabelDistance, 0);
            label.VerticalAlignment = UI.VerticalAlignment.Center;
            label.Parent = this;

            mainControl = CreateControl();
            mainControl.Alignment = (HorizontalAlignment.Fill, VerticalAlignment.Center);
            mainControl.Parent = this;

            errorPicture.Alignment = (HorizontalAlignment.Right, VerticalAlignment.Center);
            CustomTextBox.InitErrorPicture(errorPicture);
            errorPicture.Parent = this;
            errorPicture.ParentBackColor = true;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.SuggestedWidth"/> property of the main child control.
        /// </summary>
        [DefaultValue(Coord.PositiveInfinity)]
        public virtual Coord LabelSuggestedWidth
        {
            get => Label.SuggestedWidth;
            set => Label.SuggestedWidth = value;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.SuggestedWidth"/> property of the main child control.
        /// </summary>
        [DefaultValue(Coord.NaN)]
        public virtual Coord InnerSuggestedWidth
        {
            get => MainControl.SuggestedWidth;
            set => MainControl.SuggestedWidth = value;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.SuggestedHeight"/> property of the main child control.
        /// </summary>
        [DefaultValue(Coord.NaN)]
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
            set => Label.Visible = value;
        }

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
        /// Gets or sets text of the attached <see cref="Label"/> control.
        /// </summary>
        public override object? TitleAsObject
        {
            get => Label.Text;
            set => Label.Text = value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Gets attached <see cref="PictureBox"/> control which
        /// displays validation error information.
        /// </summary>
        [Browsable(false)]
        public PictureBox ErrorPicture => errorPicture;

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

        AbstractControl IControlAndLabel.Label => Label;

        AbstractControl IControlAndLabel.MainControl => MainControl;

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

        /// <inheritdoc/>
        public override bool SetFocus()
        {
            return MainControl.SetFocus();
        }

        /// <summary>
        /// Creates main child control.
        /// </summary>
        /// <remarks>
        /// For example, main control for the <see cref="TextBoxAndLabel"/> is <see cref="TextBox"/>.
        /// </remarks>
        protected abstract AbstractControl CreateControl();

        /// <summary>
        /// Creates label control.
        /// </summary>
        /// <remarks>
        /// By default <see cref="Label"/> is created.
        /// </remarks>
        protected virtual AbstractControl CreateLabel() => CreateDefaultLabel();
    }
}
