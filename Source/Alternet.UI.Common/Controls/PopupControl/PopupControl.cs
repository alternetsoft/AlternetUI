using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements popup control which is shown inside client area of another control.
    /// </summary>
    public partial class PopupControl : Border
    {
        private readonly ControlSubscriber subscriber = new();

        private PointD? minLocation;
        private bool allowNegativeLocation;
        private bool fitIntoParent = true;
        private Control? container;
        private ModalResult popupResult = ModalResult.None;
        private bool cancelOnLostFocus;
        private bool acceptOnLostFocus;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupControl"/> class.
        /// </summary>
        public PopupControl()
        {
            IgnoreLayout = true;
            Visible = false;
            ParentFont = true;
            Cursor = Cursors.Arrow;

            subscriber.AfterControlIsMouseOverChanged += (s, e) =>
            {
                if (Visible && HideOnMouseLeave && !IsMouseOver)
                {
                    CloseWhenIdle(ModalResult.Canceled, UI.PopupCloseReason.MouseOverChanged);
                }
            };
        }

        /// <summary>
        /// Occurs when popup is closed
        /// </summary>
        public event EventHandler? Closed;

        /// <summary>
        /// Enumerates known close popup reasons.
        /// </summary>
        public enum CloseReason
        {
            /// <summary>
            /// Mouse clicked inside container.
            /// </summary>
            ClickContainer,

            /// <summary>
            /// Focus was lost.
            /// </summary>
            FocusLost,

            /// <summary>
            /// Key was pressed.
            /// </summary>
            Key,

            /// <summary>
            /// Character was pressed.
            /// </summary>
            Char,

            /// <summary>
            /// Mouse moved outside the popup control.
            /// </summary>
            MouseOverChanged,

            /// <summary>
            /// Other reason.
            /// </summary>
            Other,
        }

        /// <summary>
        /// Gets or sets container where popup will be shown.
        /// </summary>
        public virtual Control? Container
        {
            get
            {
                return container;
            }

            set
            {
                if (container == value)
                    return;
                container = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to focus parent control when popup is shown.
        /// </summary>
        public virtual bool FocusParentOnShow { get; set; }

        /// <summary>
        /// Gets or sets the popup result value, which is updated when popup is closed.
        /// This property is set to <see cref="ModalResult.None"/> at the moment
        /// when popup is shown.
        /// </summary>
        [Browsable(false)]
        public virtual ModalResult PopupResult
        {
            get
            {
                return popupResult;
            }

            set
            {
                popupResult = value;
            }
        }

        /// <summary>
        /// Gets or sets the reason why popup was closed.
        /// </summary>
        public virtual PopupCloseReason? PopupCloseReason { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Space" key.
        /// </summary>
        public virtual bool AcceptOnSpace { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Tab" key.
        /// </summary>
        public virtual bool AcceptOnTab { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Enter" key.
        /// </summary>
        public virtual bool HideOnEnter { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup disappears automatically
        /// when the user presses "Escape" key.
        /// </summary>
        public virtual bool HideOnEscape { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup disappears automatically
        /// when the  mouse pointer leaves the popup control.
        /// </summary>
        public virtual bool HideOnMouseLeave { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a popup disappears automatically
        /// when the user clicks mouse outside it in the client area of the parent control.
        /// </summary>
        public virtual bool HideOnClickParent { get; set; }

        /// <summary>
        /// Gets or sets whether to focus <see cref="Container"/> when popup is closed.
        /// </summary>
        public virtual bool FocusContainerOnClose { get; set; } = true;

        /// <summary>
        /// Gets or sets whether popup should be closed with
        /// <see cref="ModalResult.Canceled"/> result
        /// when it lost focus.
        /// </summary>
        public virtual bool CancelOnLostFocus
        {
            get
            {
                return cancelOnLostFocus;
            }

            set
            {
                cancelOnLostFocus = value;
                if (cancelOnLostFocus)
                    acceptOnLostFocus = false;
            }
        }

        /// <summary>
        /// Gets or sets whether negative X or Y location is allowed.
        /// Default is False.
        /// </summary>
        public virtual bool AllowNegativeLocation
        {
            get => allowNegativeLocation;

            set
            {
                if (allowNegativeLocation == value)
                    return;
                allowNegativeLocation = value;
                if (!allowNegativeLocation)
                {
                    var location = Location;
                    if(location.X < 0 || location.Y < 0)
                        Location = location;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to adjust size of the popup to fit into
        /// the parent's client area.
        /// </summary>
        public virtual bool FitIntoParent
        {
            get
            {
                return fitIntoParent;
            }

            set
            {
                if (fitIntoParent == value)
                    return;
                fitIntoParent = value;
                UpdateMaxPopupSize();
            }
        }

        /// <summary>
        /// Gets or sets minimal possible popup window location.
        /// </summary>
        public virtual PointD? MinLocation
        {
            get
            {
                return minLocation;
            }

            set
            {
                if (minLocation == value)
                    return;
                minLocation = value;
                Bounds = Bounds;
            }
        }

        /// <inheritdoc/>
        public override RectD Bounds
        {
            get => base.Bounds;
            set
            {
                if (MinLocation is not null)
                {
                    var mx = MinLocation.Value.X;
                    var my = MinLocation.Value.Y;

                    if (value.X < mx)
                        value.X = mx;

                    if (value.Y < my)
                        value.Y = my;
                }

                if (!allowNegativeLocation)
                {
                    if (value.X < 0)
                        value.X = 0;

                    if (value.Y < 0)
                        value.Y = 0;
                }

                var sz = GetMaxPopupSize();
                if (sz is not null)
                    value.Size = SizeD.Min(sz.Value, value.Size);

                base.Bounds = value;
            }
        }

        /// <summary>
        /// Gets or sets whether popup should be closed with
        /// <see cref="ModalResult.Accepted"/> result
        /// when it lost focus.
        /// </summary>
        public virtual bool AcceptOnLostFocus
        {
            get
            {
                return acceptOnLostFocus;
            }

            set
            {
                acceptOnLostFocus = value;
                if (acceptOnLostFocus)
                    cancelOnLostFocus = false;
            }
        }

        /// <summary>
        /// Gets or sets the reason why popup was closed.
        /// </summary>
        public PopupCloseReason? Reason { get; set; }

        /// <summary>
        /// Closes popup window and raises <see cref="Closed"/> event.
        /// This is equivalent to calling <see cref="Close(PopupCloseReason?)"/> with null reason.
        /// </summary>
        public void Close()
        {
            Close(null);
        }

        /// <summary>
        /// Closes popup window and raises <see cref="Closed"/> event.
        /// </summary>
        public virtual void Close(PopupCloseReason? reason)
        {
            Reason = reason;

            if (!Visible)
            {
                return;
            }

            Hide();
            Parent = null;
            App.DoEvents();
            if(FocusContainerOnClose)
                Container?.SetFocusIfPossible();
            App.DoEvents();
            App.AddIdleTask(() =>
            {
                if (IsDisposed)
                    return;
                Closed?.Invoke(this, EventArgs.Empty);
            });
        }

        /// <summary>
        /// Sets <see cref="PopupResult"/> and calls <see cref="Close(PopupCloseReason?)"/>.
        /// </summary>
        public virtual void Close(ModalResult result, PopupCloseReason? reason = null)
        {
            PopupResult = result;
            Close();
        }

        /// <summary>
        /// Closes popup control when application goes to the idle state.
        /// </summary>
        public virtual void CloseWhenIdle(ModalResult result, PopupCloseReason? reason = null)
        {
            if (!Visible)
                return;
            App.AddIdleTask(() =>
            {
                if (IsDisposed)
                    return;
                Close(result, reason);
            });
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                PopupResult = ModalResult.None;
                AddGlobalNotification(subscriber);
                if (FocusParentOnShow)
                    Parent?.SetFocusIfPossible();
            }
            else
            {
                RemoveGlobalNotification(subscriber);
            }
        }

        /// <inheritdoc/>
        protected override void OnBeforeParentMouseDown(object? sender, MouseEventArgs e)
        {
            base.OnBeforeParentMouseDown(sender, e);
            if (HideOnClickParent && Visible)
            {
                CloseWhenIdle(
                    GetPopupResult(UI.PopupCloseReason.ClickContainer),
                    UI.PopupCloseReason.ClickContainer);
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnBeforeParentKeyDown(object? sender, KeyEventArgs e)
        {
            base.OnBeforeParentKeyDown(sender, e);

            if (HideOnEscape && e.IsEscape)
            {
                Close(ModalResult.Canceled, new(Key.Escape));
                e.Suppressed();
                return;
            }

            if (AcceptOnTab && e.IsSimpleKey(Key.Tab))
            {
                Close(ModalResult.Accepted, new(Key.Tab));
                e.Suppressed();
                return;
            }
        }

        /// <inheritdoc/>
        protected override void OnChildLostFocus(object? sender, LostFocusEventArgs e)
        {
            base.OnChildLostFocus(sender, e);
            if (ContainsFocus)
                return;
            if (CancelOnLostFocus || AcceptOnLostFocus)
            {
                CloseWhenIdle(
                    GetPopupResult(UI.PopupCloseReason.FocusLost),
                    UI.PopupCloseReason.FocusLost);
            }
        }

        /// <inheritdoc/>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (ContainsFocus)
                return;
            if(CancelOnLostFocus || AcceptOnLostFocus)
            {
                CloseWhenIdle(
                    GetPopupResult(UI.PopupCloseReason.FocusLost),
                    UI.PopupCloseReason.FocusLost);
            }
        }

        /// <summary>
        /// Gets popup result using the specified <see cref="CloseReason"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual ModalResult GetPopupResult(PopupCloseReason reason)
        {
            if (AcceptOnLostFocus)
                return ModalResult.Accepted;
            return ModalResult.Canceled;
        }

        /// <inheritdoc/>
        protected override void OnBeforeChildKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.IsEscape)
            {
                if(HideOnEscape)
                    CloseWithResult(ModalResult.Canceled);
                return;
            }

            if (e.IsEnter)
            {
                if(HideOnEnter)
                    CloseWithResult(ModalResult.Accepted);
                return;
            }

            if (e.IsSimpleKey(Key.Tab))
            {
                if(AcceptOnTab)
                    CloseWithResult(ModalResult.Accepted);
                return;
            }

            if (e.IsSimpleKey(Key.Space))
            {
                if(AcceptOnSpace)
                    CloseWithResult(ModalResult.Accepted);
                return;
            }

            void CloseWithResult(ModalResult value)
            {
                PopupResult = value;
                Close();
                e.Suppressed();
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            RemoveGlobalNotification(subscriber);
            base.DisposeManaged();
        }

        /// <summary>
        /// Gets maximal size of the popup.
        /// Returns Null if size is not limited.
        /// </summary>
        /// <returns></returns>
        protected virtual SizeD? GetMaxPopupSize()
        {
            if (!fitIntoParent || Parent is null)
                return null;
            return Parent.ClientSize;
        }

        /// <summary>
        /// Updates maximal popup size.
        /// </summary>
        protected virtual void UpdateMaxPopupSize()
        {
            if (!fitIntoParent || Parent is null)
                return;
            var sz = GetMaxPopupSize();
            if (sz is not null)
                Size = SizeD.Min(sz.Value, Size);
        }
    }
}
