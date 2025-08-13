using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

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
        /// Removes overlay with the specified <paramref name="overlayId"/>.
        /// </summary>
        /// <param name="overlayId">The unique identifier of the overlay to remove.</param>
        public virtual void RemoveOverlay(ObjectUniqueId overlayId)
        {
            if (overlays is null)
                return;

            for (int i = 0; i < overlays.Count; i++)
            {
                if (overlays[i].UniqueId == overlayId)
                {
                    overlays.RemoveAt(i);
                    if (overlays.Count == 0)
                        overlays = null;
                    return;
                }
            }
        }

        /// <summary>
        /// Removes the specified overlay from the control.
        /// </summary>
        /// <param name="overlay">The overlay to remove.</param>
        public virtual void RemoveOverlay(IControlOverlay overlay)
        {
            if (overlays is null)
                return;
            overlays.Remove(overlay);
            if (overlays.Count == 0)
                overlays = null;
        }

        /// <summary>
        /// Adds the specified overlay to the control.
        /// </summary>
        /// <param name="overlay">The overlay to add.</param>
        public virtual void AddOverlay(IControlOverlay overlay)
        {
            overlays ??= new();
            overlays.Add(overlay);
        }

        /// <inheritdoc/>
        public override Brush? GetBackground(VisualControlState state)
        {
            return HandleGetBackground(this, state);
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
        protected virtual void ShowDropDownMenu()
        {
            DropDownMenu?.ShowAsDropDown(this);
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
