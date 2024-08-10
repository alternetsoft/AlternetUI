using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with custom console.
    /// </summary>
    public interface ICustomConsole : IDisposable
    {
        /// <summary>
        /// Gets or sets background color of the new text.
        /// </summary>
        ConsoleColor BackColor { get; set; }

        /// <summary>
        /// Gets or sets foreground color of the new text.
        /// </summary>
        ConsoleColor TextColor { get; set; }

        /// <summary>
        /// Gets console width.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets console height.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Begins console updates.
        /// </summary>
        void BeginUpdate();

        /// <summary>
        /// Ends console updates.
        /// </summary>
        void EndUpdate();

        /// <summary>
        /// Clears console.
        /// </summary>
        void Clear();

        /// <summary>
        /// Fills rectangle region with the character.
        /// </summary>
        /// <param name="rect">Rectangle to fill.</param>
        /// <param name="c">Fill character.</param>
        void Fill(RectI rect, char c = '\0');

        /// <summary>
        /// Writes an empty line to console. Caret position is changed to the beginning of the next line.
        /// </summary>
        void WriteLine();

        /// <summary>
        /// Writes text to console at the current caret position.
        /// After write operation is performed, caret is moved
        /// to the beginning of the next line.
        /// </summary>
        /// <param name="text">Text to write.</param>
        void WriteLine(string text);

        /// <summary>
        /// Writes text to console at the current caret position.
        /// After write operation is performed, caret is moved to the position after the text.
        /// </summary>
        /// <param name="text">Text to write.</param>
        void Write(string text);

        /// <summary>
        /// Writes text at the specified position. Caret position is not changed.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="text">Text to write.</param>
        void Print(int x, int y, string text);

        /// <summary>
        /// Writes character at the specified position. Caret position is not changed.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="c">Character to write.</param>
        void SetChar(int x, int y, char c);

        /// <summary>
        /// Gets character at the specified position.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns></returns>
        char GetChar(int x, int y);
    }
}