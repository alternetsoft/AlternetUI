using Alternet.UI;
using System;

namespace PaintSample
{
    public abstract class Tool
    {
        private AbstractControl? canvas;

        private AbstractControl? optionsControl;
        private readonly Func<PaintSampleDocument> getDocument;

        protected Tool(Func<PaintSampleDocument> getDocument, ISelectedColors selectedColors, UndoService undoService)
        {
            this.getDocument = getDocument;
            SelectedColors = selectedColors;
            UndoService = undoService;
        }

        public AbstractControl? OptionsControl => optionsControl ??= CreateOptionsControl();

        public abstract string Name { get; }

        protected PaintSampleDocument Document => getDocument();

        protected ISelectedColors SelectedColors { get; }

        protected UndoService UndoService { get; }

        protected AbstractControl Canvas { get => canvas ?? throw new InvalidOperationException(); private set => canvas = value; }

        public void Activate(AbstractControl canvas)
        {
            if (this.canvas != null)
                throw new InvalidOperationException();

            this.canvas = canvas;
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseCaptureLost += Canvas_MouseCaptureLost;
            canvas.KeyDown += Canvas_KeyDown;
        }

        public void Deactivate()
        {
            if (canvas == null)
                throw new InvalidOperationException();

            canvas.MouseDown -= Canvas_MouseDown;
            canvas.MouseUp -= Canvas_MouseUp;
            canvas.MouseMove -= Canvas_MouseMove;
            canvas.MouseCaptureLost -= Canvas_MouseCaptureLost;
            canvas.KeyDown -= Canvas_KeyDown;

            canvas = null;
        }

        protected virtual AbstractControl? CreateOptionsControl() => null;

        protected virtual void OnKeyDown(KeyEventArgs e)
        {
        }

        protected virtual void OnMouseCaptureLost()
        {
        }

        protected virtual void OnMouseMove(MouseEventArgs e)
        {
        }

        protected virtual void OnMouseUp(MouseEventArgs e)
        {
        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {
        }

        private void Canvas_KeyDown(object? sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void Canvas_MouseCaptureLost(object? sender, EventArgs e)
        {
            OnMouseCaptureLost();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(e);
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }
    }
}