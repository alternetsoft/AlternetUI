using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IStatusBarHandler : IDisposable
    {
        bool IsOk { get; }

        bool SizingGripVisible { get; set; }

        Window? Window { get; }

        TextEllipsizeType TextEllipsize { get; set; }

        int GetFieldsCount();

        int GetBorderX();

        int GetBorderY();

        bool SetStatusText(string text, int index = 0);

        string GetStatusText(int index = 0);

        bool PushStatusText(string text, int index = 0);

        bool SetStatusWidths(int[] widths);

        bool PopStatusText(int index = 0);

        bool SetFieldsCount(int count);

        int GetStatusWidth(int index);

        StatusBarPanelStyle GetStatusStyle(int index);

        bool SetStatusStyles(StatusBarPanelStyle[] styles);

        RectI GetFieldRect(int index);

        bool SetMinHeight(int height);
    }
}
