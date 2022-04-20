using System;

namespace PaintSample
{
    internal class Command
    {
        public Command(Action execute, Action rollback)
        {
            Execute = execute;
            Rollback = rollback;
        }

        public Action Execute { get; }

        public Action Rollback { get; }
    }
}