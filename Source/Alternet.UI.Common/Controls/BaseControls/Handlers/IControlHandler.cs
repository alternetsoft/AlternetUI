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
        /// Gets old dpi. Available only in the <see cref="Control.DpiChanged"/> event handler.
        /// </summary>
        SizeI EventOldDpi { get; }

        /// <summary>
        /// Gets new dpi. Available only in the <see cref="Control.DpiChanged"/> event handler.
        /// </summary>
        SizeI EventNewDpi { get; }

        /// <summary>
        /// Gets or sets an action which is called when 'DragDrop' event is raised.
        /// </summary>
        Action<DragEventArgs>? DragDrop { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'DragOver' event is raised.
        /// </summary>
        Action<DragEventArgs>? DragOver { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'DragEnter' event is raised.
        /// </summary>
        Action<DragEventArgs>? DragEnter { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'SystemColorsChanged' event is raised.
        /// </summary>
        Action? SystemColorsChanged { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'DpiChanged' event is raised.
        /// </summary>
        Action? DpiChanged { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'Idle' event is raised.
        /// </summary>
        Action? Idle { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'Paint' event is raised.
        /// </summary>
        Action? Paint { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'MouseEnter' event is raised.
        /// </summary>
        Action? MouseEnter { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'MouseLeave' event is raised.
        /// </summary>
        Action? MouseLeave { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'MouseClick' event is raised.
        /// </summary>
        Action? MouseClick { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'VisibleChanged' event is raised.
        /// </summary>
        Action? VisibleChanged { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'TextChanged' event is raised.
        /// </summary>
        Action? TextChanged { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'MouseCaptureLost' event is raised.
        /// </summary>
        Action? MouseCaptureLost { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'DragLeave' event is raised.
        /// </summary>
        Action? DragLeave { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'SizeChanged' event is raised.
        /// </summary>
        Action? SizeChanged { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'LocationChanged' event is raised.
        /// </summary>
        Action? LocationChanged { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'Activated' event is raised.
        /// </summary>
        Action? Activated { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'Deactivated' event is raised.
        /// </summary>
        Action? Deactivated { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'HandleCreated' event is raised.
        /// </summary>
        Action? HandleCreated { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'HandleDestroyed' event is raised.
        /// </summary>
        Action? HandleDestroyed { get; set; }

        /// <inheritdoc cref="Control.Text"/>
        string Text { get; set; }

        /// <inheritdoc cref="UserControl.WantChars"/>
        bool WantChars { get; set; }

        /// <summary>
        /// Gets native control bounds. Valid only in the event handler.
        /// </summary>
        RectD EventBounds { get; }

        /// <inheritdoc cref="Control.LangDirection"/>
        LangDirection LangDirection { get; set; }

        /// <inheritdoc cref="Control.BorderStyle"/>
        ControlBorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        Control Control { get; }

        /// <summary>
        /// Gets a value indicating whether handler is attached
        /// to a <see cref="Control"/>.
        /// </summary>
        bool IsAttached { get; }

        /// <summary>
        /// Gets whether or not native control is created.
        /// </summary>
        bool IsNativeControlCreated { get; }

        /// <inheritdoc cref="Control.IntrinsicLayoutPadding"/>
        Thickness IntrinsicLayoutPadding { get; }

        /// <inheritdoc cref="Control.IntrinsicPreferredSizePadding"/>
        Thickness IntrinsicPreferredSizePadding { get; }

        /// <inheritdoc cref="Control.Bounds"/>
        RectD Bounds { get; set; }

        /// <inheritdoc cref="Control.BoundsInPixels"/>
        RectI BoundsI { get; set; }

        /// <inheritdoc cref="Control.Visible"/>
        bool Visible { get; set; }

        /// <inheritdoc cref="Control.UserPaint"/>
        bool UserPaint { get; set; }

        /// <inheritdoc cref="Control.MinimumSize"/>
        SizeD MinimumSize { get; set; }

        /// <inheritdoc cref="Control.MaximumSize"/>
        SizeD MaximumSize { get; set; }

        /// <inheritdoc cref="Control.BackgroundColor"/>
        Color BackgroundColor { get; set; }

        /// <inheritdoc cref="Control.ForegroundColor"/>
        Color ForegroundColor { get; set; }

        /// <inheritdoc cref="Control.Font"/>
        Font? Font { get; set; }

        /// <inheritdoc cref="Control.IsBold"/>
        bool IsBold { get; set; }

        /// <inheritdoc cref="Control.AllowDrop"/>
        bool AllowDrop { get; set; }

        /// <inheritdoc cref="Control.BackgroundStyle"/>
        ControlBackgroundStyle BackgroundStyle { get; set; }

        /// <inheritdoc cref="Control.ProcessIdle"/>
        bool ProcessIdle { get; set; }

        /// <inheritdoc cref="Control.ClientSize"/>
        SizeD ClientSize { get; set; }

        /// <inheritdoc cref="Control.ProcessUIUpdates"/>
        bool ProcessUIUpdates { get; set; }

        /// <inheritdoc cref="Control.IsMouseCaptured"/>
        bool IsMouseCaptured { get; }

        /// <inheritdoc cref="Control.IsHandleCreated"/>
        bool IsHandleCreated { get; }

        /// <inheritdoc cref="Control.Raise"/>
        void Raise();

        /// <inheritdoc cref="Control.CenterOnParent"/>
        void CenterOnParent(GenericOrientation direction);

        /// <inheritdoc cref="Control.SetCursor"/>
        void SetCursor(Cursor? value);

        /// <summary>
        /// Sets tooltip text.
        /// </summary>
        /// <param name="value">Tooltip text.</param>
        void SetToolTip(string? value);

        /// <inheritdoc cref="Control.Lower"/>
        void Lower();

        /// <summary>
        /// Clears toltip text.
        /// </summary>
        void UnsetToolTip();

        /// <inheritdoc cref="Control.RefreshRect"/>
        void RefreshRect(RectD rect, bool eraseBackground = true);

        /// <inheritdoc cref="Control.HandleNeeded"/>
        void HandleNeeded();

        /// <inheritdoc cref="Control.CaptureMouse"/>
        void CaptureMouse();

        /// <inheritdoc cref="Control.ReleaseMouseCapture"/>
        void ReleaseMouseCapture();

        /// <inheritdoc cref="Control.CreateDrawingContext"/>
        Graphics CreateDrawingContext();

        /// <inheritdoc cref="Control.ScreenToClient"/>
        PointD ScreenToClient(PointD point);

        /// <inheritdoc cref="Control.ClientToScreen"/>
        PointD ClientToScreen(PointD point);

        /// <inheritdoc cref="Control.DoDragDrop"/>
        DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects);

        /// <inheritdoc cref="Control.RecreateWindow"/>
        void RecreateWindow();

        /// <inheritdoc cref="Control.BeginUpdate"/>
        void BeginUpdate();

        /// <inheritdoc cref="Control.EndUpdate"/>
        void EndUpdate();

        /// <inheritdoc cref="Control.BeginInit"/>
        void BeginInit();

        /// <inheritdoc cref="Control.EndInit"/>
        void EndInit();

        /// <inheritdoc cref="Control.SaveScreenshot"/>
        void SaveScreenshot(string fileName);

        /// <inheritdoc cref="Control.IsTransparentBackgroundSupported"/>
        bool IsTransparentBackgroundSupported();

        /// <summary>
        /// Gets scale factor.
        /// </summary>
        /// <returns></returns>
        Coord GetPixelScaleFactor();

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

        /// <inheritdoc cref="Control.SetEnabled"/>
        void SetEnabled(bool value);

        /// <inheritdoc cref="Control.GetDefaultAttributesBgColor"/>
        Color GetDefaultAttributesBgColor();

        /// <inheritdoc cref="Control.GetDefaultAttributesFgColor"/>
        Color GetDefaultAttributesFgColor();

        /// <inheritdoc cref="Control.GetDefaultAttributesFont"/>
        Font? GetDefaultAttributesFont();

        /// <inheritdoc cref="Control.Update"/>
        void Update();

        /// <inheritdoc cref="Control.Invalidate()"/>
        void Invalidate();

        /// <inheritdoc cref="Control.GetHandle"/>
        IntPtr GetHandle();

        /// <inheritdoc cref="Control.GetPreferredSize(SizeD)"/>
        SizeD GetPreferredSize(SizeD availableSize);

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

        /// <inheritdoc cref="Control.OnChildInserted"/>
        void OnChildInserted(Control childControl);

        /// <inheritdoc cref="Control.OnChildRemoved"/>
        void OnChildRemoved(Control childControl);

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this
        /// handler to.</param>
        void Attach(Control control);

        /// <summary>
        /// Detaches this handler from the <see cref="Control"/> it is attached to.
        /// </summary>
        void Detach();

        /// <summary>
        /// This methods is called when the layout of the control changes.
        /// </summary>
        void OnLayoutChanged();
    }
}
