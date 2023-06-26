using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal interface IControl : ISupportInitialize, IDisposable // FrameworkElement
    {
        event EventHandler? Click;

        event EventHandler? BorderBrushChanged;

        event EventHandler<PaintEventArgs>? Paint; // !!!!!

        event EventHandler? MarginChanged;

        event EventHandler? PaddingChanged;

        event EventHandler? VisibleChanged;

        event EventHandler? MouseCaptureLost;

        event EventHandler? MouseEnter;

        event EventHandler? ToolTipChanged;

        event EventHandler? MouseLeave;

        event EventHandler? EnabledChanged;

        event EventHandler? BackgroundChanged;

        event EventHandler? ForegroundChanged;

        event EventHandler? FontChanged;

        event EventHandler? VerticalAlignmentChanged;

        event EventHandler? HorizontalAlignmentChanged;

        event EventHandler? SizeChanged;

        event EventHandler? LocationChanged;

        event EventHandler<DragEventArgs>? DragDrop; // !!!!!

        event EventHandler<DragEventArgs>? DragOver; // !!!!!

        event EventHandler<DragEventArgs>? DragEnter; // !!!!!

        event EventHandler? DragLeave;

        Size ClientSize { get; set; }

        bool IsMouseCaptured { get; }

        object? Tag { get; set; }

        Rect Bounds { get; set; }

        Point Location { get; set; }

        bool Visible { get; set; }

        bool Enabled { get; set; }

        Brush? BorderBrush { get; set; }

        public ControlHandler Handler { get; }

        bool IsDisposed { get; }

        Collection<Control> Children { get; } // !!!!!

        IReadOnlyList<FrameworkElement> ContentElements { get; } // !!!!!

        public Control? Parent { get; } // !!!!!

        Size Size { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        bool UserPaint { get; set; }

        Thickness Margin { get; set; }

        Thickness Padding { get; set; }

        Brush? Background { get; set; } // !!!!!

        Brush? Foreground { get; set; } // !!!!!

        Font? Font { get; set; } // !!!!!

        bool TabStop { get; set; }

        bool IsFocused { get; }

        string? ToolTip { get; set; }

        VerticalAlignment VerticalAlignment { get; set; }

        HorizontalAlignment HorizontalAlignment { get; set; }

        public bool AllowDrop { get; set; }

        IAsyncResult BeginInvoke(Delegate method, object?[] args);

        IAsyncResult BeginInvoke(Delegate method);

        IAsyncResult BeginInvoke(Action action);

        object? EndInvoke(IAsyncResult result);

        object? Invoke(Delegate method, object?[] args);

        object? Invoke(Delegate method);

        void Invoke(Action action);

        void CaptureMouse();

        void ReleaseMouseCapture();

        void RaiseClick(EventArgs e);

        void Show();

        void Hide();

        DrawingContext CreateDrawingContext();

        void Invalidate();

        void Update();

        void Refresh();

        void SuspendLayout();

        public Point ScreenToClient(Point point);

        public Point ClientToScreen(Point point);

        public Int32Point ScreenToDevice(Point point);

        public Point DeviceToScreen(Int32Point point);

        void ResumeLayout(bool performLayout = true);

        void BeginUpdate();

        void EndUpdate();

        void PerformLayout();

        Size GetPreferredSize(Size availableSize);

        bool SetFocus();

        void FocusNextControl(bool forward = true, bool nested = true);

        DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects);

    }

    // static Control? GetFocusedControl();
    // static Font DefaultFont        {            get;        }
    internal interface IControlProtected
    {
        IEnumerable<FrameworkElement> LogicalChildrenCollection { get; } // !!!!!

        void OnEnabledChanged(EventArgs e);

        void OnToolTipChanged(EventArgs e);

        void EnsureHandlerCreated();

        void DetachHandler();

        void OnSizeChanged(EventArgs e);

        void OnLocationChanged(EventArgs e);

        void OnLayout();

        void OnChildInserted(int childIndex, Control childControl); // !!!!!

        void OnChildRemoved(int childIndex, Control childControl); // !!!!!

        void OnClick(EventArgs e);

        void OnVisibleChanged(EventArgs e);

        void OnMouseCaptureLost();

        void OnMouseEnter();

        void OnMouseLeave();

        bool GetIsEnabled();

        void RecreateHandler();

        void OnHandlerDetaching(EventArgs e);

        IControlHandlerFactory GetEffectiveControlHandlerHactory();

        ControlHandler CreateHandler(); // !!!!!

        void CheckDisposed();

        void OnMarginChanged(EventArgs e);

        void OnPaddingChanged(EventArgs e);

        void OnPaint(PaintEventArgs e); // !!!!!

        void OnHandlerAttached(EventArgs e);

        void OnDragDrop(DragEventArgs e); // !!!!!

        void OnDragOver(DragEventArgs e);

        void OnDragEnter(DragEventArgs e);

        void OnDragLeave(EventArgs e);
    }
}