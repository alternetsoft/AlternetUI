using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class VListBox
    {
        public void OnPlatformEventSelectionChanged()
        {

        }

        public void OnPlatformEventMeasureItem()
        {
            if (UIControl is not VirtualListBox uiControl)
                return;

            var itemIndex = EventItem;

            MeasureItemEventArgs e = new(uiControl.MeasureCanvas, itemIndex);
            uiControl.MeasureItemSize(e);

            var heightDip = e.ItemHeight;
            var height = uiControl.PixelFromDip(heightDip);
            EventHeight = height;
        }

        public void OnPlatformEventDrawItem()
        {

        }

        public void OnPlatformEventDrawItemBackground()
        {

        }

        public void OnPlatformEventControlRecreated()
        {

        }
    }
}


