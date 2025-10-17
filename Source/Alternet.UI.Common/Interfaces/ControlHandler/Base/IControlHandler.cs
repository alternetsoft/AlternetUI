using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with control.
    /// </summary>
    public partial interface IControlHandler : IDisposableObject
    {
        /// <summary>
        /// Gets the reference to native graphics context.
        /// For wxWidgets backend it returns the following:
        /// - On Linux it return cairo_t* pointer.
        /// - On MacOS it returns CGContextRef pointer.
        /// </summary>
        IntPtr NativeGraphicsContext { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        bool HasBorder { get; set; }

        /// <summary>
        /// Gets whether control is visible on screen.
        /// </summary>
        bool VisibleOnScreen { get; }

        /// <inheritdoc cref="AbstractControl.Text"/>
        string Text { get; set; }

        /// <inheritdoc cref="UserControl.WantChars"/>
        bool WantChars { get; set; }

        /// <inheritdoc cref="Control.LangDirection"/>
        LangDirection LangDirection { get; set; }

        /// <inheritdoc cref="Control.BorderStyle"/>
        ControlBorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        Control? Control { get; }

        /// <summary>
        /// Gets a value indicating whether handler is attached
        /// to a <see cref="Control"/>.
        /// </summary>
        bool IsAttached { get; }

        /// <summary>
        /// Gets whether or not native control is created.
        /// </summary>
        bool IsNativeControlCreated { get; }

        /// <inheritdoc cref="AbstractControl.NativePadding"/>
        Thickness NativePadding { get; }

        /// <inheritdoc cref="AbstractControl.Bounds"/>
        RectD Bounds { get; set; }

        /// <inheritdoc cref="AbstractControl.BoundsInPixels"/>
        RectI BoundsI { get; set; }

        /// <inheritdoc cref="AbstractControl.Visible"/>
        bool Visible { get; set; }

        /// <inheritdoc cref="AbstractControl.UserPaint"/>
        bool UserPaint { get; set; }

        /// <inheritdoc cref="AbstractControl.BackgroundColor"/>
        Color BackgroundColor { get; set; }

        /// <inheritdoc cref="AbstractControl.ForegroundColor"/>
        Color ForegroundColor { get; set; }

        /// <inheritdoc cref="AbstractControl.Font"/>
        Font? Font { set; }

        /// <inheritdoc cref="AbstractControl.AllowDrop"/>
        bool AllowDrop { get; set; }

        /// <inheritdoc cref="Control.BackgroundStyle"/>
        ControlBackgroundStyle BackgroundStyle { get; set; }

        /// <inheritdoc cref="AbstractControl.ClientSize"/>
        SizeD ClientSize { get; set; }

        /// <inheritdoc cref="Control.ProcessUIUpdates"/>
        bool ProcessUIUpdates { get; set; }

        /// <inheritdoc cref="AbstractControl.IsMouseCaptured"/>
        bool IsMouseCaptured { get; }

        /// <inheritdoc cref="AbstractControl.IsHandleCreated"/>
        bool IsHandleCreated { get; }

        /// <inheritdoc cref="Window.Raise"/>
        void Raise();

        /// <summary>
        /// Resets the cached best size value so it will be recalculated the next time it is needed.
        /// </summary>
        void InvalidateBestSize();

        /// <inheritdoc cref="AbstractControl.SetCursor"/>
        void SetCursor(Cursor? value);

        /// <summary>
        /// Sets tooltip text.
        /// </summary>
        /// <param name="value">Tooltip text.</param>
        void SetToolTip(object? value);

        /// <inheritdoc cref="Window.Lower"/>
        void Lower();

        /// <summary>
        /// Clears tooltip text.
        /// </summary>
        void UnsetToolTip();

        /// <inheritdoc cref="AbstractControl.RefreshRect"/>
        void RefreshRect(RectD rect, bool eraseBackground = true);

        /// <inheritdoc cref="AbstractControl.HandleNeeded"/>
        void HandleNeeded();

        /// <inheritdoc cref="AbstractControl.CaptureMouse"/>
        void CaptureMouse();

        /// <inheritdoc cref="AbstractControl.ReleaseMouseCapture"/>
        void ReleaseMouseCapture();

        /// <inheritdoc cref="AbstractControl.CreateDrawingContext"/>
        Graphics CreateDrawingContext();

        /// <inheritdoc cref="AbstractControl.ScreenToClient"/>
        PointD ScreenToClient(PointD point);

        /// <inheritdoc cref="AbstractControl.ClientToScreen"/>
        PointD ClientToScreen(PointD point);

        /// <inheritdoc cref="AbstractControl.DoDragDrop"/>
        DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects);

        /// <inheritdoc cref="AbstractControl.RecreateWindow"/>
        void RecreateWindow();

        /// <inheritdoc cref="AbstractControl.BeginUpdate"/>
        void BeginUpdate();

        /// <inheritdoc cref="AbstractControl.EndUpdate"/>
        void EndUpdate();

        /// <inheritdoc cref="AbstractControl.BeginInit"/>
        void BeginInit();

        /// <inheritdoc cref="AbstractControl.EndInit"/>
        void EndInit();

        /// <inheritdoc cref="AbstractControl.IsTransparentBackgroundSupported"/>
        bool IsTransparentBackgroundSupported();

        /// <summary>
        /// Gets scale factor.
        /// </summary>
        /// <returns></returns>
        Coord? GetPixelScaleFactor();

        /// <inheritdoc cref="Control.GetUpdateClientRectI"/>
        RectI GetUpdateClientRectI();

        /// <summary>
        /// Resets background color.
        /// </summary>
        void ResetBackgroundColor();

        /// <summary>
        /// Resets foreground color.
        /// </summary>
        void ResetForegroundColor();

        /// <inheritdoc cref="AbstractControl.SetEnabled"/>
        void SetEnabled(bool value);

        /// <inheritdoc cref="AbstractControl.GetDefaultAttributesBgColor"/>
        Color GetDefaultAttributesBgColor();

        /// <inheritdoc cref="AbstractControl.GetDefaultAttributesFgColor"/>
        Color GetDefaultAttributesFgColor();

        /// <inheritdoc cref="AbstractControl.GetDefaultAttributesFont"/>
        Font? GetDefaultAttributesFont();

        /// <inheritdoc cref="AbstractControl.Update"/>
        void Update();

        /// <inheritdoc cref="AbstractControl.Invalidate()"/>
        void Invalidate();

        /// <inheritdoc cref="AbstractControl.GetHandle"/>
        IntPtr GetHandle();

        /// <inheritdoc cref="AbstractControl.GetPreferredSize(PreferredSizeContext)"/>
        SizeD GetPreferredSize(PreferredSizeContext context);

        /// <summary>
        /// Opens drawing context. Available only in the event handler.
        /// </summary>
        /// <returns></returns>
        Graphics OpenPaintDrawingContext();

        /// <summary>
        /// Gets native control.
        /// </summary>
        /// <returns></returns>
        object GetNativeControl();

        /// <summary>
        /// Enables or disables receiving of the touch events.
        /// </summary>
        /// <param name="flag">The mask specifying what exactly events to receive.</param>
        /// <returns></returns>
        bool EnableTouchEvents(TouchEventsMask flag);

        /// <inheritdoc cref="AbstractControl.OnChildInserted"/>
        void OnChildInserted(AbstractControl childControl);

        /// <inheritdoc cref="AbstractControl.OnChildRemoved"/>
        void OnChildRemoved(AbstractControl childControl);

        /// <summary>
        /// Called when the control is removed from its parent control.
        /// </summary>
        /// <remarks>This method allows the control to perform any necessary cleanup or state updates when
        /// it is detached from its parent.</remarks>
        /// <param name="parentControl">The parent control from which this control was removed.
        /// Cannot be <see langword="null"/>.</param>
        void OnRemovedFromParent(AbstractControl parentControl);

        /// <summary>
        /// Called when the current control is inserted into a parent control.
        /// </summary>
        /// <remarks>This method allows the control to perform any necessary initialization or setup  when
        /// it becomes part of a parent control. The behavior may depend on the specific  implementation of the parent
        /// control.</remarks>
        /// <param name="parentControl">The parent control into which this control is being inserted.
        /// Cannot be <see langword="null"/>.</param>
        void OnInsertedToParent(AbstractControl parentControl);

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this
        /// handler to.</param>
        void Attach(Control control);

        /// <summary>
        /// Sets whether the default context menu is allowed for the control.
        /// </summary>
        /// <param name="value">True to allow the default context menu; otherwise, false.</param>
        void SetAllowDefaultContextMenu(bool value);

        /// <summary>
        /// Detaches this handler from the <see cref="Control"/> it is attached to.
        /// </summary>
        void Detach();

        /// <summary>
        /// This methods is called when the layout of the control changes.
        /// </summary>
        void OnLayoutChanged();

        /// <summary>
        /// This method is called when system colors are changed.
        /// </summary>
        void OnSystemColorsChanged();

        /// <summary>
        /// Sets the rendering flags for a control, which determine how the control is rendered.
        /// </summary>
        /// <remarks>The specified flags control various aspects of the rendering behavior.
        /// Ensure that the flags provided are valid combinations of the
        /// <see cref="ControlRenderingFlags"/> enumeration. Invalid
        /// or unsupported flags may result in undefined behavior.</remarks>
        /// <param name="flags">A combination of <see cref="ControlRenderingFlags"/> values
        /// that specify the rendering options to apply.</param>
        void SetRenderingFlags(ControlRenderingFlags flags);
    }
}
