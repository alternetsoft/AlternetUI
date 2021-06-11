using System;
using System.ComponentModel;
using System.Drawing;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Control : Component, ISupportInitialize
    {
        private float width = float.NaN;

        private float height = float.NaN;
        private Thickness margin;
        private Thickness padding;
        private ControlHandler? handler;
        private Color? backgroundColor;
        private Color? foregroundColor;
        private Color? borderColor;

        private bool visible = true;

        //
        public Control()
        {
            Children.ItemInserted += Children_ItemInserted;
            Children.ItemRemoved += Children_ItemRemoved;
        }

        public event EventHandler? BorderColorChanged;

        public event EventHandler<PaintEventArgs>? Paint;

        public event EventHandler? MarginChanged;

        public event EventHandler? PaddingChanged;

        public event EventHandler? VisibleChanged;

        public event EventHandler? BackgroundColorChanged;

        public event EventHandler? ForegroundColorChanged;

        public string? Name { get; set; } // todo: maybe use Site.Name?

        public bool Visible
        {
            get => visible;

            set
            {
                if (visible == value)
                    return;

                visible = value;
                OnVisibleChanged();
                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value)
                    return;

                borderColor = value;
                BorderColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public ControlHandler Handler
        {
            get
            {
                EnsureHandlerCreated();
                return handler ?? throw new InvalidOperationException();
            }
        }

        public bool IsDisposed { get; private set; }

        [Content]
        public Collection<Control> Children { get; } = new Collection<Control>();

        public Control? Parent { get; internal set; }

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

        public Color? BackgroundColor
        {
            // todo: change to brush?
            get => backgroundColor;
            set
            {
                if (backgroundColor == value)
                    return;

                backgroundColor = value;
                BackgroundColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Color? ForegroundColor
        {
            // todo: change to brush?
            get => foregroundColor;
            set
            {
                if (foregroundColor == value)
                    return;

                foregroundColor = value;
                ForegroundColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private IControlHandlerFactory? ControlHandlerFactory { get; set; }

        public Control? TryFindControl(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (Name == name)
                return this;

            foreach (var child in Children)
            {
                var result = child.TryFindControl(name);
                if (result != null)
                    return result;
            }

            return null;
        }

        public void Show() => Visible = true;

        public void Hide() => Visible = false;

        public Control FindControl(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return TryFindControl(name) ?? throw new InvalidOperationException();
        }

        public DrawingContext CreateDrawingContext() => Handler.CreateDrawingContext();

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

        public virtual SizeF GetPreferredSize(SizeF availableSize)
        {
            return Handler.GetPreferredSize(availableSize);
        }

        public void BeginInit()
        {
            SuspendLayout();
        }

        public void EndInit()
        {
            ResumeLayout();
        }

        internal void InvokePaint(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnPaint(e);
        }

        protected internal void EnsureHandlerCreated()
        {
            if (handler == null)
            {
                CreateAndAttachHandler();
            }
        }

        protected internal void DetachHandler()
        {
            if (handler == null)
                throw new InvalidOperationException();
            handler.Detach();
            handler = null;
        }

        private protected void SetVisibleValue(bool value) => visible = value;

        protected virtual void OnVisibleChanged()
        {
        }

        protected void RecreateHandler()
        {
            if (handler != null)
                OnDetachHandler();

            Update();
        }

        protected virtual void OnDetachHandler()
        {
            DetachHandler();
        }

        protected IControlHandlerFactory GetEffectiveControlHandlerHactory() => ControlHandlerFactory ?? Application.Current.VisualTheme.ControlHandlerFactory;

        protected virtual ControlHandler CreateHandler() => GetEffectiveControlHandlerHactory().CreateControlHandler(this);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                if (disposing)
                {
                    foreach (var child in Handler.AllChildren)
                        child.Dispose();

                    Children.Clear();
                    Handler.VisualChildren.Clear();

                    DetachHandler();
                }

                IsDisposed = true;
            }
        }

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

        protected virtual void OnAttachHandler()
        {
            if (handler == null)
                throw new InvalidOperationException();

            handler.Attach(this);
        }

        private void CreateAndAttachHandler()
        {
            handler = CreateHandler();
            OnAttachHandler();
        }

        private void Children_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = this;
        }

        private void Children_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = null;
        }
    }
}