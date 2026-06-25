using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a class that provides factory methods for creating instances of inner popup controls.
    /// </summary>
    public class KnownPopupControls
    {
        /// <summary>
        /// Gets or sets the default instance of <see cref="KnownPopupControls"/> used by the framework.
        /// </summary>
        public static KnownPopupControls Default { get; set; } = new ();

        /// <summary>
        /// Creates a new instance of the <see cref="InnerPopupToolBar"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="InnerPopupToolBar"/> class.</returns>
        public virtual InnerPopupToolBar CreateInnerPopupToolBar(InnerPopupToolBar.CreateFlags flags)
        {
            return new InnerPopupToolBar(flags);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="InnerPopupTreeView"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="InnerPopupTreeView"/> class.</returns>
        public virtual InnerPopupTreeView CreateInnerPopupTreeView(InnerPopupTreeView.CreateFlags flags)
        {
            return new InnerPopupTreeView(flags);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PopupControl{T}"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="PopupControl{T}"/> class.</returns>
        public virtual PopupControl<T> CreatePopupControl<T>()
            where T : AbstractControl, new()
        {
            return new PopupControl<T>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PopupControl"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="PopupControl"/> class.</returns>
        public virtual PopupControl CreatePopupControl()
        {
            return new PopupControl();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ResizablePopupControl{T}"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="ResizablePopupControl{T}"/> class.</returns>
        public virtual ResizablePopupControl<T> CreateResizablePopupControl<T>(ResizablePopupControl.CreateFlags flags)
            where T : AbstractControl, new()
        {
            return new ResizablePopupControl<T>(flags);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ResizablePopupControl"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="ResizablePopupControl"/> class.</returns>
        public virtual ResizablePopupControl CreateResizablePopupControl(ResizablePopupControl.CreateFlags flags)
        {
            return new ResizablePopupControl(flags);
        }
    }
}
