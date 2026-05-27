using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with status bar control.
    /// </summary>
    public interface IStatusBarHandler : IDisposable
    {
        /// <inheritdoc cref="StatusBar.IsOk"/>
        bool IsOk { get; }

        /// <inheritdoc cref="StatusBar.SizingGripVisible"/>
        bool SizingGripVisible { get; set; }

        /// <inheritdoc cref="StatusBar.AttachedTo"/>
        AbstractControl? AttachedTo { get; }

        /// <inheritdoc cref="StatusBar.TextEllipsis"/>
        TextEllipsisType TextEllipsis { get; set; }

        /// <inheritdoc cref="StatusBar.GetFieldsCount"/>
        int GetFieldsCount();

        /// <inheritdoc cref="StatusBar.GetBorderX"/>
        int GetBorderX();

        /// <inheritdoc cref="StatusBar.GetBorderY"/>
        int GetBorderY();

        /// <inheritdoc cref="StatusBar.SetStatusText"/>
        bool SetStatusText(string text, int index = 0);

        /// <inheritdoc cref="StatusBar.GetStatusText"/>
        string GetStatusText(int index = 0);

        /// <inheritdoc cref="StatusBar.PushStatusText"/>
        bool PushStatusText(string text, int index = 0);

        /// <inheritdoc cref="StatusBar.SetStatusWidths"/>
        bool SetStatusWidths(int[] widths);

        /// <inheritdoc cref="StatusBar.PopStatusText"/>
        bool PopStatusText(int index = 0);

        /// <inheritdoc cref="StatusBar.SetFieldsCount"/>
        bool SetFieldsCount(int count);

        /// <inheritdoc cref="StatusBar.GetStatusWidth"/>
        int GetStatusWidth(int index);

        /// <inheritdoc cref="StatusBar.GetStatusStyle"/>
        StatusBarPanelStyle GetStatusStyle(int index);

        /// <inheritdoc cref="StatusBar.SetStatusStyles"/>
        bool SetStatusStyles(StatusBarPanelStyle[] styles);

        /// <inheritdoc cref="StatusBar.GetFieldRect"/>
        RectI GetFieldRect(int index);

        /// <inheritdoc cref="StatusBar.SetMinHeight"/>
        bool SetMinHeight(int height);
    }
}
