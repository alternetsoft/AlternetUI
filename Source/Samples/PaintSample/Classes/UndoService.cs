using Alternet.Drawing;
using System;

namespace PaintSample
{
    public class UndoService
    {
        private PaintSampleDocument? document;

        const int StackSize = 20;

        private readonly DropoutStack<Bitmap> undoStack = new(StackSize);

        private readonly DropoutStack<Bitmap> redoStack = new(StackSize);

        public event EventHandler? Changed;

        public bool CanUndo => undoStack.Count > 0;

        public bool CanRedo => redoStack.Count > 0;

        public PaintSampleDocument? Document
        {
            get => document;
            set
            {
                if (document == value)
                    return;

                document = value;

                undoStack.Clear();
                redoStack.Clear();

                RaiseChanged();
            }
        }

        public void Do(Action action)
        {
            if (document == null)
                throw new InvalidOperationException();

            var bitmapBeforeAction = new Bitmap(document.Bitmap);
            action();
            undoStack.Push(bitmapBeforeAction);
            redoStack.Clear();
            RaiseChanged();
        }

        public void Undo()
        {
            if (document == null)
                throw new InvalidOperationException();

            if (!CanUndo)
                throw new InvalidOperationException();

            var bitmapBeforeUndo = new Bitmap(document.Bitmap);
            var bitmap = undoStack.Pop();
            document.Bitmap = bitmap;
            redoStack.Push(bitmapBeforeUndo);
            RaiseChanged();
        }

        public void Redo()
        {
            if (document == null)
                throw new InvalidOperationException();

            if (!CanRedo)
                throw new InvalidOperationException();

            var bitmapBeforeRedo = new Bitmap(document.Bitmap);
            var bitmap = redoStack.Pop();
            document.Bitmap = bitmap;
            undoStack.Push(bitmapBeforeRedo);
            RaiseChanged();
        }

        private void RaiseChanged() => Changed?.Invoke(this, EventArgs.Empty);
    }
}