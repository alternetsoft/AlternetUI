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
        /// Gets old dpi. Available only in the <see cref="AbstractControl.DpiChanged"/> event handler.
        /// </summary>
        SizeI EventOldDpi { get; }

        /// <summary>
        /// Gets new dpi. Available only in the <see cref="AbstractControl.DpiChanged"/> event handler.
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

        /// <inheritdoc cref="AbstractControl.Text"/>
        string Text { get; set; }

        /// <inheritdoc cref="UserControl.WantChars"/>
        bool WantChars { get; set; }

        /// <summary>
        /// Gets native control bounds. Valid only in the event handler.
        /// </summary>
        RectD EventBounds { get; }

        /// <inheritdoc cref="AbstractControl.LangDirection"/>
        LangDirection LangDirection { get; set; }

        /// <inheritdoc cref="AbstractControl.BorderStyle"/>
        ControlBorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        AbstractControl Control { get; }

        /// <summary>
        /// Gets a value indicating whether handler is attached
        /// to a <see cref="Control"/>.
        /// </summary>
        bool IsAttached { get; }

        /// <summary>
        /// Gets whether or not native control is created.
        /// </summary>
        bool IsNativeControlCreated { get; }

        /// <inheritdoc cref="AbstractControl.IntrinsicLayoutPadding"/>
        Thickness IntrinsicLayoutPadding { get; }

        /// <inheritdoc cref="AbstractControl.IntrinsicPreferredSizePadding"/>
        Thickness IntrinsicPreferredSizePadding { get; }

        /// <inheritdoc cref="AbstractControl.Bounds"/>
        RectD Bounds { get; set; }

        /// <inheritdoc cref="AbstractControl.BoundsInPixels"/>
        RectI BoundsI { get; set; }

        /// <inheritdoc cref="AbstractControl.Visible"/>
        bool Visible { get; set; }

        /// <inheritdoc cref="AbstractControl.UserPaint"/>
        bool UserPaint { get; set; }

        /// <inheritdoc cref="AbstractControl.MinimumSize"/>
        SizeD MinimumSize { get; set; }

        /// <inheritdoc cref="AbstractControl.MaximumSize"/>
        SizeD MaximumSize { get; set; }

        /// <inheritdoc cref="AbstractControl.BackgroundColor"/>
        Color BackgroundColor { get; set; }

        /// <inheritdoc cref="AbstractControl.ForegroundColor"/>
        Color ForegroundColor { get; set; }

        /// <inheritdoc cref="AbstractControl.Font"/>
        Font? Font { get; set; }

        /// <inheritdoc cref="AbstractControl.IsBold"/>
        bool IsBold { get; set; }

        /// <inheritdoc cref="AbstractControl.AllowDrop"/>
        bool AllowDrop { get; set; }

        /// <inheritdoc cref="AbstractControl.BackgroundStyle"/>
        ControlBackgroundStyle BackgroundStyle { get; set; }

        /// <inheritdoc cref="AbstractControl.ProcessIdle"/>
        bool ProcessIdle { get; set; }

        /// <inheritdoc cref="AbstractControl.ClientSize"/>
        SizeD ClientSize { get; set; }

        /// <inheritdoc cref="AbstractControl.ProcessUIUpdates"/>
        bool ProcessUIUpdates { get; set; }

        /// <inheritdoc cref="AbstractControl.IsMouseCaptured"/>
        bool IsMouseCaptured { get; }

        /// <inheritdoc cref="AbstractControl.IsHandleCreated"/>
        bool IsHandleCreated { get; }

        /// <inheritdoc cref="AbstractControl.Raise"/>
        void Raise();

        /// <inheritdoc cref="AbstractControl.CenterOnParent"/>
        void CenterOnParent(GenericOrientation direction);

        /// <inheritdoc cref="AbstractControl.SetCursor"/>
        void SetCursor(Cursor? value);

        /// <summary>
        /// Sets tooltip text.
        /// </summary>
        /// <param name="value">Tooltip text.</param>
        void SetToolTip(string? value);

        /// <inheritdoc cref="AbstractControl.Lower"/>
        void Lower();

        /// <summary>
        /// Clears toltip text.
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

        /// <inheritdoc cref="AbstractControl.SaveScreenshot"/>
        void SaveScreenshot(string fileName);

        /// <inheritdoc cref="AbstractControl.IsTransparentBackgroundSupported"/>
        bool IsTransparentBackgroundSupported();

        /// <summary>
        /// Gets scale factor.
        /// </summary>
        /// <returns></returns>
        Coord GetPixelScaleFactor();

        /// <inheritdoc cref="AbstractControl.GetUpdateClientRectI"/>
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

        /// <inheritdoc cref="AbstractControl.GetPreferredSize(SizeD)"/>
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

        /// <inheritdoc cref="AbstractControl.OnChildInserted"/>
        void OnChildInserted(AbstractControl childControl);

        /// <inheritdoc cref="AbstractControl.OnChildRemoved"/>
        void OnChildRemoved(AbstractControl childControl);

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this
        /// handler to.</param>
        void Attach(AbstractControl control);

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
