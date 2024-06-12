using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;

using SkiaSharp.Views.Maui;

namespace Alternet.UI
{
    public static class MauiTouchUtils
    {
        public static TouchEventArgs Convert(SKTouchEventArgs e)
        {
            TouchEventArgs result = new();

            result.Id = e.Id;
            result.ActionType = (TouchAction)e.ActionType;
            result.DeviceType = (TouchDeviceType)e.DeviceType;
            result.Location = GraphicsFactory.PixelToDip(((PointD)e.Location).ToPoint());
            result.InContact = e.InContact;
            result.WheelDelta = e.WheelDelta;
            result.Pressure = e.Pressure;
            result.MouseButton = e.MouseButton.ToAlternet();
            return result;
        }
    }
}
