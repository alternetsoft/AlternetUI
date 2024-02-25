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
        /// <summary>
        /// Enumerates known handler types.
        /// </summary>
        public enum HandlerType
        {
            /// <summary>
            /// Native handler type.
            /// </summary>
            Native,

            /// <summary>
            /// Generic type.
            /// </summary>
            Generic,
        }

        internal static void NotifyCaptureLost()
        {
            Native.Control.NotifyCaptureLost();
        }

        internal static void PerformDefaultLayout(
            Control container,
            RectD childrenLayoutBounds,
            IReadOnlyList<Control> childs)
        {
            foreach (var control in childs)
            {
                var preferredSize = control.GetPreferredSizeLimited(childrenLayoutBounds.Size);

                var horizontalPosition =
                    LayoutFactory.AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.HorizontalAlignment);
                var verticalPosition =
                    LayoutFactory.AlignVertical(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.VerticalAlignment);

                control.Bounds = new RectD(
                    horizontalPosition.Origin,
                    verticalPosition.Origin,
                    horizontalPosition.Size,
                    verticalPosition.Size);
            }
        }

        internal static SizeD GetPreferredSizeDefaultLayout(Control container, SizeD availableSize)
        {
            if (container.HasChildren)
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

        /// <summary>
        /// Creates a handler for the control.
        /// </summary>
        /// <remarks>
        /// You typically should not call the <see cref="CreateHandler"/>
        /// method directly.
        /// The preferred method is to call the
        /// <see cref="EnsureHandlerCreated"/> method, which forces a handler
        /// to be created for the control.
        /// </remarks>
        internal virtual ControlHandler CreateHandler()
        {
            return new GenericControlHandler();
        }

        internal void RaiseNativeSizeChanged()
        {
            OnNativeSizeChanged(EventArgs.Empty);
        }

        internal void RaiseDeactivated()
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
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

            if (e.ChangedButton == MouseButton.Left)
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

        internal void RaiseProcessException(ControlExceptionEventArgs e)
        {
            OnProcessException(e);
            ProcessException?.Invoke(this, e);
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
                e.CurrentTarget = form;
                form.OnKeyDown(e);
                if (e.Handled)
                    return;
            }
            else
                form = null;

            while (control is not null && control != form)
            {
                e.CurrentTarget = control;
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
                e.CurrentTarget = form;
                form.OnKeyUp(e);
                if (e.Handled)
                    return;
            }
            else
                form = null;

            while (control is not null && control != form)
            {
                e.CurrentTarget = control;
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

        internal void RaiseChildInserted(Control childControl)
        {
            OnChildInserted(childControl);
            ChildInserted?.Invoke(this, new BaseEventArgs<Control>(childControl));
        }

        internal void RaiseChildRemoved(Control childControl)
        {
            OnChildInserted(childControl);
            ChildRemoved?.Invoke(this, new BaseEventArgs<Control>(childControl));
        }

        internal void RaisePaint(PaintEventArgs e)
        {
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

        /// <summary>
        /// Gets the sizer of which this control is a member, if any, otherwise <c>null</c>.
        /// </summary>
        /// <returns></returns>
        internal ISizer? GetContainingSizer()
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return null;

            var sizer = nativeControl.GetContainingSizer();

            if (sizer == IntPtr.Zero)
                return null;

            return new Sizer(sizer, false);
        }

        /// <summary>
        /// This method calls SetSizer() and then updates the initial control size to the
        /// size needed to accommodate all sizer elements and sets the size hints which,
        /// if this control is a top level one, prevent the user from resizing it to be
        /// less than this minimal size.
        /// </summary>
        /// <param name="sizer">The sizer to set. Pass <c>null</c> to disassociate
        /// and conditionally delete the control's sizer.</param>
        /// <param name="deleteOld">If <c>true</c> (the default), this will delete any
        /// pre-existing sizer. Pass <c>false</c> if you wish to handle deleting
        /// the old sizer yourself but remember to do it yourself in this case
        /// to avoid memory leaks.</param>
        internal void SetSizerAndFit(ISizer? sizer, bool deleteOld = false)
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return;

            if (sizer is null)
                nativeControl.SetSizerAndFit(IntPtr.Zero, deleteOld);
            else
                nativeControl.SetSizerAndFit(sizer.Handle, deleteOld);
        }

        /// <summary>
        /// Gets the sizer associated with the control by a previous call to <see cref="SetSizer"/>,
        /// or <c>null</c>.
        /// </summary>
        internal ISizer? GetSizer()
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return null;

            var sizer = nativeControl.GetSizer();

            if (sizer == IntPtr.Zero)
                return null;

            return new Sizer(sizer, false);
        }

        /// <summary>
        /// Sets the control to have the given layout sizer.
        /// </summary>
        /// <param name="sizer">The sizer to set. Pass <c>null</c> to disassociate
        /// and conditionally delete the control's sizer.</param>
        /// <param name="deleteOld">If <c>true</c> (the default), this will delete any
        /// pre-existing sizer. Pass <c>false</c> if you wish to handle deleting
        /// the old sizer yourself but remember to do it yourself in this case
        /// to avoid memory leaks.</param>
        /// <remarks>
        /// The control will then own the object, and will take care of its deletion.
        /// If an existing layout constraints object is already owned by the control,
        /// it will be deleted if the <paramref name="deleteOld"/> parameter is <c>true</c>.
        /// </remarks>
        /// <remarks>
        /// This function will also update layout so that the sizer will be effectively
        /// used to layout the control children whenever it is resized.
        /// </remarks>
        /// <remarks>
        /// This function enables and disables Layout automatically.
        /// </remarks>
        internal void SetSizer(ISizer? sizer, bool deleteOld = true)
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return;

            if (sizer is null)
                nativeControl.SetSizer(IntPtr.Zero, deleteOld);
            else
                nativeControl.SetSizer(sizer.Handle, deleteOld);
        }
    }
}
