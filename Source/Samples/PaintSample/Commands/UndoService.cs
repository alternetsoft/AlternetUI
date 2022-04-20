using System;
using System.Collections.Generic;

namespace PaintSample
{
    internal class UndoService
    {
        private Stack<Command> undoStack = new Stack<Command>();

        private Stack<Command> redoStack = new Stack<Command>();

        public event EventHandler? Changed;

        public bool CanUndo => undoStack.Count > 0;

        public bool CanRedo => redoStack.Count > 0;

        public void Do(Command command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();
            RaiseChanged();
        }

        public void Undo()
        {
            if (CanUndo)
                throw new InvalidOperationException();

            var command = undoStack.Pop();
            command.Rollback();
            redoStack.Push(command);
            RaiseChanged();
        }

        private void RaiseChanged() => Changed?.Invoke(this, EventArgs.Empty);
    }
}