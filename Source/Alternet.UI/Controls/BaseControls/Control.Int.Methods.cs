using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        internal static void NotifyCaptureLost()
        {
            Native.Control.NotifyCaptureLost();
        }

        internal static void PerformDefaultLayout(Control container)
        {
            var childrenLayoutBounds = container.ChildrenLayoutBounds;
            foreach (var control in container.Handler.AllChildrenIncludedInLayout)
            {
                var preferredSize = control.GetPreferredSizeLimited(childrenLayoutBounds.Size);

                var horizontalPosition =
                    AlignedLayout.AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        preferredSize);
                var verticalPosition =
                    AlignedLayout.AlignVertical(
                        childrenLayoutBounds,
                        control,
                        preferredSize);

                control.Handler.Bounds = new RectD(
                    horizontalPosition.Origin,
                    verticalPosition.Origin,
                    horizontalPosition.Size,
                    verticalPosition.Size);
            }
        }

        internal static SizeD GetPreferredSizeDefaultLayout(Control container, SizeD availableSize)
        {
            if (container.HasChildren || container.Handler.HasVisualChildren)
                return container.Handler.GetSpecifiedOrChildrenPreferredSize(availableSize);
            return container.Handler.GetNativeControlSize(availableSize);
        }

        internal static Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return Native.Control.GetClassDefaultAttributesBgColor(
                (int)controlType,
                (int)renderSize);
        }

        internal static Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return Native.Control.GetClassDefaultAttributesFgColor(
                (int)controlType,
                (int)renderSize);
        }

        internal static Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            var font = Native.Control.GetClassDefaultAttributesFont(
                (int)controlType,
                (int)renderSize);
            return Font.FromInternal(font);
        }

        internal void RaiseNativeSizeChanged()
        {
            OnNativeSizeChanged(EventArgs.Empty);
        }

        internal void RaiseDeactivated()
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
            /*Application.DebugLog($"Deactivated {Name}");*/
        }

        internal void RaiseHandleCreated()
        {
            OnHandleCreated(EventArgs.Empty);
            HandleCreated?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseHandleDestroyed()
        {
            OnHandleDestroyed(EventArgs.Empty);
            HandleDestroyed?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseActivated()
        {
            Activated?.Invoke(this, EventArgs.Empty);
            /*Application.DebugLog($"Activated {Name}");*/
        }

        internal Color? GetDefaultAttributesBgColor()
        {
            CheckDisposed();
            return NativeControl?.GetDefaultAttributesBgColor();
        }

        internal Color? GetDefaultAttributesFgColor()
        {
            CheckDisposed();
            return NativeControl?.GetDefaultAttributesFgColor();
        }

        internal Font? GetDefaultAttributesFont()
        {
            CheckDisposed();
            return Font.FromInternal(NativeControl?.GetDefaultAttributesFont());
        }

        /// <summary>
        /// Forces the re-creation of the handler for the control.
        /// </summary>
        /// <remarks>
        /// The <see cref="RecreateHandler"/> method is called whenever
        /// re-execution of handler creation logic is needed.
        /// For example, this may happen when visual theme changes.
        /// </remarks>
        internal void RecreateHandler()
        {
            if (handler != null)
                DetachHandler();

            Invalidate();
        }

        /// <summary>
        /// Gets an <see cref="IControlHandlerFactory"/> to use when creating
        /// new control handlers for this control.
        /// </summary>
        internal IControlHandlerFactory GetEffectiveControlHandlerHactory() =>
            ControlHandlerFactory ??
                Application.Current.VisualTheme.ControlHandlerFactory;

        internal void RaiseKeyPress(KeyPressEventArgs e)
        {
            var control = this;
            var form = ParentWindow;
            if (form is not null && form.KeyPreview)
            {
                form.OnKeyPress(e);
                if (e.Handled)
                    return;
            }
            else
                form = null;

            while (control is not null && control != form)
            {
                control.OnKeyPress(e);

                if (e.Handled)
                    return;
                control = control.Parent;
            }
        }

        internal void RaiseMouseMove(MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        internal void RaiseMouseUp(MouseEventArgs e)
        {
            OnMouseUp(e);

            if(e.ChangedButton == MouseButton.Left)
            {
                OnMouseLeftButtonUp(e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                OnMouseRightButtonUp(e);
            }
        }

        internal void RaiseMouseDown(MouseEventArgs e)
        {
            OnMouseDown(e);

            if (e.ChangedButton == MouseButton.Left)
            {
                OnMouseLeftButtonDown(e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                OnMouseRightButtonDown(e);
            }
        }

        internal void RaiseMouseWheel(MouseEventArgs e)
        {
            OnMouseWheel(e);
        }

        internal void RaiseMouseDoubleClick(MouseEventArgs e)
        {
            OnMouseDoubleClick(e);
        }

        internal void RaiseKeyDown(KeyEventArgs e)
        {
            var control = this;
            var form = ParentWindow;
            if (form is not null && form.KeyPreview)
            {
                form.OnKeyDown(e);
                if (e.Handled)
                    return;
            }
            else
                form = null;

            while (control is not null && control != form)
            {
                control.OnKeyDown(e);

                if (e.Handled)
                    return;
                control = control.Parent;
            }
        }

        internal void RaiseKeyUp(KeyEventArgs e)
        {
            var control = this;
            var form = ParentWindow;
            if (form is not null && form.KeyPreview)
            {
                form.OnKeyUp(e);
                if (e.Handled)
                    return;
            }
            else
                form = null;

            while (control is not null && control != form)
            {
                control.OnKeyUp(e);
                if (e.Handled)
                    return;
                control = control.Parent;
            }
        }

        internal void RaiseTextChanged(EventArgs e) => OnTextChanged(e);

        internal void RaiseSizeChanged(EventArgs e) => OnSizeChanged(e);

        internal void RaiseScroll(ScrollEventArgs e) => OnScroll(e);

        internal void RaiseLocationChanged(EventArgs e) => OnLocationChanged(e);

        internal void RaiseMouseCaptureLost()
        {
            OnMouseCaptureLost(EventArgs.Empty);
            MouseCaptureLost?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseMouseEnter()
        {
            RaiseIsMouseOverChanged();
            OnMouseEnter(EventArgs.Empty);
            MouseEnter?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseCurrentStateChanged()
        {
            OnCurrentStateChanged(EventArgs.Empty);
            CurrentStateChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseIsMouseOverChanged()
        {
            OnIsMouseOverChanged(EventArgs.Empty);
            IsMouseOverChanged?.Invoke(this, EventArgs.Empty);
            RaiseCurrentStateChanged();
        }

        internal void RaiseMouseLeave()
        {
            RaiseIsMouseOverChanged();
            OnMouseLeave(EventArgs.Empty);
            MouseLeave?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseChildInserted(Control childControl) =>
            OnChildInserted(childControl);

        internal void RaiseChildRemoved(Control childControl) =>
            OnChildInserted(childControl);

        internal void RaisePaint(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnPaint(e);
            Paint?.Invoke(this, e);
        }

        internal void RaiseGotFocus(EventArgs e)
        {
            OnGotFocus(e);
            GotFocus?.Invoke(this, e);
            Designer?.RaiseGotFocus(this);
            RaiseCurrentStateChanged();
        }

        internal void RaiseLostFocus(EventArgs e)
        {
            OnLostFocus(e);
            LostFocus?.Invoke(this, e);
            RaiseCurrentStateChanged();
        }

        internal void SetParentInternal(Control? value)
        {
            parent = value;
            LogicalParent = value;
        }

        internal virtual SizeD GetPreferredSizeLimited(SizeD availableSize)
        {
            var result = GetPreferredSize(availableSize);
            var minSize = MinimumSize;
            var maxSize = MaximumSize;
            var preferredSize = result.ApplyMinMax(minSize, maxSize);
            return preferredSize;
        }

        internal void RaiseDragStart(DragStartEventArgs e) => OnDragStart(e);

        internal void RaiseDragDrop(DragEventArgs e) => OnDragDrop(e);

        internal void RaiseDragOver(DragEventArgs e) => OnDragOver(e);

        internal void RaiseDragEnter(DragEventArgs e) => OnDragEnter(e);

        internal void RaiseDragLeave(EventArgs e) => OnDragLeave(e);

        internal void SendMouseDownEvent(int x, int y)
        {
            Handler?.NativeControl?.SendMouseDownEvent(x, y);
        }

        internal void SendMouseUpEvent(int x, int y)
        {
            Handler?.NativeControl?.SendMouseUpEvent(x, y);
        }
    }
}
