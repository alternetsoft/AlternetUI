using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Panel with Ok, Cancel and Apply buttons.
    /// </summary>
    [ControlCategory("Panels")]
    public partial class PanelOkCancelButtons : StackPanel
    {
        private Button? applyButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelOkCancelButtons"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PanelOkCancelButtons(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelOkCancelButtons"/> class.
        /// </summary>
        public PanelOkCancelButtons()
        {
            SuspendLayout();
            try
            {
                Orientation = StackPanelOrientation.Horizontal;
                HorizontalAlignment = UI.HorizontalAlignment.Right;
                VerticalAlignment = UI.VerticalAlignment.Center;
                OkButton.Parent = this;
                CancelButton.Parent = this;
                OkButton.Click += HandleOkButtonClick;
                CancelButton.Click += HandleCancelButtonClick;
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Gets or sets default margin for the buttons.
        /// </summary>
        public static Thickness DefaultButtonMargin { get; set; } = 5;

        /// <summary>
        /// Gets 'Ok' button.
        /// </summary>
        [Browsable(false)]
        public Button OkButton { get; } = new()
        {
            Text = CommonStrings.Default.ButtonOk,
            Margin = DefaultButtonMargin,
            VerticalAlignment = UI.VerticalAlignment.Center,
            HorizontalAlignment = UI.HorizontalAlignment.Right,
            IsDefault = true,
        };

        /// <summary>
        /// Gets 'Cancel' button.
        /// </summary>
        [Browsable(false)]
        public Button CancelButton { get; } = new()
        {
            Text = CommonStrings.Default.ButtonCancel,
            Margin = DefaultButtonMargin,
            VerticalAlignment = UI.VerticalAlignment.Center,
            HorizontalAlignment = UI.HorizontalAlignment.Right,
            IsCancel = true,
        };

        /// <summary>
        /// Gets 'Apply' button.
        /// </summary>
        [Browsable(false)]
        public Button ApplyButton
        {
            get
            {
                if (applyButton is null)
                {
                    applyButton = new()
                    {
                        Margin = DefaultButtonMargin,
                        Text = CommonStrings.Default.ButtonApply,
                        VerticalAlignment = UI.VerticalAlignment.Center,
                        HorizontalAlignment = UI.HorizontalAlignment.Right,
                        Visible = false,
                        Parent = this,
                    };
                }

                return applyButton;
            }
        }

        /// <summary>
        /// Gets or sets whether 'Apply' button is visible.
        /// </summary>
        public virtual bool ShowApplyButton
        {
            get
            {
                return ApplyButton.Visible;
            }

            set
            {
                ApplyButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether 'Ok' and 'Cancel' buttons change
        /// <see cref="Window.ModalResult"/> when they are clicked.
        /// </summary>
        public virtual bool UseModalResult { get; set; }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        internal Coord SuggestedMinHeight
        {
            get
            {
                return OkButton.Bounds.Height + OkButton.Margin.Vertical;
            }
        }

        /// <summary>
        /// Called when 'Cancel' button is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleCancelButtonClick(object? sender, EventArgs e)
        {
            ApplyModalResult(ModalResult.Canceled);
        }

        /// <summary>
        /// Called when 'Ok' button is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleOkButtonClick(object? sender, EventArgs e)
        {
            ApplyModalResult(ModalResult.Accepted);
        }

        private void ApplyModalResult(ModalResult modalResult)
        {
            if (!UseModalResult)
                return;
            if (ParentWindow is not DialogWindow dialog)
                return;
            dialog.ModalResult = modalResult;
        }
    }
}
