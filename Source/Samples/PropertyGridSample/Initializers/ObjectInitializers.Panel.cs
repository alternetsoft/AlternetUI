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
        public static void InitPanel(object control)
        {
            if (control is not Panel panel)
                return;
            panel.SuggestedSize = 150;
            panel.KeyPress += Panel_KeyPress;

            static void Panel_KeyPress(object? sender, KeyPressEventArgs e)
            {
                var prefix = "Panel.KeyPress: ";
                var s = prefix + e.KeyChar;
                Application.LogReplace(s, prefix);
            }
        }

    }
}
