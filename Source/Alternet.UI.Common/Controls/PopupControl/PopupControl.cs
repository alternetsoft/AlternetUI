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
        private bool fitParentScrollbars = true;
        private AbstractControl? container;
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
        /// Gets or sets the action to be executed when the popup is closed.
        /// </summary>
        /// <remarks>This property allows you to define a custom action to be invoked
        /// when the popup is closed.
        /// If set to <see langword="null"/>, no action will be performed.</remarks>
        public Action? ClosedAction { get; set; }

        /// <summary>
        /// Gets or sets the area that should not be covered by the popup.
        /// </summary>
        public virtual RectD? ExcludedArea { get; set; }

        /// <summary>
        /// Gets or sets container where popup will be shown.
        /// </summary>
        public virtual AbstractControl? Container
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
        /// Gets or sets a value indicating whether the control should adjust its size
        /// to fit within the scrollable area of its parent. Default is True.
        /// </summary>
        /// <remarks>When set to <see langword="true"/>, the control will automatically resize to ensure
        /// it does not exceed the scrollable bounds of its parent container.
        /// Changing this property triggers an update to the maximum allowable
        /// size of the popup.</remarks>
        public virtual bool FitParentScrollbars
        {
            get => fitParentScrollbars;
            set
            {
                if (fitParentScrollbars == value)
                    return;
                fitParentScrollbars = value;
                UpdateMaxPopupSize();
            }
        }

        /// <summary>
        /// Gets or sets whether to adjust size of the popup to fit into
        /// the parent's client area. Default is True.
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
        /// Retrieves the container associated with the current control.
        /// </summary>
        /// <remarks>This method returns the parent control if it is set;
        /// otherwise, it returns the container control.
        /// The returned value may be <see langword="null"/> if neither a parent nor a container
        /// is defined.</remarks>
        /// <returns>An <see cref="AbstractControl"/> representing the parent or container
        /// of the current control, or <see langword="null"/> if no container is available.</returns>
        public virtual AbstractControl? GetContainer()
        {
            return Parent ?? Container;
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
                ClosedAction?.Invoke();
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

        /// <summary>
        /// Adjusts the specified position to ensure that the associated rectangle
        /// is fully visible within the given container bounds.
        /// </summary>
        /// <remarks>This method ensures that the rectangle, defined by the specified position and its
        /// size, is fully contained within the provided container bounds.
        /// If the rectangle extends beyond the container
        /// bounds, the position is adjusted to bring it back into view.
        /// Additionally, if the rectangle intersects with
        /// an excluded area, alternative positions are suggested to avoid
        /// the excluded area while maintaining visibility within the container bounds.</remarks>
        /// <param name="position">A reference to the position of the top-left corner of the rectangle.
        /// This value may be adjusted to ensure visibility.</param>
        /// <param name="containerBounds">The bounds of the container within which
        /// the rectangle must be visible.</param>
        /// <param name="adjustLine">A boolean value indicating whether
        /// to adjust the position vertically to account for additional spacing, such
        /// as line height. Defaults to <see langword="true"/>.</param>
        public virtual void EnsureVisible(
            ref PointD position,
            RectD containerBounds,
            bool adjustLine = true)
        {
            RectD rect = new(position, Bounds.Size);

            if (!containerBounds.Contains(rect))
            {
                if (rect.X < containerBounds.Left)
                    position.X = containerBounds.Left + 1;
                else
                if (rect.Right > containerBounds.Right)
                    position.X = containerBounds.Right - rect.Width - 1;

                if (rect.Y < containerBounds.Top)
                    position.Y = containerBounds.Top + 1;
                else
                    if (rect.Bottom > containerBounds.Bottom)
                {
                    var h = adjustLine ? GetContainerFontHeight() : 0;
                    position.Y = Math.Min(
                        position.Y - rect.Height - h,
                        containerBounds.Bottom - rect.Height) - 1;
                }
            }

            bool IntersectWithExcludedArea(PointD p)
            {
                if (ExcludedArea.HasValue)
                {
                    rect = new(p, Bounds.Size);
                    RectD excluded = ExcludedArea.Value;
                    if (excluded.IntersectsWith(rect))
                    {
                        return true;
                    }
                }

                return false;
            }

            (HorizontalAlignment H, VerticalAlignment V)[] suggest =
            [
                (HorizontalAlignment.Right, VerticalAlignment.Top),
                (HorizontalAlignment.Right, VerticalAlignment.Bottom),
                (HorizontalAlignment.Left, VerticalAlignment.Top),
                (HorizontalAlignment.Left, VerticalAlignment.Bottom),
            ];

            bool TrySuggestions(ref PointD position)
            {
                foreach (var (h, v) in suggest)
                {
                    rect = AlignUtils.AlignRectInRect(
                        new(position, Bounds.Size),
                        containerBounds,
                        h,
                        v,
                        shrinkSize: false);

                    if (!IntersectWithExcludedArea(rect.Location))
                    {
                        position = rect.Location;
                        return true;
                    }
                }

                return false;
            }

            if (position.IsAnyNegative || IntersectWithExcludedArea(position))
            {
                TrySuggestions(ref position);
            }
        }

        /// <summary>
        /// Updates maximal popup size.
        /// </summary>
        public virtual void UpdateMaxPopupSize()
        {
            if (!fitIntoParent || Parent is null)
                return;
            var sz = GetMaxPopupSize();
            if (sz is not null)
                Size = SizeD.Min(sz.Value, Size);
        }

        /// <summary>
        /// Calculates the height of the font used by the container.
        /// </summary>
        /// <remarks>If the container is not available, the current instance is used as the fallback.
        /// The height is determined based on the font's metrics and the associated
        /// measurement canvas.</remarks>
        /// <returns>The height of the font as a <see cref="double"/> value.</returns>
        public virtual double GetContainerFontHeight()
        {
            var c = GetContainer() ?? this;
            var h = c.RealFont.GetHeight(MeasureCanvas);
            return h;
        }

        /// <summary>
        /// Calculates the size of the scrollbar corner for the container.
        /// </summary>
        /// <remarks>This method determines the size of the scrollbar corner for the container by
        /// retrieving the associated container or defaulting to the current instance.
        /// The size is calculated using the
        /// container's scrollbar settings.</remarks>
        /// <returns>A <see cref="SizeD"/> representing the dimensions of the scrollbar corner.</returns>
        public virtual SizeD GetContainerScrollbarSize()
        {
            var p = GetContainer() ?? this;
            var cornerSize = ScrollBar.GetCornerSize(p);
            return cornerSize;
        }

        /// <summary>
        /// Calculates the bounding rectangle of the container in which popup can be displayed.
        /// </summary>
        /// <returns>A <see cref="RectD"/> representing the container's location and size,
        /// or <see langword="null"/> if the
        /// container or size is not available.</returns>
        public virtual RectD? GetContainerRect()
        {
            var size = GetMaxPopupSize();
            var container = GetContainer();

            if (size is null || container is null)
                return null;

            return (container.Location, size.Value);
        }

        /// <summary>
        /// Gets maximal size of the popup.
        /// Returns Null if size is not limited.
        /// </summary>
        /// <returns></returns>
        public virtual SizeD? GetMaxPopupSize()
        {
            if (!fitIntoParent)
                return null;

            var p = GetContainer();
            if (p is null)
                return null;

            var cornerSize = FitParentScrollbars ? GetContainerScrollbarSize() : 0;
            var result = p.ClientSize - cornerSize;
            result.Width -= cornerSize.Width;

            return result;
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
    }
}
