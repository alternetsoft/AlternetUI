using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static bool LogScrollBarPosition = true;

        public static void InitXScrollBar(object control)
        {
            if (control is not XScrollBar scrollBar)
                return;
            scrollBar.SuggestedWidth = 250;
            scrollBar.Scroll += ScrollBar_Scroll;
            scrollBar.IsVerticalChanged += ScrollBar_IsVerticalChanged;

            void ScrollBar_Scroll(object sender, ScrollEventArgs e)
            {
                if (!LogScrollBarPosition)
                    return;
                App.AddIdleTask(() =>
                {
                    App.Log($"Scrollbar {e.Type}, Value: {scrollBar.Value}");
                });
            }

            static void ScrollBar_IsVerticalChanged(object? sender, EventArgs e)
            {
                if (sender is not XScrollBar scrollBar)
                    return;
                if (scrollBar.IsVertical)
                    scrollBar.SuggestedSize = (float.NaN, 250);
                else
                    scrollBar.SuggestedSize = (250, float.NaN);
                scrollBar.PerformLayout();
            }
        }
    }
}
