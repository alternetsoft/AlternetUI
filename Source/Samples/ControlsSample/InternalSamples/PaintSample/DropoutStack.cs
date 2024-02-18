#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintSample
{
    internal class DropoutStack<T>
    {
        private Stack<T> stack = new();

        public DropoutStack(int maxCount)
        {
            MaxCount = maxCount;
        }

        public int Count => stack.Count;

        public int MaxCount { get; }

        public void Push(T value)
        {
            stack.Push(value);
            if (stack.Count > MaxCount)
            {
                var oldStack = stack;
                stack = new Stack<T>();
                foreach (var item in oldStack.Reverse().Skip(1))
                    stack.Push(item);

                var overflowItem = oldStack.Last();
                if (overflowItem is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        public T Pop()
        {
            return stack.Pop();
        }

        public T Peek()
        {
            return stack.Peek();
        }

        public void Clear()
        {
            while (Count > 0)
            {
                var item = Pop();
                if (item is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}