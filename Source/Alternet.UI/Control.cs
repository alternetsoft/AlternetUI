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

        public Control()
        {
            Controls.ItemInserted += Controls_ItemInserted;
            Controls.ItemRemoved += Controls_ItemRemoved;

            Handler = CreateHandler();
        }

        public ControlHandler Handler { get; }

        public bool IsDisposed { get; private set; }

        public Collection<Control> Controls { get; } = new Collection<Control>();

        public Control? Parent { get; private set; }

        public void Update() => Handler.Update();

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

        protected virtual ControlHandler CreateHandler() => new NativeControlHandler(this);

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

        private void Controls_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = this;
        }

        private void Controls_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = null;
        }

        public event EventHandler<PaintEventArgs>? Paint;

        internal void InvokePaint(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnPaint(e);
        }

        public bool UserPaint { get; set; }

        protected virtual void OnPaint(PaintEventArgs e)
        {
            Paint?.Invoke(this, e);
        }
    }
}