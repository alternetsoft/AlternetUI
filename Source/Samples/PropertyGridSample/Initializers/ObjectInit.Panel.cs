using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal partial class ObjectInit
    {
        public static void InitPanel(object control)
        {
            if (control is not Panel panel)
                return;
            panel.SuggestedSize = 150;
            panel.KeyPress += Panel_KeyPress;
            panel.Scroll += Panel_Scroll;

            static void Panel_KeyPress(object? sender, KeyPressEventArgs e)
            {
                var prefix = "Panel.KeyPress: ";
                var s = prefix + e.KeyChar;
                Application.LogReplace(s, prefix);
            }
        }

        private static void Panel_Scroll(object sender, ScrollEventArgs e)
        {
            var s = $"Panel.Scroll: {e.Type}";
            Application.LogReplace($"{s}, {e.ScrollOrientation}, {e.NewValue}", s);
        }
    }
}
