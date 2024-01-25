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
    public class PanelOkCancelButtons : StackPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PanelOkCancelButtons"/> class.
        /// </summary>
        public PanelOkCancelButtons()
        {
            SuspendLayout();
            try
            {
                Orientation = StackPanelOrientation.Horizontal;
                HorizontalAlignment = HorizontalAlignment.Right;
                VerticalAlignment = VerticalAlignment.Center;
                OkButton.Parent = this;
                CancelButton.Parent = this;
                ApplyButton.Parent = this;
                OkButton.Click += OkButton_Click;
                CancelButton.Click += CancelButton_Click;
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Gets or sets default button margin for the 'Ok', 'Cancel' and 'Apply' buttons.
        /// </summary>
        public static Thickness DefaultButtonMargin { get; set; } = new(5);

        /// <summary>
        /// Gets 'Ok' button.
        /// </summary>
        [Browsable(false)]
        public Button OkButton { get; } = new()
        {
            Text = CommonStrings.Default.ButtonOk,
            Margin = DefaultButtonMargin,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
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
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
            IsCancel = true,
        };

        /// <summary>
        /// Gets 'Apply' button.
        /// </summary>
        [Browsable(false)]
        public Button ApplyButton { get; } = new()
        {
            Margin = DefaultButtonMargin,
            Text = CommonStrings.Default.ButtonApply,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
            Visible = false,
        };

        /// <summary>
        /// Gets or sets whether 'Apply' button is visible.
        /// </summary>
        public bool ShowApplyButton
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
        /// <see cref="DialogWindow.ModalResult"/> when they are clicked.
        /// </summary>
        public bool UseModalResult { get; set; }

        internal double SuggestedMinHeight
        {
            get
            {
                return OkButton.Bounds.Height + OkButton.Margin.Vertical;
            }
        }

        private void ApplyModalResult(ModalResult modalResult)
        {
            if (!UseModalResult)
                return;
            if (ParentWindow is not DialogWindow dialog)
                return;
            dialog.ModalResult = modalResult;
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            ApplyModalResult(ModalResult.Canceled);
        }

        private void OkButton_Click(object? sender, EventArgs e)
        {
            ApplyModalResult(ModalResult.Accepted);
        }
    }
}
