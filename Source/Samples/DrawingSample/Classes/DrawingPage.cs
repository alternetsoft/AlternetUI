using Alternet.UI;
using Alternet.Drawing;
using System;

namespace DrawingSample
{
    internal abstract class DrawingPage
    {
        private AbstractControl? settingsControl;
        private AbstractControl? canvas;

        public AbstractControl? Canvas
        {
            get => canvas;

            set
            {
                if (canvas == value)
                    return;

                if (canvas != null)
                    canvas.Paint -= Canvas_Paint;

                var oldValue = canvas;
                canvas = value;

                if (canvas != null)
                    canvas.Paint += Canvas_Paint;

                OnCanvasChanged(oldValue, value);
            }
        }

        protected virtual void OnCanvasChanged(AbstractControl? oldValue, AbstractControl? value)
        {
        }

        public AbstractControl SettingsControl => settingsControl ??= CreateSettingsControl();

        public abstract string Name { get; }

        public abstract void Draw(Graphics dc, RectD bounds);

        protected abstract AbstractControl CreateSettingsControl();

        public virtual void OnActivated()
        {
        }

        public virtual void OnDeactivated()
        {
        }

        private void Canvas_Paint(object? sender, PaintEventArgs e)
        {
            var b = e.ClientRectangle;
            e.Graphics.FillRectangle(Color.White.AsBrush, b);
            Draw(e.Graphics, b);
        }
    }
}