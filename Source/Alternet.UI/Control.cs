using System;
using System.ComponentModel;
using System.Drawing;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Control : Component
    {
        private float width = float.NaN;

        private float height = float.NaN;
        private Thickness margin;

        public Control()
        {
            Controls.ItemInserted += Controls_ItemInserted;
            Controls.ItemRemoved += Controls_ItemRemoved;

            Handler = CreateHandler();
        }

        public event EventHandler<PaintEventArgs>? Paint;

        public event EventHandler? MarginChanged;

        public ControlHandler Handler { get; }

        public bool IsDisposed { get; private set; }

        public Collection<Control> Controls { get; } = new Collection<Control>();

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

        protected virtual void OnPaint(PaintEventArgs e)
        {
            Paint?.Invoke(this, e);
        }

        private void Controls_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = this;
        }

        private void Controls_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = null;
        }
    }
}