using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Parent class for all owner draw controls.
    /// </summary>
    [ControlCategory("Other")]
    public partial class UserControl : Control
    {
        private bool hasBorder;
        private RichTextBoxScrollBars scrollBars = RichTextBoxScrollBars.None;
        private bool showDropDownMenuWhenClicked = true;
        private List<IControlOverlay>? overlays;
        private HVAlignment? dropDownMenuPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public UserControl(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControl"/> class.
        /// </summary>
        public UserControl()
        {
            SuspendHandlerTextChange();
            hasBorder = GetDefaultHasBorder();
            UserPaint = true;
        }

        /// <summary>
        /// Occurs when a drop-down menu is about to be displayed,
        /// allowing the event handler to cancel the operation.
        /// </summary>
        /// <remarks>This event is raised before the drop-down menu is shown.
        /// Handlers can inspect the event arguments to determine the context of the menu
        /// and set the <see cref="CancelEventArgs.Cancel"/>
        /// property to <see langword="true"/> to prevent the menu from being displayed.</remarks>
        public event EventHandler<BaseCancelEventArgs>? DropDownMenuShowing;

        /// <summary>
        /// Gets or sets the alignment position of the drop-down menu.
        /// Based on this property, the menu will be aligned horizontally and vertically
        /// relative to the control bounds. If not set, the menu will be aligned
        /// to the control's bottom-left corner. Default is <see langword="null"/>.
        /// </summary>
        [Browsable(false)]
        public virtual HVAlignment? DropDownMenuPosition
        {
            get => dropDownMenuPosition;
            set => dropDownMenuPosition = value;
        }

        /// <summary>
        /// Gets or sets different behavior and visualization options.
        /// </summary>
        [Browsable(false)]
        public virtual ControlRefreshOptions RefreshOptions { get; set; }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.UserPaintControl;

        /// <summary>
        /// Gets or sets <see cref="ContextMenu"/> which is shown when control is clicked
        /// with left mouse button. Do not mix this with <see cref="ContextMenu"/> which is
        /// shown when right mouse button is clicked.
        /// </summary>
        [Browsable(false)]
        public virtual ContextMenu? DropDownMenu { get; set; }

        /// <summary>
        /// Gets or sets the type of scroll bars displayed in the control.
        /// </summary>
        [Browsable(false)]
        public virtual RichTextBoxScrollBars ScrollBars
        {
            get
            {
                return scrollBars;
            }

            set
            {
                if (scrollBars == value)
                    return;
                scrollBars = value;

                var (horizontal, vertical) = ScrollBarUtils.AsHiddenOrVisible(value);
                VertScrollBarInfo = VertScrollBarInfo.WithVisibility(vertical);
                HorzScrollBarInfo = HorzScrollBarInfo.WithVisibility(horizontal);
            }
        }

        /// <summary>
        /// Gets or sets whether control wants to get all char/key events for all keys.
        /// </summary>
        /// <remarks>
        /// Use this to indicate that the control wants to get all char/key events for all keys
        /// - even for keys like TAB or ENTER which are usually used for dialog navigation and
        /// which wouldn't be generated without this style. If you need to use this style in
        /// order to get the arrows or etc., but would still like to have normal keyboard
        /// navigation take place, you should call Navigate in response to the key events
        /// for Tab and Shift-Tab.
        /// </remarks>
        [Browsable(false)]
        public virtual bool WantChars
        {
            get
            {
                return Handler.WantChars;
            }

            set
            {
                Handler.WantChars = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        [Browsable(true)]
        public override bool HasBorder
        {
            get
            {
                return hasBorder;
            }

            set
            {
                if (hasBorder == value)
                    return;
                hasBorder = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the drop down menu
        /// is shown when the control is clicked. Default is <see langword="true"/>.
        /// </summary>
        [Browsable(true)]
        public virtual bool ShowDropDownMenuWhenClicked
        {
            get => showDropDownMenuWhenClicked;

            set
            {
                showDropDownMenuWhenClicked = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of overlays associated with the control.
        /// </summary>
        /// <remarks>Setting this property updates the internal overlay collection
        /// and invalidates the control,
        /// prompting a redraw to reflect the changes.</remarks>
        [Browsable(false)]
        public virtual IReadOnlyList<IControlOverlay>? Overlays
        {
            get => overlays;
            set
            {
                overlays = value?.ToList();
                Invalidate();
            }
        }

        /// <summary>
        /// Sets <see cref="AbstractControl.StateObjects"/> colors and backgrounds
        /// for the state specified
        /// in the <paramref name="state"/> parameter to the
        /// colors from <paramref name="fontAndColor"/>.
        /// </summary>
        /// <param name="state">Affected control state.</param>
        /// <param name="fontAndColor">Colors.</param>
        public virtual void SetStateColors(
            VisualControlState state,
            IReadOnlyFontAndColor? fontAndColor)
        {
            if (fontAndColor is null && StateObjects?.Colors is null
                && StateObjects?.Backgrounds is null)
                return;
            StateObjects ??= new();
            StateObjects.Colors ??= new();
            StateObjects.Backgrounds ??= new();
            StateObjects.Colors.SetObject(fontAndColor, state);
            StateObjects.Backgrounds.SetObject(fontAndColor?.BackgroundColor?.AsBrush, state);
        }

        /// <summary>
        /// Gets whether this control has overlays.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasOverlays()
        {
            return overlays?.Count > 0;
        }

        /// <summary>
        /// Gets overlays attached to this control.
        /// </summary>
        /// <returns></returns>
        public virtual IReadOnlyList<IControlOverlay>? GetOverlays()
        {
            return overlays;
        }

        /// <summary>
        /// Removes an overlay with the specified unique identifier from the overlays collection.
        /// </summary>
        /// <param name="overlayId">The unique identifier of the overlay to remove.</param>
        /// <param name="invalidate">A value indicating whether to invalidate the control
        /// after removing the overlay. If <see langword="true"/>,
        /// the control is invalidated; otherwise, it is not.</param>
        /// <returns><see langword="true"/> if the overlay was successfully removed;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool RemoveOverlay(ObjectUniqueId? overlayId, bool invalidate = true)
        {
            if (overlays is null || overlayId is null)
                return false;

            for (int i = 0; i < overlays.Count; i++)
            {
                if (overlays[i].UniqueId == overlayId)
                {
                    overlays.RemoveAt(i);
                    if (overlays.Count == 0)
                        overlays = null;
                    if (invalidate)
                        Invalidate();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes overlays from the control that match the specified flags.
        /// </summary>
        /// <remarks>This method iterates through the collection of overlays and removes
        /// those that match the specified flags. If <paramref name="invalidate"/> is
        /// <see langword="true"/>, the control is invalidated
        /// to reflect the changes.</remarks>
        /// <param name="flags">The flags that determine which overlays to remove.</param>
        /// <param name="invalidate">A value indicating whether the control should
        /// be invalidated after removing the overlays. The default is
        /// <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if any overlays were removed;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool RemoveOverlays(ControlOverlayFlags flags, bool invalidate = true)
        {
            if (overlays is null)
                return false;

            for (int i = overlays.Count - 1; i >= 0; i--)
            {
                if ((overlays[i].Flags & flags) != 0)
                {
                    overlays.RemoveAt(i);
                }
            }

            if (invalidate)
                Invalidate();

            return true;
        }

        /// <summary>
        /// Removes the specified overlay from the control.
        /// </summary>
        /// <param name="overlay">The overlay to remove.</param>
        /// <param name="invalidate">The <see langword="bool"/> indicating whether
        /// to invalidate the control.</param>
        /// <returns><see langword="true"/> if the overlay was successfully removed;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool RemoveOverlay(IControlOverlay overlay, bool invalidate = true)
        {
            if (overlays is null)
                return false;
            var removed = overlays.Remove(overlay);
            if (!removed)
                return false;
            if (overlays.Count == 0)
                overlays = null;
            if (invalidate)
                Invalidate();
            return true;
        }

        /// <summary>
        /// Adds the specified overlay to the control.
        /// </summary>
        /// <param name="overlay">The overlay to add.</param>
        public virtual void AddOverlay(IControlOverlay overlay)
        {
            overlays ??= new();
            overlays.Add(overlay);
            Invalidate();
        }

        /// <inheritdoc/>
        public override Brush? GetBackground(VisualControlState state)
        {
            return HandleGetBackground(this, state);
        }

        /// <summary>
        /// Gets rectangle to which the overlay should be fitted.
        /// </summary>
        /// <returns>A <see cref="RectD"/> representing the overlay rectangle.
        /// This typically corresponds to the client
        /// rectangle of the object.</returns>
        public virtual RectD GetOverlayRectangle()
        {
            return ClientRectangle;
        }

        /// <summary>
        /// Displays an overlay tooltip with an error message based on the provided exception.
        /// </summary>
        /// <remarks>This method is a convenience wrapper for displaying error messages
        /// in an overlay tooltip. It uses the exception's message as the tooltip content
        /// and provides default values for optional
        /// parameters when not specified.</remarks>
        /// <param name="title">The title of the tooltip. If <see langword="null"/>,
        /// a default error title is used.</param>
        /// <param name="e">The exception message will be displayed in the tooltip.
        /// This parameter cannot be <see langword="null"/>.</param>
        /// <param name="alignment">The horizontal and vertical alignment of the tooltip
        /// relative to its target. If <see langword="null"/>, a
        /// default alignment is used.</param>
        /// <param name="options">Flags that control the behavior of the tooltip,
        /// such as whether it dismisses automatically after a set
        /// interval. The default value is
        /// <see cref="OverlayToolTipFlags.DismissAfterInterval"/>.</param>
        /// <param name="dismissIntervalMilliseconds">The time, in milliseconds, after
        /// which the tooltip will automatically dismiss if the <paramref name="options"/> include
        /// <see cref="OverlayToolTipFlags.DismissAfterInterval"/>.
        /// If <see langword="null"/>, a default interval is used.</param>
        /// <returns>An <see cref="ObjectUniqueId"/> that uniquely identifies the
        /// displayed tooltip.</returns>
        public virtual ObjectUniqueId ShowOverlayToolTipWithError(
            object? title,
            object e,
            HVAlignment? alignment = null,
            OverlayToolTipFlags options = OverlayToolTipFlags.DismissAfterInterval,
            int? dismissIntervalMilliseconds = null)
        {
            string msg;

            if(e is Exception exception)
            {
                msg = exception.Message;
            }
            else
            {
                msg = e.ToString();
            }

            return ShowOverlayToolTip(
                title ?? ErrorMessages.Default.ErrorTitle,
                msg,
                MessageBoxIcon.Error,
                alignment,
                options,
                dismissIntervalMilliseconds);
        }

        /// <summary>
        /// Displays a simple overlay tooltip with the specified
        /// message and optional customization parameters.
        /// </summary>
        /// <remarks>If both <paramref name="location"/> and <paramref name="alignment"/>
        /// are <see langword="null"/>, the tooltip defaults to being
        /// centered on the screen.</remarks>
        /// <param name="message">The content to display in the tooltip.
        /// If <see langword="null"/>, an empty string is displayed.</param>
        /// <param name="location">The optional screen location where the tooltip should appear.
        /// If <see langword="null"/>, the tooltip is
        /// aligned based on the specified <paramref name="alignment"/>.</param>
        /// <param name="alignment">The optional alignment of the tooltip
        /// when <paramref name="location"/> is <see langword="null"/>.
        /// Defaults to centered alignment if not specified.</param>
        /// <param name="options">Flags that control the behavior of the tooltip,
        /// such as whether it dismisses automatically or requires
        /// manual dismissal. The default value is
        /// <see cref="OverlayToolTipFlags.DismissAfterInterval"/>.</param>
        /// <param name="dismissIntervalMilliseconds">The optional time, in milliseconds,
        /// after which the tooltip is automatically dismissed. This parameter is
        /// only applicable if <paramref name="options"/> includes
        /// <see cref="OverlayToolTipFlags.DismissAfterInterval"/>.
        /// If <see langword="null"/>, a default interval is used.</param>
        /// <returns>An <see cref="ObjectUniqueId"/> representing the unique
        /// identifier of the displayed tooltip. This can be
        /// used to manage or dismiss the tooltip programmatically.</returns>
        public virtual ObjectUniqueId ShowOverlayToolTipSimple(
            object? message,
            PointD? location = null,
            HVAlignment? alignment = null,
            OverlayToolTipFlags options = OverlayToolTipFlags.DismissAfterInterval,
            int? dismissIntervalMilliseconds = null)
        {
            OverlayToolTipParams data = new()
            {
                Text = message?.ToString() ?? string.Empty,
                Options = options,
                DismissInterval = dismissIntervalMilliseconds,
            };

            if (location is null)
            {
                data.HorizontalAlignment = alignment?.Horizontal ?? HorizontalAlignment.Center;
                data.VerticalAlignment = alignment?.Vertical ?? VerticalAlignment.Center;
            }
            else
            {
                data.Location = location.Value;
            }

            return ShowOverlayToolTip(data);
        }

        /// <summary>
        /// Displays an overlay tooltip with the specified title, message, and options.
        /// </summary>
        /// <remarks>This method provides a convenient way to display
        /// an overlay tooltip with customizable
        /// content, appearance, and behavior. If more advanced customization is required,
        /// consider using the overload
        /// that accepts an <see cref="OverlayToolTipParams"/> object.</remarks>
        /// <param name="title">The title of the tooltip. If <see langword="null"/>,
        /// an empty string is used.</param>
        /// <param name="message">The message content of the tooltip.
        /// If <see langword="null"/>, an empty string is used.</param>
        /// <param name="icon">An optional icon to display in the tooltip.
        /// If <see langword="null"/>, no icon is shown.</param>
        /// <param name="alignment">An optional alignment specification for the tooltip.
        /// If <see langword="null"/>, the tooltip is centered
        /// horizontally and vertically.</param>
        /// <param name="options">A set of flags that control the behavior of the tooltip.
        /// The default is <see cref="OverlayToolTipFlags.DismissAfterInterval"/>.</param>
        /// <param name="dismissIntervalMilliseconds">An optional duration, in milliseconds,
        /// after which the tooltip is automatically dismissed. If <see langword="null"/>,
        /// the default interval is used.</param>
        /// <returns>An <see cref="ObjectUniqueId"/> that uniquely identifies
        /// the displayed tooltip. This can be used to manage
        /// or dismiss the tooltip programmatically.</returns>
        public virtual ObjectUniqueId ShowOverlayToolTip(
            object? title,
            object? message,
            MessageBoxIcon? icon = null,
            HVAlignment? alignment = null,
            OverlayToolTipFlags options = OverlayToolTipFlags.DismissAfterInterval,
            int? dismissIntervalMilliseconds = null)
        {
            OverlayToolTipParams data = new()
            {
                Title = title?.ToString() ?? string.Empty,
                Text = message?.ToString() ?? string.Empty,
                Icon = icon,
                HorizontalAlignment = alignment?.Horizontal ?? HorizontalAlignment.Center,
                VerticalAlignment = alignment?.Vertical ?? VerticalAlignment.Center,
                Options = options,
                DismissInterval = dismissIntervalMilliseconds,
            };

            return ShowOverlayToolTip(data);
        }

        /// <summary>
        /// Displays an overlay tooltip at the specified location with the provided
        /// content and removal interval.
        /// </summary>
        /// <param name="data">The content, styling and behavior parameters for the tooltip.</param>
        /// <returns>A unique identifier for the created overlay tooltip, which
        /// can be used to manage or remove it later.</returns>
        public virtual ObjectUniqueId ShowOverlayToolTip(OverlayToolTipParams data)
        {
            data.Font ??= RealFont;
            data.ContainerBounds ??= GetOverlayRectangle();
            var container = data.ContainerBounds.Value;

            data.MaxWidth ??= Math.Max(RichToolTip.DefaultMaxWidth ?? 0, container.Width);
            data.ScaleFactor = ScaleFactor;

            if (data.Options.HasFlag(OverlayToolTipFlags.UseSystemColors))
            {
                data.UsesSystemColors();
            }

            var overlay = new ControlOverlayWithToolTip()
            {
                ToolTip = data,
            };

            overlay.UpdateImage();

            var imageSize = overlay.Image?.SizeDip(this) ?? SizeD.Empty;

            var alignedRect = new RectD(data.Location ?? PointD.Empty, imageSize);

            if (imageSize.IsPositive && data.HasAlignment)
            {
                alignedRect = AlignUtils.AlignRectInRect(
                    alignedRect,
                    container,
                    data.HorizontalAlignment,
                    data.VerticalAlignment);
            }

            overlay.Location = alignedRect.Location;

            AddOverlay(overlay);

            if(data.DismissAfterInterval)
                overlay.SetRemovalTimer(data.DismissInterval, this);
            return overlay.UniqueId;
        }

        /// <inheritdoc/>
        public override PaintEventHandler? GetBackgroundAction(VisualControlState state)
        {
            return HandleGetBackgroundActions(this, state);
        }

        /// <summary>
        /// Default painting method of the <see cref="UserControl"/>
        /// and its descendants.
        /// </summary>
        /// <param name="e">Paint arguments.</param>
        public virtual void DefaultPaint(PaintEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override BorderSettings? GetBorderSettings(VisualControlState state)
        {
            return HandleGetBorderSettings(this, state);
        }

        internal static Brush? HandleGetBackground(
            AbstractControl control,
            VisualControlState state)
        {
            var overrideValue = control.Backgrounds?.GetObjectOrNormal(state);
            if (overrideValue is not null)
                return overrideValue;

            var result = control.GetDefaultTheme()?.DarkOrLight(control.IsDarkBackground);
            var brush = result?.Backgrounds?.GetObjectOrNormal(state);
            brush ??= control.BackgroundColor?.AsBrush;
            return brush;
        }

        internal static PaintEventHandler? HandleGetBackgroundActions(
            AbstractControl control,
            VisualControlState state)
        {
            var overrideValue = control.BackgroundActions?.GetObjectOrNormal(state);
            if (overrideValue is not null)
                return overrideValue;

            var theme = control.GetDefaultTheme()?.DarkOrLight(control.IsDarkBackground);
            var result = theme?.BackgroundActions?.GetObjectOrNormal(state);
            return result;
        }

        internal static BorderSettings? HandleGetBorderSettings(
            AbstractControl control,
            VisualControlState state)
        {
            var overrideValue = control.Borders?.GetObjectOrNull(state);
            if (overrideValue is not null)
                return overrideValue;

            var result = control.GetDefaultTheme()?.DarkOrLight(control.IsDarkBackground);
            return result?.Borders?.GetObjectOrNull(state);
        }

        internal static void HandleOnVisualStateChanged(
            AbstractControl control,
            ControlRefreshOptions refreshOptions)
        {
            var options = refreshOptions;

            if (options.HasFlag(ControlRefreshOptions.RefreshOnState))
            {
                control.Refresh();
                return;
            }

            var data = control.StateObjects;
            if (data is null)
                return;

            bool RefreshOnBorder() => options.HasFlag(ControlRefreshOptions.RefreshOnBorder) &&
                data.HasOtherBorders;
            bool RefreshOnImage() => options.HasFlag(ControlRefreshOptions.RefreshOnImage) &&
                data.HasOtherImages;
            bool RefreshOnColor() => options.HasFlag(ControlRefreshOptions.RefreshOnColor) &&
                data.HasOtherColors;
            bool RefreshOnBackground() => options.HasFlag(ControlRefreshOptions.RefreshOnBackground) &&
                data.HasOtherBackgrounds;

            if (RefreshOnBorder() || RefreshOnImage() || RefreshOnBackground()
                || RefreshOnColor())
                control.Refresh();
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!Enabled)
                return;
            RaiseClick(e);
            if(ShowDropDownMenuWhenClicked)
                ShowDropDownMenu();
            Invalidate();
        }

        /// <summary>
        /// Shows attached drop down menu.
        /// </summary>
        protected virtual void ShowDropDownMenu(Action? afterShow = null)
        {
            if (!Enabled)
                return;

            if(DropDownMenuShowing is not null)
            {
                var args = new BaseCancelEventArgs();
                DropDownMenuShowing(this, args);
                if (args.Cancel)
                    return;
            }

            DropDownMenu?.ShowAsDropDown(this, afterShow, DropDownMenuPosition);
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            DefaultPaint(e);

            if(overlays is not null)
            {
                foreach (var overlay in overlays)
                {
                    overlay.OnPaint(this, e);
                }
            }

            DefaultPaintDebug(e);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(!HasOverlays())
                return;
            switch (e.Key)
            {
                case Key.Escape:
                    RemoveOverlays(ControlOverlayFlags.RemoveOnEscape);
                    break;
                case Key.Enter:
                    RemoveOverlays(ControlOverlayFlags.RemoveOnEnter);
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void OnVisualStateChanged(EventArgs e)
        {
            base.OnVisualStateChanged(e);
            HandleOnVisualStateChanged(this, RefreshOptions);
        }

        /// <summary>
        /// Gets default value for <see cref="HasBorder"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual bool GetDefaultHasBorder() => true;

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            SystemSettings.ResetColors();
            Invalidate();
        }

        [Conditional("DEBUG")]
        private void DefaultPaintDebug(PaintEventArgs e)
        {
            if (DebugUtils.IsDebugDefined && ContainerControl.ShowDebugFocusRect)
            {
                if (FocusedControl?.Parent == this)
                {
                    e.Graphics.FillBorderRectangle(
                        FocusedControl.Bounds.Inflated(),
                        null,
                        BorderSettings.DebugBorder);
                }
            }
        }
    }
}
