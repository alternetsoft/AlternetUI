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
    public abstract class ControlAndLabel : StackPanel
    {
        private readonly Label label = new()
        {
            Margin = new Thickness(0, 0, 5, 0),
            VerticalAlignment = VerticalAlignment.Center,
        };

        private readonly PictureBox errorPicture = new()
        {
            Margin = new Thickness(5, 0, 0, 0),
        };

        private readonly Control mainControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAndLabel"/> class.
        /// </summary>
        public ControlAndLabel()
            : base()
        {
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
        public Size InnerSuggestedSize
        {
            get => MainControl.SuggestedSize;
            set => MainControl.SuggestedSize = value;
        }

        /// <summary>
        /// Gets attached <see cref="Label"/> control.
        /// </summary>
        public Label Label => label;

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
        public PictureBox ErrorPicture => errorPicture;

        /// <summary>
        /// Gets main child control.
        /// </summary>
        public Control MainControl => mainControl;

        /// <summary>
        /// Creates main child control.
        /// </summary>
        /// <remarks>
        /// For example, main control for the <see cref="TextBoxAndLabel"/> is <see cref="TextBox"/>.
        /// </remarks>
        protected abstract Control CreateControl();
    }
}
