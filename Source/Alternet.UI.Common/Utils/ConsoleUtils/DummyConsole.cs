using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class DummyConsole : DisposableObject, ICustomConsole
    {
        public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;

        public int Width { get; } = 80;

        public int Height { get; } = 25;

        public void BeginUpdate()
        {
        }

        public void Clear()
        {
        }

        public void EndUpdate()
        {
        }

        public void Fill(RectI rect, char c = '\0')
        {
        }

        public char GetChar(int x, int y)
        {
            return (char)0;
        }

        public void Print(int x, int y, string text)
        {
        }

        public void SetChar(int x, int y, char c)
        {
        }

        public void Write(string text)
        {
        }

        public void WriteLine()
        {
        }

        public void WriteLine(string text)
        {
        }
    }
}
