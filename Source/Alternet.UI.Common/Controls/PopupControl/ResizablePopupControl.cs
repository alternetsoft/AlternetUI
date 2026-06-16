using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a resizable popup control with a window like border and title bar.
    /// This popup control is supposed to be shown inside the client area of another control.
    /// </summary>
    public partial class ResizablePopupControl : PopupControl
    {
        private ResizableWindowBorder? border;
        private ScrollViewer? scrollViewer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizablePopupControl"/> class.
        /// </summary>
        public ResizablePopupControl()
        {
        }

        /// <summary>
        /// Gets the border control of the popup.
        /// </summary>
        public ResizableWindowBorder BorderControl
        {
            get
            {
                if (border is null)
                {
                    border = new();
                    border.ParentFont = true;
                    border.Parent = this;
                    border.GripControl.Target = this;
                    border.SetResizeTarget(this);
                }

                return border;
            }
        }

        /// <summary>
        /// Gets the scroll viewer control of the popup.
        /// </summary>
        public ScrollViewer ScrollViewer
        {
            get
            {
                if (scrollViewer is null)
                {
                    scrollViewer = new();
                    scrollViewer.VerticalAlignment = VerticalAlignment.Fill;
                    scrollViewer.Parent = BorderControl.FillPanel;
                    scrollViewer.ParentBackColor = true;
                    scrollViewer.ParentForeColor = true;
                }

                return scrollViewer;
            }
        }
    }
}
