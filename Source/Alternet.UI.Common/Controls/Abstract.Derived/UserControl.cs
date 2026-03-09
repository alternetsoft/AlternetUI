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

        static UserControl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public UserControl(AbstractControl parent)
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
        /// Occurs after the control has finished its painting operation.
        /// </summary>
        /// <remarks>This event is raised after the control's painting logic has been completed.
        /// It can be used to perform additional custom drawing or to respond to the completion
        /// of the painting process.</remarks>
        public event PaintEventHandler? AfterPaint;

        /// <summary>
        /// Gets or sets different behavior and visualization options.
        /// </summary>
        [Browsable(false)]
        public virtual ControlRefreshOptions RefreshOptions { get; set; }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.UserPaintControl;

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

        /// <inheritdoc/>
        public override Brush? GetBackground(VisualControlState state)
        {
            return UserControlHelper.GetBackground(this, state);
        }

        /// <inheritdoc/>
        public override PaintEventHandler? GetBackgroundAction(VisualControlState state)
        {
            return UserControlHelper.GetBackgroundActions(this, state);
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
            return UserControlHelper.GetBorderSettings(this, state);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            HandleClickTrigger(ClickTriggerKind.MouseDown, e);
            if (!Enabled)
                return;
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonUp(MouseEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            HandleClickTrigger(ClickTriggerKind.MouseUp, e);
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            DefaultPaint(e);

            DefaultPaintDebug(e);

            RaiseAfterPaint(e);
        }

        /// <inheritdoc/>
        protected override void OnAfterPaintChildren(PaintEventArgs e)
        {
            base.OnAfterPaintChildren(e);

            PaintOverlays(e);
        }

        /// <summary>
        /// Raises the <see cref="AfterPaint"/> event and invokes the <c>OnAfterPaint</c> method.
        /// </summary>
        /// <remarks>This method is called to notify subscribers that the painting operation
        /// has completed. It first calls the <c>OnAfterPaint</c> method,
        /// allowing derived classes to handle the event, and then raises
        /// the <see cref="AfterPaint"/> event.</remarks>
        /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
        protected void RaiseAfterPaint(PaintEventArgs e)
        {
            OnAfterPaint(e);
            AfterPaint?.Invoke(this, e);
        }

        /// <summary>
        /// Invoked after the control has completed its painting operations.
        /// </summary>
        /// <remarks>This method is called to allow for additional processing or custom
        /// behavior after the control's painting is complete.
        /// Derived classes can override this method to implement custom post-painting logic.</remarks>
        /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data,
        /// including the graphics context used for painting.</param>
        protected virtual void OnAfterPaint(PaintEventArgs e)
        {
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
            UserControlHelper.OnVisualStateChanged(this, RefreshOptions);
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
            Invalidate();
        }

        /// <summary>
        /// Provides default debug related painting behavior for the control when the application
        /// is compiled in debug mode.
        /// </summary>
        /// <remarks>This method is only executed when the "DEBUG" conditional compilation
        /// symbol is defined. It is intended for rendering debug-specific visual elements,
        /// such as focus rectangles, to assist in debugging layout or focus-related issues.</remarks>
        /// <param name="e">The <see cref="PaintEventArgs"/> containing data for the paint event.</param>
        [Conditional("DEBUG")]
        protected virtual void DefaultPaintDebug(PaintEventArgs e)
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

        /// <inheritdoc/>
        protected override HandlerType GetRequiredHandlerType()
        {
            return base.GetRequiredHandlerType();
        }
    }
}
