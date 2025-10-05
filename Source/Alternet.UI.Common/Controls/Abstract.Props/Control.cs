using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Control handled by the operating system.
    /// It is not suggested to create <see cref="Control"/> directly.
    /// </summary>
    public partial class Control : AbstractControl
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
        private bool userPaint;

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
                UpdateCursor();
            }
        }

        /// <inheritdoc/>
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }

            set
            {
                base.Visible = value;
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
            get => userPaint;

            set
            {
                if (userPaint == value)
                    return;
                if (value && !CanUserPaint)
                    return;
                if (DisposingOrDisposed)
                    return;
                userPaint = value;
                Handler.UserPaint = value;
            }
        }

        /// <inheritdoc/>
        public override SizeD ClientSize
        {
            get
            {
                if (IsDummy)
                    return SizeD.Empty;
                var result = SafeHandler?.ClientSize ?? SizeD.Empty;

                var size = Size;
                if (result.Width <= 0)
                    result.Width = size.Width;
                if (result.Height <= 0)
                    result.Height = size.Height;

                return result;
            }
        }

        /// <inheritdoc/>
        public override bool IsScrollable
        {
            get
            {
                return SafeHandler?.IsScrollable ?? false;
            }

            set
            {
                if (IsScrollable == value || DisposingOrDisposed)
                    return;
                Handler.IsScrollable = value;
            }
        }

        /// <inheritdoc/>
        public override RectD Bounds
        {
            get => SafeHandler?.Bounds ?? RectD.Empty;
            set
            {
                value = CoerceBounds(value);
                if (Bounds == value || DisposingOrDisposed)
                    return;
                Handler.Bounds = value;
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

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public override bool HasBorder
        {
            get
            {
                if (DisposingOrDisposed)
                    return false;
                return Handler.HasBorder;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.HasBorder = value;
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
        public override bool IsPlatformControl
        {
            get
            {
                if (App.Handler is null)
                    return true;

                return App.Handler.IsPlatformControl(this);
            }
        }

        /// <inheritdoc/>
        public override Thickness NativePadding
        {
            get => SafeHandler?.NativePadding ?? Thickness.Empty;
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
        public override object NativeControl
        {
            get
            {
                return SafeHandler?.GetNativeControl() ?? new();
            }
        }

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

        /// <inheritdoc/>
        public override bool AllowDefaultContextMenu
        {
            get
            {
                return base.AllowDefaultContextMenu;
            }

            set
            {
                if (AllowDefaultContextMenu == value)
                    return;
                base.AllowDefaultContextMenu = value;
                SafeHandler?.SetAllowDefaultContextMenu(value);
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
        public override void RaiseVisibleChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            Handler.Visible = Visible;
            base.RaiseVisibleChanged(e);
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
        public override int EndUpdate()
        {
            var result = base.EndUpdate();

            if (this is UserControl)
            {
                if (result == 0)
                    Invalidate();
            }
            else
            {
                SafeHandler?.EndUpdate();
            }

            return result;
        }

        /// <inheritdoc/>
        public override int BeginUpdate()
        {
            var result = base.BeginUpdate();

            if (!DisposingOrDisposed)
            {
                if (this is not UserControl)
                    Handler.BeginUpdate();
            }

            return result;
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

        /// <summary>
        /// Called when handler's text property is changed.
        /// </summary>
        public virtual void RaiseHandlerTextChanged(string s)
        {
            if (DisposingOrDisposed)
                return;
            if (handlerTextChanging > 0)
                return;

            handlerTextChanging++;
            try
            {
                Text = s;
            }
            finally
            {
                handlerTextChanging--;
            }
        }

        /// <inheritdoc/>
        public override bool SetFocus()
        {
            if (DisposingOrDisposed)
                return false;
            var window = ParentWindow;
            if (window is null || window.State == WindowState.Minimized)
                return false;
            return Handler.SetFocus();
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
        public override ScrollBarInfo GetScrollBarInfo(bool isVertical)
        {
            if (DisposingOrDisposed)
                return ScrollBarInfo.Default;
            if (isVertical)
                return Handler.VertScrollBarInfo;
            else
                return Handler.HorzScrollBarInfo;
        }

        /// <inheritdoc/>
        public override void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
            if (DisposingOrDisposed)
                return;
            if (isVertical)
                Handler.VertScrollBarInfo = value;
            else
                Handler.HorzScrollBarInfo = value;
        }

        /// <summary>
        /// Updates cursor and call's <see cref="IControlHandler.SetCursor"/>.
        /// </summary>
        public virtual void UpdateCursor(Cursor? overrideCursor = null)
        {
            if(overrideCursor is null)
                SafeHandler?.SetCursor(Cursor);
            else
                SafeHandler?.SetCursor(overrideCursor);
        }

        /// <inheritdoc/>
        public override void RaiseHandleCreated(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            if (BackgroundColor is not null)
                Handler.BackgroundColor = BackgroundColor;
            if (ForegroundColor is not null)
                Handler.ForegroundColor = ForegroundColor;
            base.RaiseHandleCreated(e);
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
                var requiredHandlerType = GetRequiredHandlerType();

                switch (requiredHandlerType)
                {
                    case HandlerType.Native:
                        handler = CreateHandler();
                        break;
                    default:
                    case HandlerType.Generic:
                        handler = ControlFactory.Handler.CreateControlHandler(this);
                        break;
                    case HandlerType.OpenGL:
                        handler = ControlFactory.Handler.CreateOpenGLControlHandler(this);
                        break;
                }

                handler.Attach(this);

                handler.SetEnabled(Enabled);
                RaiseHandleCreated(EventArgs.Empty);
                handler.Visible = Visible;
                ApplyChildren();

                OnHandlerAttached(EventArgs.Empty);

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
            handler.Detach();
            handler = null;
        }

        /// <inheritdoc/>
        protected override void UpdateFocusFlags(bool canSelect, bool tabStop)
        {
            base.UpdateFocusFlags(canSelect, tabStop);
            SafeHandler?.UpdateFocusFlags(canSelect, tabStop);
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            SuspendHandlerTextChange();

            try
            {
                var coercedText = CoerceTextForHandler(Text);
                var forced = StateFlags.HasFlag(ControlFlags.ForceTextChange);

                if (forced || Handler.Text != coercedText)
                    Handler.Text = coercedText;
            }
            finally
            {
                ResumeHandlerTextChange();
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
        protected override SizeD GetBestSizeWithoutPadding(PreferredSizeContext context)
        {
            return SafeHandler?.GetPreferredSize(context) ?? SizeD.Empty;
        }

        /// <inheritdoc/>
        protected override void RaiseEnabledChanged(EventArgs e)
        {
            SafeHandler?.SetEnabled(Enabled);
            base.RaiseEnabledChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Suspends live binding between native control's Text and control's Text.
        /// </summary>
        protected void SuspendHandlerTextChange()
        {
            handlerTextChanging++;
        }

        /// <summary>
        /// Resumes live binding between native control's Text and control's Text.
        /// </summary>
        protected void ResumeHandlerTextChange()
        {
            handlerTextChanging--;
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

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        /// <inheritdoc/>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
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
