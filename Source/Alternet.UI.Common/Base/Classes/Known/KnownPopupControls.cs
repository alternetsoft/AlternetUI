using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a class that provides factory methods for creating instances of inner popup controls.
    /// This class can be extended to provide custom implementations of the inner popup controls.
    /// Library uses this class to create instances of inner popup controls,
    /// such as <see cref="InnerPopupTextBox"/> and <see cref="InnerPopupToolBar"/>. Assign 
    /// a custom instance to <see cref="Default"/> to override the default behavior.
    /// </summary>
    public partial class KnownPopupControls : HostedDisposableObject
    {
        /// <summary>
        /// Gets or sets the default instance of <see cref="KnownPopupControls"/> used by the framework.
        /// </summary>
        public static KnownPopupControls Default { get; set; } = new ();

        private InnerPopupTextBox? popupTextBox;

        /// <summary>
        /// Gets instance of <see cref="InnerPopupTextBox"/> control.
        /// If it is not created yet, it will be created using <see cref="CreateInnerPopupTextBox"/> method.
        /// A single instance of <see cref="InnerPopupTextBox"/> returned by
        /// this property can be resused for multiple editing operations. For example different
        /// <see cref="VirtualListBox"/> controls share
        /// the same instance of <see cref="InnerPopupTextBox"/> for editing their items.
        /// </summary>
        /// <returns></returns>
        public virtual InnerPopupTextBox PopupTextBox
        {
            get
            {
                popupTextBox ??= CreateInnerPopupTextBox();
                return popupTextBox;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="InnerPopupTextBox"/> control.
        /// Override this method to provide a custom implementation of <see cref="InnerPopupTextBox"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="InnerPopupTextBox"/> control.</returns>
        public virtual InnerPopupTextBox CreateInnerPopupTextBox()
        {
            return new InnerPopupTextBox();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="InnerPopupToolBar"/> class.
        /// Override this method to provide a custom implementation of <see cref="InnerPopupToolBar"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="InnerPopupToolBar"/> class.</returns>
        public virtual InnerPopupToolBar CreateInnerPopupToolBar(InnerPopupToolBar.CreateFlags flags)
        {
            return new InnerPopupToolBar(flags);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="InnerPopupTreeView"/> class.
        /// Override this method to provide a custom implementation of <see cref="InnerPopupTreeView"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="InnerPopupTreeView"/> class.</returns>
        public virtual InnerPopupTreeView CreateInnerPopupTreeView(InnerPopupTreeView.CreateFlags flags)
        {
            return new InnerPopupTreeView(flags);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PopupControl{T}"/> class.
        /// Override this method to provide a custom implementation of <see cref="PopupControl{T}"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="PopupControl{T}"/> class.</returns>
        public virtual PopupControl<T> CreatePopupControl<T>()
            where T : AbstractControl, new()
        {
            return new PopupControl<T>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PopupControl"/> class.
        /// Override this method to provide a custom implementation of <see cref="PopupControl"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="PopupControl"/> class.</returns>
        public virtual PopupControl CreatePopupControl()
        {
            return new PopupControl();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ResizablePopupControl{T}"/> class.
        /// Override this method to provide a custom implementation of <see cref="ResizablePopupControl{T}"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="ResizablePopupControl{T}"/> class.</returns>
        public virtual ResizablePopupControl<T> CreateResizablePopupControl<T>(ResizablePopupControl.CreateFlags flags)
            where T : AbstractControl, new()
        {
            return new ResizablePopupControl<T>(flags);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ResizablePopupControl"/> class.
        /// Override this method to provide a custom implementation of <see cref="ResizablePopupControl"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="ResizablePopupControl"/> class.</returns>
        public virtual ResizablePopupControl CreateResizablePopupControl(ResizablePopupControl.CreateFlags flags)
        {
            return new ResizablePopupControl(flags);
        }
    }
}
