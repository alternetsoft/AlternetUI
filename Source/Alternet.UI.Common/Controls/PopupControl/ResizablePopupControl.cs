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
        private CreateFlags flags;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizablePopupControl"/> class.
        /// </summary>
        public ResizablePopupControl(CreateFlags flags)
        {
            this.flags = flags;
        }

        /// <summary>
        /// Enumerates the flags that can be used to customize the creation of the <see cref="ResizablePopupControl"/>.
        /// </summary>
        [Flags]
        public enum CreateFlags
        {
            /// <summary>
            /// No flags specified. The popup will not have scroll bars and will not be resizable.
            /// </summary>
            None = 0,
            
            /// <summary>
            /// The popup can be resized by the user.
            /// </summary>
            Resizable = 1,

            /// <summary>
            /// The popup will have scroll bars if the content exceeds the visible area.
            /// </summary>
            Scrollable = 2,

            /// <summary>
            /// The popup will have both scroll bars and can be resized by the user.
            /// </summary>
            ResizableAndScrollable = Resizable | Scrollable,
        }

        /// <summary>
        /// Gets the border control of the popup.
        /// </summary>
        public virtual ResizableWindowBorder BorderControl
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
        public virtual ScrollViewer ScrollViewer
        {
            get
            {
                if (scrollViewer is null)
                {
                    scrollViewer = new();
                    scrollViewer.VerticalAlignment = VerticalAlignment.Fill;

                    if (flags.HasFlag(CreateFlags.Resizable))
                    {
                        scrollViewer.Parent = BorderControl.FillPanel;
                    }
                    else
                    {
                        scrollViewer.Parent = this;
                    }

                    scrollViewer.ParentBackColor = true;
                    scrollViewer.ParentForeColor = true;
                }

                return scrollViewer;
            }
        }
    }
}
