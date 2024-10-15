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
                Handler.SetCursor(value);
            }
        }

        /// <inheritdoc/>
        public override bool TabStop
        {
            get
            {
                return Handler.TabStop;
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
                return Handler.IsFocused;
            }
        }

        /// <inheritdoc/>
        public override SizeD ClientSize
        {
            get
            {
                if (IsDummy)
                    return SizeD.Empty;
                return Handler.ClientSize;
            }

            set
            {
                if (ClientSize == value)
                    return;

                DoInsideLayout(() =>
                {
                    Handler.ClientSize = value;
                });
            }
        }

        /// <inheritdoc/>
        public override bool IsScrollable
        {
            get
            {
                return Handler.IsScrollable;
            }

            set
            {
                Handler.IsScrollable = value;
            }
        }

        /// <inheritdoc/>
        public override Color RealBackgroundColor
        {
            get
            {
                return Handler.BackgroundColor;
            }
        }

        /// <inheritdoc/>
        public override Color RealForegroundColor
        {
            get
            {
                return Handler.ForegroundColor;
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
                return Handler.LangDirection;
            }

            set
            {
                if (value == LangDirection.Default)
                    return;
                Handler.LangDirection = value;
            }
        }

        /// <inheritdoc/>
        public override Font? RealFont => Handler.Font;

        /// <inheritdoc/>
        public override bool UserPaint
        {
            get => Handler.UserPaint;

            set
            {
                if (value && !CanUserPaint)
                    return;
                Handler.UserPaint = value;
            }
        }

        /// <inheritdoc/>
        public override RectD Bounds
        {
            get => Handler.Bounds;
            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);
                if (Bounds == value)
                    return;
                Handler.Bounds = value;
            }
        }

        /// <inheritdoc/>
        public override SizeD MinimumSize
        {
            get
            {
                return Handler.MinimumSize;
            }

            set
            {
                if (MinimumSize == value)
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
                return Handler.MaximumSize;
            }

            set
            {
                if (MaximumSize == value)
                    return;
                Handler.MaximumSize = value;
                PerformLayout();
            }
        }

        /// <inheritdoc/>
        public override bool AllowDrop
        {
            get => Handler.AllowDrop;
            set => Handler.AllowDrop = value;
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
                return Handler.BackgroundStyle;
            }

            set
            {
                if (value == ControlBackgroundStyle.Transparent)
                    return;
                Handler.BackgroundStyle = value;
            }
        }

        /// <inheritdoc/>
        public override RectI BoundsInPixels
        {
            get
            {
                return Handler.BoundsI;
            }

            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);
                if (BoundsInPixels == value)
                    return;
                Handler.BoundsI = value;
            }
        }

        /// <inheritdoc/>
        public override Thickness IntrinsicLayoutPadding
        {
            get => Handler.IntrinsicLayoutPadding;
        }

        /// <inheritdoc/>
        public override Thickness IntrinsicPreferredSizePadding
            => Handler.IntrinsicPreferredSizePadding;

        /// <summary>
        /// Gets or sets whether <see cref="AbstractControl.Idle"/> event is fired.
        /// </summary>
        [Browsable(false)]
        public virtual bool ProcessIdle
        {
            get
            {
                return Handler.ProcessIdle;
            }

            set
            {
                Handler.ProcessIdle = value;
            }
        }

        /// <inheritdoc/>
        public override bool IsGraphicControl
        {
            get
            {
                return !Handler.CanSelect;
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
                return Handler.CanSelect;
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
        public override object NativeControl => Handler.GetNativeControl();

        /// <summary>
        /// Gets a <see cref="IControlHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public virtual IControlHandler Handler
        {
            get
            {
                EnsureHandlerCreated();
                return handler ?? throw new InvalidOperationException();
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
                return Handler.IsMouseCaptured;
            }
        }

        internal bool ProcessUIUpdates
        {
            get
            {
                return Handler.ProcessUIUpdates;
            }

            set
            {
                Handler.ProcessUIUpdates = value;
            }
        }

        /// <summary>
        /// Gets or sets border style of the control.
        /// </summary>
        protected virtual ControlBorderStyle BorderStyle
        {
            get
            {
                return Handler.BorderStyle;
            }

            set
            {
                Handler.BorderStyle = value;
            }
        }

        /// <inheritdoc/>
        protected override bool BindScrollEvents
        {
            get
            {
                return Handler.BindScrollEvents;
            }

            set
            {
                Handler.BindScrollEvents = value;
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
        public override void RecreateHandler()
        {
            if (handler != null)
                DetachHandler();

            Invalidate();
        }

        /// <inheritdoc/>
        public override void RaiseVisibleChanged()
        {
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
            Handler.OnChildRemoved(childControl);
        }

        /// <summary>
        /// Gets the update rectangle region bounding box in client coordinates. This method
        /// can be used in paint events. Returns rectangle in pixels.
        /// </summary>
        /// <returns></returns>
        public virtual RectI GetUpdateClientRectI()
        {
            return Handler.GetUpdateClientRectI();
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
            return Handler.DoDragDrop(data, allowedEffects);
        }

        /// <inheritdoc/>
        public override int BeginUpdate()
        {
            Handler.BeginUpdate();
            return base.BeginUpdate();
        }

        /// <inheritdoc/>
        public override Color? GetDefaultAttributesBgColor()
        {
            CheckDisposed();
            return Handler.GetDefaultAttributesBgColor();
        }

        /// <inheritdoc/>
        public override Color? GetDefaultAttributesFgColor()
        {
            CheckDisposed();
            return Handler.GetDefaultAttributesFgColor();
        }

        /// <inheritdoc/>
        public override Font? GetDefaultAttributesFont()
        {
            CheckDisposed();
            return Handler.GetDefaultAttributesFont();
        }

        /// <inheritdoc/>
        public override void RaiseBackgroundColorChanged()
        {
            if (BackgroundColor is null)
                ResetBackgroundColor(ResetColorType.Auto);
            else
                Handler.BackgroundColor = BackgroundColor;
            base.RaiseBackgroundColorChanged();
        }

        /// <summary>
        /// Centers the window.
        /// </summary>
        /// <param name="direction">Specifies the direction for the centering.</param>
        /// <remarks>
        /// If the window is a top level one (i.e. doesn't have a parent), it will be
        /// centered relative to the screen anyhow.
        /// </remarks>
        public virtual void CenterOnParent(GenericOrientation direction)
        {
            Handler.CenterOnParent(direction);
        }

        /// <inheritdoc/>
        public override void RefreshRect(RectD rect, bool eraseBackground = true)
        {
            Handler.RefreshRect(rect, eraseBackground);
        }

        /// <inheritdoc/>
        public override Graphics CreateDrawingContext()
        {
            return Handler.CreateDrawingContext();
        }

        /// <inheritdoc/>
        public override void Invalidate()
        {
            Handler.Invalidate();
        }

        /// <inheritdoc/>
        public override bool SetFocus()
        {
            return Handler.SetFocus();
        }

        /// <inheritdoc/>
        public override void Update()
        {
            Handler.Update();
        }

        /// <inheritdoc/>
        public override void HandleNeeded()
        {
            Handler.HandleNeeded();
        }

        /// <inheritdoc/>
        public override bool IsTransparentBackgroundSupported()
        {
            return Handler.IsTransparentBackgroundSupported();
        }

        /// <inheritdoc/>
        public override PointD ScreenToClient(PointD point)
        {
            return Handler.ScreenToClient(point);
        }

        /// <inheritdoc/>
        public override PointD ClientToScreen(PointD point)
        {
            return Handler.ClientToScreen(point);
        }

        /// <inheritdoc/>
        public override void HideToolTip()
        {
            Handler.UnsetToolTip();
            Handler.SetToolTip(GetRealToolTip());
        }

        /// <inheritdoc/>
        public override void BeginInit()
        {
            base.BeginInit();
            Handler.BeginInit();
        }

        /// <inheritdoc/>
        public override void EndInit()
        {
            Handler.EndInit();
            base.EndInit();
        }

        /// <inheritdoc/>
        public override IntPtr GetHandle()
        {
            return Handler.GetHandle();
        }

        /// <inheritdoc/>
        public override int EndUpdate()
        {
            Handler.EndUpdate();
            return base.EndUpdate();
        }

        /// <inheritdoc/>
        public override void CaptureMouse()
        {
            Handler.CaptureMouse();
        }

        /// <inheritdoc/>
        public override void ReleaseMouseCapture()
        {
            Handler.ReleaseMouseCapture();
        }

        /// <inheritdoc/>
        public override void RecreateWindow()
        {
            Handler.RecreateWindow();
        }

        /// <inheritdoc/>
        public override void FocusNextControl(bool forward = true, bool nested = true)
        {
            Handler.FocusNextControl(forward, nested);
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
                Handler.SaveScreenshot(fileName);
            }
            finally
            {
                ScreenShotCounter--;
            }
        }

        /// <inheritdoc/>
        public override ScrollBarInfo GetScrollBarInfo(bool isVertical)
        {
            if (isVertical)
                return Handler.VertScrollBarInfo;
            return Handler.HorzScrollBarInfo;
        }

        /// <inheritdoc/>
        public override void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
            if (isVertical)
            {
                Handler.VertScrollBarInfo = value;
            }
            else
            {
                Handler.HorzScrollBarInfo = value;
            }

            base.SetScrollBarInfo(isVertical, value);
        }

        /// <inheritdoc/>
        public override void RaiseHandleCreated()
        {
            if (BackgroundColor is not null)
                Handler.BackgroundColor = BackgroundColor;
            if (ForegroundColor is not null)
                Handler.ForegroundColor = ForegroundColor;
            base.RaiseHandleCreated();
        }

        internal virtual void OnHandlerDpiChanged()
        {
            var oldDpi = Handler.EventOldDpi;
            var newDpi = Handler.EventNewDpi;

            var e = new DpiChangedEventArgs(oldDpi, newDpi);
            RaiseDpiChanged(e);
        }

        internal virtual void OnHandlerVerticalScrollBarValueChanged()
        {
            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollBarOrientation.Vertical,
                NewValue = Handler.GetScrollBarEvtPosition(),
                Type = Handler.GetScrollBarEvtKind(),
            };
            RaiseScroll(args);
        }

        internal virtual void OnHandlerVisibleChanged()
        {
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

        internal virtual void OnHandlerHorizontalScrollBarValueChanged()
        {
            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollBarOrientation.Horizontal,
                NewValue = Handler.GetScrollBarEvtPosition(),
                Type = Handler.GetScrollBarEvtKind(),
            };
            RaiseScroll(args);
        }

        internal virtual void OnHandlerPaint()
        {
            if (!UserPaint)
                return;
            using var dc = Handler.OpenPaintDrawingContext();

            var paintArgs = new PaintEventArgs(dc, ClientRectangle);
            RaisePaint(paintArgs);
            /*
            if(this is UserControl)
                RaisePaintRecursive(paintArgs, true, true);
            */
        }

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
                throw new InvalidOperationException();
            OnHandlerDetaching(EventArgs.Empty);
            UnbindHandlerEvents();
            handler.Detach();
            handler = null;
        }

        /// <inheritdoc/>
        protected override void UpdateFocusFlags(bool canSelect, bool tabStop)
        {
            Handler.SetFocusFlags(canSelect, tabStop && canSelect, canSelect);
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            if (handlerTextChanging == 0)
            {
                var coercedText = CoerceTextForHandler(Text);
                var forced = StateFlags.HasFlag(ControlFlags.ForceTextChange);

                if (forced || Handler.Text != coercedText)
                    Handler.Text = coercedText;
            }
        }

        /// <inheritdoc/>
        protected override Coord RequestScaleFactor()
        {
            return Handler.GetPixelScaleFactor();
        }

        /// <summary>
        /// Unbinds events from the handler.
        /// </summary>
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
            Handler.DpiChanged = null;
        }

        /// <summary>
        /// Binds events to the handler.
        /// </summary>
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
            Handler.DpiChanged = OnHandlerDpiChanged;
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
        protected override SizeD GetNativeControlSize(SizeD availableSize)
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
        /// Called when handler's text property is changed.
        /// </summary>
        protected virtual void OnHandlerTextChanged()
        {
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
            Handler.SetEnabled(Enabled);
            base.RaiseEnabledChanged(EventArgs.Empty);
        }

        /// <inheritdoc/>
        protected override void InternalSetColor(bool isBackground, Color? color)
        {
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
        protected override void OnFontChanged(EventArgs e)
        {
            Handler.Font = HasDefaultFont ? null : Font;
            Handler.IsBold = IsBold;
        }

        /// <inheritdoc/>
        protected override void OnToolTipChanged(EventArgs e)
        {
            Handler.SetToolTip(GetRealToolTip());
        }
    }
}
