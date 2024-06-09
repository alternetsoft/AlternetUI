using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        /// <summary>
        /// Ensures that the control <see cref="Handler"/> is created,
        /// creating and attaching it if necessary.
        /// </summary>
        protected internal void EnsureHandlerCreated()
        {
            if (handler == null)
            {
                CreateAndAttachHandler();
            }

            void CreateAndAttachHandler()
            {
                if (GetRequiredHandlerType() == HandlerType.Native)
                    handler = CreateHandler();
                else
                    handler = ControlFactory.Handler.CreateControlHandler(this);

                handler.Attach(this);

                handler.Visible = Visible;
                handler.SetEnabled(Enabled);
                ApplyChildren();

                OnHandlerAttached(EventArgs.Empty);

                BindHandlerEvents();

                void ApplyChildren()
                {
                    if (!HasChildren)
                        return;
                    for (var i = 0; i < Children.Count; i++)
                        handler.OnChildInserted(Children[i]);
                }
            }
        }

        /// <summary>
        /// Disconnects the current control <see cref="Handler"/> from
        /// the control.
        /// This method calls <see cref="IControlHandler.Detach"/>.
        /// </summary>
        protected internal void DetachHandler()
        {
            if (handler == null)
                throw new InvalidOperationException();
            OnHandlerDetaching(EventArgs.Empty);
            UnbindHandlerEvents();
            handler.Detach();
            handler = null;
        }

        /// <summary>
        /// Gets the size of the control specified in its
        /// <see cref="Control.SuggestedWidth"/> and <see cref="Control.SuggestedHeight"/>
        /// properties or calculates preferred size from its children.
        /// </summary>
        protected virtual SizeD GetSpecifiedOrChildrenPreferredSize(SizeD availableSize)
        {
            var specifiedWidth = SuggestedWidth;
            var specifiedHeight = SuggestedHeight;
            if (!Coord.IsNaN(specifiedWidth) && !Coord.IsNaN(specifiedHeight))
                return new SizeD(specifiedWidth, specifiedHeight);

            var maxSize = GetChildrenMaxPreferredSizePadded(availableSize);
            var maxWidth = maxSize.Width;
            var maxHeight = maxSize.Height;

            var width = Coord.IsNaN(specifiedWidth) ? maxWidth : specifiedWidth;
            var height = Coord.IsNaN(specifiedHeight) ? maxHeight : specifiedHeight;

            return new SizeD(width, height);
        }

        /// <summary>
        /// Returns a preferred size of control with an added padding.
        /// </summary>
        protected SizeD GetChildrenMaxPreferredSizePadded(SizeD availableSize)
        {
            return GetPaddedPreferredSize(GetChildrenMaxPreferredSize(availableSize));
        }

        /// <summary>
        /// Returns the size of the area which can fit all the children of this
        /// control, with an added padding.
        /// </summary>
        protected virtual SizeD GetPaddedPreferredSize(SizeD preferredSize)
        {
            var padding = Padding;
            var intrinsicPadding = Handler.IntrinsicPreferredSizePadding;
            return preferredSize + padding.Size + intrinsicPadding.Size;
        }

        /// <summary>
        /// Gets the size of the area which can fit all the children of this control.
        /// </summary>
        protected virtual SizeD GetChildrenMaxPreferredSize(SizeD availableSize)
        {
            Coord maxWidth = 0;
            Coord maxHeight = 0;

            foreach (var control in AllChildrenInLayout)
            {
                var preferredSize =
                    control.GetPreferredSize(availableSize) + control.Margin.Size;
                maxWidth = Math.Max(preferredSize.Width, maxWidth);
                maxHeight = Math.Max(preferredSize.Height, maxHeight);
            }

            return new SizeD(maxWidth, maxHeight);
        }

        protected virtual void UnbindHandlerEvents()
        {
            Handler.TextChanged = null;
            Handler.HandleCreated = null;
            Handler.HandleDestroyed = null;
            Handler.Activated = null;
            Handler.Deactivated = null;
            Handler.Idle = null;
            Handler.Paint = null;
            Handler.VisibleChanged = null;
            Handler.MouseEnter = null;
            Handler.MouseLeave = null;
            Handler.MouseCaptureLost = null;
            Handler.DragLeave = null;
            Handler.GotFocus = null;
            Handler.LostFocus = null;
            Handler.SizeChanged = null;
            Handler.LocationChanged = null;
            Handler.VerticalScrollBarValueChanged = null;
            Handler.HorizontalScrollBarValueChanged = null;
            Handler.DragOver = null;
            Handler.DragEnter = null;
            Handler.DragDrop = null;
            Handler.SystemColorsChanged = null;
        }

        protected virtual void BindHandlerEvents()
        {
            Handler.MouseEnter = RaiseMouseEnterOnTarget;
            Handler.MouseLeave = RaiseMouseLeaveOnTarget;
            Handler.HandleCreated = RaiseHandleCreated;
            Handler.HandleDestroyed = RaiseHandleDestroyed;
            Handler.Activated = RaiseActivated;
            Handler.Deactivated = RaiseDeactivated;
            Handler.Paint = OnHandlerPaint;
            Handler.VisibleChanged = OnHandlerVisibleChanged;
            Handler.MouseCaptureLost = RaiseMouseCaptureLost;
            Handler.GotFocus = RaiseGotFocus;
            Handler.LostFocus = RaiseLostFocus;
            Handler.Idle = RaiseIdle;
            Handler.VerticalScrollBarValueChanged = OnHandlerVerticalScrollBarValueChanged;
            Handler.HorizontalScrollBarValueChanged = OnHandlerHorizontalScrollBarValueChanged;
            Handler.DragLeave = RaiseDragLeave;
            Handler.SizeChanged = RaiseHandlerSizeChanged;
            Handler.LocationChanged = RaiseHandlerLocationChanged;
            Handler.DragOver = RaiseDragOver;
            Handler.DragEnter = RaiseDragEnter;
            Handler.DragDrop = RaiseDragDrop;
            Handler.TextChanged = OnHandlerTextChanged;
            Handler.SystemColorsChanged = RaiseSystemColorsChanged;
        }

        /// <summary>
        /// Sets a specified <see cref="ControlStyles" /> flag to either <see langword="true" />
        /// or <see langword="false" />.</summary>
        /// <param name="flag">The <see cref="ControlStyles" /> bit to set.</param>
        /// <param name="value">
        /// <see langword="true" /> to apply the specified style to the control;
        /// otherwise, <see langword="false" />.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void SetStyle(ControlStyles flag, bool value)
        {
            controlStyle = value ? (controlStyle | flag) : (controlStyle & ~flag);
        }

        /// <summary>
        /// Adds list of shortcuts associated with the control and it's
        /// child controls. Only visible and enabled child controls are queried.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// An example of the correct implementation
        /// can be found in <see cref="SpeedButton.GetShortcuts"/>.
        /// </remarks>
        protected virtual IReadOnlyList<(ShortcutInfo Shortcut, Action Action)>? GetShortcuts()
        {
            if (!HasChildren)
                return null;

            List<(ShortcutInfo Shortcut, Action Action)>? result = null;

            foreach (var child in Children)
            {
                if (!child.Visible || !child.Enabled)
                    continue;
                var childShortcuts = child.GetShortcuts();
                if (childShortcuts is null)
                    continue;
                result ??= new();
                result.AddRange(childShortcuts);
            }

            return result;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (FocusedControl == this)
                FocusedControl = null;
            if (HoveredControl == this)
                HoveredControl = null;

            Designer?.RaiseDisposed(this);
            /*var children = Handler.AllChildren.ToArray();*/

            SuspendLayout();
            if (HasChildren)
                Children.Clear();
            ResumeLayout(performLayout: false);

            // TODO
            /* foreach (var child in children) child.Dispose();*/

            DetachHandler();
        }

        /// <summary>
        /// Gets default layout in case when <see cref="Layout"/> property
        /// is null.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method returns <see cref="LayoutStyle.Basic"/> in <see cref="Control"/>.
        /// </remarks>
        protected virtual LayoutStyle GetDefaultLayout()
        {
            return LayoutStyle.Basic;
        }

        /// <summary>
        /// Gets whether child control ignores layout.
        /// </summary>
        /// <param name="control"></param>
        protected virtual bool ChildIgnoresLayout(Control control)
        {
            return !control.Visible || control.IgnoreLayout;
        }

        protected virtual SizeD GetNativeControlSize(SizeD availableSize)
        {
            if (IsDummy)
                return SizeD.Empty;
            var s = Handler.GetPreferredSize(availableSize);
            s += Padding.Size;
            return new SizeD(
                Coord.IsNaN(SuggestedWidth) ? s.Width : SuggestedWidth,
                Coord.IsNaN(SuggestedHeight) ? s.Height : SuggestedHeight);
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
        protected virtual IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateControlHandler(this);
        }

        protected void SetVisibleValue(bool value) => visible = value;

        /// <summary>
        /// Gets required control handler type.
        /// </summary>
        /// <returns></returns>
        protected virtual HandlerType GetRequiredHandlerType() => HandlerType.Native;
    }
}
