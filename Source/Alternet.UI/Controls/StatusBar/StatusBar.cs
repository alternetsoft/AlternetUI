using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a status bar control.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public class StatusBar : NonVisualControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class.
        /// </summary>
        public StatusBar()
        {
            Panels.ItemInserted += Items_ItemInserted;
            Panels.ItemRemoved += Items_ItemRemoved;
        }

        /// <summary>
        /// Gets a collection of <see cref="StatusBarPanel"/> objects associated with the status bar.
        /// </summary>
        [Content]
        public Collection<StatusBarPanel> Panels { get; } = new() { ThrowOnNullAdd = true };

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.StatusBar;

        /// <summary>
        /// Gets a <see cref="StatusBarHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public new StatusBarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (StatusBarHandler)base.Handler;
            }
        }

        public bool IgnorePanels { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a sizing grip is displayed in the
        /// lower-right corner of the control.
        /// </summary>
        public bool SizingGripVisible
        {
            get => Handler.SizingGripVisible;
            set => Handler.SizingGripVisible = value;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements => Panels;

        public bool IsOk => StatusBarHandle != IntPtr.Zero;

        internal IntPtr StatusBarHandle => (Handler.NativeControl as Native.StatusBar).RealHandle;

        internal override bool IsDummy => true;

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection => Panels;

        /// <inheritdoc/>
        public override void BeginUpdate()
        {
            base.BeginUpdate();
        }

        /// <inheritdoc/>
        public override void EndUpdate()
        {
            ApplyPanels();
            base.EndUpdate();
        }

        internal void ApplyPanels()
        {
            if (InUpdates || IgnorePanels)
                return;
            var count = Panels.Count;
            var widths = new int[count];
            var styles = new StatusBarPanelStyle[count];

            for(int i = 0; i < count; i++)
            {
                widths[i] = Panels[i].Width;
                styles[i] = Panels[i].Style;
            }

            if(count == 0)
            {
                SetFieldsCount(1);
                SetStatusText(null);
                return;
            }

            SetFieldsCount(count);
            SetStatusWidths(widths);
            SetStatusStyles(styles);

            for (int i = 0; i < count; i++)
            {
                SetStatusText(Panels[i].Text, i);
            }
        }

        public int? GetFieldsCount()
        {
            if (StatusBarHandle == IntPtr.Zero)
                return null;
            return Native.WxStatusBarFactory.GetFieldsCount(StatusBarHandle);
        }

        public bool SetStatusText(string? text = null, int number = 0)
        {
            if (StatusBarHandle == IntPtr.Zero)
                return false;
            text ??= string.Empty;
            Native.WxStatusBarFactory.SetStatusText(StatusBarHandle, text, number);
            return true;
        }

        public string? GetStatusText(int number = 0)
        {
            if (StatusBarHandle == IntPtr.Zero)
                return null;
            return Native.WxStatusBarFactory.GetStatusText(StatusBarHandle, number);
        }

        public bool PushStatusText(string? text = null, int number = 0)
        {
            if (StatusBarHandle == IntPtr.Zero)
                return false;
            text ??= string.Empty;
            Native.WxStatusBarFactory.PushStatusText(StatusBarHandle, text, number);
            return true;
        }

        public bool PopStatusText(int number = 0)
        {
            if (StatusBarHandle == IntPtr.Zero)
                return false;
            Native.WxStatusBarFactory.PopStatusText(StatusBarHandle, number);
            return true;
        }

        public bool SetStatusWidths(int[] widths)
        {
            if (StatusBarHandle == IntPtr.Zero || widths.Length == 0)
                return false;
            Native.WxStatusBarFactory.SetStatusWidths(StatusBarHandle, widths);
            return true;
        }

        public bool SetFieldsCount(int number)
        {
            if (StatusBarHandle == IntPtr.Zero)
                return false;
            Native.WxStatusBarFactory.SetFieldsCount(StatusBarHandle, number);
            return true;
        }

        public int? GetStatusWidth(int n)
        {
            if (StatusBarHandle == IntPtr.Zero)
                return null;
            return Native.WxStatusBarFactory.GetStatusWidth(StatusBarHandle, n);
        }

        public StatusBarPanelStyle? GetStatusStyle(int n)
        {
            if (StatusBarHandle == IntPtr.Zero)
                return null;
            return (StatusBarPanelStyle)Native.WxStatusBarFactory.GetStatusStyle(StatusBarHandle, n);
        }

        public bool SetStatusStyles(StatusBarPanelStyle[] styles)
        {
            if (StatusBarHandle == IntPtr.Zero || styles.Length == 0)
                return false;
            var result = styles.Cast<int>().ToArray();
            Native.WxStatusBarFactory.SetStatusStyles(StatusBarHandle, result);
            return true;
        }

        public Int32Rect? GetFieldRect(int i)
        {
            if (StatusBarHandle == IntPtr.Zero)
                return null;
            return Native.WxStatusBarFactory.GetFieldRect(StatusBarHandle, i);
        }

        public bool SetMinHeight(int height)
        {
            if (StatusBarHandle == IntPtr.Zero)
                return false;
            Native.WxStatusBarFactory.SetMinHeight(StatusBarHandle, height);
            return true;
        }

        public int? GetBorderX()
        {
            if (StatusBarHandle == IntPtr.Zero)
                return null;
            return Native.WxStatusBarFactory.GetBorderX(StatusBarHandle);
        }

        public int? GetBorderY()
        {
            if (StatusBarHandle == IntPtr.Zero)
                return null;
            return Native.WxStatusBarFactory.GetBorderY(StatusBarHandle);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateStatusBarHandler(this);
        }

        private void Items_ItemInserted(object? sender, int index, StatusBarPanel item)
        {
            item.PropertyChanged += Item_PropertyChanged;
            ApplyPanels();
        }

        private void Item_PropertyChanged(object sender, EventArgs e)
        {
            ApplyPanels();
        }

        private void Items_ItemRemoved(object? sender, int index, StatusBarPanel item)
        {
            item.PropertyChanged -= Item_PropertyChanged;
            ApplyPanels();
        }
    }
}