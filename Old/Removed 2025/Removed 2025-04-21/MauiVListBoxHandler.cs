using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class MauiVListBoxHandler : MauiControlHandler, IVListBoxHandler
    {
        public int ItemsCount
        {
            set
            {
            }
        }

        public RectD? GetItemRect(int index)
        {
            return null;
        }

        public int GetVisibleBegin()
        {
            return 0;
        }

        public int GetVisibleEnd()
        {
            return 0;
        }

        public int? HitTest(PointD position)
        {
            return null;
        }

        public void RefreshRow(int row)
        {
        }

        public void RefreshRows(int from, int to)
        {
        }

        public bool ScrollToRow(int row)
        {
            return false;
        }
    }
}
