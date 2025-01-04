using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Control handled by the operating system.
    /// </summary>
    public class Control : AbstractControl
    {
        /// <summary>
        /// Gets an empty control for use in the different places.
        /// Do not change any properties of this control.
        /// </summary>
        public static readonly Control Empty = new EmptyControl()
        {
            Visible = false,
            Enabled = false,
        };

        private int handlerTextChanging;
        private IControlHandler? handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractControl"/> class.
        /// </summary>
        public Control()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractControl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public Control(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <inheritdoc/>
        public override Cursor? Cursor
        {
            get
            {
                return base.Cursor;
            }

            set
            {
                if (Cursor == value)
                    return;
                base.Cursor = value;
                SafeHandler?.SetCursor(value);
            }
        }

        /// <inheritdoc/>
        public override bool VisibleOnScreen
        {
            get
            {
                return SafeHandler?.VisibleOnScreen ?? false;
            }
        }

        /// <inheritdoc/>
        public override bool TabStop
        {
            get
            {
                return SafeHandler?.TabStop ?? false;
            }

            set
            {
                if (TabStop == value)
                    return;
                UpdateFocusFlags(CanSelect, value);
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override bool Focused
        {
            get
            {
                return SafeHandler?.IsFocused ?? false;
            }
        }

        /// <inheritdoc/>
        public override SizeD ClientSize
        {
            get
            {
                if (IsDummy)
                    return SizeD.Empty;
                return SafeHandler?.ClientSize ?? SizeD.Empty;
            }

            set
            {
                if (ClientSize == value || SafeHandler is null)
                    return;

                DoInsideLayout(() =>
                {
                    Handler.ClientSize = value;
                });
            }
        }

        /// <inheritdoc/>
        public override Color RealBackgroundColor
        {
            get
            {
                return SafeHandler?.BackgroundColor ?? SystemColors.Window;
            }
        }

        /// <inheritdoc/>
        public override Color RealForegroundColor
        {
            get
            {
                return SafeHandler?.ForegroundColor ?? SystemColors.WindowText;
            }
        }

        /// <summary>
        /// Gets or sets the language direction for this control.
        /// </summary>
        /// <remarks>
        /// Note that <see cref="LangDirection.Default"/> is returned if layout direction
        /// is not supported.
        /// </remarks>
        [Browsable(false)]
        public virtual LangDirection LangDirection
        {
            get
            {
                return SafeHandler?.LangDirection ?? LangDirection.LeftToRight;
            }

            set
            {
                if (value == LangDirection.Default || DisposingOrDisposed)
                    return;
                Handler.LangDirection = value;
            }
        }

        /// <inheritdoc/>
        public override bool UserPaint
        {
            get => SafeHandler?.UserPaint ?? false;

            set
            {
                if (value && !CanUserPaint)
                    return;
                if (DisposingOrDisposed)
                    return;
                Handler.UserPaint = value;
            }
        }

        /// <inheritdoc/>
        public override RectD Bounds
        {
            get => SafeHandler?.Bounds ?? RectD.Empty;
            set
            {
                value.Size = value.Size.ApplyMinMax(MinimumSize, MaximumSize);
                if (Bounds == value || DisposingOrDisposed)
                    return;
                Handler.Bounds = value;
            }
        }

        /// <inheritdoc/>
        public override SizeD MinimumSize
        {
            get
            {
                return SafeHandler?.MinimumSize ?? SizeD.Empty;
            }

            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);
                if (MinimumSize == value || DisposingOrDisposed)
                    return;
                Handler.MinimumSize = value;
                PerformLayout();
            }
        }

        /// <inheritdoc/>
        public override SizeD MaximumSize
        {
            get
            {
                return SafeHandler?.MaximumSize ?? SizeD.Empty;
            }

            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);
                if (MaximumSize == value || DisposingOrDisposed)
                    return;
                Handler.MaximumSize = value;
                PerformLayout();
            }
        }

        /// <inheritdoc/>
        public override bool AllowDrop
        {
            get => SafeHandler?.AllowDrop ?? false;

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.AllowDrop = value;
            }
        }

        /// <summary>
        /// Gets or sets the background style of the control.
        /// </summary>
        /// <remarks><see cref="ControlBackgroundStyle.Transparent"/> style is not possible
        /// to set as it is not supported on all platforms.</remarks>
        [Browsable(false)]
        public virtual ControlBackgroundStyle BackgroundStyle
        {
            get
            {
                return SafeHandler?.BackgroundStyle ?? ControlBackgroundStyle.Paint;
            }

            set
            {
                if (value == ControlBackgroundStyle.Transparent || DisposingOrDisposed)
                    return;
                Handler.BackgroundStyle = value;
            }
        }

        /// <inheritdoc/>
        public override RectI BoundsInPixels
        {
            get
            {
                return SafeHandler?.BoundsI ?? RectI.Empty;
            }

            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);
                if (BoundsInPixels == value || DisposingOrDisposed)
                    return;
                Handler.BoundsI = value;
            }
        }

        /// <inheritdoc/>
        public override bool IsPlatformControl => true;

        /// <inheritdoc/>
        public override Thickness IntrinsicLayoutPadding
        {
            get => SafeHandler?.IntrinsicLayoutPadding ?? Thickness.Empty;
        }

        /// <inheritdoc/>
        public override Thickness IntrinsicPreferredSizePadding
        {
            get
            {
                return SafeHandler?.IntrinsicPreferredSizePadding ?? Thickness.Empty;
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="AbstractControl.Idle"/> event is fired.
        /// </summary>
        [Browsable(false)]
        public virtual bool ProcessIdle
        {
            get
            {
                return SafeHandler?.ProcessIdle ?? false;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ProcessIdle = value;
            }
        }

        /// <inheritdoc/>
        public override bool IsGraphicControl
        {
            get
            {
                return !CanSelect;
            }

            set
            {
                if (IsGraphicControl == value)
                    return;
                UpdateFocusFlags(!value, !value);
            }
        }

        /// <inheritdoc/>
        public override bool CanSelect
        {
            get
            {
                return SafeHandler?.CanSelect ?? false;
            }

            set
            {
                if (CanSelect == value)
                    return;
                UpdateFocusFlags(value, TabStop);
            }
        }

        /// <inheritdoc/>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                base.Text = value;
            }
        }

        /// <inheritdoc/>
        public override object NativeControl => SafeHandler?.GetNativeControl() ?? new();

        /// <summary>
        /// Gets a <see cref="IControlHandler"/> associated with this class
        /// or Null if control is disposed.
        /// </summary>
        [Browsable(false)]
        public IControlHandler? SafeHandler
        {
            get
            {
                if (DisposingOrDisposed)
                    return null;
                return Handler;
            }
        }

        /// <summary>
        /// Gets a <see cref="IControlHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public virtual IControlHandler Handler
        {
            get
            {
                EnsureHandlerCreated();
                return handler ??= new HandlerForDisposed();
            }
        }

        /// <summary>
        /// Gets or sets border style of the control.
        /// </summary>
        [Browsable(false)]
        public virtual ControlBorderStyle BorderStyle
        {
            get
            {
                return SafeHandler?.BorderStyle ?? ControlBorderStyle.Default;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.BorderStyle = value;
            }
        }

        /// <inheritdoc/>
        public override bool IsHandleCreated
        {
            get
            {
                return (handler is not null) && handler.IsNativeControlCreated
                    && Handler.IsHandleCreated;
            }
        }

        /// <inheritdoc/>
        public override bool IsMouseCaptured
        {
            get
            {
                return SafeHandler?.IsMouseCaptured ?? false;
            }
        }

        internal bool ProcessUIUpdates
        {
            get
            {
                return SafeHandler?.ProcessUIUpdates ?? false;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ProcessUIUpdates = value;
            }
        }

        /// <summary>
        /// Gets <see cref="IControlHandler"/> for the control if it is possible.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static IControlHandler? RequireHandler(AbstractControl? control)
        {
            return (control as Control)?.Handler;
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override void RecreateHandler()
        {
            if (handler != null)
                DetachHandler();

            Invalidate();
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override void RaiseVisibleChanged()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Visible = Visible;
            base.RaiseVisibleChanged();
        }

        /// <inheritdoc/>
        public override void RaiseChildInserted(int index, AbstractControl childControl)
        {
            base.RaiseChildInserted(index, childControl);
            Handler.OnChildInserted(childControl);
        }

        /// <inheritdoc/>
        public override void RaiseChildRemoved(AbstractControl childControl)
        {
            base.RaiseChildRemoved(childControl);
            if(handler is not null)
                Handler.OnChildRemoved(childControl);
        }

        /// <summary>
        /// Gets the update rectangle region bounding box in client coordinates. This method
        /// can be used in paint events. Returns rectangle in pixels.
        /// </summary>
        /// <returns></returns>
        public virtual RectI GetUpdateClientRectI()
        {
            return SafeHandler?.GetUpdateClientRectI() ?? RectI.Empty;
        }

        /// <summary>
        /// Gets the update rectangle region bounding box in client coordinates. This method
        /// can be used in paint events. Returns rectangle in device-independent units.
        /// </summary>
        /// <returns></returns>
        public virtual RectD GetUpdateClientRect()
        {
            var resultI = GetUpdateClientRectI();
            var resultD = PixelToDip(resultI);
            return resultD;
        }

        /// <inheritdoc/>
        public override DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return SafeHandler?.DoDragDrop(data, allowedEffects) ?? DragDropEffects.None;
        }

        /// <inheritdoc/>
        public override int BeginUpdate()
        {
            if (!DisposingOrDisposed)
            {
                Handler.BeginUpdate();
            }

            return base.BeginUpdate();
        }

        /// <inheritdoc/>
        public override Color? GetDefaultAttributesBgColor()
        {
            return SafeHandler?.GetDefaultAttributesBgColor();
        }

        /// <inheritdoc/>
        public override Color? GetDefaultAttributesFgColor()
        {
            return SafeHandler?.GetDefaultAttributesFgColor();
        }

        /// <inheritdoc/>
        public override Font? GetDefaultAttributesFont()
        {
            return SafeHandler?.GetDefaultAttributesFont();
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override void RaiseBackgroundColorChanged()
        {
            if (BackgroundColor is null)
                ResetBackgroundColor(ResetColorType.Auto);
            else
            {
                if(!DisposingOrDisposed)
                    Handler.BackgroundColor = BackgroundColor;
            }

            base.RaiseBackgroundColorChanged();
        }

        /// <inheritdoc/>
        public override void RefreshRect(RectD rect, bool eraseBackground = true)
        {
            if (CanSkipInvalidate())
                return;
            SafeHandler?.RefreshRect(rect, eraseBackground);
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override Graphics CreateDrawingContext()
        {
            return SafeHandler?.CreateDrawingContext() ?? new PlessGraphics();
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override void Invalidate()
        {
            if (CanSkipInvalidate())
                return;
            if(!DisposingOrDisposed)
                Handler.Invalidate();
            base.Invalidate();
        }

        /// <inheritdoc/>
        public override bool SetFocus()
        {
            return SafeHandler?.SetFocus() ?? false;
        }

        /// <inheritdoc/>
        public override void Update()
        {
            if (DisposingOrDisposed)
                return;
            if (CanSkipInvalidate())
                return;
            Handler.Update();
        }

        /// <inheritdoc/>
        public override void HandleNeeded()
        {
            SafeHandler?.HandleNeeded();
        }

        /// <inheritdoc/>
        public override bool IsTransparentBackgroundSupported()
        {
            return SafeHandler?.IsTransparentBackgroundSupported() ?? false;
        }

        /// <summary>
        /// Unbinds events from the handler.
        /// </summary>
        [Browsable(false)]
        public virtual void UnbindHandlerEvents()
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
            Handler.DragOver = null;
            Handler.DragEnter = null;
            Handler.DragDrop = null;
            Handler.SystemColorsChanged = null;
            Handler.DpiChanged = null;
        }

        /// <summary>
        /// Binds events to the handler.
        /// </summary>
        [Browsable(false)]
        public virtual void BindHandlerEvents()
        {
            if (DisposingOrDisposed)
                return;

            Handler.MouseEnter = RaiseMouseEnterOnTarget;
            Handler.MouseLeave = RaiseMouseLeaveOnTarget;
            Handler.HandleCreated = RaiseHandleCreated;
            Handler.HandleDestroyed = RaiseHandleDestroyed;
            Handler.Activated = RaiseActivated;
            Handler.Deactivated = RaiseDeactivated;
            Handler.Paint = OnHandlerPaint;
            Handler.VisibleChanged = OnHandlerVisibleChanged;
            Handler.MouseCaptureLost = RaiseMouseCaptureLost;

            Handler.GotFocus = () =>
            {
                RaiseGotFocus(Handler.EventFocusedControl);
            };

            Handler.LostFocus = () =>
            {
                RaiseLostFocus(Handler.EventFocusedControl);
            };

            Handler.Idle = RaiseIdle;
            Handler.DragLeave = RaiseDragLeave;
            Handler.SizeChanged = RaiseHandlerSizeChanged;
            Handler.LocationChanged = RaiseContainerLocationChanged;
            Handler.DragOver = RaiseDragOver;
            Handler.DragEnter = RaiseDragEnter;
            Handler.DragDrop = RaiseDragDrop;
            Handler.TextChanged = OnHandlerTextChanged;
            Handler.SystemColorsChanged = RaiseSystemColorsChanged;
            Handler.DpiChanged = OnHandlerDpiChanged;
        }

        /// <inheritdoc/>
        public override PointD ScreenToClient(PointD point)
        {
            return SafeHandler?.ScreenToClient(point) ?? point;
        }

        /// <inheritdoc/>
        public override PointD ClientToScreen(PointD point)
        {
            return SafeHandler?.ClientToScreen(point) ?? point;
        }

        /// <inheritdoc/>
        public override void HideToolTip()
        {
            SafeHandler?.UnsetToolTip();
            SafeHandler?.SetToolTip(GetRealToolTip());
        }

        /// <inheritdoc/>
        public override void BeginInit()
        {
            base.BeginInit();
            SafeHandler?.BeginInit();
        }

        /// <inheritdoc/>
        public override void InvalidateBestSize()
        {
            SafeHandler?.InvalidateBestSize();
        }

        /// <inheritdoc/>
        public override void EndInit()
        {
            SafeHandler?.EndInit();
            base.EndInit();
        }

        /// <inheritdoc/>
        public override IntPtr GetHandle()
        {
            return SafeHandler?.GetHandle() ?? default;
        }

        /// <inheritdoc/>
        public override int EndUpdate()
        {
            SafeHandler?.EndUpdate();
            return base.EndUpdate();
        }

        /// <inheritdoc/>
        public override void CaptureMouse()
        {
            SafeHandler?.CaptureMouse();
        }

        /// <inheritdoc/>
        public override void ReleaseMouseCapture()
        {
            SafeHandler?.ReleaseMouseCapture();
        }

        /// <inheritdoc/>
        public override void RecreateWindow()
        {
            SafeHandler?.RecreateWindow();
        }

        /// <inheritdoc/>
        public override void FocusNextControl(bool forward = true, bool nested = true)
        {
            if (Keyboard.ProcessTabInternally)
                base.FocusNextControl(forward, nested);
            else
                SafeHandler?.FocusNextControl(forward, nested);
        }

        /// <summary>
        /// Saves screenshot of this control.
        /// </summary>
        /// <param name="fileName">Name of the file to which screenshot
        /// will be saved.</param>
        /// <remarks>This function works only on Windows for WxWidgets port.</remarks>
        public virtual void SaveScreenshot(string fileName)
        {
            ScreenShotCounter++;
            try
            {
                SafeHandler?.SaveScreenshot(fileName);
            }
            finally
            {
                ScreenShotCounter--;
            }
        }

        /// <inheritdoc/>
        public override void RaiseHandleCreated()
        {
            if (DisposingOrDisposed)
                return;

            if (BackgroundColor is not null)
                Handler.BackgroundColor = BackgroundColor;
            if (ForegroundColor is not null)
                Handler.ForegroundColor = ForegroundColor;
            base.RaiseHandleCreated();
        }

        internal virtual void OnHandlerDpiChanged()
        {
            if (DisposingOrDisposed)
                return;

            var oldDpi = Handler.EventOldDpi;
            var newDpi = Handler.EventNewDpi;

            var e = new DpiChangedEventArgs(oldDpi, newDpi);
            RaiseDpiChanged(e);
        }

        internal virtual void OnHandlerVisibleChanged()
        {
            if (DisposingOrDisposed)
                return;

            bool visible = Handler.Visible;
            Visible = visible;

            if (App.IsLinuxOS && visible)
            {
                // todo: this is a workaround for a problem on Linux when
                // ClientSize is not reported correctly until the window is shown
                // So we need to relayout all after the proper client size is available
                // This should be changed later in respect to RedrawOnResize functionality.
                // Also we may need to do this for top-level windows.
                // Doing this on Windows results in strange glitches like disappearing
                // tab controls' tab.
                // See https://forums.wxwidgets.org/viewtopic.php?f=1&t=47439
                PerformLayout();
            }
        }

        internal virtual void OnHandlerPaint()
        {
            if (DisposingOrDisposed)
                return;

            if (!UserPaint)
                return;
            var clientRect = ClientRectangle;
            if (clientRect.SizeIsEmpty)
                return;
            if (!VisibleOnScreen)
                return;

            var e = new PaintEventArgs(() => Handler.OpenPaintDrawingContext(), clientRect);

            try
            {
                RaisePaint(e);
            }
            finally
            {
                if (e.GraphicsAllocated)
                {
                    e.Graphics.Dispose();
                }
                else
                {
                }
            }
        }

        /// <summary>
        /// Ensures that the control <see cref="Handler"/> is created,
        /// creating and attaching it if necessary.
        /// </summary>
        protected internal void EnsureHandlerCreated()
        {
            if (handler == null)
            {
                if (DisposingOrDisposed)
                {
                    handler = new HandlerForDisposed();
                    return;
                }

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
                    {
                        var child = Children[i];
                        if (child is GenericControl)
                            continue;
                        handler.OnChildInserted(child);
                    }
                }
            }
        }

        /// <summary>
        /// Disconnects the current control <see cref="Handler"/> from
        /// the control.
        /// This method calls <see cref="IControlHandler.Detach"/>.
        /// </summary>
        protected internal override void DetachHandler()
        {
            if (handler == null)
                return;
            OnHandlerDetaching(EventArgs.Empty);
            UnbindHandlerEvents();
            handler.Detach();
            handler = null;
        }

        /// <inheritdoc/>
        protected override void UpdateFocusFlags(bool canSelect, bool tabStop)
        {
            SafeHandler?.SetFocusFlags(canSelect, tabStop && canSelect, canSelect);
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (handlerTextChanging == 0)
            {
                var coercedText = CoerceTextForHandler(Text);
                var forced = StateFlags.HasFlag(ControlFlags.ForceTextChange);

                if (forced || Handler.Text != coercedText)
                    Handler.Text = coercedText;
            }
        }

        /// <inheritdoc/>
        protected override Coord? RequestScaleFactor()
        {
            return SafeHandler?.GetPixelScaleFactor();
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

        /// <inheritdoc/>
        protected override SizeD GetBestSizeWithoutPadding(SizeD availableSize)
        {
            return SafeHandler?.GetPreferredSize(availableSize) ?? SizeD.Empty;
        }

        /// <summary>
        /// Called when handler's text property is changed.
        /// </summary>
        protected virtual void OnHandlerTextChanged()
        {
            if (DisposingOrDisposed)
                return;
            if (handlerTextChanging > 0)
                return;

            handlerTextChanging++;
            try
            {
                Text = Handler.Text;
            }
            finally
            {
                handlerTextChanging--;
            }
        }

        /// <inheritdoc/>
        protected override void RaiseEnabledChanged(EventArgs e)
        {
            SafeHandler?.SetEnabled(Enabled);
            base.RaiseEnabledChanged(EventArgs.Empty);
        }

        /// <inheritdoc/>
        protected override void InternalSetColor(bool isBackground, Color? color)
        {
            if (DisposingOrDisposed)
                return;

            base.InternalSetColor(isBackground, color);

            if (isBackground)
            {
                if (color is null)
                    Handler.ResetBackgroundColor();
                else
                    Handler.BackgroundColor = color;
            }
            else
            {
                if (color is null)
                    Handler.ResetForegroundColor();
                else
                    Handler.ForegroundColor = color;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            UnbindHandlerEvents();
            base.DisposeManaged();
        }

        /// <inheritdoc/>
        protected override void OnFontChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            Handler.Font = RealFont;
        }

        /// <inheritdoc/>
        protected override void OnToolTipChanged(EventArgs e)
        {
            SafeHandler?.SetToolTip(GetRealToolTip());
        }

        private class EmptyControl : Control
        {
            protected override IControlHandler CreateHandler()
            {
                return HandlerForDisposed.Default;
            }
        }
    }
}
