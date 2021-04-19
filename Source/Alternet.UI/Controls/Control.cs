using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Control : Component
    {
        private float width = float.NaN;

        private float height = float.NaN;
        private Thickness margin;
        private Thickness padding;
        private ControlHandler? handler;

        public Control()
        {
            Children.ItemInserted += Children_ItemInserted;
            VisualChildren.ItemInserted += VisualChildren_ItemInserted;

            Children.ItemRemoved += Children_ItemRemoved;
            VisualChildren.ItemRemoved += VisualChildren_ItemRemoved;

            CreateAndAttachHandler();
        }

        public DrawingContext CreateDrawingContext() => Handler.CreateDrawingContext();

        private void CreateAndAttachHandler()
        {
            handler = CreateHandler();
            handler.Attach(this);
        }

        public event EventHandler<PaintEventArgs>? Paint;

        public event EventHandler? MarginChanged;

        public event EventHandler? PaddingChanged;

        public ControlHandler Handler
        { 
            get => handler ?? throw new InvalidOperationException();
        }

        public bool IsDisposed { get; private set; }

        public Collection<Control> Children { get; } = new Collection<Control>();

        public Collection<Control> VisualChildren { get; } = new Collection<Control>();

        public IEnumerable<Control> AllChildren => VisualChildren.Concat(Children);

        public Control? Parent { get; private set; }

        public virtual float Width
        {
            get => width;
            set
            {
                if (width == value)
                    return;

                width = value;
            }
        }

        public virtual float Height
        {
            get => height;
            set
            {
                if (height == value)
                    return;

                height = value;
            }
        }

        public bool UserPaint { get; set; }

        public Thickness Margin
        {
            get => margin;
            set
            {
                if (margin == value)
                    return;

                margin = value;

                OnMarginChanged(EventArgs.Empty);
                MarginChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Thickness Padding
        {
            get => padding;
            set
            {
                if (padding == value)
                    return;

                padding = value;

                OnPaddingChanged(EventArgs.Empty);
                PaddingChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Update() => Handler.Update();

        public void SuspendLayout()
        {
            Handler.SuspendLayout();
        }

        public void ResumeLayout(bool performLayout = true)
        {
            Handler.ResumeLayout();
            if (performLayout)
                PerformLayout();
        }

        public void PerformLayout()
        {
            Handler.PerformLayout();
        }

        public SizeF GetPreferredSize(SizeF availableSize)
        {
            return Handler.GetPreferredSize(availableSize);
        }

        internal void InvokePaint(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnPaint(e);
        }

        IControlHandlerFactory? ControlHandlerFactory { get; set; }

        protected IControlHandlerFactory GetEffectiveControlHandlerHactory() => ControlHandlerFactory ?? Application.Current.VisualTheme.ControlHandlerFactory;

        protected virtual ControlHandler CreateHandler() => GetEffectiveControlHandlerHactory().CreateControlHandler(this);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                if (disposing)
                {
                    foreach (var child in AllChildren)
                        child.Dispose();
                    Children.Clear();
                    VisualChildren.Clear();

                    if (handler == null)
                        throw new InvalidOperationException();
                    var nativeControl = handler.NativeControl; // todo
                    handler.Detach();
                    nativeControl?.Dispose();
                    handler = null;
                }

                IsDisposed = true;
            }
        }

        public bool IsVisualChild { get; private set; }

        protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        protected virtual void OnMarginChanged(EventArgs e)
        {
        }

        protected virtual void OnPaddingChanged(EventArgs e)
        {
        }

        protected virtual void OnPaint(PaintEventArgs e)
        {
            Paint?.Invoke(this, e);
        }

        private void Children_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = this;
        }

        private void Children_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = null;
        }

        private void VisualChildren_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = this;
            e.Item.IsVisualChild = true;
        }

        private void VisualChildren_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = null;
            e.Item.IsVisualChild = false;
        }
    }
}