using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class ComboBox
    {
        [Flags]
        private enum DrawItemFlags
        {
            // when set, we are painting the selected item in control,
            // not in the popup
            PaintingControl = 0x0001,

            // when set, we are painting an item which should have
            // focus rectangle painted in the background. Text color
            // and clipping region are then appropriately set in
            // the default OnDrawBackground implementation.
            PaintingSelected = 0x0002,
        }

        public void OnPlatformEventSelectedItemChanged()
        {
            if (UIControl is not UI.ComboBox uiControl)
                return;
            var selectedIndex = SelectedIndex;
            uiControl.SelectedIndex = selectedIndex == -1 ? null : selectedIndex;
        }

        public void OnPlatformEventMeasureItem()
        {
            var uiControl = UIControl as UI.ComboBox;
            if (uiControl?.ItemPainter is null)
                return;
            var defaultHeightPixels = DefaultOnMeasureItem();
            var defaultHeight = uiControl.PixelToDip(defaultHeightPixels);
            var result = uiControl.ItemPainter.GetHeight(uiControl, EventItem, defaultHeight);
            if (result >= 0)
            {
                EventResultInt = uiControl.PixelFromDip(result);
                EventCalled = true;
            }
        }

        public void OnPlatformEventMeasureItemWidth()
        {
            var uiControl = UIControl as UI.ComboBox;
            if (uiControl?.ItemPainter is null)
                return;
            var defaultWidthPixels = DefaultOnMeasureItemWidth();
            var defaultWidth = uiControl.PixelToDip(defaultWidthPixels);
            var result = uiControl.ItemPainter.GetWidth(uiControl, EventItem, defaultWidth);
            if (result >= 0)
            {
                EventResultInt = uiControl.PixelFromDip(result);
                EventCalled = true;
            }
        }

        public void OnPlatformEventDrawItem()
        {
        }

        public void OnPlatformEventDrawItemBackground()
        {
        }

        public void OnPlatformEventAfterShowPopup()
        {
            var uiControl = UIControl as UI.ComboBox;
            uiControl?.RaiseDropDown();
        }

        public void OnPlatformEventAfterDismissPopup()
        {
            var uiControl = UIControl as UI.ComboBox;
            uiControl?.RaiseDropDownClosed();
        }
    }
}