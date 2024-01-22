using System;
using System.Collections.Generic;
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
    public abstract class ControlAndLabel : StackPanel, IControlAndLabel
    {
        /// <summary>
        /// Gets or sets default distance between control and label.
        /// </summary>
        public static double DefaultControlLabelDistance = 5;

        /// <summary>
        /// Gets or sets function that creates default labels for the <see cref="ControlAndLabel"/>
        /// controls.
        /// </summary>
        public static Func<Control> CreateDefaultLabel = () => new Label();

        private readonly PictureBox errorPicture = new()
        {
            Margin = new Thickness(DefaultControlLabelDistance, 0, 0, 0),
        };

        private readonly Control label;
        private readonly Control mainControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndLabel"/> class.
        /// </summary>
        public ControlAndLabel()
            : base()
        {
            label = CreateLabel();
            label.Margin = new Thickness(0, 0, DefaultControlLabelDistance, 0);
            label.VerticalAlignment = VerticalAlignment.Center;

            Orientation = StackPanelOrientation.Horizontal;
            label.Parent = this;
            mainControl = CreateControl();
            mainControl.VerticalAlignment = VerticalAlignment.Center;
            mainControl.Parent = this;
            TextBox.InitErrorPicture(errorPicture);
            errorPicture.Parent = this;
        }

        /// <summary>
        /// Gets or sets <see cref="Control.ColumnIndex"/> property of the attached
        /// <see cref="Label"/> control.
        /// </summary>
        public int? LabelColumnIndex
        {
            get
            {
                return Label.ColumnIndex;
            }

            set
            {
                Label.ColumnIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Control.SuggestedWidth"/> property of the main child control.
        /// </summary>
        public double LabelSuggestedWidth
        {
            get => Label.SuggestedWidth;
            set => Label.SuggestedWidth = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Control.SuggestedWidth"/> property of the main child control.
        /// </summary>
        public double InnerSuggestedWidth
        {
            get => MainControl.SuggestedWidth;
            set => MainControl.SuggestedWidth = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Control.SuggestedHeight"/> property of the main child control.
        /// </summary>
        public double InnerSuggestedHeight
        {
            get => MainControl.SuggestedHeight;
            set => MainControl.SuggestedHeight = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Control.SuggestedSize"/> property of the main child control.
        /// </summary>
        public SizeD InnerSuggestedSize
        {
            get => MainControl.SuggestedSize;
            set => MainControl.SuggestedSize = value;
        }

        /// <summary>
        /// Gets attached <see cref="Label"/> control.
        /// </summary>
        [Browsable(false)]
        public Control Label => label;

        /// <summary>
        /// Gets or sets visibility of the attached <see cref="Label"/> control.
        /// </summary>
        public bool LabelVisible
        {
            get => Label.Visible;
            set => Label.Visible = value;
        }

        /// <summary>
        /// Gets or sets visibility of the attached <see cref="PictureBox"/> control which
        /// displays validation error information.
        /// </summary>
        public bool ErrorPictureVisible
        {
            get => ErrorPicture.Visible;
            set => ErrorPicture.Visible = value;
        }

        /// <summary>
        /// Gets or sets text of the attached <see cref="Label"/> control.
        /// </summary>
        public string Title
        {
            get => Label.Text;
            set => Label.Text = value;
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
        public Control MainControl => mainControl;

        Control IControlAndLabel.Label => Label;

        Control IControlAndLabel.MainControl => MainControl;

        /// <summary>
        /// Creates main child control.
        /// </summary>
        /// <remarks>
        /// For example, main control for the <see cref="TextBoxAndLabel"/> is <see cref="TextBox"/>.
        /// </remarks>
        protected abstract Control CreateControl();

        /// <summary>
        /// Creates label control.
        /// </summary>
        /// <remarks>
        /// By default <see cref="Label"/> is created.
        /// </remarks>
        protected virtual Control CreateLabel() => CreateDefaultLabel();
    }
}
