using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IVListBoxHandler : IListBoxHandler
    {
        int ItemsCount { get; set; }

        bool HScrollBarVisible { get; set; }

        public bool VScrollBarVisible { get; set; }

        public ListBoxSelectionMode SelectionMode { get; set; }

        public RectD? GetItemRect(int index);

        bool ScrollRows(int rows);

        bool ScrollRowPages(int pages);

        void RefreshRow(int row);

        void RefreshRows(int from, int to);

        int GetVisibleEnd();

        int GetVisibleBegin();

        bool IsSelected(int line);

        bool IsVisible(int line);

        void ClearItems();

        void ClearSelected();

        void SetSelected(int index, bool value);

        int GetFirstSelected();

        int GetNextSelected();

        int GetSelectedCount();

        int GetSelection();

        int ItemHitTest(PointD position);

        void SetSelection(int selection);

        void SetSelectionBackground(Color color);

        bool IsCurrent(int current);

        bool DoSetCurrent(int current);
    }
}
