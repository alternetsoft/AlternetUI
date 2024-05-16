using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// The <see cref="Popup"/> control displays content in a separate window that floats
    /// over the current application window.
    /// </summary>
    [DesignerCategory("Code")]
    [ControlCategory("Hidden")]
    internal partial class Popup : Control
    {
        /*private Window? owner;*/

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        public Popup()
        {
            IgnoreLayout = true;
            SetVisibleValue(false);

            Bounds = new RectD(0, 0, 100, 100);
        }

        /*/// <summary>
        /// Occurs when the value of the <see cref="Owner"/> property changes.
        /// </summary>
        public event EventHandler? OwnerChanged;*/

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Popup;

        /*/// <summary>
        /// Gets or sets the window that owns this popup.
        /// </summary>
        public Window? Owner
        {
            get => owner;

            set
            {
                owner = value;
                OnOwnerChanged(EventArgs.Empty);
                OwnerChanged?.Invoke(this, EventArgs.Empty);
            }
        }*/

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user clicks mouse outside it or if it loses focus in any other way.
        /// </summary>
        /// <remarks>
        /// This style can be useful for implementing custom combobox-like controls for example.
        /// </remarks>
        public bool IsTransient
        {
            get
            {
                return Handler.NativeControl.IsTransient;
            }

            set
            {
                Handler.NativeControl.IsTransient = value;
            }
        }

        /// <summary>
        /// Gets or sets border style of the popup.
        /// </summary>
        public new ControlBorderStyle BorderStyle
        {
            get => base.BorderStyle;
            set => base.BorderStyle = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a popup window will take focus from
        /// its parent window.
        /// </summary>
        /// <remarks>
        /// By default in Windows, a popup window will not take focus from its parent window.
        /// However many standard controls, including common ones such as <see cref="TextBox"/>,
        /// need focus to function correctly and will not work when placed on a default popup.
        /// This flag can be used to make the popup take focus and let all controls work but at
        /// the price of not allowing the parent window to keep focus while the popup is shown,
        /// which can also be sometimes desirable. This style is currently only implemented
        /// in Windows and simply does nothing under the other platforms.
        /// </remarks>
        public bool PuContainsControls
        {
            get
            {
                return Handler.NativeControl.PuContainsControls;
            }

            set
            {
                Handler.NativeControl.PuContainsControls = value;
            }
        }

        private new PopupHandler Handler
        {
            get
            {
                CheckDisposed();
                return (PopupHandler)base.Handler;
            }
        }

        /// <summary>
        /// Move the popup window to the right position, i.e. such that it is entirely visible.
        /// </summary>
        /// <param name="ptOrigin">Must be given in screen coordinates.</param>
        /// <param name="sizePopup">The size of the popup window.</param>
        /// <remarks>
        /// The popup is positioned at (ptOrigin + size) if it opens below and to the right
        /// (default), at (ptOrigin - sizePopup) if it opens above and to the left.
        /// </remarks>
        /// <remarks>
        /// <paramref name="ptOrigin"/> and <paramref name="sizePopup"/> are specified in pixels.
        /// </remarks>
        public void SetPositionInPixels(PointI ptOrigin, SizeI sizePopup)
        {
            Handler.NativeControl.Position(ptOrigin, sizePopup);
        }

        /// <summary>
        /// Popup the window (this will show it too).
        /// </summary>
        /// <param name="focusControl">If this is non-NULL, it will be kept focused
        /// while this window is shown, otherwise this window itself will receive focus.
        /// In any case, the
        /// popup will disappear automatically if it loses focus because of a
        /// user action.</param>
        public void DoPopup(Control? focusControl = null)
        {
            if (focusControl is null)
                Handler.NativeControl.DoPopup(default);
            else
                Handler.NativeControl.DoPopup(WxPlatform.WxWidget(focusControl));
        }

        /// <summary>
        /// Hide the popup window.
        /// </summary>
        public void Dismiss()
        {
            Handler.NativeControl.Dismiss();
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler() => new PopupHandler();

        internal class PopupHandler : WxControlHandler, IControlHandler
        {
            /// <summary>
            /// Gets a <see cref="Popup"/> this handler provides the implementation for.
            /// </summary>
            public new Popup Control => (Popup)base.Control;

            public new Native.Popup NativeControl => (Native.Popup)base.NativeControl!;

            /// <summary>
            /// Changes size of the popup to fit the size of its content.
            /// </summary>
            public void SetSizeToContent(WindowSizeToContentMode mode)
            {
                if (mode == WindowSizeToContentMode.None)
                    return;

                var newSize = Control.GetChildrenMaxPreferredSizePadded(new SizeD(double.PositiveInfinity, double.PositiveInfinity));
                if (newSize != SizeD.Empty)
                {
                    var currentSize = Control.ClientSize;
                    switch (mode)
                    {
                        case WindowSizeToContentMode.Width:
                            newSize.Height = currentSize.Height;
                            break;

                        case WindowSizeToContentMode.Height:
                            newSize.Width = currentSize.Width;
                            break;

                        case WindowSizeToContentMode.WidthAndHeight:
                            break;

                        default:
                            throw new Exception();
                    }

                    Control.ClientSize = newSize + new SizeD(1, 0);
                    Control.ClientSize = newSize;
                    Control.Refresh();
                    NativeControl.SendSizeEvent();
                }
            }

            internal override Native.Control CreateNativeControl()
            {
                return new Native.Popup();
            }

            protected override void OnAttach()
            {
                base.OnAttach();

                /*ApplyOwner();*/

                /*Control.OwnerChanged += Control_OwnerChanged;*/
            }

            protected override void OnDetach()
            {
                /*Control.OwnerChanged -= Control_OwnerChanged;*/

                base.OnDetach();
            }

            /*private void ApplyOwner()
            {
                var newOwner = Control.Owner?.Handler?.NativeControl;
                var oldOwner = NativeControl.ParentRefCounted;
                if (newOwner == oldOwner)
                    return;

                oldOwner?.RemoveChild(NativeControl);

                if (newOwner == null)
                    return;

                newOwner.AddChild(NativeControl);
            }*/

            /*private void Control_OwnerChanged(object? sender, EventArgs e)
            {
                ApplyOwner();
            }*/
        }
    }
}