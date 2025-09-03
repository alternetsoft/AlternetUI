using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dummy <see cref="IStatusBarHandler"/> interface handler which does nothing.
    /// </summary>
    public class PlessStatusBarHandler : DisposableObject, IStatusBarHandler
    {
        /// <inheritdoc/>
        public bool IsOk { get; }

        /// <inheritdoc/>
        public bool SizingGripVisible { get; set; }

        /// <inheritdoc/>
        public AbstractControl? AttachedTo { get; }

        /// <inheritdoc/>
        public TextEllipsisType TextEllipsis { get; set; }

        /// <inheritdoc/>
        public int GetBorderX()
        {
            return 0;
        }

        /// <inheritdoc/>
        public int GetBorderY()
        {
            return 0;
        }

        /// <inheritdoc/>
        public RectI GetFieldRect(int index)
        {
            return RectI.Empty;
        }

        /// <inheritdoc/>
        public int GetFieldsCount()
        {
            return 0;
        }

        /// <inheritdoc/>
        public StatusBarPanelStyle GetStatusStyle(int index)
        {
            return 0;
        }

        /// <inheritdoc/>
        public string GetStatusText(int index = 0)
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        public int GetStatusWidth(int index)
        {
            return 0;
        }

        /// <inheritdoc/>
        public bool PopStatusText(int index = 0)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool PushStatusText(string text, int index = 0)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SetFieldsCount(int count)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SetMinHeight(int height)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SetStatusStyles(StatusBarPanelStyle[] styles)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SetStatusText(string text, int index = 0)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool SetStatusWidths(int[] widths)
        {
            return false;
        }
    }
}
