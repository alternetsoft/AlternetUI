using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dummy user interface action simulator which does nothing.
    /// </summary>
    public class DummyActionSimulatorHandler : DisposableObject, IActionSimulatorHandler
    {
        /// <inheritdoc/>
        public bool SendChar(Key keyCode, RawModifierKeys modifiers = RawModifierKeys.None)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SendKeyDown(Key keyCode, RawModifierKeys modifiers = RawModifierKeys.None)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SendKeyUp(Key keyCode, RawModifierKeys modifiers = RawModifierKeys.None)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SendMouseClick(MouseButton button = MouseButton.Left)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SendMouseDblClick(MouseButton button = MouseButton.Left)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SendMouseDown(MouseButton button = MouseButton.Left)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SendMouseDragDrop(int x1, int y1, int x2, int y2, MouseButton button = MouseButton.Left)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SendMouseMove(PointI point)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SendMouseUp(MouseButton button = MouseButton.Left)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SendSelect(string text)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SendText(string text)
        {
            return false;
        }
    }
}
