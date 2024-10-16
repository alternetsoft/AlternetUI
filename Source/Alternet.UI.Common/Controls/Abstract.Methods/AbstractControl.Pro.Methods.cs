using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Disconnects the current control handler from the control.
        /// </summary>
        protected internal virtual void DetachHandler()
        {
        }

        /// <summary>
        /// Gets the size of the control specified in its
        /// <see cref="AbstractControl.SuggestedWidth"/> and <see cref="AbstractControl.SuggestedHeight"/>
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
        /// Requests scale factor from the parent or other available sources.
        /// </summary>
        /// <returns></returns>
        protected virtual Coord? RequestScaleFactor()
        {
            return Parent?.ScaleFactor;
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
            var intrinsicPadding = IntrinsicPreferredSizePadding;
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

        /// <summary>
        /// Called to modify text before it is assigned to the handler.
        /// </summary>
        /// <param name="s">New text.</param>
        protected virtual string CoerceTextForHandler(string s)
        {
            return s;
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
        /// Adds list of shortcuts associated with the control and its
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

            var children = AllChildren.ToArray();

            SuspendLayout();
            if (HasChildren)
                Children.Clear();
            ResumeLayout(performLayout: false);

            DetachHandler();

            try
            {
                foreach (var child in children)
                {
                    child.ParentDisposed();
                }
            }
            catch (Exception e)
            {
                if (DebugUtils.IsDebugDefined)
                {
                    LogUtils.LogExceptionToFile(e);
                    throw;
                }
            }
        }

        /// <summary>
        /// Called when parent is disposed.
        /// </summary>
        protected virtual void ParentDisposed()
        {
            var children = AllChildren.ToArray();
            foreach (var child in children)
            {
                child.ParentDisposed();
            }
        }

        /// <summary>
        /// Gets default layout in case when <see cref="Layout"/> property
        /// is null.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method returns <see cref="LayoutStyle.Basic"/> in <see cref="AbstractControl"/>.
        /// </remarks>
        protected virtual LayoutStyle GetDefaultLayout()
        {
            return LayoutStyle.Basic;
        }

        /// <summary>
        /// Gets whether child control ignores layout.
        /// </summary>
        /// <param name="control"></param>
        protected virtual bool ChildIgnoresLayout(AbstractControl control)
        {
            return !control.Visible || control.IgnoreLayout;
        }

        /// <summary>
        /// Gets size of the native control based on the specified available size.
        /// </summary>
        /// <param name="availableSize">Available size for the control.</param>
        /// <returns></returns>
        protected virtual SizeD GetNativeControlSize(SizeD availableSize)
        {
            var s = SizeD.Empty;
            s += Padding.Size;
            return new SizeD(
                Coord.IsNaN(SuggestedWidth) ? s.Width : SuggestedWidth,
                Coord.IsNaN(SuggestedHeight) ? s.Height : SuggestedHeight);
        }

        /// <summary>
        /// Paints internal caret for user-painted controls.
        /// This method is used on some platforms when system caret
        /// is not available.
        /// </summary>
        /// <param name="e">Paint arguments.</param>
        protected virtual void PaintCaret(PaintEventArgs e)
        {
            if (!UserPaint)
                return;
            if (!Focused)
                return;
            if (caretInfo is null)
                return;

            if (caretInfo.IsDisposed)
                CaretInfo = null;

            if (!caretInfo.Visible)
                return;

            caretInfo.Paint(this, e);
        }

        /// <summary>
        /// Sets internal color.
        /// </summary>
        protected virtual void InternalSetColor(bool isBackground, Color? color)
        {
            if (isBackground)
            {
                backgroundColor = color;
            }
            else
            {
                foregroundColor = color;
            }
        }

        /// <summary>
        /// Resets color.
        /// </summary>
        protected virtual void ResetColor(bool isBackground, ResetColorType method = ResetColorType.Auto)
        {
            if (method == ResetColorType.Auto)
                InternalSetColor(isBackground, null);
            else
            {
                var colors = ColorUtils.GetResetColors(method, this);
                var color = isBackground ? colors?.BackgroundColor : colors?.ForegroundColor;
                InternalSetColor(isBackground, color);
            }

            Refresh();
        }

        /// <summary>
        /// Gets <see cref="Color"/> which is used to draw background of the label text.
        /// </summary>
        /// <returns>By default returns <see cref="Color.Empty"/> which means do not
        /// draw background under the label text. In this case control's background
        /// is used.</returns>
        protected virtual Color GetLabelBackColor(VisualControlState state)
        {
            var color = StateObjects?.Colors?.GetObjectOrNull(state)?.BackgroundColor;

            return color ?? TextBackColor ?? Color.Empty;
        }

        /// <summary>
        /// Gets <see cref="Font"/> which is used to draw label's text.
        /// </summary>
        /// <returns></returns>
        protected virtual Font GetLabelFont(VisualControlState state)
        {
            var font = StateObjects?.Colors?.GetObjectOrNull(state)?.Font;

            var result = font ?? Font ?? UI.AbstractControl.DefaultFont;
            if (IsBold)
                result = result.AsBold;
            return result;
        }

        /// <summary>
        /// Gets <see cref="Color"/> which is used to draw label's text.
        /// </summary>
        /// <returns></returns>
        protected virtual Color GetLabelForeColor(VisualControlState state)
        {
            var color = StateObjects?.Colors?.GetObjectOrNull(state)?.ForegroundColor;

            if (color is null)
            {
                if (Enabled)
                    color = ForeColor;
                else
                    color = SystemColors.GrayText;
            }

            return color;
        }

        /// <summary>
        /// Sets visible field value. This is internal method and should not be called
        /// directly.
        /// </summary>
        /// <param name="value"></param>
        protected void SetVisibleValue(bool value) => visible = value;

        /// <summary>
        /// Gets required control handler type.
        /// </summary>
        /// <returns></returns>
        protected virtual HandlerType GetRequiredHandlerType() => HandlerType.Native;
    }
}
