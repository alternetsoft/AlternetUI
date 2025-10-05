using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class ComboBox
    {
        private ComboBoxItemPaintEventArgs? paintEventArgs;

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
            var uiControl = UIControl as UI.ComboBox;
            if (uiControl?.ItemPainter is null)
                return;
            DrawItem(false);
            EventCalled = true;
        }

        public void OnPlatformEventDrawItemBackground()
        {
            var uiControl = UIControl as UI.ComboBox;

            if (uiControl?.ItemPainter is null)
                return;
            DrawItem(true);
            EventCalled = true;
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

        private void DrawItem(ComboBoxItemPaintEventArgs prm)
        {
            var uiControl = UIControl as UI.ComboBox;
            uiControl?.ItemPainter?.Paint(uiControl, prm);
        }

        private void DrawItem(bool drawBackground)
        {
            if (UIControl is not UI.ComboBox uiControl)
                return;

            var flags = (DrawItemFlags)EventFlags;
            var isPaintingControl = flags.HasFlag(DrawItemFlags.PaintingControl);

            var ptr = Native.Control.OpenDrawingContextForDC(EventDc, false);
            var dc = new Drawing.WxGraphics(ptr);

            var rect = uiControl.PixelToDip(EventRect);

            void DefaultPaintMethod()
            {
                if (drawBackground)
                    DefaultOnDrawBackground();
                else
                    DefaultOnDrawItem();
            }

            if (paintEventArgs is null)
            {
                paintEventArgs = new ComboBoxItemPaintEventArgs(uiControl, dc, rect);
            }
            else
            {
                paintEventArgs.Graphics = dc;
                paintEventArgs.ClientRectangle = rect;
            }

            const int ItemIndexNotFound = -1;
            paintEventArgs.DefaultPaintAction = DefaultPaintMethod;
            paintEventArgs.IsSelected = flags.HasFlag(DrawItemFlags.PaintingSelected);
            paintEventArgs.IsPaintingControl = isPaintingControl;
            paintEventArgs.IsIndexNotFound = EventItem == ItemIndexNotFound;
            paintEventArgs.ItemIndex = EventItem;
            paintEventArgs.IsPaintingBackground = drawBackground;
            DrawItem(paintEventArgs);
            paintEventArgs.Graphics = null!;
            dc.Dispose();
        }
    }
}