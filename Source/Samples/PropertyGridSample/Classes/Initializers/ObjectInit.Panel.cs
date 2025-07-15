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
        public static void InitGenericTabControl(object control)
        {
            if (control is not TabControl tabControl)
                return;
            InitGenericTabControl(tabControl);
        }

        public static void InitGenericTabControl(TabControl tabControl, bool withButtons = true)
        {
            tabControl.SuggestedSize = (300, 300);

            var panel1 = Internal("Panel 1");

            tabControl.Add("Panel 1", panel1);
            tabControl.Add("Panel 2", () => { return Internal("Panel 2"); });
            tabControl.Add("Panel 3", () => { return Internal("Panel 3"); });
            tabControl.Add("Panel 4", () => { return Internal("Panel 4"); });

            tabControl.SetTabSvg(0, KnownSvgImages.ImgGear, null, LightDarkColors.Blue);
            tabControl.SetTabSvg(1, MessageBoxSvg.Error);
            tabControl.SetTabSvg(2, MessageBoxSvg.Information);

            AbstractControl Internal(string title)
            {
                if(withButtons)
                    return CreatePanelWithButtons(title);
                var result = new Panel();
                result.Title = title;
                return result;
            }
        }

        public static void InitPanel(object control)
        {
            if (control is not Panel panel)
                return;
            panel.HasBorder = true;
            panel.SuggestedSize = 150;
            panel.KeyPress += Panel_KeyPress;
            panel.Scroll += Panel_Scroll;

            static void Panel_KeyPress(object? sender, KeyPressEventArgs e)
            {
                var prefix = "Panel.KeyPress: ";
                var s = prefix + e.KeyChar;
                App.LogReplace(s, prefix);
            }
        }

        private static void Panel_Scroll(object sender, ScrollEventArgs e)
        {
            var s = $"Panel.Scroll: {e.Type}";
            App.LogReplace($"{s}, {e.ScrollOrientation}, {e.NewValue}", s);
        }
    }
}
