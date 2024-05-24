using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp.Views.Maui;

using Alternet.UI.Extensions;

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
            result.Location = e.Location.ToAlternet();
            result.InContact = e.InContact;
            result.WheelDelta = e.WheelDelta;
            result.Pressure = e.Pressure;
            result.MouseButton = e.MouseButton.ToAlternet();
            return result;
        }
    }
}
