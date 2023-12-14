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
            scrollBar.IsVerticalChanged += ScrollBar_IsVerticalChanged;

            static void ScrollBar_Scroll(object sender, ScrollEventArgs e)
            {
                Application.LogBeginSection();
                Application.Log($"Scrollbar Event {e.Type}, New: {e.NewValue} Old: {e.OldValue}");
                (sender as ScrollBar)?.LogScrollbarInfo();
                Application.LogEndSection();
            }

            static void ScrollBar_IsVerticalChanged(object? sender, EventArgs e)
            {
                if (sender is not ScrollBar scrollBar)
                    return;
                if (scrollBar.IsVertical)
                    scrollBar.SuggestedSize = (Double.NaN, 250);
                else
                    scrollBar.SuggestedSize = (250, Double.NaN);
                scrollBar.PerformLayout();
            }
        }
    }
}
