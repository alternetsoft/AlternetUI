using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Devices;

namespace Alternet.UI
{
    internal partial class MauiDisplayHandler : DisposableObject, IDisplayHandler
    {
        static MauiDisplayHandler()
        {
        }

        public MauiDisplayHandler()
        {
        }

        public MauiDisplayHandler(int index)
        {
        }

        public bool IsOk => true;

        public static Coord GetDefaultScaleFactor()
        {
            return (Coord)DeviceDisplay.Current.MainDisplayInfo.Density;
        }

        public void Log()
        {
            App.LogFileIsEnabled = true;
            App.Log($"Pixel Width: {DeviceDisplay.Current.MainDisplayInfo.Width}");
            App.Log($"Pixel Height: {DeviceDisplay.Current.MainDisplayInfo.Height}");
            App.Log($"Density: {DeviceDisplay.Current.MainDisplayInfo.Density}");
            App.Log($"Orientation: {DeviceDisplay.Current.MainDisplayInfo.Orientation}");
            App.Log($"Rotation: {DeviceDisplay.Current.MainDisplayInfo.Rotation}");
            App.Log($"Refresh Rate: {DeviceDisplay.Current.MainDisplayInfo.RefreshRate}");
            /*
                Pixel Width: 3840
                Pixel Height: 2160
                Density: 2
                Orientation: Landscape
                Rotation: Rotation0
                Refresh Rate: 60
            */

            /*
                The density is the scaling or a factor that can be used to
                convert between physical pixels and scaled pixels. For example,
                on high resolution displays, the physical number of pixels increases,
                but the scaled pixels remain the same.

                In a practical example for iOS, the Retina display will have a density
                of 2.0 or 3.0, but the units used to lay out a view does not change much.
                A view with a UI width of 100 may be 100 physical pixels (density = 1) on
                a non-Retina device, but be 200 physical pixels (density = 2) on a Retina device.

                On Windows, the density works similarly, and may often relate to the
                scale used in the display. On some monitors, the scale is set
                to 100% (density = 1), but on other high resolution monitors,
                the scale may be set to 200% (density = 2) or even 250% (density = 2.5).
            */
        }

        public RectI GetClientArea()
        {
            return GetGeometry();
        }

        public RectI GetGeometry()
        {
            var info = DeviceDisplay.Current.MainDisplayInfo;
            var width = info.Width;
            var height = info.Height;
            return (0, 0, (int)width, (int)height);
        }

        public string GetName()
        {
            return "MainDisplay";
        }

        public Coord GetScaleFactor()
        {
            var result = GetDefaultScaleFactor();
            return result;
        }

        public bool IsPrimary()
        {
            return true;
        }
    }
}
