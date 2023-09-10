using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Panel with Ok, Cancel and Apply buttons.
    /// </summary>
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
                Children.Add(OkButton);
                Children.Add(CancelButton);
                Children.Add(ApplyButton);
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Gets or sets default button margin for the Ok, Cancel and Apply buttons.
        /// </summary>
        public static Thickness DefaultButtonMargin { get; set; } = new(5);

        /// <summary>
        /// Gets Ok button.
        /// </summary>
        public Button OkButton { get; } = new()
        {
            Text = CommonStrings.Default.ButtonOk,
            Margin = DefaultButtonMargin,
            IsDefault = true,
        };

        /// <summary>
        /// Gets Cancel button.
        /// </summary>
        public Button CancelButton { get; } = new()
        {
            Text = CommonStrings.Default.ButtonCancel,
            Margin = DefaultButtonMargin,
            IsCancel = true,
        };

        /// <summary>
        /// Gets Apply button.
        /// </summary>
        public Button ApplyButton { get; } = new()
        {
            Margin = DefaultButtonMargin,
            Text = CommonStrings.Default.ButtonApply,
            Visible = false,
        };
    }
}
