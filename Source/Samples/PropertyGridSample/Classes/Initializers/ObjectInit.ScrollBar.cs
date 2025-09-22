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
        public static bool LogScrollBarPosition = false;

        public static void InitScrollBar(object control)
        {
            if (control is not ScrollBar scrollBar)
                return;
            scrollBar.SuggestedWidth = 250;
            scrollBar.Scroll += ScrollBar_Scroll;
            scrollBar.IsVerticalChanged += ScrollBar_IsVerticalChanged;

            static void ScrollBar_Scroll(object sender, ScrollEventArgs e)
            {
                if (!LogScrollBarPosition)
                    return;
                App.AddIdleTask(() =>
                {
                    App.LogBeginSection();
                    App.Log($"Scrollbar {e.Type}, New: {e.NewValue} Old: {e.OldValue}");
                    (sender as ScrollBar)?.LogInfo();
                    App.LogEndSection();
                });
            }

            static void ScrollBar_IsVerticalChanged(object? sender, EventArgs e)
            {
                if (sender is not ScrollBar scrollBar)
                    return;
                if (scrollBar.IsVertical)
                    scrollBar.SuggestedSize = (Coord.NaN, 250);
                else
                    scrollBar.SuggestedSize = (250, Coord.NaN);
                scrollBar.PerformLayout();
            }
        }
    }
}
