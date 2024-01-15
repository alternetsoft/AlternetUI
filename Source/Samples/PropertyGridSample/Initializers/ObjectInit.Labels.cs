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
        public static void InitGenericLabel(object control)
        {
            if (control is not GenericLabel label)
                return;
            label.Text = "GenericLabel";
            label.HorizontalAlignment = HorizontalAlignment.Left;
        }

        public static void InitLabel(object control)
        {
            if (control is not Label label)
                return;
            label.Text = "Label";
            label.HorizontalAlignment = HorizontalAlignment.Left;
        }

        public static void InitLinkLabel(object control)
        {
            if (control is not LinkLabel label)
                return;
            var s = "https://www.google.com";
            label.Text = "LinkLabel";
            label.Url = s;
        }
    }
}