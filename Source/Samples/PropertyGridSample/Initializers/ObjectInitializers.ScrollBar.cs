using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal partial class ObjectInitializers
    {
        public static void InitScrollBar(object control)
        {
            if (control is not ScrollBar scrollBar)
                return;
            scrollBar.SuggestedWidth = 250;
            scrollBar.Scroll += ScrollBar_Scroll;
        }

        private static void ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            Application.Log($"Scrollbar {e.Type} New: {e.NewValue} Old: {e.OldValue}");
        }
    }
}
